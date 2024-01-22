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
            if (Dipped)
            {
                return scoopTotal + (premiumFlavours * 2) + Toppings.Count() + 2;
            }
            return scoopTotal + (premiumFlavours * 2) + Toppings.Count();
        }
        public override string ToString()
        {
            string flavoursString = string.Join(", ", Flavours.Select(flavour => flavour.Type));
            string toppingsString = string.Join(", ", Toppings.Select(topping => topping.Type));
            return $"Option: {Option}\tScoops: {Scoop}\tFlavours: {flavoursString}\tToppings: {toppingsString}" +
                $"\tDipped: {Dipped}";
        }
    }
}
