using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_ChangZe_Elvis
{
    class Customer
    {
        public string Name { get; set; }
        public int MemberId { get; set; }
        public DateOnly Dob {  get; set; } //I changed the data type of DOB to dateonly as the time is not required for DOB
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory { get; set; } = new List<Order>();
        public PointCard Rewards { get; set; }

        public Customer() { }
        public Customer(string name, int memberId, string dob)
        {
            Name = name;
            MemberId = memberId;
            Dob = DateOnly.FromDateTime(DateTime.ParseExact(dob, "d/M/yyyy", CultureInfo.InvariantCulture));
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
            int userBirthDate = Dob.Day;
            int userBirthMonth = Dob.Month;
            int day = dateTime.Day;
            int month = dateTime.Month;

            if (day == userBirthDate && month == userBirthMonth)
            {
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            return $"Name: {Name}\tID No: {MemberId}\tDOB: {Dob}";
        }
    }
}
