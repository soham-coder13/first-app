using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WorkflowManager.Models;

namespace WorkflowManager
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        public async Task<string> Get(string id, string authorName, string authorEmail, string comment, string location)
        {
            Approval approval = new Approval
            {
                Id = Convert.ToInt32(id),
                Author = new Author { Name = authorName, Email = authorEmail },
                Comment = comment,
                Location = location
            };

            return await SendJsonObject(approval);
        }

        private async Task<string> SendJsonObject(Approval approval)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{ConfigurationManager.AppSetting["Elsa:Server:BaseUrl"]}");
                var response = client.PostAsJsonAsync("/v1/documents", approval).Result;
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                else
                    return "Internal Server Error";
            }
        }
    }
}
