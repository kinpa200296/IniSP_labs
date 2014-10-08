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
        private bool _disposed;

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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing){}
            _disposed = true;
        }

        public bool Equals(Merchandise other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_name, other._name) && _price.Equals(other._price) && _count == other._count;
        }

        public static bool operator ==(Merchandise p1, Merchandise p2)
        {
            return !ReferenceEquals(p1, null) ? p1.Equals(p2) : ReferenceEquals(p2, null);
        }

        public static bool operator !=(Merchandise p1, Merchandise p2)
        {
            return !(p1 == p2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Merchandise)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Price.GetHashCode();
                hashCode = (hashCode * 397) ^ Count;
                return hashCode;
            }
        }

        ~Merchandise()
        {
            Dispose(false);
        }
    }
}
