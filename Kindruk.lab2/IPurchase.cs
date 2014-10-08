using System;
using System.Collections.Generic;

namespace Kindruk.lab2
{
    interface IPurchase<T> : IList<T>, IDisposable
            where T : IMerchandise
    {
    }
}
