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
            CultureInfo newCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = newCulture;
            //This is to ensure that the timezone is the same regardless of which devices are running the program as some devices
            //may be running on American time which is in the MM/dd/yyyy format, while UK uses dd/MM/yyyy which is what is used
            //in this program
            Dob = DateOnly.FromDateTime(DateTime.Parse(dob));
        }

        public void MakeOrder()
        {
            // Empty as method is being created inside Program.cs and utilises other methods made in Program.cs
            // so making it in Program.cs is more convenient and memory-efficient
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
