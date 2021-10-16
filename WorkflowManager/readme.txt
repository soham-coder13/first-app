PROJECT:
Workflow Manager

PROJECT DESCRIPTION:
Workflow Manager is a tool by which an Organization can standardize their business processes.
It allows management to keep a track of tasks performed and the status of the same. 
In this project, Elsa has been used which is an open source .Net Standard Library that is used to have a Workflow Manager in place.
In order to get mails for all the tasks, smtp email server needs to be configured. We can use "smtp4dev" in order to create a fake smtp email server in our machine.
This project has the following workflow in place.
1. From the Document Approval page, User provides the details and the document link for approval.
2. Once User clicks on "Send" button, a POST request is generated to the Workflow Manager and a mail is generated to the concerned mail id for approval.
3. The reviewer can then open the link of the document to be approved. He will have three options----Approve, Reject or Remind Later. 
4. On clicking on "Remind Later" link, the reviewer will get get a reminder mail every 20 seconds(configurable).
5. If the reviewer clicks on "Approve" link, a mail will be generated to the User who requested for the approval about the approval.
6. If the reviewer clicks on "Decline" link, the User who requested for the document approval will get a rejection mail and the reviewer will be notifed that he has to comment about the rejection cause so that the User can correct the same. A mail will be triggered every 30 seconds(configurable) which will have the link to comment the reason and also another which will enable the User to approve the document in case rejection was done by accident.
7. Once the reason for rejection was sent, the User will recieve the same in a mail and the workflow cycle will be finished.

PREREQUISITES:
.Net 5.0 SDK along with Visual Studio 2019

AUTHOR:
Soham Chakraborty

LICENSE:
Free to be used and modified by anyone