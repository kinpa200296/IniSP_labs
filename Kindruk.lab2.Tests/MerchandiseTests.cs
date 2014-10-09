using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kindruk.lab2.Tests
{
    [TestClass]
    public class MerchandiseTests
    {
        [TestMethod]
        public void EqualsTest()
        {
            var p1 = new Merchandise("dishwasher", 1050.99);
            var p2 = new Merchandise("towel", 12.99, 4);
            Assert.AreEqual(false, p1 == p2);
            Assert.AreEqual(false, p2.Equals(new Merchandise("towel", 12.99)));
            Assert.AreEqual(true, p2.Equals(new Merchandise("towel", 12.99, 4)));
            Assert.AreEqual(true, p1 != p2);
            Assert.AreEqual(true, p1.Equals(p1));
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.AreEqual("blanket    29,99", (new Merchandise("blanket", 29.99)).ToString());
            Assert.AreEqual("blanket    1    29.99", (new Merchandise("blanket", 29.99)).ToString("f", CultureInfo.InvariantCulture));
            Assert.AreEqual("screen cleaner    5    9,99", (new Merchandise("screen cleaner", 9.99, 5)).ToString("f"));
            Assert.AreEqual("screen cleaner    49,95", (new Merchandise("screen cleaner", 9.99, 5)).ToString());
            try
            {
                (new Merchandise("table", 119.99)).ToString("E");
            }
            catch (FormatException e)
            {
                Assert.AreEqual(Merchandise.UnknownFormat, e.Message);
            }
        }

        [TestMethod]
        public void PropertiesExceptionsTest()
        {
            Merchandise a;
            try
            {
                a = new Merchandise("   ", 32);
                Console.WriteLine(a.Cost());
            }
            catch (ArgumentNullException) { }
            try
            {
                a = new Merchandise("", 32);
                Console.WriteLine(a.Cost());
            }
            catch (ArgumentNullException) { }
            try
            {
                a = new Merchandise("bread", -3.99);
                Console.WriteLine(a.Cost());
            }
            catch (ArgumentOutOfRangeException) { }
            try
            {
                a = new Merchandise("bread", 3.99, -2);
                Console.WriteLine(a.Cost());
            }
            catch (ArgumentOutOfRangeException) { }
        }
    }
}
