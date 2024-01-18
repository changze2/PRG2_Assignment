using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Assignment1_ChangZe_Elvis
{
    class Cone : IceCream
    {
        public bool Dipped { get; set; }

        public Cone() { }
        public Cone(string option, int scoop, List<Flavour> flavours, List<Topping> toppings, bool dipped)
            : base(option, scoop, flavours, toppings)
        {
            Dipped = dipped;
        }

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
