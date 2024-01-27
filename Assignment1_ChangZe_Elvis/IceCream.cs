using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_ChangZe_Elvis
{
    abstract class IceCream
    {
        public string Option { get; set; }
        public int Scoop { get; set; }
        public List<Flavour> Flavours { get; set; } = new List<Flavour>();
        public List<Topping> Toppings { get; set; } = new List<Topping>();

        public IceCream() { }
        public IceCream(string option, int scoop, List<Flavour> flavours, List<Topping> toppings)
        {
            Option = option;
            Scoop = scoop;
            Flavours = flavours;
            Toppings = toppings;
        }

        public abstract double CalculatePrice();

        public override string ToString()
        {
            string toppingsString = "";
            if (Toppings.Count == 0)
            {
                toppingsString = "no toppings";
            }
            else
            {
                toppingsString = $"{Toppings.Count} topping(s) of {toppingsString}" +
                    string.Join(", ", Toppings.Select(topping => topping.Type));
            }
            string flavoursString = "are " + string.Join(", ", Flavours.Select(flavour => flavour.Type));
            
            if (Flavours.Count == 1)
            {
                flavoursString = flavoursString.Replace("are", "is");
            }

            return $"Icecream {Option} with {Scoop} scoop(s). Flavour(s) " +
                $"{flavoursString} with {toppingsString}.";
        }
    }
}
