﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_ChangZe_Elvis
{
    class Cup : IceCream
    {
        public Cup() { }
        public Cup(string option, int scoop, List<Flavour> flavours, List<Topping> toppings)
            : base(option, scoop, flavours, toppings) { }

        public override double CalculatePrice()
        {
            if (Scoop == 1)
            {
                return 4.0;
            }
            else if (Scoop == 2)
            {
                return 5.5;
            }
            else if (Scoop == 3)
            {
                return 6.5;
            }
            else
            {
                return 0;
            }
        }
    }
}
