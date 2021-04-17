using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrawlerService;
using CrawlerService.Models;
using Ganss.XSS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BestBuy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        // GET: api/Search/Item
        [HttpGet]
        [HttpGet("{id}", Name = "Search")]
        public async Task<IEnumerable<Item>> Get(string searchItem, int itemCount = 20)
        {
            SiteInfo site = new SiteInfo();
            List<Item> searchResults;
            var sanitizer = new HtmlSanitizer();

            if (string.IsNullOrEmpty(searchItem))
                throw new Exception("Please enter an item");

            string searchKey = sanitizer.Sanitize(searchItem.Trim());   //check for scripts entered
            itemCount = itemCount >= 20 ? itemCount : 20;
            
            //check if item has already been searched
            string obj = HttpContext.Session.GetString(searchKey);
            if (!string.IsNullOrEmpty(searchKey))
            {
                if (string.IsNullOrEmpty(obj))
                {
                    searchResults = await site.GetSearchResults(searchKey);
                    HttpContext.Session.SetString(searchKey, JsonConvert.SerializeObject(searchResults));
                    return searchResults.OrderBy(s => s.Price).Take(itemCount);
                }
                else
                {
                    searchResults = JsonConvert.DeserializeObject<List<Item>>(obj);
                    return searchResults.OrderBy(s => s.Price).Take(itemCount);
                }
            }
            else
                throw new Exception("Invalid Search Key Entered");
        }
    }
}
