namespace ConsolePlayer
{
    class MathExtension//дополнительные математические функции
    {
        // Наибольший общий делитель
        public static long Gcd(long a, long b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else b %= a;
            }
            return a + b;
        }

        // Наименьший общий множитель
        public static long Lcm(long a, long b)
        {
            var val = Gcd(a, b);
            return (a / val) * b;
        }
    }
}
