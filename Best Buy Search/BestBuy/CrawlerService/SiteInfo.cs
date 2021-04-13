using CrawlerService.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrawlerService
{
    public class SiteInfo
    {
        public async Task<List<Item>> GetSearchResults(string searchItem)
        {
            List<Item> itemList = new List<Item>();

            var items = await FetchItemsFromAmazon(Sites.AmazonUrl, Sites.AmazonLink, searchItem);
            itemList.AddRange(items);

            items = await FetchItemsFromFlipkart(Sites.FlipkartUrl, Sites.FlipkartLink, searchItem);
            itemList.AddRange(items);

            items = await FetchItemsFromPaytm(Sites.PaytmUrl, Sites.PaytmLink, searchItem);
            itemList.AddRange(items);

            return itemList;
        }

        private static async Task<List<Item>> FetchItemsFromAmazon(string baseUrl, string searchUrl, string searchItem)
        {
            List<Item> items = new List<Item>();
            var client = new HttpClient();
            var doc = new HtmlDocument();

            doc.LoadHtml(await client.GetStringAsync(searchUrl + searchItem));

            //get the main div where each items are placed within
            var searchResults = doc.DocumentNode.Descendants("span")
                                                .Where(div => div.GetAttributeValue("cel_widget_id", "")
                                                .StartsWith("MAIN-SEARCH_RESULTS-")).ToList();

            foreach (var result in searchResults)
            {
                try
                {
                    var sp = result.Descendants("span").Where(s => s.InnerHtml.ToLower().Equals("sponsored"));
                    if (sp.Count() > 0)
                        continue;

                    string name = result.Descendants("h2").FirstOrDefault().Descendants("span").FirstOrDefault().InnerText;
                    string link = result.Descendants("h2").FirstOrDefault().Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
                    //string rating = result.Descendants("i").FirstOrDefault().Descendants("span").FirstOrDefault().InnerText;
                    string price = result.Descendants("span").Where(s => s.GetAttributeValue("class", "").Equals("a-price-whole")).FirstOrDefault().InnerText;
                    string image = result.Descendants("img").FirstOrDefault().GetAttributeValue("src", "");

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(link) && !string.IsNullOrEmpty(price))
                    {
                        Item item = new Item
                        {
                            Name = name,
                            Link = baseUrl + link,
                            Source = "Amazon",
                            Price = Convert.ToDecimal(price.Trim('₹')),
                            Image = image
                        };

                        items.Add(item);
                    }
                }
                catch (Exception) { }
            }

            return items;
        }

        private static async Task<List<Item>> FetchItemsFromFlipkart(string baseUrl, string searchUrl, string searchItem)
        {
            List<Item> items = new List<Item>();
            var client = new HttpClient();
            var doc = new HtmlDocument();

            doc.LoadHtml(await client.GetStringAsync(searchUrl + searchItem));

            //get the main div where each items are placed within
            var searchResults = doc.DocumentNode.Descendants("div")
                                                .Where(div => div.GetAttributeValue("class", "")
                                                .Equals("_2B099V")).ToList();

            foreach (var result in searchResults)
            {
                try
                {
                    string title = result.Descendants("div").FirstOrDefault().InnerText;
                    string desc = result.Descendants("a").FirstOrDefault().GetAttributeValue("title", "");
                    string link = result.Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
                    string price = result.Descendants("a").LastOrDefault().Descendants("div").FirstOrDefault().Descendants("div").FirstOrDefault().InnerText;
                    string image = result.PreviousSibling.Descendants("img").FirstOrDefault().GetAttributeValue("src", "");

                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(desc) && !string.IsNullOrEmpty(link) && !string.IsNullOrEmpty(price))
                    {
                        Item item = new Item
                        {
                            Name = title + " " + desc,
                            Link = baseUrl + link,
                            Source = "Flipkart",
                            Price = Convert.ToDecimal(price.Trim('₹')),
                            Image = image
                        };

                        items.Add(item);
                    }
                }
                catch (Exception) { }
            }

            return items;
        }

        private static async Task<List<Item>> FetchItemsFromPaytm(string baseUrl, string searchUrl, string searchItem)
        {
            List<Item> items = new List<Item>();
            var client = new HttpClient();
            var doc = new HtmlDocument();

            doc.LoadHtml(await client.GetStringAsync(searchUrl + searchItem));

            //get the main div where each items are placed within
            var searchResults = doc.DocumentNode.Descendants("div")
                                                .Where(div => div.GetAttributeValue("class", "")
                                                .Equals("_3WhJ")).ToList();

            foreach (var result in searchResults)
            {
                try
                {
                    string name = result.Descendants("div").Where(d => d.GetAttributeValue("class", "").Equals("UGUy")).FirstOrDefault().InnerText;
                    string link = result.Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
                    string price = result.Descendants("div").Where(d => d.GetAttributeValue("class", "").Equals("_1kMS")).FirstOrDefault().InnerText;
                    string image = result.Descendants("img").FirstOrDefault().GetAttributeValue("src", "");

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(link) && !string.IsNullOrEmpty(price))
                    {
                        Item item = new Item
                        {
                            Name = name,
                            Link = baseUrl + link,
                            Source = "Paytm Mall",
                            Price = Convert.ToDecimal(price.Trim('₹')),
                            Image = image
                        };

                        items.Add(item);
                    }
                }
                catch (Exception) { }
            }

            return items;
        }
    }
}
