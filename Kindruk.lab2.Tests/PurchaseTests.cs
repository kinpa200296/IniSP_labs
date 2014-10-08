using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kindruk.lab2.Tests
{
    [TestClass]
    public class PurchaseTests
    {
        [TestMethod]
        public void TotalCostTest()
        {
            var p = new Purchase<Merchandise>
            {
                new Merchandise("dishwasher", 1050.99),
                new Merchandise("towel", 12.99, 4),
                new Merchandise("toyrobot", 17.79, 2),
                new Merchandise("blank Tshirt", 9.99, 5)
            };
            Assert.AreEqual(1188.48, p.TotalCost());
        }

        [TestMethod]
        public void CopyToTest()
        {
            var a = new Merchandise[2];
            var p = new Purchase<Merchandise>
            {
                new Merchandise("dishwasher", 1050.99),
                new Merchandise("towel", 12.99, 4),
                new Merchandise("toyrobot", 17.79, 2),
                new Merchandise("blank Tshirt", 9.99, 5)
            };
            p.CopyTo(a, 2);
            Assert.AreEqual(85.53, a.Sum(item => item.Cost()));
            try
            {
                p.CopyTo(a, 0);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(Purchase<Merchandise>.TooFewSpace, e.Message);
            }
            try
            {
                p.CopyTo(null, 0);
            }
            catch (ArgumentNullException) {}
            try
            {
                p.CopyTo(a, -32);
            }
            catch (ArgumentOutOfRangeException) {}
            try
            {
                p.CopyTo(a, 65);
            }
            catch (ArgumentOutOfRangeException) {}
        }
    }
}
