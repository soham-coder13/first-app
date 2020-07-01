using Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatePrice
{
    public class FinalPrice
    {
        public decimal CalculatePriceAfterDiscounts(Dictionary<string,int> dict_cart, string promotion)
        {
            ItemPrice price = new ItemPrice();
            decimal totalPrice = 0.0m;
            DataTable dtPriceList = price.GetItemPrice();

            //If promotion not applied, return the total price of the items added to cart
            if(string.IsNullOrEmpty(promotion))
            {
                foreach(DataRow item in dtPriceList.Rows)
                {
                    if(dict_cart.Keys.Contains(item["Item"].ToString()))
                    {
                        totalPrice += Convert.ToDecimal(item["Price"]) * Convert.ToDecimal(dict_cart[item["Item"].ToString()]);
                    }
                }
            }

            return totalPrice;
        }
    }
}
