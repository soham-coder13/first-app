using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerService.Models
{
    public class Item
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Source { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
    }
}
