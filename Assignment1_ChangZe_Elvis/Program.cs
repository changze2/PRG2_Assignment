// See https://aka.ms/new-console-template for more information


using Assignment1_ChangZe_Elvis;
using System.Globalization;

Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();
Queue <Order> goldOrderQueue = new Queue<Order>();
Queue <Order> orderQueue = new Queue<Order>();

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
        customer.Rewards = new PointCard { Tier = line[3], Points = Convert.ToInt32(line[4]) };
        customer.Rewards.PunchCard = Convert.ToInt16(line[5]);
        customerDict.Add(id, customer);
    }
}

void Menu()
{
    Console.WriteLine(
        "==============================" +
        "\n            Menu" +
        "\n==============================" +
        "\n[1] List all customers" +
        "\n[2] List all orders" +
        "\n[3] Register new customer" +
        "\n[4] Create order" +
        "\n[5] Display order detaiils" +
        "\n[6] Modify order" +
        "\n[0] Exit program" +
        "\n------------------------------");
}
void CustomerInfo()
{
    foreach (Customer customer in customerDict.Values)
    {
        Console.WriteLine(customer.ToString()+"\t"+customer.Rewards.ToString());
    }
}

void DisplayCurrentOrders()
{
    if (orderQueue.Count == 0 || goldOrderQueue.Count == 0)
    {
        Console.WriteLine("No orders.");
        return;
    }
    Console.WriteLine("Gold Order Queue" +
        "\n------------------");
    foreach (Order order in goldOrderQueue)
    {
        Console.WriteLine(order.ToString());
    }
    Console.WriteLine("\nNormal Order Queue" +
        "\n------------------");
    foreach(Order order in orderQueue)
    {
        Console.WriteLine(order.ToString());
    }
}
/*Customer customer1 = new Customer("Amelia", 666888, "01/01/1998");
customer1.Rewards = new PointCard { Tier = "Gold", Points = 150 };
customer1.CurrentOrder = new Order(1, DateTime.Now);

Customer customer2 = new Customer("Bob", 888666, "01/02/2000");
customer2.Rewards = new PointCard { Tier = "Ordinary", Points = 5 };
customer2.CurrentOrder = new Order(2, DateTime.Now);

Customer customer3 = new Customer("Cody", 898989, "02/02/2001");
customer3.Rewards = new PointCard { Tier = "Silver", Points = 65 };

DisplayCurrentOrders(customer1);
DisplayCurrentOrders(customer2);
DisplayCurrentOrders(customer3);*/

void DisplayCustomerOrders(Customer customer)
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
void RegisterCustomer()
{
    try
    {
        Console.Write("Enter customer name: ");
        string name = Console.ReadLine().Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty or whitespace.");
        }
        //This line of code will trigger if the name is null or whitespace
        Console.Write("Enter customer id number (e.g 650992): ");
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
        {
            throw new ArgumentException("Invalid customer ID. Please follow the examples (e.g 650992).");
        }
        //This line of code reads a customer ID from the console, attempts to parse it into an integer using `int.TryParse`,
        //and checks if the parsing fails or if the parsed integer is not a positive value. If either condition is true,
        //it throws an `ArgumentException` with a message indicating that the customer ID is invalid and
        //provides an example for the correct format (e.g., "650992").
        Console.Write("Enter date of birth (DD/MM/YYYY): ");
        string dobString = Console.ReadLine();
        if (!DateTime.TryParseExact(dobString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
        {
            throw new ArgumentException("Invalid date of birth format. Please use DD/MM/YYYY.");
        }
        //This condition checks if the DateTime.TryParseExact method fails to parse the dobString using the specified format "dd/MM/yyyy."
        //If the parsing fails, the method returns false, and the ! operator negates it, making the condition true.
        //In this case, if the date of birth format is invalid, the block inside the if statement is executed, and an ArgumentException is thrown.

        Customer newCustomer = new Customer(name, id, dob.ToString("dd/MM/yyyy"));
        PointCard newPointCard = new PointCard(0, 0);
        newPointCard.Tier = "Ordinary";
        newCustomer.Rewards = newPointCard;
        Console.WriteLine("Registration Successful!");
        customerDict.Add(id, newCustomer);
        AppendToCsvFile(newCustomer);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Invalid input: {ex.Message}");
    }
    catch (Exception)
    {
        Console.WriteLine("An unexpected error occurred during registration.");
    }
}

//ELVIS: I CREATED ANOTHER METHOD FOR APPENDING NEW CUSTOMER INFORMATION
//       INTO THE CSV FILE
void AppendToCsvFile(Customer customer)
{
    string relativePath = @"..\..\..\customers.csv";
    string filePath = Path.GetFullPath(relativePath, Directory.GetCurrentDirectory());
    string csvLine = $"{customer.Name},{customer.MemberId},{customer.Dob},{customer.Rewards.Tier}," +
        $"{customer.Rewards.Points},{customer.Rewards.PunchCard}";
    File.AppendAllLines(filePath, new[] { csvLine });
}

while (true)
{
    Menu();
/*    try
    {*/
        Console.Write("Enter option: ");
        int option = Convert.ToInt16(Console.ReadLine());

        if (option < 0 || option > 6)
        {
            throw new Exception();
        }
        if (option == 0)
        {
            Console.WriteLine("Program ended.");
            break;
        }
        Console.WriteLine();
        switch (option)
        {
            case 1:
                CustomerInfo();
                break;
            case 2:
                DisplayCurrentOrders();
                break;
            case 3:
                RegisterCustomer();
                break;
        }
        Console.WriteLine();
    //}
/*    catch
    {
        Console.WriteLine("Invalid option entered. Please re-enter.\n");
    }*/
}
