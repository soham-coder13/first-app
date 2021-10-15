using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WorkflowManager.Models;

namespace WorkflowManager
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        public async Task<string> Get(string id, string reason)
        {
            Reason comment = new Reason
            {
                Id = Convert.ToInt32(id),
                Comment = reason
            };

            return await SendJsonObject(comment);
        }

        private async Task<string> SendJsonObject(Reason comment)
        {
            using (var client=new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{ConfigurationManager.AppSetting["Elsa:Server:BaseUrl"]}/v1/comment/{Convert.ToString(comment.Id)}"),
                    Content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                else
                    return "Internal Server Error";
            }
        }
    }
}
