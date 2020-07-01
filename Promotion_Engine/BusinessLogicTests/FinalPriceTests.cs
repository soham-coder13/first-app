using System;
using System.Collections.Generic;
using CalculatePrice;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogicTests
{
    [TestClass]
    public class FinalPriceTests
    {
        [TestMethod]
        public void Scenario_NoPromotion_Test()
        {
            Dictionary<string, int> dictCart = new Dictionary<string, int>();
            FinalPrice cal = new FinalPrice();

            dictCart.Add("A", 1);
            dictCart.Add("B", 1);
            dictCart.Add("C", 1);

            decimal actualValue = cal.CalculatePriceAfterDiscounts(dictCart, string.Empty);

            Assert.IsTrue(actualValue == 100.0M);
        }

        [TestMethod]
        public void Scenario_A_Test()
        {
            Dictionary<string, int> dictCart = new Dictionary<string, int>();
            FinalPrice cal = new FinalPrice();

            dictCart.Add("A", 5);
            dictCart.Add("B", 5);
            dictCart.Add("C", 1);

            decimal actualValue = cal.CalculatePriceAfterDiscounts(dictCart, "Apply-A");

            Assert.IsTrue(actualValue == 400.0M);
        }

        [TestMethod]
        public void Scenario_B_Test()
        {
            Dictionary<string, int> dictCart = new Dictionary<string, int>();
            FinalPrice cal = new FinalPrice();

            dictCart.Add("A", 5);
            dictCart.Add("B", 5);
            dictCart.Add("C", 1);

            decimal actualValue = cal.CalculatePriceAfterDiscounts(dictCart, "Apply-B");

            Assert.IsTrue(actualValue == 390.0M);
        }

        [TestMethod]
        public void Scenario_C_D_Test()
        {
            Dictionary<string, int> dictCart = new Dictionary<string, int>();
            FinalPrice cal = new FinalPrice();

            dictCart.Add("A", 3);
            dictCart.Add("B", 5);
            dictCart.Add("C", 1);
            dictCart.Add("D", 1);

            decimal actualValue = cal.CalculatePriceAfterDiscounts(dictCart, "Apply-C_D");

            Assert.IsTrue(actualValue == 390.0M);
        }
    }
}
