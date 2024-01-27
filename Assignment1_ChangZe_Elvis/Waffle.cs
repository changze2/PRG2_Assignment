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
            int premiumFlavours = 0;
            foreach (Flavour flavour in Flavours)
            {
                if (flavour.Premium)
                {
                    premiumFlavours++;
                }
            }
            double scoopTotal = 0;
            if (Scoop == 1)
            {
                scoopTotal = 4.0;
            }
            else if (Scoop == 2)
            {
                scoopTotal = 5.5;
            }
            else if (Scoop == 3)
            {
                scoopTotal = 6.5;
            }
            if (WaffleFlavour != "Original")
            {
                return scoopTotal + (premiumFlavours * 2) + Toppings.Count() + 3;
            }
            return scoopTotal + (premiumFlavours * 2) + Toppings.Count();
        }
        public override string ToString()
        {
            return base.ToString() + $". Waffle flavour is {WaffleFlavour}.";
        }
    }
}
