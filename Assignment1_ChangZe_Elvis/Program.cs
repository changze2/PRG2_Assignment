// See https://aka.ms/new-console-template for more information


using Assignment1_ChangZe_Elvis;

Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();

using (StreamReader sr = new StreamReader("customers.csv"))
{
    string? s = sr.ReadLine();
    if (s != null) { }
    while ((s = sr.ReadLine()) != null)
    {
        string[] line = s.Split(',');
        string name = line[0];
        int id = Convert.ToInt32(line[1]);
        string dob = line[2];
        Customer customer = new Customer(name, id, dob);
        customerDict.Add(id, customer);
    }
}

void CustomerInfo()
{
    foreach (Customer customer in customerDict.Values)
    {
        Console.WriteLine(customer.ToString());
    }
}

CustomerInfo();
//Console.Write("Enter option: ");
//string option = Console.ReadLine();
//Console.Write("Enter scoops: ");
//int scoops = Convert.ToInt32(Console.ReadLine());

//IceCream icecream = new Cup(option, scoops);
//Console.WriteLine(icecream.ToString());
void CurrentOrders()
{
    Customer customer1 = new Customer("Amelia", 666888, "01/01/1998");
    customer1.Rewards = new PointCard { Tier = "Gold", Points = 150 };
    customer1.CurrentOrder = new Order(1, DateTime.Now);

    Customer customer2 = new Customer("Bob", 888666, "01/02/2000");
    customer2.Rewards = new PointCard { Tier = "Ordinary", Points = 5 };
    customer2.CurrentOrder = new Order(2, DateTime.Now);

    Customer customer3 = new Customer("Cody", 898989, "02/02/2001");
    customer3.Rewards = new PointCard { Tier = "Silver", Points = 65 };

    DisplayCurrentOrders(customer1);
    DisplayCurrentOrders(customer2);
    DisplayCurrentOrders(customer3);
}

void DisplayCurrentOrders(Customer customer)
{
    Console.WriteLine($"Customer: {customer.Name}\t Tier: {customer.Rewards.Tier}");

    if (customer.CurrentOrder != null)
    {
        Console.WriteLine($"Order ID: {customer.CurrentOrder.Id}\t Order Date: {customer.CurrentOrder.TimeReceived.ToString("dd/MM/yyyy HH:mm:ss")}");
    }
    else
    {
        Console.WriteLine("No current order");
    }
}
CurrentOrders();
void RegisterCustomer()
{
    Console.Write("Enter customer name: ");
    string name = Console.ReadLine();
    Console.Write("Enter customer id number (e.g 650992): ");
    int id  = Convert.ToInt32(Console.ReadLine());
    Console.Write("Enter date of birth (DD-MM-YYYY): ");
    string dobString = Console.ReadLine();
    Customer newCustomer = new Customer(name, id, dobString);
    PointCard newPointCard = new PointCard();
    newCustomer.Rewards = newPointCard;
    Console.WriteLine("Registration Successful!");
    AppendToCsvFile(newCustomer);

}
//ELVIS: I CREATED ANOTHER METHOD FOR APPENDING NEW CUSTOMER INFORMATION
//       INTO THE CSV FILE
void AppendToCsvFile(Customer customer)
{
    string filePath = "customers.csv";
    string csvLine = $"{customer.Name},{customer.MemberId},{customer.Dob}";
    File.AppendAllLines(filePath, new[] { csvLine });
}

RegisterCustomer();