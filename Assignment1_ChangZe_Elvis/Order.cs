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
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFulfilled { get; set; }
        public List<IceCream> IceCreamList { get; set; } = new List<IceCream>();

        public Order() { }
        public Order(int id, DateTime timeReceived)
        {
            Id = id;
            TimeReceived = timeReceived;
        }

        public void ModifyIceCream(int position)
        {

        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }

        public void DeleteIceCream(int position2)
        {
            IceCream DeleteIC = IceCreamList[position2];
            Console.WriteLine($"Ice Cream Information: {DeleteIC.ToString()}\n");
            IceCreamList.Remove(DeleteIC);
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
            if (TimeFulfilled == null)
            {
                return $"Order id: {Id}\tTime Received: {TimeReceived}";
            }
            else
            {
                return $"Order id: {Id}\tTime Received: {TimeReceived}\tTime Fulfilled: {TimeFulfilled}";
            }
        }
    }
}
