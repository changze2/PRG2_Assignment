using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_ChangZe_Elvis
{
    class Waffle : IceCream
    {
        public string WaffleFlavour { get; set; }

        public Waffle() { }
        public Waffle(string option, int scoop, List<Flavour> flavours, List<Topping> toppings, string waffleFlavour)
            : base(option, scoop, flavours, toppings)
        {
            WaffleFlavour = waffleFlavour;
        }
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
