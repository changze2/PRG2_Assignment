using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_ChangZe_Elvis
{
    class Order
    {
        public int Id { get; set; }
        public int MemberId { get; set; } // added an extra field for conveniency sake
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFulfilled { get; set; }
        public List<IceCream> IceCreamList { get; set; } = new List<IceCream>();

        public Order() { }
        public Order(int id, DateTime timeReceived, int memberId)
        {
            Id = id;
            TimeReceived = timeReceived;
            MemberId = memberId; // I added an extra field and parameter to the Order to make it more convenient
        }

        public void ModifyIceCream(int position)
        {

        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }

        public void DeleteIceCream(int position)
        {

        }
        public double CalculateTotal()
        {
            double total = 0;
            foreach (IceCream iceCream in IceCreamList)
            {
                total += iceCream.CalculatePrice();
            }
            return total;
        }

        public override string ToString()
        {
            string timeFulfilled = "";
            if (TimeFulfilled == null)
            {
                timeFulfilled = "Awaiting checkout";
            }
            else
            {
                timeFulfilled = TimeFulfilled.ToString();
            }
            return $"Order: {Id}" +
                $"\nTime received: {TimeReceived}" +
                $"\nTime fulfilled: {timeFulfilled}";
        }
    }
}
