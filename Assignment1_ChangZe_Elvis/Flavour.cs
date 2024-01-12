using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_ChangZe_Elvis
{
    class Flavour
    {
        public string Type { get; set; }
        public bool Premium { get; set; }
        public int Quality { get; set; }

        public Flavour() { }
        public Flavour(string type, bool premium, int quality)
        {
            Type = type;
            Premium = premium;
            Quality = quality;
        }

        public override string ToString()
        {
            return $"Type: {Type}\tPremium: {Premium}\tQuality: {Quality}";
        }
    }
}
