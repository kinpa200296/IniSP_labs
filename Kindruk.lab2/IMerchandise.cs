using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kindruk.lab2
{
    interface IMerchandise
    {
        string Name { get; set; }
        double Price { get; set; }
        int Count { get; set; }
    }
}
