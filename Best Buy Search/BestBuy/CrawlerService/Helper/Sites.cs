using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerService
{
    public static class Sites
    {
        private static string amazon = "https://www.amazon.in/s?k=";
        private static string flipkart = "https://www.flipkart.com/search?q=";
        private static string paytm = "https://paytmmall.com/shop/search?q=";
        private static string snapdeal = "https://www.snapdeal.com/search?keyword=";

        private static string amazonUrl = "https://www.amazon.in";
        private static string flipkartUrl = "https://www.flipkart.com";
        private static string paytmUrl = "https://paytmmall.com";
        private static string snapdealUrl = "https://www.snapdeal.com";

        public static string AmazonLink
        {
            get
            {
                return amazon;
            }
        }

        public static string FlipkartLink
        {
            get
            {
                return flipkart;
            }
        }

        public static string PaytmLink
        {
            get
            {
                return paytm;
            }
        }

        public static string SnapdealLink
        {
            get
            {
                return snapdeal;
            }
        }

        public static string AmazonUrl
        {
            get
            {
                return amazonUrl;
            }
        }

        public static string FlipkartUrl
        {
            get
            {
                return flipkartUrl;
            }
        }

        public static string PaytmUrl
        {
            get
            {
                return paytmUrl;
            }
        }

        public static string SnapdealUrl
        {
            get
            {
                return snapdealUrl;
            }
        }
    }
}
