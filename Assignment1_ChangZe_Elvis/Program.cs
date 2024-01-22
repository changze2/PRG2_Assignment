// See https://aka.ms/new-console-template for more information


using Assignment1_ChangZe_Elvis;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static System.Formats.Asn1.AsnWriter;

Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();
Dictionary<int, Order> orderDict = new Dictionary<int, Order>();
Queue <Order> goldOrderQueue = new Queue<Order>();
Queue <Order> orderQueue = new Queue<Order>();

InitCustomers();
InitOrders();

while (true)
{
    Menu();
    try
    {
        Console.Write("Enter option: ");
        int option = Convert.ToInt16(Console.ReadLine().Trim());

        if (option < 0 || option > 6)
        {
            throw new ArgumentException();
        }
        if (option == 0)
        {
            Console.WriteLine("Program ended.");
            break;
        }
        Console.WriteLine();

        //We chose to use switch case statements as it runs faster than using if else statements. if else statements
        //run each if statementsin order while switch case statements run the case where the condition
        //is met, such as input = 1 and so on.
        switch (option)
        {
            case 1:
                DisplayCustomerInfo();
                Console.WriteLine($"There are {customerDict.Count} customers.");
                break;
            case 2:
                DisplayCurrentOrders();
                break;
            case 3:
                RegisterCustomer();
                break;
            case 4:
                Console.WriteLine("To be implemented soon.");
                break;
            case 5:
                Console.WriteLine("To be implemented soon.");
                break;
            case 6:
                Console.WriteLine("To be implemented soon.");
                break;
        }
        Console.WriteLine();
    }
    catch (ArgumentException)
    {
        Console.WriteLine("\nPlease enter an option between 1-6.\n");
    }
    catch
    {
        Console.WriteLine("\nInvalid option entered. Please enter a valid option.\n");
    }

}

void InitCustomers()
{
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
            customer.Rewards = new PointCard(Convert.ToInt32(line[4]), Convert.ToInt16(line[5]));
            customer.Rewards.Tier = line[3];
            customerDict.Add(id, customer);
        }
    }
}

void InitOrders()
{
    Dictionary<int, int> orderByMember = new Dictionary<int, int>();
    using (StreamReader sr = new StreamReader("orders.csv"))
    {
        string? s = sr.ReadLine();
        if (s != null) { }
        while ((s = sr.ReadLine()) != null)
        {
            string[] line = s.Split(',');
            int orderId = Convert.ToInt16(line[0]);
            int memberId = Convert.ToInt32(line[1]);
            DateTime timeReceived = Convert.ToDateTime(line[2]);
            DateTime timeFulfilled = Convert.ToDateTime(line[3]);
            string option = line[4];
            int scoops = Convert.ToInt16(line[5]);
            bool dipped = false;
            if (line[6] != "") { dipped = Convert.ToBoolean(line[6]); }
            string waffleFlavour = line[7];
            List<Flavour> flavourList = new List<Flavour>();
            List<Topping> toppingsList = new List<Topping>();
            List<string> flavourStrings = new List<string> { line[8], line[9], line[10] };
            List<string> toppingsStrings = new List<string> { line[11], line[12], line[13], line[14] };
            foreach (string flavour in flavourStrings)
            {
                if (flavour == "") { continue; }
                flavourList.Add(FlavourPremiumCheck(flavour));
            }
            foreach (string topping in toppingsStrings)
            {
                if (topping == "") { continue; }
                toppingsList.Add(new Topping(topping));
            }
            IceCream icecream = null;
            if (option == "Waffle")
            {
                icecream = new Waffle(option, scoops, flavourList, toppingsList, waffleFlavour);
            }
            else if (option == "Cone")
            {
                icecream = new Cone(option, scoops, flavourList, toppingsList, dipped);
            }
            else
            {
                icecream = new Cup(option, scoops, flavourList, toppingsList);
            }
            if (orderDict.ContainsKey(orderId) == false)
            {
                Order order = new Order(orderId, timeReceived);
                order.TimeFulfilled = timeFulfilled;
                order.IceCreamList.Add(icecream);
                orderDict[orderId] = order;
                orderByMember[memberId] = orderId;
                if (customerDict[memberId].Rewards.Tier == "Gold")
                {
                    goldOrderQueue.Enqueue(order);
                }
                else
                {
                    orderQueue.Enqueue(order);
                }
            }
            else
            {
                orderDict[orderId].TimeFulfilled = timeFulfilled;
                orderDict[orderId].IceCreamList.Add(icecream);
            }
        }
    }
    foreach (int memberId in orderByMember.Keys)
    {
        customerDict[memberId].OrderHistory.Add(orderDict[orderByMember[memberId]]);
    }
}

Flavour FlavourPremiumCheck(string flavourOrg)
{
    string flavour = flavourOrg.ToLower();
    if (flavour == "vanilla" || flavour == "chocolate" || flavour == "strawberry")
    {
        return new Flavour(flavourOrg, false, 1);
    }
    else if (flavour == "durian" || flavour == "ube" || flavour == "sea salt")
    {
        return new Flavour(flavourOrg, true, 1);
    }
    return null;
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

//Option 1 - List all customers
void DisplayCustomerInfo()
{
    Console.WriteLine("================================================================================" +
        "\n                                   Customers" +
        "\n================================================================================" +
        $"\n{"Name",-13} | {"ID",-8} | {"DOB",-11} | {"Tier",-10} | {"Points",-6} | {"Punch Card",-5}" +
        "\n--------------------------------------------------------------------------------");
    foreach (Customer customer in customerDict.Values)
    {
        Console.WriteLine($"{customer.Name,-13} | {customer.MemberId,-8} | {customer.Dob,-11} | {customer.Rewards.Tier,-10} | " +
            $"{customer.Rewards.Points,-6} | {customer.Rewards.PunchCard,-5}");
        //Console.WriteLine(customer.ToString()+"\t"+customer.Rewards.ToString());
    }
}

//Option 2 - List all current orders
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
        foreach (IceCream icecream in order.IceCreamList)
        {
            Console.WriteLine(icecream.ToString());
        }
    }
    Console.WriteLine("\nNormal Order Queue" +
        "\n------------------");
    foreach (Order order in orderQueue)
    {
        Console.WriteLine(order.ToString());
        foreach (IceCream icecream in order.IceCreamList)
        {
            Console.WriteLine(icecream.ToString());
        }
    }
}

//Option 3 - Register a new customer
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

        Console.Write("Enter customer id number (e.g 123456): ");
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0 || id.ToString().Length != 6 )
        {
            throw new ArgumentException("Invalid customer ID. Please follow the examples (e.g 123456).");
        }
        else if (customerDict.ContainsKey(id))
        {
            throw new ArgumentException($"Sorry, {id} has already been registered to another customer.");
        }
        //This line of code reads a customer ID from the console, attempts to parse it into an integer using `int.TryParse`,
        //and checks if the parsing fails or if the parsed integer is not a positive value. If either condition is true,
        //it throws an `ArgumentException` with a message indicating that the customer ID is invalid and
        //provides an example for the correct format (e.g., "123456").

        Console.Write("Enter date of birth (DD-MM-YYYY): ");
        string dobString = Console.ReadLine();
        if (!DateTime.TryParseExact(dobString, "d-M-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
        {
            throw new ArgumentException("Invalid date of birth format. Please use DD-MM-YYYY.");
        }
        //This condition checks if the DateTime.TryParseExact method fails to parse the dobString using the specified format "dd/MM/yyyy."
        //If the parsing fails, the method returns false, and the ! operator negates it, making the condition true.
        //In this case, if the date of birth format is invalid, the block inside the if statement is executed, and an ArgumentException is thrown.

        Customer newCustomer = new Customer(name, id, dob.ToString());
        PointCard newPointCard = new PointCard(0, 0);
        newPointCard.Tier = "Ordinary";
        newCustomer.Rewards = newPointCard;
        Console.WriteLine("Registration Successful!");
        customerDict.Add(id, newCustomer);
        AppendCustomerToCsvFile(newCustomer);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception)
    {
        Console.WriteLine("An unexpected error occurred during registration.");
    }
}

//Option 4 - Create customer's order
void CreateCustomerOrder()
{
    DisplayCustomerInfo();
    Console.Write("\nEnter ID of customer: ");
    int id = Convert.ToInt32(Console.ReadLine());
    Customer customer = customerDict[id];
    Order order = new Order();
    Console.Write("Enter your option of icecream (Cup, Cone, Waffle");
    string option = Console.ReadLine().ToLower();
    Console.Write("Enter number of scoops (1-3)");
    int scoops = Convert.ToInt16(Console.ReadLine());
    for (int i = 0; i != scoops; i++)
    {
        Console.WriteLine($"Scoop {i}");
        List<Flavour> flavourList = new List<Flavour>();
        List<Topping> toppingsList = new List<Topping>();
        Console.Write("Enter flavour of icecream: ");
        string flavour = Console.ReadLine();
        Console.Write("Enter toppings for this scoop (enter \"None\" for no toppings): ");
        string toppings = Console.ReadLine().ToLower();
        flavourList.Add(FlavourPremiumCheck(flavour));
        if (toppings != "none")
        {
            toppingsList.Add(new Topping(toppings));
        }
        IceCream icecream = null;
        if (option == "waffle")
        {
            Console.Write("Enter waffle flavour: ");
            string waffleFlavour = Console.ReadLine();
            icecream = new Waffle(option, scoops, flavourList, toppingsList, waffleFlavour);
        }
        else if (option == "cone")
        {
            bool dipped = false;
            icecream = new Cone(option, scoops, flavourList, toppingsList, dipped);
        }
        else if (option == "cup")
        {
            icecream = new Cup(option, scoops, flavourList, toppingsList);
        }
        /*if (orderDict.ContainsKey(orderId) == false)
        {
            Order order = new Order(orderId, timeReceived);
            order.TimeFulfilled = timeFulfilled;
            order.IceCreamList.Add(icecream);
            orderDict[orderId] = order;
            orderByMember[memberId] = orderId;
            if (customerDict[memberId].Rewards.Tier == "Gold")
            {
                goldOrderQueue.Enqueue(order);
            }
            else
            {
                orderQueue.Enqueue(order);
            }
        }
        else
        {
            orderDict[orderId].TimeFulfilled = timeFulfilled;
            orderDict[orderId].IceCreamList.Add(icecream);
        }*/
    }
}

//Option 5 - Display order details of a customer.
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

//New method for appending customer information into csv file
void AppendCustomerToCsvFile(Customer customer)
{
    string relativePath = @"..\..\..\customers.csv";
    string filePath = Path.GetFullPath(relativePath, Directory.GetCurrentDirectory());
    string csvLine = $"{customer.Name},{customer.MemberId},{customer.Dob},{customer.Rewards.Tier}," +
        $"{customer.Rewards.Points},{customer.Rewards.PunchCard}";
    File.AppendAllLines(filePath, new[] { csvLine });
}

