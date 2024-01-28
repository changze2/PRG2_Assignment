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
            string toppingsString = "0 toppings";
            if (Toppings.Count != 0)
            {
                toppingsString = $"{Toppings.Count} topping(s) of " +
                    string.Join(", ", Toppings.Select(topping => topping.Type));
            }
            string flavoursString = $"{Flavours.Count} flavour(s) of " +
                string.Join(", ", Flavours.Select(flavour => flavour.Type));
            return $"{Option} Icecream with {Scoop} scoop(s)" +
                $"\n{flavoursString}\n{toppingsString}";
        }
    }
}
