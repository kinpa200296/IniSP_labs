using System;

namespace Kindruk.lab2
{
    interface IMerchandise : IDisposable, IEquatable<Merchandise>
    {
        string Name { get; set; }
        double Price { get; set; }
        int Count { get; set; }
    }
}
