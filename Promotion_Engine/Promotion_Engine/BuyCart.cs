using CalculatePrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promotion_Engine
{
    class BuyCart
    {
        static void Main(string[] args)
        {
            Console.WriteLine("************Welcome to Shopping Cart************");
            Console.WriteLine("");

            Console.WriteLine("Please specify the quantity of A you want to order: ");
            int a = Convert.ToInt16(Console.ReadLine());

            Console.WriteLine("Please specify the quantity of B you want to order: ");
            int b = Convert.ToInt16(Console.ReadLine());

            Console.WriteLine("Please specify the quantity of C you want to order: ");
            int c = Convert.ToInt16(Console.ReadLine());

            Console.WriteLine("Please specify the quantity of D you want to order: ");
            int d = Convert.ToInt16(Console.ReadLine());

            Console.WriteLine("Please specify the quantity of coupon you want to apply:\n 1. Apply-A  \n 2. Apply-B  \n 3. Apply-C_D");
            int coupon = Convert.ToInt16(Console.ReadLine());

            Dictionary<string, int> dict = new Dictionary<string, int>();
            dict.Add("A", a);
            dict.Add("B", b);
            dict.Add("C", c);
            dict.Add("D", d);

            FinalPrice price = new FinalPrice();
            decimal totalValue = 0;

            switch(coupon)
            {
                case 1: totalValue = price.CalculatePriceAfterDiscounts(dict, "Apply-A");
                    break;
                case 2:
                    totalValue = price.CalculatePriceAfterDiscounts(dict, "Apply-B");
                    break;
                case 3:
                    totalValue = price.CalculatePriceAfterDiscounts(dict, "Apply-C_D");
                    break;
                default:
                    totalValue = price.CalculatePriceAfterDiscounts(dict, "");
                    break;
            }

            Console.WriteLine("");
            Console.WriteLine("Your final price is: {0}", totalValue);
            Console.ReadKey();
        }
    }
}
