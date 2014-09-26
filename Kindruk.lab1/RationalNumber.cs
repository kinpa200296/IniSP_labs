using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Kindruk.lab1
{
    public class RationalNumber : IEquatable<RationalNumber>, IComparable<RationalNumber>, IFormattable
    {
        public const string DenominatorOutOfRange = "Знаменатель должен быть целым числом.";
        public const string UnknownNumberFormat = "Неизвестный формат представления числа.";
        public const string FormatProviderNullReference = "formatProvider дал пустую ссылку.";

        long _numerator;
        long _denominator = 1;

        #region properties

        public long Numerator 
        {
            get 
            {
                var val = MathExtension.Gcd(Math.Abs(_numerator), _denominator); //общий множитель
                _numerator /= val;
                _denominator /= val;
                return _numerator; 
            }
            set { _numerator = value; }
        }

        public long Denominator 
        {
            get
            {
                long val = MathExtension.Gcd(Math.Abs(_numerator), _denominator);// общий множитель
                _numerator /= val;
                _denominator /= val;
                return _denominator; 
            }
            set 
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(DenominatorOutOfRange);
                _denominator = value;
            }
        }
        #endregion

        #region constructors

        public RationalNumber(long numerator, long denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public RationalNumber(double value)
        {
            var r = Parse(value.ToString("0.###############", CultureInfo.CurrentCulture));
            Numerator = r.Numerator;
            Denominator = r.Denominator;
        }

        #endregion

        public static RationalNumber Parse(string str)
        {
            return Parse(str, CultureInfo.CurrentCulture);
        }

        public static RationalNumber Parse(string str, IFormatProvider formatProvider)
        {
            if (!TryParse(str, formatProvider))
                throw new FormatException(UnknownNumberFormat);
            var c = str.Split("+().,/".ToCharArray());
            var cnt = c.Length;
            for (var i = 0; i < c.Length; i++)
                if (string.IsNullOrEmpty(c[i]))
                {
                    cnt--;
                    if (i != c.Length - 1)
                        c[i] = c[i + 1];
                }
            RationalNumber r1, r2;
            if (cnt == 3)
            {
                r1 = new RationalNumber(long.Parse(c[0]), 1);
                r2 = new RationalNumber(long.Parse(c[1]), (long)Math.Pow(10, c[1].Length));
                var r3 = new RationalNumber(long.Parse(c[2]), (long)(Math.Pow(10, c[1].Length)*(Math.Pow(10, c[2].Length)-1)));
                r2.Numerator *= c[0] == "-0" ? -1 : 1;
                r3.Numerator *= c[0] == "-0" ? -1 : 1;
                return r1 + r2 + r3;
            }
            if (str.Contains("/") && cnt == 2)
            {
                return new RationalNumber(long.Parse(c[0]), long.Parse(c[1]));
            }
            if (str.Contains("(") && cnt == 2)
            {
                r1 = new RationalNumber(long.Parse(c[0]), 1);
                r2 = new RationalNumber(long.Parse(c[1]), (long) Math.Pow(10, c[1].Length) - 1);
                r2.Numerator *= c[0] == "-0" ? -1 : 1;
                return r1 + r2;
            }
            if (cnt == 2)
            {
                r1 = new RationalNumber(long.Parse(c[0]), 1);
                r2 = new RationalNumber(long.Parse(c[1]), (long) Math.Pow(10, c[1].Length));
                r2.Numerator *= c[0] == "-0" ? -1 : 1;
                return r1 + r2;
            }
            return new RationalNumber(long.Parse(c[0]), 1);
        }

        public static bool TryParse(string str)
        {
            return TryParse(str, CultureInfo.CurrentCulture);
        }

        public static bool TryParse(string str, IFormatProvider formatProvider)
        {
            var match = Regex.Match(str, @"^[+-]?\d+$");
            if (match.Success)
                return true;
            var cultureInfo = formatProvider as CultureInfo;
            if (cultureInfo == null)
                throw new NullReferenceException(FormatProviderNullReference);
            match = Regex.Match(str, cultureInfo.NumberFormat.NumberDecimalSeparator == "." ? @"^[+-]?\d+.\d*\(\d+\)$" : @"^[+-]?\d+,\d*\(\d+\)$");
            if (match.Success)
                return true;
            match = Regex.Match(str, cultureInfo.NumberFormat.NumberDecimalSeparator == "." ? @"^[+-]?\d+.\d+$" : @"^[+-]?\d+,\d+$");
            if (match.Success)
                return true;
            match = Regex.Match(str, @"^[+-]?\d+\/\d+$");
            return match.Success;
        }

        public override string ToString()
        {
            return ToString("F", CultureInfo.CurrentCulture);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F";
            if (formatProvider == null)
                formatProvider = CultureInfo.CurrentCulture;
            var cultureInfo = formatProvider as CultureInfo;
            if (cultureInfo == null)
                throw new NullReferenceException(FormatProviderNullReference);
            switch (format.ToUpperInvariant())
            {
                case "F":
                    return Numerator.ToString(formatProvider)+"/"+Denominator.ToString(formatProvider);
                case "R":
                    return ToRealNumber(cultureInfo, formatProvider);
                case "P":
                    return ToPeriodicNumber(cultureInfo, formatProvider);
                default:
                    throw new FormatException(UnknownNumberFormat);
            }
        }

        private string ToRealNumber(CultureInfo cultureInfo, IFormatProvider formatProvider)
        {
            var wholePart = Numerator / Denominator;
            double fractionalPart = Numerator % Denominator;
            fractionalPart = fractionalPart / Denominator;
            var sb = new StringBuilder(fractionalPart.ToString("0.###############", formatProvider));
            sb.Remove(0, Numerator < 0 ? 3 : 2);
            return (Numerator < 0 ? "-" : "") + wholePart.ToString(formatProvider) + cultureInfo.NumberFormat.NumberDecimalSeparator + sb;
        }

        private string ToPeriodicNumber(CultureInfo cultureInfo, IFormatProvider formatProvider)
        {
            var wholePart = Numerator / Denominator;
            var fractionalPart = Numerator % Denominator;
            var sb = new StringBuilder(wholePart.ToString(formatProvider));
            sb.Append(cultureInfo.NumberFormat.NumberDecimalSeparator);
            var nonFractionalPart = sb.Length;
            while (fractionalPart > 0)
            {
                fractionalPart *= 10;
                sb.Append((fractionalPart/Denominator).ToString(formatProvider));
                fractionalPart %= Denominator;
                for (var i = 1; i <= (sb.Length - nonFractionalPart)/2; i++)
                {
                    var period = i;
                    for (var j = sb.Length; j > sb.Length - i; j--)
                        if (sb[j - 1] != sb[j - i - 1])
                        {
                            period = 0;
                            break;
                        }
                    if (period != 0)
                    {
                        sb.Remove(sb.Length - period, period);
                        sb.Insert(sb.Length - period, '(');
                        sb.Append(')');
                        fractionalPart = 0;
                        break;
                    }
                }
            }
            return sb.ToString();
        }

        #region overloaded math operations

        public static RationalNumber operator +(RationalNumber p1, RationalNumber p2)
        {
            var val = MathExtension.Lcm(p1.Denominator, p2.Denominator); // Наименьший общий множитель
            return new RationalNumber(p1.Numerator * (val / p1.Denominator) + p2.Numerator * (val / p2.Denominator), val);
        }

        public static RationalNumber operator -(RationalNumber p1, RationalNumber p2)
        {
            var val = MathExtension.Lcm(p1.Denominator, p2.Denominator); // Наименьший общий множитель
            return new RationalNumber(p1.Numerator * (val / p1.Denominator) - p2.Numerator * (val / p2.Denominator), val);
        }

        public static RationalNumber operator *(RationalNumber p1, RationalNumber p2)
        {
            return new RationalNumber(p1.Numerator * p2.Numerator, p1.Denominator * p2.Denominator);
        }

        public static RationalNumber operator /(RationalNumber p1, RationalNumber p2)
        {
            long numerator = p1.Numerator*p2.Denominator, denominator = p2.Numerator*p1.Denominator;
            if (denominator < 0)
            {
                numerator *= -1;
                denominator *= -1;
            }
            return denominator != 0 ? new RationalNumber(numerator, denominator) : new RationalNumber(0, 1);
        }

        public static double Log(RationalNumber p)
        {
            return Math.Log(p.Numerator) - Math.Log(p.Denominator);
        }

        public static double Log(RationalNumber p, double newBase)
        {
            return Math.Log(p.Numerator, newBase) - Math.Log(p.Denominator, newBase);
        }

        public static double Log10(RationalNumber p)
        {
            return Math.Log10(p.Numerator) - Math.Log10(p.Denominator);
        }

        public static RationalNumber Abs(RationalNumber p)
        {
            return new RationalNumber(Math.Abs(p.Numerator), p.Denominator);
        }

        #endregion

        public bool Equals(RationalNumber other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return (Numerator == other.Numerator) && (Denominator == other.Denominator);
        }

        public int CompareTo(RationalNumber other)
        {
            if (ReferenceEquals(other, null))
                return 1;
            var val = MathExtension.Lcm(Denominator, other.Denominator);
            val = Numerator * (val / Denominator) - other.Numerator * (val / other.Denominator);
            if (val > 0)
                return 1;
            if (val < 0)
                return -1;
            return 0;
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == GetType() && Equals((RationalNumber)obj);
        }

        public override int GetHashCode()
        {
            return (int)(((Denominator % 2000000013) * 37 + (Math.Abs(Numerator) % 2000000013) * 13) % 2000000013);
        }

        #region overloaded comparison operators

        public static bool operator ==(RationalNumber p1, RationalNumber p2)
        {
            return !ReferenceEquals(p1, null) ? p1.Equals(p2) : ReferenceEquals(p2, null);
        }

        public static bool operator !=(RationalNumber p1, RationalNumber p2)
        {
            return !(p1 == p2);
        }

        public static bool operator >(RationalNumber p1, RationalNumber p2)
        {
            return p1.CompareTo(p2) > 0;
        }

        public static bool operator <(RationalNumber p1, RationalNumber p2)
        {
            return p1.CompareTo(p2) < 0;
        }

        public static bool operator >=(RationalNumber p1, RationalNumber p2)
        {
            return p1.CompareTo(p2) >= 0;
        }

        public static bool operator <=(RationalNumber p1, RationalNumber p2)
        {
            return p1.CompareTo(p2) <= 0;
        }

        #endregion
    }
}
