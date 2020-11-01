using Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CalculatePrice
{
    public class FinalPrice
    {
        public decimal CalculatePriceAfterDiscounts(Dictionary<string, int> dict_cart, string promotion)
        {
            decimal totalPrice = 0.0m;

            //If promotion not applied, return the total price of the items added to cart
            if (string.IsNullOrEmpty(promotion))
            {
                totalPrice = GetTotalPrice(dict_cart);
            }
            else
            {
                switch (promotion)
                {
                    case "Apply-A":
                        totalPrice = Apply_A(dict_cart);
                        break;
                    case "Apply-B":
                        totalPrice = Apply_B(dict_cart);
                        break;
                    case "Apply-C_D":
                        totalPrice = Apply_C_D(dict_cart);
                        break;
                    default:
                        totalPrice = CalculatePriceAfterDiscounts(dict_cart, string.Empty);
                        break;
                }
            }

            return totalPrice;
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
