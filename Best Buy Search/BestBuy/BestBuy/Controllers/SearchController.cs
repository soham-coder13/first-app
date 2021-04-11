using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CrawlerService;
using CrawlerService.Models;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;

namespace BestBuy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        // GET: api/Search/Item
        [HttpGet]
        [HttpGet("{id}", Name = "Search")]
        public async Task<IEnumerable<Item>> Get(string searchItem)
        {
            SiteInfo site = new SiteInfo();
            var sanitizer = new HtmlSanitizer();

            if (string.IsNullOrEmpty(searchItem))
                throw new Exception("Please enter an item");

            string searchKey = sanitizer.Sanitize(searchItem.Trim());
            if (!string.IsNullOrEmpty(searchKey))
            {
                List<Item> searchResults = await site.GetSearchResults(searchItem);

                return searchResults.OrderBy(s => s.Price).Take(20);
            }
            else
                throw new Exception("Invalid Search Key Entered");
        }
    }
}
