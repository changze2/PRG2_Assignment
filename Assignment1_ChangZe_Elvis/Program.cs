// See https://aka.ms/new-console-template for more information


using Assignment1_ChangZe_Elvis;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static System.Formats.Asn1.AsnWriter;

Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();
Dictionary<int, Order> orderDict = new Dictionary<int, Order>();
Queue <Order> goldOrderQueue = new Queue<Order>();
Queue <Order> orderQueue = new Queue<Order>();
List<string> icecreamOptions = new List<string> { "cup", "cone", "waffle" };
List<string> toppingOptions = new List<string> { "sprinkles", "mochi", "sago", "oreos" };
List<string> flavourOptions = new List<string> { "vanilla", "chocolate", "strawberry", "durian", "ube", "sea salt" };
List<string> waffleOptions = new List<string> { "red velvel", "charcoal", "pandan" };

InitCustomers();
InitOrders();

Console.WriteLine(
    " _____ _____ _______             _" +
    "\n|_   _/ ____|__   __|           | |" +
    "\n  | || |       | |_ __ ___  __ _| |_ ___" +
    "\n  | || |       | | '__/ _ \\/ _` | __/ __|" +
    "\n _| || |____   | | | |  __/ (_| | |_\\__ \\" +
    "\n|_____\\_____|  |_|_|  \\___|\\__,_|\\__|___/\n");
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
                CreateCustomerOrder();
                break;
            case 5:
                Console.WriteLine("To be implemented.");
                //DisplayCustomerOrders();
                break;
            case 6:
                Console.WriteLine("To be implemented.");
                //ModifyOrderDetails();
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
                Order order = new Order(orderId, timeReceived, memberId);
                order.TimeFulfilled = timeFulfilled;
                order.IceCreamList.Add(icecream);
                orderDict[orderId] = order;
                customerDict[memberId].OrderHistory.Add(order);
                //OrderQueue(customerDict[memberId], order);
            }
            else
            {
                orderDict[orderId].TimeFulfilled = timeFulfilled;
                orderDict[orderId].IceCreamList.Add(icecream);
            }
        }
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
        "\n[5] Display order details" +
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
    }
    Console.WriteLine("--------------------------------------------------------------------------------");
}

//Option 2 - List all current orders
void DisplayCurrentOrders()
{
    if (goldOrderQueue.Count == 0 && orderQueue.Count == 0)
    {
        Console.WriteLine("No orders has been made.");
        return;
    }
    Console.WriteLine("Gold Order Queue" +
        "\n------------------");
    if (goldOrderQueue.Count == 0)
    {
        Console.WriteLine("No orders.");
    }
    else
    {
        foreach (Order order in goldOrderQueue)
        {
            Console.WriteLine(order.ToString());
            foreach (IceCream icecream in order.IceCreamList)
            {
                Console.WriteLine(icecream.ToString());
            }
            Console.WriteLine();
        }
    }

    Console.WriteLine("\nNormal Order Queue" +
        "\n------------------");
    if (orderQueue.Count == 0)
    {
        Console.WriteLine("No orders.");
    }
    else
    {
        foreach (Order order in orderQueue)
        {
            Console.WriteLine(order.ToString());
            foreach (IceCream icecream in order.IceCreamList)
            {
                Console.WriteLine(icecream.ToString());
            }
            Console.WriteLine();
        }
    }
    return;
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
    try
    {
        int id = Convert.ToInt32(Console.ReadLine());
        Customer customer = customerDict[id];
        int orderId = orderDict.Count + 1;
        while (true)
        {
            Order order = new Order(orderId, DateTime.Now, id);
            Console.Write("Enter your option of icecream (Cup, Cone, Waffle): ");
            string option = Console.ReadLine().Trim().ToLower();
            if (!icecreamOptions.Contains(option))
            {
                throw new ArgumentException("Please enter a valid icecream option.");
            }
            Console.Write("Enter number of scoops (1-3): ");
            if (!int.TryParse(Console.ReadLine(),out int scoops))
            {
                throw new ArgumentException("Please enter a valid number.");
            }
            else if (scoops < 1 || scoops > 3)
            {
                throw new ArgumentException("Please only enter 1-3 scoops.");
            }
            IceCream icecream = null;
            List<Flavour> flavourList = new List<Flavour>();
            List<Topping> toppingsList = new List<Topping>();
            Console.WriteLine();
            DisplayFlavours();
            for (int i = 0; i != scoops; i++)
            {
                Console.WriteLine($"\nScoop {i + 1}");
                Console.Write("Enter flavour of icecream: ");
                string flavour = Console.ReadLine();
                if (!flavourOptions.Contains(flavour))
                {
                    throw new ArgumentException("Please enter a valid flavour.");
                }
                flavourList.Add(FlavourPremiumCheck(flavour));
                if (option == "waffle")
                {
                    DisplayWaffleFlavours();
                    Console.Write("Enter waffle flavour: ");
                    string waffleFlavour = Console.ReadLine().Trim().ToLower();
                    if (!waffleOptions.Contains(waffleFlavour))
                    {
                        throw new ArgumentException("Please enter a valid waffle flavour.");
                    }
                    icecream = new Waffle(CapitaliseStr(option), scoops, flavourList, toppingsList,
                        CapitaliseStr(waffleFlavour));
                }
                else if (option == "cone")
                {
                    Console.Write("Would you like to change the cone to chocolate-dipped? [Y/N]: ");
                    string dippedInput = Console.ReadLine().Trim().ToLower();
                    if (dippedInput != "y" || dippedInput != "n")
                    {
                        throw new ArgumentException("Please enter Y or N.");
                    }
                    bool dipped = false;
                    if (dippedInput == "y")
                    {
                        dipped = true;
                    }
                    icecream = new Cone(option, scoops, flavourList, toppingsList, dipped);
                }
                else if (option == "cup")
                {
                    icecream = new Cup(option, scoops, flavourList, toppingsList);
                }
                Console.WriteLine();
            }
            DisplayToppings();
            Console.WriteLine("You can enter a maximum of 4 toppings.");
            for (int i = 0; i < 4; i++)
            {
                Console.Write($"Enter topping {i+1} (enter \"None\" for no toppings): ");
                string toppings = Console.ReadLine().Trim().ToLower();
                if (!toppingOptions.Contains(toppings))
                {
                    throw new ArgumentException("Please enter a valid topping.");
                }
                else if (toppings == "none")
                {
                    break;
                }
                else
                {
                    icecream.Toppings.Add(new Topping(CapitaliseStr(toppings)));
                }
            }
            order.AddIceCream(icecream);
            Console.Write("\nWould you like to add another icecream? [Y/N]: ");
            string repeat = Console.ReadLine().Trim().ToLower();
            if (repeat != "y" || repeat != "n")
            {
                throw new ArgumentException("Please enter Y or N.");
            }
            if (repeat == "n")
            {
                customer.CurrentOrder = order;
                orderDict[orderId] = order;
                OrderQueue(customer, order);
                break;
            }
        }
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("An unexpected error while making order.");
    }
}

//Option 5 - Display order details of a customer.
void DisplayCustomerOrders()
{
    DisplayCustomerInfo();
    Console.WriteLine();
    Console.Write("Please input customer id (e.g 123456) to select the customer: ");
    int selected_id = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine();
    foreach (Order orders in customerDict[selected_id].OrderHistory)
    {
        Console.WriteLine(orders.ToString());
        foreach (IceCream iceCream in orders.IceCreamList)
        {
            Console.WriteLine(iceCream.ToString());
        }
        Console.WriteLine();
    }
}

//Option 6 - Modify order details
void ModifyOrderDetails()
{
    DisplayCustomerInfo();
    Console.WriteLine();
    Console.Write("Please input customer id (e.g 123456) to select the customer: ");
    int selected_id = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine();
    /*Console.WriteLine(customerDict[selected_id].CurrentOrder.ToString());
    foreach (IceCream iceCream in customerDict[selected_id].CurrentOrder)
    {
        Console.WriteLine(iceCream.ToString());
    }
    */
    Console.WriteLine();
    Console.WriteLine(
        "\n==============================" +
        "\n            Option" +
        "\n==============================" +
        "\n[1] Choose an existing ice cream object to modify" +
        "\n[2] Add an entirely new ice cream object to the order" +
        "\n[3] Choose an existing ice cream object to delete from the order" +
        "\n==============================");
    Console.Write("Please input your option: ");
    int option = Convert.ToInt32(Console.ReadLine());
    if (option == 1)
    {
        Console.WriteLine("Please select which ice cream to modify: ");
        Console.WriteLine("");
    }
    else if (option == 2)
    {

    }
    else if (option == 3)
    {

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
    return;
}

//New method for appending customer information into csv file
void AppendOrderToCsvFile(Order order)
{
    string relativePath = @"..\..\..\orders.csv";
    string filePath = Path.GetFullPath(relativePath, Directory.GetCurrentDirectory());
    foreach (IceCream icecream in order.IceCreamList)
    {
        string flavoursString = "";
        string toppingsString = "";
        for (int i = 0;i < 3; i++)
        {
            try { flavoursString += $"{icecream.Flavours[i].Type},"; }
            catch { flavoursString += ","; }
        }
        for (int i = 0; i < 4; i++)
        {
            try { toppingsString += $"{icecream.Toppings[i].Type},"; }
            catch { toppingsString += ","; }
            
        }
        string dipped = "";
        string waffleFlavour = "";
        if (icecream.Option == "Cone")
        {
            Cone cone = (Cone)icecream;
            dipped = cone.Dipped.ToString().ToUpper();
        }
        if (icecream.Option == "Waffle")
        {
            Waffle waffle = (Waffle)icecream;
            waffleFlavour = waffle.WaffleFlavour;
        }
        string csvLine = $"{order.Id},{order.MemberId},{order.TimeReceived.ToString("dd/MM/yyyy HH:mm")}," +
            $"{order.TimeFulfilled},{icecream.Option},{icecream.Scoop},{dipped},{waffleFlavour}," +
            $"{flavoursString.Trim(',')},{toppingsString.Trim(',')}";
        File.AppendAllLines(filePath, new[] { csvLine });
    }
    return;
}

Flavour FlavourPremiumCheck(string flavourOrg)
{
    string flavour = flavourOrg.ToLower();
    if (flavour == "vanilla" || flavour == "chocolate" || flavour == "strawberry")
    {
        return new Flavour(CapitaliseStr(flavourOrg), false, 1);
    }
    else if (flavour == "durian" || flavour == "ube" || flavour == "sea salt")
    {
        return new Flavour(CapitaliseStr(flavourOrg), true, 1);
    }
    return null;
}

void DisplayFlavours()
{
    Console.WriteLine(
        "====================" +
        "\n      Flavours" +
        "\n====================");
    foreach (string flavour in flavourOptions)
    {
        if (flavourOptions.IndexOf(flavour) < 3)
        {
            Console.WriteLine($"{CapitaliseStr(flavour),-10} |");
            continue;
        }
        Console.WriteLine($"{CapitaliseStr(flavour),-10} | +$2.00");
    }
    return;
}

void DisplayToppings()
{
    Console.WriteLine(
        "====================" +
        "\n      Toppings" +
        "\n====================");
    foreach (string topping in toppingOptions)
    {
        Console.WriteLine($"{CapitaliseStr(topping),-10} | +$1.00");
    }
    return;
}

void DisplayWaffleFlavours()
{
    Console.WriteLine(
        "====================" +
        "\n  Waffle Flavours" +
        "\n====================");
    foreach (string waffleFlavour in waffleOptions)
    {
        Console.WriteLine($"{CapitaliseStr(waffleFlavour),-10} | +$3.00");
    }
    return;
}

void OrderQueue(Customer customer, Order order)
{
    if (customer.Rewards.Tier == "Gold")
    {
        goldOrderQueue.Enqueue(order);
        return;
    }
    orderQueue.Enqueue(order);
    return;
}

string CapitaliseStr(string str)
{
    string str1 = char.ToUpper(str[0]) + str.Substring(1);
    return str1;
}