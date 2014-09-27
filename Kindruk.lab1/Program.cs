using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Kindruk.lab1
{
    class Program
    {
        static void Main()
        {
            var r1 = RationalNumber.Parse("12/9");
            var r2 = RationalNumber.Parse("16/25");
            Console.WriteLine("{0:r}", r1 / r2);
            Console.WriteLine(r1 < r2);
            Console.WriteLine(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            var m = Regex.Match("12.12(324)", @"^[+-]?\d+.\d*\(\d+\)$");
            Console.WriteLine(m.Success);
            r1 = RationalNumber.Parse("3.021(8512)", CultureInfo.InvariantCulture);
            Console.WriteLine("{0:p}", r1);
            r1 = RationalNumber.Parse("-3/4");
            Console.WriteLine("{0:p}", r1);
            Console.WriteLine("{0:p}", RationalNumber.Parse("3,08(885)"));
            Console.WriteLine("{0:p}", new RationalNumber(1000000 + 7, 1000000 + 9));
            Console.ReadKey();
        }
    }
}
