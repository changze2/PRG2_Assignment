using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_ChangZe_Elvis
{
    class PointCard
    {
        public int Points { get; set; }
        public int PunchCard { get; set; }
        public string Tier { get; set; }

        public PointCard() { }
        public PointCard(int points, int punchCard)
        {
            Points = points;
            PunchCard = punchCard;
        }

        public void AddPoints(int add)
        {
            Points += add;
        }
        public void RedeemPoints(int redeem)
        {
            //Order.CalculateTotal() -= (redeem * 0.02);
        }

        public void Punch()
        {

        }

        public override string ToString()
        {
            return $"Tier: {Tier}, Points: {Points}";
        }
    }
}
