using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_ChangZe_Elvis
{
    class Customer
    {
        public string Name { get; set; }
        public int Memberid { get; set; }
        public DateTime Dob {  get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory { get; set; } = new List<Order>();
        public PointCard Rewards { get; set; }

        public Customer() { }
        public Customer(string name, int memberid, DateTime dob)
        {
            Name = name;
            Memberid = memberid;
            Dob = dob;
        }

        public Order MakeOrder()
        {
            Console.Write("Enter your option: ");
            string option = Console.ReadLine();
            Console.Write("Enter number of scoops: ");
            Order order = new Order();
            return order;
        } 

        public bool IsBirthday()
        {
            DateTime dateTime = DateTime.Now;
            int userBirthDate = BirthDate;
            int userBirthMonth = BirthMonth;
            int day = dateTime.Day;
            int month = dateTime.Month;

            if (day == BirthDate && month != BirthMonth)
            {
                return true;
            }
            else if (day != BirthDate && month == BirthMonth)
            {
                return false;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Member ID: {Memberid}";
        }
    }
}
