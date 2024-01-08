using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_ChangZe_Elvis
{
    class Waffle : IceCream
    {
        public override double CalculatePrice()
        {
            if (Scoop == 1)
            {
                return 7.0;
            }
            else if (Scoop == 2)
            {
                return 8.5;
            }
            else if (Scoop == 3)
            {
                return 9.5;
            }
            else
            {
                return 0;
            }
        }
    }
}
