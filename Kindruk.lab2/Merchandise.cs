using System;

namespace Kindruk.lab2
{
    class Merchandise : IMerchandise
    {
        public const string NameCannotBeNull = "Имя товара не может быть пустым указателем, пустой строкой или строкой содержащей только пробелы";
        public const string PriceOutOfRange = "Цена товара - положительное число, большее нуля";
        public const string CountOutOfRange = "Количество товаров - натуральное число";

        private string _name;
        private double _price;
        private int _count;

        #region properties
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Na" + "me", NameCannotBeNull);
                _name = value;
            }
        }

        public double Price
        {
            get { return _price; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Pri" + "ce", PriceOutOfRange);
                _price = value;
            }
        }

        public int Count
        {
            get { return _count; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Co" + "unt", CountOutOfRange);
                _count = value;
            }
        }
        #endregion

        #region constructors

        public Merchandise(string name, double price)
        {
            Name = name;
            Price = price;
            Count = 1;
        }

        public Merchandise(string name, double price,  int count)
        {
            Name = name;
            Price = price;
            Count = count;
        }
        #endregion
    }
}
