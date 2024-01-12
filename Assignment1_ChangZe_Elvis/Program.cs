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