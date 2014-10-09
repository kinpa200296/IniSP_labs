using System;

namespace Kindruk.lab2
{
    public interface IMerchandise : IDisposable, IEquatable<Merchandise>, IFormattable
    {
        /// <summary>
        /// Наименование товара
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Цена за единицу товара
        /// </summary>
        double Price { get; set; }
        /// <summary>
        /// Количество единиц товара
        /// </summary>
        int Count { get; set; }

        /// <summary>
        /// Общая стоимость данного товара, в зависимости от кол-ва взятых единиц
        /// </summary>
        /// <returns></returns>
        double Cost();
    }
}
