using System;

namespace Kindruk.lab2
{
    class Program
    {
        static void Main()
        {
            var p = new Purchase<Merchandise>
            {
                new Merchandise("dishwasher", 1050.99),
                new Merchandise("towel", 12.99, 4),
                new Merchandise("toyrobot", 17.79, 2),
                new Merchandise("blank Tshirt", 9.99, 5)
            };
            foreach (var item in p)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine(p.TotalCost());

            Console.ReadKey();
        }
    }
}
