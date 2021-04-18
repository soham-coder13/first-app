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

            var amazonTask = FetchItemsFromAmazon(Sites.AmazonUrl, Sites.AmazonLink, searchItem);
            var flipkartTask = FetchItemsFromFlipkart(Sites.FlipkartUrl, Sites.FlipkartLink, searchItem);
            var paytmTask = FetchItemsFromPaytm(Sites.PaytmUrl, Sites.PaytmLink, searchItem);
            var snapdealTask = FetchItemsFromSnapdeal(Sites.SnapdealUrl, Sites.SnapdealLink, searchItem);

            await Task.WhenAll(amazonTask, flipkartTask, paytmTask, snapdealTask);
            itemList.AddRange(await amazonTask);
            itemList.AddRange(await flipkartTask);
            itemList.AddRange(await paytmTask);
            itemList.AddRange(await snapdealTask);

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
                                                .Where(div => div.GetAttributeValue("class", "").Equals("_2B099V") 
                                                || div.GetAttributeValue("class", "").Equals("_4ddWXP")).ToList();

            foreach (var result in searchResults)
            {
                try
                {
                    string title = result.Descendants("div").FirstOrDefault().InnerText;
                    if (title.Trim().ToLower().Equals("ad"))
                        continue;
                    string desc = string.Empty;
                    foreach (var node in result.Descendants("a"))
                    {
                        if (!string.IsNullOrEmpty(node.GetAttributeValue("title", "")))
                        {
                            desc = node.GetAttributeValue("title", "");
                            break;
                        }
                    }
                    string link = string.Empty;
                    foreach (var node in result.Descendants("a"))
                    {
                        if (!string.IsNullOrEmpty(node.GetAttributeValue("href", "")))
                        {
                            link = node.GetAttributeValue("href", "");
                            break;
                        }
                    }
                    string price = result.Descendants("a").LastOrDefault().Descendants("div").FirstOrDefault().Descendants("div").FirstOrDefault().InnerText;
                    string image = string.Empty; //result.PreviousSibling.Descendants("img").FirstOrDefault().GetAttributeValue("src", "");

                    if ((!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(desc)) && !string.IsNullOrEmpty(link) && !string.IsNullOrEmpty(price))
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

        private static async Task<List<Item>> FetchItemsFromSnapdeal(string baseUrl, string searchUrl, string searchItem)
        {
            List<Item> items = new List<Item>();
            var client = new HttpClient();
            var doc = new HtmlDocument();

            doc.LoadHtml(await client.GetStringAsync(searchUrl + searchItem));

            //get the main div where each items are placed within
            var searchResults = doc.DocumentNode.Descendants("div")
                                                .Where(div => div.GetAttributeValue("class", "")
                                                .Contains("product-tuple-listing")).ToList();

            foreach (var result in searchResults)
            {
                try
                {
                    string name = result.Descendants("div").Where(d => d.GetAttributeValue("class", "").Equals("product-tuple-description ")).FirstOrDefault().Descendants("p").FirstOrDefault().InnerText;
                    string link = result.Descendants("div").Where(d => d.GetAttributeValue("class", "").Equals("product-tuple-description ")).FirstOrDefault().Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
                    string price = result.Descendants("div").Where(d => d.GetAttributeValue("class", "").Equals("product-tuple-description ")).FirstOrDefault().Descendants("span").Where(s => s.GetAttributeValue("class", "").Contains("product-desc-price")).FirstOrDefault().InnerText;
                    string image = result.Descendants("img").FirstOrDefault().GetAttributeValue("src", "");

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(link) && !string.IsNullOrEmpty(price))
                    {
                        Item item = new Item
                        {
                            Name = name,
                            Link = link,
                            Source = "Snapdeal",
                            Price = Convert.ToDecimal(price.Split(' ')[1].Replace(",", string.Empty)),
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
