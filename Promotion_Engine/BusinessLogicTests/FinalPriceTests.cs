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
        public void Scenario_A_Test()
        {
            Dictionary<string, int> dictCart = new Dictionary<string, int>();
            FinalPrice cal = new FinalPrice();

            dictCart.Add("A", 1);
            dictCart.Add("B", 1);
            dictCart.Add("C", 1);

            decimal expectedValue = cal.CalculatePriceAfterDiscounts(dictCart, "Apply-A");

            Assert.IsTrue(expectedValue == 100.0M);
        }

        [TestMethod]
        public void Scenario_B_Test()
        {
            Dictionary<string, int> dictCart = new Dictionary<string, int>();
            FinalPrice cal = new FinalPrice();

            dictCart.Add("A", 5);
            dictCart.Add("B", 5);
            dictCart.Add("C", 1);

            decimal expectedValue = cal.CalculatePriceAfterDiscounts(dictCart, "Apply-A");

            Assert.IsTrue(expectedValue == 370.0M);
        }

        [TestMethod]
        public void Scenario_CD_Test()
        {
            Dictionary<string, int> dictCart = new Dictionary<string, int>();
            FinalPrice cal = new FinalPrice();

            dictCart.Add("A", 3);
            dictCart.Add("B", 5);
            dictCart.Add("C", 1);
            dictCart.Add("D", 1);

            decimal expectedValue = cal.CalculatePriceAfterDiscounts(dictCart, "Apply-A");

            Assert.IsTrue(expectedValue == 280.0M);
        }
    }
}
