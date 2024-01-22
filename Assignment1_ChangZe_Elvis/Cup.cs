using System;
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
            return scoopTotal + (premiumFlavours * 2) + Toppings.Count();
        }
        public override string ToString()
        {
            string flavoursString = string.Join(", ", Flavours.Select(flavour => flavour.Type));
            string toppingsString = string.Join(", ", Toppings.Select(topping => topping.Type));
            return $"Option: {Option}\tScoops: {Scoop}\tFlavours: {flavoursString}\tToppings: {toppingsString}";
        }
    }
}
