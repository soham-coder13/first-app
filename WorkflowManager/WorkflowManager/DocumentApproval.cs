using System.Net;
using System.Net.Http;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Email;
using Elsa.Activities.Http;
using Elsa.Activities.Http.Extensions;
using Elsa.Activities.Http.Models;
using Elsa.Activities.Primitives;
using Elsa.Activities.Temporal;
using Elsa.Builders;
using NodaTime;

namespace WorkflowManager
{
    public class DocumentApproval : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            string baseUrl = ConfigurationManager.AppSetting["Elsa:Server:BaseUrl"];

            builder
                .WithDisplayName("Document Approval Workflow")
                .HttpEndpoint(activity => activity
                    .WithPath("/v1/documents")
                    .WithMethod(HttpMethod.Post.Method)
                    .WithReadContent())
                .SetVariable("Document", context => context.GetInput<HttpRequestModel>()!.Body)
                .SendEmail(activity => activity
                    .WithSender("workflow@acme.com")
                    .WithRecipient("mgmt@acme.com")
                    .WithSubject(context => $"Document received from {context.GetVariable<dynamic>("Document")!.Author.Name}")
                    .WithBody(context =>
                    {
                        var document = context.GetVariable<dynamic>("Document")!;
                        var author = document!.Author;
                        var location = document!.Location;
                        return $"<a href=\"{location}\">Document</a> from {author.Name} received for review.<br /><a href=\"{context.GenerateSignalUrl("Approve")}\">Approve</a> or <a href=\"{context.GenerateSignalUrl("Reject")}\">Reject</a> or <a href=\"{context.GenerateSignalUrl("Remind")}\">Remind Later</a>";
                    }))
                .WriteHttpResponse(
                    HttpStatusCode.OK,
                    "<h1>Request for Approval Sent</h1><p>Your document has been received and will be reviewed shortly.</p>",
                    "text/html")
                .Then<Fork>(activity => activity.WithBranches("Approve", "Reject", "Remind"), fork =>
                {
                    fork
                        .When("Approve")
                        .SignalReceived("Approve")
                        .WriteHttpResponse(HttpStatusCode.OK, "Thank you for approving the document. Your decision will be notified to the User.", "text/html")
                        .SendEmail(activity => activity
                            .WithSender("workflow@acme.com")
                            .WithRecipient(context => context.GetVariable<dynamic>("Document")!.Author.Email)
                            .WithSubject(context => $"Document {context.GetVariable<dynamic>("Document")!.Id} Approved!")
                            .WithBody(context => $"Great job {context.GetVariable<dynamic>("Document")!.Author.Name}, that document is perfect."))
                        .ThenNamed("Join");

                    fork
                        .When("Reject")
                        .SignalReceived("Reject")
                        .SendEmail(activity => activity
                            .WithSender("workflow@acme.com")
                            .WithRecipient(context => context.GetVariable<dynamic>("Document")!.Author.Email)
                            .WithSubject(context => $"Document {context.GetVariable<dynamic>("Document")!.Id} Rejected")
                            .WithBody(context => $"Nice try {context.GetVariable<dynamic>("Document")!.Author.Name}, but that document needs work."))
                        .WriteHttpResponse(HttpStatusCode.OK, "Please <a href=\"/CommentForm\">comment></a> the reason for rejection to complete your review", "text/html")
                        .SetVariable("Status", "rejected")
                        .Then<Fork>(activity => activity.WithBranches("Comment", "Comment Reminder"), fork =>
                          {
                              fork
                                  .When("Comment")
                                  .Timer(Duration.FromSeconds(2)).WithName("Get Comment")
                                  .HttpEndpoint(activity => activity
                                      .WithPath(context => $"/v1/comment/{context.GetVariable<dynamic>("Document")!.Id}")
                                      .WithMethod(HttpMethod.Get.Method)
                                      .WithReadContent())
                                  .SetVariable("Comment", context => context.GetInput<HttpRequestModel>()!.Body)
                                  .WriteHttpResponse(HttpStatusCode.Accepted, "<h2>Comment Received</h2><p>We have notified the User</p>", "text/html")
                                  .SendEmail(activity => activity
                                      .WithSender("workflow@acme.com")
                                      .WithRecipient(context => context.GetVariable<dynamic>("Document")!.Author.Email)
                                      .WithSubject(context => $"Reason for Rejection of Document: {context.GetVariable<dynamic>("Document")!.Id}")
                                      .WithBody(context => $"Your Document was not approved since: <b>{context.GetVariable<dynamic>("Comment")!.Reason}</b>"))
                                  .ThenNamed("Join");

                              fork
                                  .When("Comment Reminder")
                                  .Timer(Duration.FromSeconds(30)).WithName("Comment Reminder")
                                  .SendEmail(activity => activity
                                      .WithSender("workflow@acme.com")
                                      .WithRecipient("mgmt@acme.com")
                                      .WithSubject(context => $"{context.GetVariable<dynamic>("Document")!.Author.Name} is waiting to know the reason behind rejection of Document: {context.GetVariable<dynamic>("Document")!.Id}")
                                      .WithBody(context => $"Please provde a <a href=\"{baseUrl}/CommentForm\">comment</a> for rejection of Document: {context.GetVariable<dynamic>("Document")!.Id}.<br />If rejected accidentally, click to <a href=\"{context.GenerateSignalUrl("Approve")}\">Approve</a>"))
                                  .ThenNamed("Comment Reminder");
                          })
                        .ThenNamed("Join");

                    fork
                        .When("Remind")
                        .SignalReceived("Remind")
                        .WriteHttpResponse(HttpStatusCode.OK, "You will be reminded after 20 seconds", "text/html")
                        .StartIn(Duration.FromSeconds(20)).WithName("Reminder")
                        .IfTrue(context => context.GetVariable<string>("Status") != null && context.GetVariable<string>("Status").Equals("rejected"), outcome => outcome.ThenNamed("Get Comment"))
                        .SendEmail(activity => activity
                                .WithSender("workflow@acme.com")
                                .WithRecipient("mgmt@acme.com")
                                .WithSubject(context => $"{context.GetVariable<dynamic>("Document")!.Author.Name} is waiting for your review!")
                                .WithBody(context =>
                                    $"Don't forget to review <a href=\"{context.GetVariable<dynamic>("Document")!.Location}\">Document</a> from {context.GetVariable<dynamic>("Document")!.Author.Name} received for review.<br /><a href=\"{context.GenerateSignalUrl("Approve")}\">Approve</a> or <a href=\"{context.GenerateSignalUrl("Reject")}\">Reject</a>"))
                            .ThenNamed("Reminder");
                })
                .Add<Join>(join => join.WithMode(Join.JoinMode.WaitAny)).WithName("Join");
        }
    }
}
