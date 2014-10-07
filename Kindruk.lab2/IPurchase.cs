using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kindruk.lab2
{
    interface IPurchase<T> : IList<T>, IDisposable
    {
    }
}
