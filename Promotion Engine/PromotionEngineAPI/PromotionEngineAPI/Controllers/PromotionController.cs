using CalculatePrice;
using Dal;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PromotionEngineAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ApiPolicy")]
    public class PromotionController : ControllerBase
    {
        [HttpGet]
        public ResultModel GetPrice(int item_a, int item_b, int item_c, int item_d, string promotion = "")
        {
            decimal totalPrice = 0.0m;
            FinalPrice totalCost = new FinalPrice();

            Dictionary<string, int> dict_cart = new Dictionary<string, int>();
            dict_cart.Add("A", item_a);
            dict_cart.Add("B", item_b);
            dict_cart.Add("C", item_c);
            dict_cart.Add("D", item_d);

            totalPrice = totalCost.CalculatePriceAfterDiscounts(dict_cart, promotion);

            return new ResultModel { totalAmount = totalPrice };
        }

        private decimal GetTotalPrice(Dictionary<string, int> dict_cart)
        {
            decimal totalPrice = 0.0m;
            ItemPrice price = new ItemPrice();
            DataTable dtPriceList = price.GetItemPrice();

            foreach (DataRow item in dtPriceList.Rows)
            {
                if (dict_cart.Keys.Contains(item["Item"].ToString()))
                {
                    totalPrice += Convert.ToDecimal(item["Price"]) * Convert.ToDecimal(dict_cart[item["Item"].ToString()]);
                }
            }

            return totalPrice;
        }

        private decimal Apply_A(Dictionary<string, int> dict_cart)
        {
            decimal finalPrice = 0;
            Promotion prom = new Promotion();
            DataTable dtDisc = prom.GetDiscounts();

            foreach (DataRow dr in dtDisc.Rows)
            {
                if (dr["Item"].ToString().Equals("A") && dict_cart.Keys.Contains("A") && Convert.ToInt16(dict_cart["A"]) > 0)
                {
                    int eligibleItems = Convert.ToInt16(Math.Floor(Convert.ToDecimal(Convert.ToInt16(dict_cart["A"]) / Convert.ToInt16(dr["Qty"]))));
                    int uneligibleItems = Convert.ToInt16(dict_cart["A"]) % Convert.ToInt16(dr["Qty"]);

                    finalPrice += Convert.ToDecimal(eligibleItems * Convert.ToDecimal(dr["Discount"]));
                    dict_cart["A"] = uneligibleItems;

                    break;
                }
            }

            finalPrice += GetTotalPrice(dict_cart);

            return finalPrice;
        }

        private decimal Apply_B(Dictionary<string, int> dict_cart)
        {
            decimal finalPrice = 0;
            Promotion prom = new Promotion();
            DataTable dtDisc = prom.GetDiscounts();

            foreach (DataRow dr in dtDisc.Rows)
            {
                if (dr["Item"].ToString().Equals("B") && dict_cart.Keys.Contains("B") && Convert.ToInt16(dict_cart["B"]) > 0)
                {
                    int eligibleItems = Convert.ToInt16(Math.Floor(Convert.ToDecimal(Convert.ToInt16(dict_cart["B"]) / Convert.ToInt16(dr["Qty"]))));
                    int uneligibleItems = Convert.ToInt16(dict_cart["B"]) % Convert.ToInt16(dr["Qty"]);

                    finalPrice += Convert.ToDecimal(eligibleItems * Convert.ToDecimal(dr["Discount"]));
                    dict_cart["B"] = uneligibleItems;

                    break;
                }
            }

            finalPrice += GetTotalPrice(dict_cart);

            return finalPrice;
        }

        private decimal Apply_C_D(Dictionary<string, int> dict_cart)
        {
            decimal finalPrice = 0;
            Promotion prom = new Promotion();
            DataTable dtDisc = prom.GetDiscounts();

            foreach (DataRow dr in dtDisc.Rows)
            {
                if (dr["Item"].ToString().Equals("C;D") && dict_cart.Keys.Contains("C") && Convert.ToInt16(dict_cart["C"]) > 0 && dict_cart.Keys.Contains("D") && Convert.ToInt16(dict_cart["D"]) > 0)
                {
                    if (Convert.ToInt16(dict_cart["C"]) == Convert.ToInt16(dict_cart["D"]))
                    {
                        int eligibleItems = Convert.ToInt16(Math.Floor(Convert.ToDecimal(Convert.ToInt16(dict_cart["D"]) / Convert.ToInt16(dr["Qty"]))));
                        int uneligibleItems = Convert.ToInt16(dict_cart["D"]) % Convert.ToInt16(dr["Qty"]);

                        finalPrice += Convert.ToDecimal(eligibleItems * Convert.ToDecimal(dr["Discount"]));
                        dict_cart["D"] = 0;
                        dict_cart["C"] = 0;
                    }
                    else if (Convert.ToInt16(dict_cart["C"]) < Convert.ToInt16(dict_cart["D"]))
                    {
                        int eligibleItems = Convert.ToInt16(Math.Floor(Convert.ToDecimal(Convert.ToInt16(dict_cart["C"]) / Convert.ToInt16(dr["Qty"]))));
                        int uneligibleItems = Convert.ToInt16(dict_cart["C"]) % Convert.ToInt16(dr["Qty"]);

                        finalPrice += Convert.ToDecimal(eligibleItems * Convert.ToDecimal(dr["Discount"]));
                        dict_cart["C"] = uneligibleItems;
                        dict_cart["D"] = dict_cart["D"] - dict_cart["C"];
                    }
                    else if (Convert.ToInt16(dict_cart["C"]) > Convert.ToInt16(dict_cart["D"]))
                    {
                        int eligibleItems = Convert.ToInt16(Math.Floor(Convert.ToDecimal(Convert.ToInt16(dict_cart["D"]) / Convert.ToInt16(dr["Qty"]))));
                        int uneligibleItems = Convert.ToInt16(dict_cart["D"]) % Convert.ToInt16(dr["Qty"]);

                        finalPrice += Convert.ToDecimal(eligibleItems * Convert.ToDecimal(dr["Discount"]));
                        dict_cart["D"] = uneligibleItems;
                        dict_cart["C"] = dict_cart["C"] - dict_cart["D"];
                    }

                    break;
                }
            }

            finalPrice += GetTotalPrice(dict_cart);

            return finalPrice;
        }
    }
}
