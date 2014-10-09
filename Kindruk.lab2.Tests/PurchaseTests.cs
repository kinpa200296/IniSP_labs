using System;
using System.Globalization;
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
        public void AddTest()
        {
            var p = new Purchase<Merchandise>();
            Assert.AreEqual(0, p.TotalCost());
            p.Add(new Merchandise("dishwasher", 1050.99));
            Assert.AreEqual(1050.99, p.TotalCost());
            p.Add(new Merchandise("towel", 12.99, 4));
            Assert.AreEqual(51.96, p[1].Cost());
            Assert.AreEqual(1102.95, p.TotalCost());
            p.Add(new Merchandise("toyrobot", 17.79, 2));
            Assert.AreEqual(35.58, p[2].Cost());
            Assert.AreEqual(1138.53, p.TotalCost());
            p.Add(new Merchandise("blank Tshirt", 9.99, 5));
            Assert.AreEqual(49.95, p[3].Cost());
            Assert.AreEqual(1188.48, p.TotalCost());
        }

        [TestMethod]
        public void ClearTest()
        {
            var p = new Purchase<Merchandise>();
            Assert.AreEqual(0, p.TotalCost());
            p.Add(new Merchandise("dishwasher", 1050.99));
            Assert.AreEqual(1050.99, p.TotalCost());
            p.Add(new Merchandise("towel", 12.99, 4));
            Assert.AreEqual(51.96, p[1].Cost());
            Assert.AreEqual(1102.95, p.TotalCost());
            p.Clear();
            try
            {
                Console.WriteLine(p[2]);
            }
            catch (ArgumentOutOfRangeException) { }
            Assert.AreEqual(0, p.Count);
            p.Add(new Merchandise("toyrobot", 17.79, 2));
            Assert.AreEqual(35.58, p.TotalCost());
            p.Add(new Merchandise("blank Tshirt", 9.99, 5));
            Assert.AreEqual(49.95, p[1].Cost());
            Assert.AreEqual(85.53, p.TotalCost());
        }

        [TestMethod]
        public void ContainsTest()
        {
            var p = new Purchase<Merchandise>();
            var r1 = new Merchandise("dishwasher", 1050.99);
            var r2 = new Merchandise("towel", 12.99, 4);
            var r3 = new Merchandise("toyrobot", 17.79, 2);
            var r4 = new Merchandise("blank Tshirt", 9.99, 5);
            Assert.AreEqual(false, p.Contains(r1));
            Assert.AreEqual(false, p.Contains(r2));
            Assert.AreEqual(false, p.Contains(r3));
            Assert.AreEqual(false, p.Contains(r4));
            p.Add(r1);
            Assert.AreEqual(true, p.Contains(r1));
            Assert.AreEqual(false, p.Contains(r2));
            Assert.AreEqual(false, p.Contains(r3));
            Assert.AreEqual(false, p.Contains(r4));
            p.Add(new Merchandise("towel", 12.99, 4));
            Assert.AreEqual(true, p.Contains(r1));
            Assert.AreEqual(true, p.Contains(r2));
            Assert.AreEqual(false, p.Contains(r3));
            Assert.AreEqual(false, p.Contains(r4));
            p.Add(new Merchandise("toyrobot", 17.79, 2));
            Assert.AreEqual(true, p.Contains(r1));
            Assert.AreEqual(true, p.Contains(r2));
            Assert.AreEqual(true, p.Contains(r3));
            Assert.AreEqual(false, p.Contains(r4));
            p.Clear();
            Assert.AreEqual(false, p.Contains(r1));
            Assert.AreEqual(false, p.Contains(r2));
            Assert.AreEqual(false, p.Contains(r3));
            Assert.AreEqual(false, p.Contains(r4));
            p.Add(new Merchandise("blank Tshirt", 9.99, 5));
            Assert.AreEqual(false, p.Contains(r1));
            Assert.AreEqual(false, p.Contains(r2));
            Assert.AreEqual(false, p.Contains(r3));
            Assert.AreEqual(true, p.Contains(r4));
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

        [TestMethod]
        public void LinqTestWhere()
        {
            var p = new Purchase<Merchandise>
            {
                new Merchandise("dishwasher", 1050.99),
                new Merchandise("towel", 12.99, 4),
                new Merchandise("toyrobot", 17.79, 2),
                new Merchandise("blank Tshirt", 9.99, 5)
            };
            var r1 = p.Where(item => item.Cost() > 100);
            var merchandises = r1 as Merchandise[] ?? r1.ToArray();
            Assert.AreEqual(1, merchandises.Count());
            Assert.AreEqual(true, merchandises.First().Equals(new Merchandise("dishwasher", 1050.99)));
        }

        [TestMethod]
        public void LinqTestSelect()
        {
            var p = new Purchase<Merchandise>
            {
                new Merchandise("dishwasher", 1050.99),
                new Merchandise("towel", 12.99, 4),
                new Merchandise("toyrobot", 17.79, 2),
                new Merchandise("blank Tshirt", 9.99, 5)
            };
            var r1 = p.Select(item => (item.Cost() < 50) ? item : null).Where(item => item != null).ToArray();
            Assert.AreEqual(2, r1.Count());
            Assert.AreEqual("toyrobot    2    17.79", r1[0].ToString("f", CultureInfo.InvariantCulture));
            Assert.AreEqual("blank Tshirt    5    9.99", r1[1].ToString("f", CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void LinqTestOrder()
        {
            var p = new Purchase<Merchandise>
            {
                new Merchandise("dishwasher", 1050.99),
                new Merchandise("towel", 12.99, 4),
                new Merchandise("toyrobot", 17.79, 2),
                new Merchandise("blank Tshirt", 9.99, 5)
            };
            var r1 = p.OrderBy(item => item.Price);
            Merchandise prev = null;
            foreach (var item in r1)
            {
                if (prev != null)
                {
                    Assert.AreEqual(true, prev.Price < item.Price);
                }
                prev = item;
            }
            r1 = p.OrderBy(item => item.Count);
            prev = null;
            foreach (var item in r1)
            {
                if (prev != null)
                {
                    Assert.AreEqual(true, prev.Count < item.Count);
                }
                prev = item;
            }
            r1 = p.OrderBy(item => item.Cost());
            prev = null;
            foreach (var item in r1)
            {
                if (prev != null)
                {
                    Assert.AreEqual(true, prev.Cost()< item.Cost());
                }
                prev = item;
            }
        }

        [TestMethod]
        public void LinqTestGroupBy()
        {
            var p = new Purchase<Merchandise>
            {
                new Merchandise("dishwasher", 1050.99),
                new Merchandise("towel", 12.99, 4),
                new Merchandise("toyrobot", 17.79, 2),
                new Merchandise("blank Tshirt", 9.99, 5),
                new Merchandise("dish", 9.99, 10),
                new Merchandise("fork", 3.99, 10),
                new Merchandise("spoon", 4.99, 10),
                new Merchandise("knife", 3.99, 10)
            };
            var r1 = p.GroupBy(item => item.Count);
            r1 = r1.Where(item => item.Count() > 1);
            var enumerable = r1 as IGrouping<int, Merchandise>[] ?? r1.ToArray();
            Assert.AreEqual(1, enumerable.Count());
            foreach(var group in enumerable)
            {
                var array = group.ToArray();
                foreach (var merchandise in array)
                {
                    Assert.AreEqual(10, merchandise.Count);
                }
            }
            var r2 = p.GroupBy(item => item.Price);
            r2 = r2.Where(item => item.Count() > 1);
            var groups = r2 as IGrouping<double, Merchandise>[] ?? r2.ToArray();
            Assert.AreEqual(2, groups.Count());
            foreach (var group in groups)
            {
                var array = group.ToArray();
                foreach (var merchandise in array)
                {
                    Assert.AreEqual(true, (Math.Abs(merchandise.Price - 3.99) < 0.0001 || Math.Abs(merchandise.Price - 9.99) < 0.0001));
                }
            }
        }

        [TestMethod]
        public void LinqTestAgregate()
        {
            var p = new Purchase<Merchandise>
            {
                new Merchandise("dishwasher", 1050.99),
                new Merchandise("towel", 12.99, 4),
                new Merchandise("toyrobot", 17.79, 2),
                new Merchandise("blank Tshirt", 9.99, 5)
            };
            Assert.AreEqual(9.99, p.Min(item => item.Price));
            Assert.AreEqual(35.58, p.Min(item => item.Cost()));
            Assert.AreEqual(5, p.Max(item => item.Count));
            Assert.AreEqual(1188.48 ,p.Sum(item => item.Cost()));
            p.RemoveAt(0);
            Assert.AreEqual(137.49, p.Sum(item => item.Cost()));
            Assert.AreEqual(1, p.Count(item => item.Cost() > 50));
            Assert.AreEqual(2, p.Count(item => item.Cost() > 40));
        }
    }
}
