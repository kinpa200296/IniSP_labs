using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Числитель рационального числа
        /// </summary>
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
        /// <summary>
        /// Знаменать рационального числа
        /// </summary>
        public long Denominator 
        {
            get
            {
                var val = MathExtension.Gcd(Math.Abs(_numerator), _denominator);// общий множитель
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
        /// <summary>
        /// Возвращает объект RationalNumber с заданными числителем и знаменателем
        /// </summary>
        /// <param name="numerator">Числитель рационального числа</param>
        /// <param name="denominator">Знаменать рационального числа</param>
        public RationalNumber(long numerator, long denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        /// <summary>
        /// Возвращает объект RationalNumber соответствующий заданному числу
        /// </summary>
        /// <param name="value">Вещественное число</param>
        public RationalNumber(double value)
        {
            var r = Parse(value.ToString("0.###############", CultureInfo.CurrentCulture));
            Numerator = r.Numerator;
            Denominator = r.Denominator;
        }

        #endregion

        /// <summary>
        /// Преобразует строковое значение, заданное в виде дроби, вещественного числа или числа с периодом
        /// в объект типа RationalNumber с учетом местных региональных стандартов
        /// </summary>
        /// <param name="str">Содержит число в виде дроби, вещественного числа или числа с периодом</param>
        /// <returns></returns>
        public static RationalNumber Parse(string str)
        {
            return Parse(str, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Преобразует строковое значение, заданное в виде дроби, вещественного числа или числа с периодом
        /// в объект типа RationalNumber с учетом заданных региональных стандартов
        /// </summary>
        /// <param name="str">Содержит число в виде дроби, вещественного числа или числа с периодом</param>
        /// <param name="formatProvider">Параметр, содержащий региональные стандарты</param>
        /// <returns></returns>
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
                        for (var j = i; j < c.Length-2; j++)
                            c[j] = c[j + 1];
                }
            RationalNumber r1, r2;
            if (cnt == 3)
            {
                r1 = new RationalNumber(long.Parse(c[0]), 1);
                r2 = new RationalNumber(long.Parse(c[1]), (long)Math.Pow(10, c[1].Length));
                var r3 = new RationalNumber(long.Parse(c[2]), (long)(Math.Pow(10, c[1].Length)*(Math.Pow(10, c[2].Length)-1)));
                r2.Numerator *= (c[0] == "-0" || r1.Numerator < 0) ? -1 : 1;
                r3.Numerator *= (c[0] == "-0" || r1.Numerator < 0) ? -1 : 1;
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
                r2.Numerator *= (c[0] == "-0" || r1.Numerator < 0) ? -1 : 1;
                return r1 + r2;
            }
            if (cnt == 2)
            {
                r1 = new RationalNumber(long.Parse(c[0]), 1);
                r2 = new RationalNumber(long.Parse(c[1]), (long) Math.Pow(10, c[1].Length));
                r2.Numerator *= (c[0] == "-0" || r1.Numerator < 0) ? -1 : 1;
                return r1 + r2;
            }
            return new RationalNumber(long.Parse(c[0]), 1);
        }

        /// <summary>
        /// Проверяет возможность преобразования числа
        /// в объект типа RationalNumber с учетом местных региональных стандартов
        /// </summary>
        /// <param name="str">Содержит число в виде дроби, вещественного числа или числа с периодом</param>
        /// <returns></returns>
        public static bool TryParse(string str)
        {
            return TryParse(str, CultureInfo.CurrentCulture);
        }
        
        /// <summary>
        /// Проверяет возможность преобразования числа
        /// в объект типа RationalNumber с учетом заданных региональных стандартов
        /// </summary>
        /// <param name="str">Содержит число в виде дроби, вещественного числа или числа с периодом</param>
        /// <param name="formatProvider">Параметр, содержащий региональные стандарты</param>
        /// <returns></returns>
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

        /// <summary>
        /// Возвращает строковое значение соответствующее значения объекта
        /// в виде дроби с учетом местных региональных стандартов
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("F", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Возвращает строковое значение соответствующее значения объекта
        /// с учетом заданных региональных стандартов в заданном формате
        /// Флаги форматирования:
        /// F - в виде дроби
        /// R - в виде вещественного числа
        /// P - в виде числа с периодом
        /// </summary>
        /// <param name="format">Параметр форматирования</param>
        /// <returns></returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Возвращает строковое значение соответствующее значения объекта
        /// с учетом заданных региональных стандартов в заданном формате
        /// Флаги форматирования:
        /// F - в виде дроби
        /// R - в виде вещественного числа
        /// P - в виде числа с периодом
        /// </summary>
        /// <param name="format">Параметр форматирования</param>
        /// <param name="formatProvider">Параметр, содержащий региональные стандарты</param>
        /// <returns></returns>
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
            if (wholePart == 0 && fractionalPart < 0)
                sb.Insert(0, '-');
            sb.Append(cultureInfo.NumberFormat.NumberDecimalSeparator);
            fractionalPart = Math.Abs(fractionalPart);
            var list = new List<long>();
            var hashset = new HashSet<long>();
            while (fractionalPart > 0)
            {
                fractionalPart *= 10;
                if (hashset.Contains(fractionalPart))
                    break;
                hashset.Add(fractionalPart);
                list.Add(fractionalPart);
                fractionalPart %= Denominator;
            }
            foreach (var item in list)
            {
                if (item == fractionalPart)
                    sb.Append('(');
                sb.Append((item / Denominator).ToString(formatProvider));
            }
            if (hashset.Contains(fractionalPart))
                sb.Append(')');
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
