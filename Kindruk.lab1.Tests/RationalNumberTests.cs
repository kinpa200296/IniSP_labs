using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kindruk.lab1.Tests
{
    [TestClass]
    public class RationalNumberTests
    {
        [TestMethod]
        public void ParseAndToStringTestsFractionFormat()
        {
            var r = RationalNumber.Parse("-12/4");
            Assert.AreEqual(-3, r.Numerator);
            Assert.AreEqual(1, r.Denominator);
            r = RationalNumber.Parse("421/856");
            Assert.AreEqual("421/856", r.ToString("f"));
            Assert.AreEqual("421/856", r.ToString("F"));
            Assert.AreEqual("421/856", r.ToString());
            Assert.AreEqual("421/856", r.ToString("f", CultureInfo.CurrentCulture));
            Assert.AreEqual("421/856", r.ToString("F", CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void ParseAndToStringTestsRealFormat()
        {
            var r = new RationalNumber(3.0);
            r = r + RationalNumber.Parse("1/4");
            Assert.AreEqual("3.25", r.ToString("r", CultureInfo.InvariantCulture));
            Assert.AreEqual("3,25", r.ToString("r", CultureInfo.CurrentCulture));
            r = new RationalNumber(3.1);
            r += RationalNumber.Parse("1/9");
            Assert.AreEqual(3.211111111111111, double.Parse(r.ToString("r")));
            Assert.AreEqual(3.211111111111111, double.Parse(r.ToString("R", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void ParseAndToStringTestsPeriodicFormat()
        {
            var r = new RationalNumber(3.0);
            r = r + RationalNumber.Parse("0.25", CultureInfo.InvariantCulture);
            Assert.AreEqual("3.25", r.ToString("r", CultureInfo.InvariantCulture));
            Assert.AreEqual("3,25", r.ToString("r", CultureInfo.CurrentCulture));
            r = new RationalNumber(3.1);
            r += RationalNumber.Parse("0,(1)");
            Assert.AreEqual("3,2(1)", r.ToString("P"));
            Assert.AreEqual("3.2(1)", r.ToString("p", CultureInfo.InvariantCulture));
            r = new RationalNumber(3.0);
            r = r + RationalNumber.Parse("0.(3)", CultureInfo.InvariantCulture);
            Assert.AreEqual("3,(3)", r.ToString("p"));
        }

        [TestMethod]
        public void DenominatorOutOfRangeTest()
        {
            try
            {
                var r = new RationalNumber(1, 0);
                Console.WriteLine(r.ToString());
            }
            catch (ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, RationalNumber.DenominatorOutOfRange);
            }
            try
            {
                var r = new RationalNumber(1, -15);
                Console.WriteLine(r.ToString());
            }
            catch (ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, RationalNumber.DenominatorOutOfRange);
            }
        }

        [TestMethod]
        public void UnknownNumberFormatTest()
        {
            try
            {
                var r = RationalNumber.Parse("12,1W(23)3");
                Console.WriteLine(r.ToString());
            }
            catch (FormatException e)
            {
                StringAssert.Contains(e.Message, RationalNumber.UnknownNumberFormat);
            }
        }

        [TestMethod]
        public void FormatProviderNullReferenceTest()
        {
            CultureInfo culture = null;
            try
            {
                var r = RationalNumber.Parse("12,23", culture);
                Console.WriteLine(r.ToString());
            }
            catch (NullReferenceException e)
            {
                StringAssert.Contains(e.Message, RationalNumber.FormatProviderNullReference);
            }
            try
            {
                var r = RationalNumber.Parse("12,3");
                Console.WriteLine(r.ToString("f", culture));
            }
            catch (NullReferenceException e)
            {
                StringAssert.Contains(e.Message, RationalNumber.FormatProviderNullReference);
            }
        }

        [TestMethod]
        public void MathOperationsTest()
        {
            var r1 = RationalNumber.Parse("12/85");
            var r2 = RationalNumber.Parse("-17/5");
            var r3 = RationalNumber.Parse("144/51");
            Assert.AreEqual("-277/85", (r1 + r2).ToString());
            Assert.AreEqual("301/85", (r1 - r2).ToString());
            Assert.AreEqual("-0.48", (r1*r2).ToString("r", CultureInfo.InvariantCulture));
            Assert.AreEqual("-12/289", (r1/r2).ToString());
            Assert.AreEqual("576/1445", (r1*r3).ToString());
            Assert.AreEqual("0,05", (r1/r3).ToString("p"));
        }

        [TestMethod]
        public void ComparisonOperationsTest()
        {
            var r1 = RationalNumber.Parse("12/85");
            var r2 = RationalNumber.Parse("-17/5");
            var r3 = RationalNumber.Parse("144/51");
            Assert.AreEqual(true, r1 > r1 + r2);
            Assert.AreEqual(true, r1 < r1 - r2);
            Assert.AreEqual(true, RationalNumber.Parse("-0,48") == r1*r2);
            Assert.AreEqual(true, RationalNumber.Parse("0,05") != r1*r2);
            Assert.AreEqual(true, RationalNumber.Parse("0,05") != r1/r2);
            Assert.AreEqual(true, RationalNumber.Parse("0,05") >= r1/r3);
            Assert.AreEqual(true, RationalNumber.Parse("0,05") >= r1/r2);
            Assert.AreEqual(true, RationalNumber.Parse("0,05") <= r1*r3);
            Assert.AreEqual(true, RationalNumber.Parse("0,05") <= r1/r3);
        }
    }
}
