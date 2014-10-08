using System;
using System.Collections.Generic;

namespace Kindruk.lab2
{
    public interface IPurchase<T> : IList<T>, IDisposable
            where T : IMerchandise
    {
        /// <summary>
        /// Общая стоимость покупки
        /// </summary>
        /// <returns></returns>
        double TotalCost();
    }
}
