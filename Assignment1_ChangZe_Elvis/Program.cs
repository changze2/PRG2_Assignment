// See https://aka.ms/new-console-template for more information
using Assignment1_ChangZe_Elvis;
using Microsoft.VisualBasic.FileIO;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

//This is all collection classes to be created
Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>(); 
Dictionary<int, Order> orderDict = new Dictionary<int, Order>();
Queue <Order> goldOrderQueue = new Queue<Order>();
Queue <Order> orderQueue = new Queue<Order>();

// We made lists of all the options so that we can validate the inputs later on
List<string> icecreamOptions = new List<string> { "cup", "cone", "waffle" };
List<string> toppingOptions = new List<string> { "sprinkles", "mochi", "sago", "oreos" };
List<string> flavourOptions = new List<string> { "vanilla", "chocolate", "strawberry", "durian", "ube", "sea salt" };
List<string> waffleOptions = new List<string> { "original", "red velvet", "charcoal", "pandan" };

// Calling the methods outside the main loop to initialise orders and customers
InitCustomers();
InitOrders();

Console.WriteLine(
    " _____ _____ _______             _" +
    "\n|_   _/ ____|__   __|           | |" +
    "\n  | || |       | |_ __ ___  __ _| |_ ___" +
    "\n  | || |       | | '__/ _ \\/ _` | __/ __|" +
    "\n _| || |____   | | | |  __/ (_| | |_\\__ \\" +
    "\n|_____\\_____|  |_|_|  \\___|\\__,_|\\__|___/" +
    "\n\nHi I'm Otto, your Friendly Neighbourhood Robot Scooper! Take a look at our options below!");

// This is where we run our main while loop for our program
while (true)
{
    Menu(); // Menu Method called for each iteration

    try // Using a try catch block to ensure program wont end over unexpected errors
    {
        Console.Write("Enter option: ");

        // Did not do validation for this as if there is any error, it would be catched with an 
        // error message
        int option = Convert.ToInt16(Console.ReadLine().Trim());

        // ArgumentException will be triggered if option is not meet the range 1-8
        if (option < 0 || option > 8) 
        {
            throw new ArgumentException();
        }

        // Ensure that while loop can be left with exit option
        if (option == 0) 
        {
            Console.WriteLine("Program ended.");
            break;
        }
        Console.WriteLine();

        //We chose to use switch case statements as it runs faster than using if else statements.
        //if else statements run each "if" statements in order of implementation, while switch
        //case statements run the case when the condition is met, such as input = 1 and so on.
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
                DisplayCustomerOrders();
                break;
            case 6:
                ModifyOrderDetails();
                break;
            case 7:
                ProcessNCheckOut();
                break;
            case 8:
                DisplayMonthlyAndYearAmount();
                break;
        }
        Console.WriteLine();
    }

    // Catch statements to output appropriate error messages based on what errors occurred.
    catch (ArgumentException)
    {
        Console.WriteLine("\nPlease enter an option between 1-8.\n");
    }
    catch
    {
        Console.WriteLine("\nInvalid option entered. Please enter a valid option.\n");
    }
}

//This method is to read the customers.csv and create the relevant customer objects to add
//to customerDict
void InitCustomers()
{
    // We used StreamReader as it is more efficient since it reads line by line
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine();
        if (s != null) { }
        while ((s = sr.ReadLine()) != null)
        {
            string[] line = s.Split(',');
            string name = line[0];
            int id = Convert.ToInt32(line[1]);

            // Reasoning for using string for dob is in Customer class implementation
            string dob = line[2];
            Customer customer = new Customer(name, id, dob);
            customer.Rewards = new PointCard(Convert.ToInt32(line[4]), Convert.ToInt16(line[5]));
            customer.Rewards.Tier = line[3];
            customerDict.Add(id, customer);

            // Since we can assume that there are no null values in the customer.csv, there is
            // no need to include validation for unexpected values.
        }
    }
}

//This method is to read the orders.csv and create the relevant order objects to add
//to orderDict
void InitOrders()
{
    // We also used StreamReader for initialising orders as it is more efficient since it reads
    // line by line
    using (StreamReader sr = new StreamReader("orders.csv"))
    {
        string? s = sr.ReadLine();
        if (s != null) { }
        while ((s = sr.ReadLine()) != null)
        {
            string[] line = s.Split(',');

            // Creating all the variables that will not have null values
            int orderId = Convert.ToInt16(line[0]);
            int memberId = Convert.ToInt32(line[1]);
            DateTime timeReceived = Convert.ToDateTime(line[2]);

            // All orders in orders.csv will be completed, so timefulfilled will not be null
            DateTime timeFulfilled = Convert.ToDateTime(line[3]);
            string option = line[4];
            int scoops = Convert.ToInt16(line[5]);

            // Initialising the dipped bool first, and creating it properly if it is not null
            bool dipped = false;
            if (line[6] != "") { dipped = Convert.ToBoolean(line[6]); }

            // Initialising the waffle flavour as a string, since if its null, it would not be printed
            string waffleFlavour = line[7];

            // Creating all the collection classes needed in order to create a proper Customer object which 
            // has one to many associations to Flavour and Topping classes
            List<Flavour> flavourList = new List<Flavour>();
            List<Topping> toppingsList = new List<Topping>();

            // We had to create a separate string List in order to iterate through it to properly create 
            // the Flavour and Topping objects
            List<string> flavourStrings = new List<string> { line[8], line[9], line[10] };
            List<string> toppingsStrings = new List<string> { line[11], line[12], line[13], line[14] };

            // Using foreach loops to check if the string is null, if it is not null, then the objects can
            // be created and added into the proper object Lists
            foreach (string flavour in flavourStrings)
            {
                if (flavour == "") { continue; } // Using continue to reiterate if null

                // We created a new method to properly create the flavour by checking if it is premium
                flavourList.Add(FlavourPremiumCheck(flavour));
            }
            foreach (string topping in toppingsStrings)
            {
                if (topping == "") { continue; }
                toppingsList.Add(new Topping(topping));
            }

            // Initialising the icecream object first, since one line only contains information for
            // one icecream object
            IceCream icecream = null;

            // Using if statements to check which icecream object to create.
            if (option == "Waffle")
            {
                icecream = new Waffle(option, scoops, flavourList, toppingsList, waffleFlavour);
            }
            else if (option == "Cone")
            {
                icecream = new Cone(option, scoops, flavourList, toppingsList, dipped);
            }
            else // This is for the Cup object
            {
                icecream = new Cup(option, scoops, flavourList, toppingsList);
            }

            // This is to check if it belongs to an already created object, for example a second icecream
            // that is in the order
            // The orderDict was already created outside of the main loop
            // Using ! before the bool statement (orderDict.ContainsKey(orderId)) means it this block 
            // will run if the bool statement is false
            if (!orderDict.ContainsKey(orderId))
            {
                Order order = new Order(orderId, timeReceived, memberId);
                order.TimeFulfilled = timeFulfilled;
                order.IceCreamList.Add(icecream);
                orderDict[orderId] = order;
                customerDict[memberId].OrderHistory.Add(order);
            }

            // If the icecream object is meant to be added to an exisiting order, so a new order need not
            // be created
            else
            {
                orderDict[orderId].TimeFulfilled = timeFulfilled;
                orderDict[orderId].IceCreamList.Add(icecream);
            }
        }
    }
}

//This is the Menu method that prints all the options that can be entered.
void Menu()
{
    Console.WriteLine(
        "====================================" +
        "\n               Menu" +
        "\n====================================" +
        "\n[1] List all customers" +
        "\n[2] List all orders" +
        "\n[3] Register new customer" +
        "\n[4] Create order" +
        "\n[5] Display order details" +
        "\n[6] Modify order" +
        "\n[7] Checkout order" +
        "\n[8] Display monthly charge per year" +
        "\n[0] Exit program" +
        "\n------------------------------------");
}

// Option 1 - List all customers
// No try catch block is needed since it does not require user input
void DisplayCustomerInfo()
{
    Console.WriteLine("================================================================================" +
        $"\n|{"Customers",39}{" ",39}|" +
        "\n================================================================================" +
        $"\n|{"Name",-13} | {"ID",-8} | {"DOB",-11} | {"Tier",-10} | {"Points",-6} | {"Punch Card",-15}|" +
        "\n--------------------------------------------------------------------------------");

    // Using a foreach loop to loop through the customerDict collection class so that the objects can be 
    // accessed without indexing
    foreach (Customer customer in customerDict.Values)
    {
        Console.WriteLine($"|{customer.Name,-13} | {customer.MemberId,-8} | {customer.Dob,-11} | {customer.Rewards.Tier,-10} | " +
            $"{customer.Rewards.Points,-6} | {customer.Rewards.PunchCard,-15}|");
    }
    Console.WriteLine("--------------------------------------------------------------------------------");
    return;
}

// Option 2 - List all current orders
// No try catch block since it does not require user input
void DisplayCurrentOrders()
{
    // This is to check if both queues are empty. If they are, then a message would be printed saying
    // there is no orders, and it would end the method's runtime
    if (goldOrderQueue.Count == 0 && orderQueue.Count == 0)
    {
        Console.WriteLine("No orders have been made.");
        return;
    }

    // Printing the gold order first
    Console.WriteLine("Gold Order Queue" +
        "\n===================");

    // If the gold order is empty, then it would show no orders and move on to normaml queue
    if (goldOrderQueue.Count == 0)
    {
        Console.WriteLine("No orders.");
    }
    else
    {
        // Using a foreach loop to loop through the order queue to display the orders using a method created
        // that helps display the order in proper format
        foreach (Order order in goldOrderQueue)
        {
            DisplayOrder(order);
        }
    }

    // Same thing with the gold order queue
    Console.WriteLine("\nNormal Order Queue" +
        "\n===================");
    if (orderQueue.Count == 0)
    {
        Console.WriteLine("No orders.");
    }
    else
    {
        foreach (Order order in orderQueue)
        {
            DisplayOrder(order);
        }
    }
    return; 
}
    

// Option 3 - Register a new customer
void RegisterCustomer()
{
    // Using a try catch block to catch any errors that may occur from user input
    try
    {
        Console.Write("Enter your name: ");
        // using .Trim() to remove any trailing whitespaces and lower to standardise all names
        string name = Console.ReadLine().Trim().ToLower(); 

        // This block of code will trigger if the name is null or whitespace
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("\nName cannot be empty or whitespace.");
        }

        // This block of code will trigger if the name is not made of all alphabets
        // the .All(char.IsLetter) function returns a bool if it follows the conditions
        // inside the .All() function
        else if (!name.All(c => char.IsLetter(c) || c == ' '))
        {
            throw new ArgumentException("\nName can only contain alphabets.");
        }

        // This is to ensure that the outputs are properly formatted.
        else if (name.Length > 13)
        {
            throw new ArgumentException("\nSorry, please enter a shorter name.");
        }

        Console.Write("Enter customer id number (e.g 123456): ");

        // Using int.TryParse() to try and convert the user input into an int. An ArgumentException
        // would occur if the conversion does not work, or if the id is a negative number or not a 6
        // digit number. It would also reject inputs where starting digit is 0.
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0 || id.ToString().Length != 6 )
        {
            throw new ArgumentException("\nInvalid customer ID. Please follow the format (e.g 123456).");
        }

        // This to check if the id is already taken by another customer, which prevents duplicates
        else if (customerDict.ContainsKey(id))
        {
            throw new ArgumentException($"\nSorry, {id} has already been registered to another customer.");
        }

        Console.Write("Enter date of birth (DD-MM-YYYY): ");
        string dobString = Console.ReadLine();

        // Similar to TryParse, we used TryParseExact to ensure that the string entered follows
        // the exact format of dd-mm-yyyy, however we used d-M-yyyy as it also allows inputs where
        // the day and month can be entered as single digits.
        // CultureInfo.InvariantCulture and DateTimeStyles.None is used to the format is not affected by the
        // user's computer settings. DateTimeStyles is used to set the timezone, there it is best to put as None
        if (!DateTime.TryParseExact(dobString, "d-M-yyyy", CultureInfo.InvariantCulture, 
            DateTimeStyles.None, out DateTime dob))
        {
            throw new ArgumentException("\nInvalid date of birth format. Please use DD-MM-YYYY.");
        }

        // Funny little output if dob entered exceeds current date
        if (dob > DateTime.Now)
        {
            throw new ArgumentException("\nWow, are you born in the future?");
        }

        // Creating the Customer object as well as the Rewards object since its associated
        // to Customer
        Customer newCustomer = new Customer(CapitaliseStr(name), id, dob.ToString());

        // All new customers start with 0 points and 0 punch-card with Ordinary tier
        PointCard newPointCard = new PointCard(0, 0);
        newPointCard.Tier = "Ordinary";
        newCustomer.Rewards = newPointCard;
        Console.WriteLine("Registration Successful!");

        // Adding the new customer object into the customerDict collection class, as well as
        // updating the customers.csv file with another method
        customerDict.Add(id, newCustomer);
        AppendCustomerToCsvFile(newCustomer);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine("Registration failed!");
    }
    catch (Exception)
    {
        Console.WriteLine("An unexpected error occurred during registration.");
    }
}

// Option 4 - Create customer's order
void CreateCustomerOrder()
{
    DisplayCustomerInfo();
    Console.Write("\nEnter ID of customer: ");

    // Using a try catch block to catch any errors that may occur from user input
    try
    {
        // Using TryParse to try to convert user input into an int, and that the id belongs to a
        // registered customer
        if (!int.TryParse(Console.ReadLine(), out int id) || !customerDict.ContainsKey(id))
        {
            throw new ArgumentException("\nPlease enter a valid customer id.");
        }
        Customer customer = customerDict[id];

        // This is to ensure that the customer cannot make multiple orders without checking the previous
        // one first
        if (customer.CurrentOrder !=  null)
        {
            throw new ArgumentException("\nPlease checkout before ordering again.");
        }
        // This is to ensure that all order ids will be unique since orderDict contains all the orders, past 
        // and present
        int orderId = orderDict.Count + 1;
        Order order = new Order(orderId, DateTime.Now, id);

        // This is to allow user to add multiple icecreams to an order
        while (true)
        {
            // We created a method to create the icecream
            IceCream icecream = CreateIceCream();
            order.AddIceCream(icecream);
            Console.Write("\nWould you like to add another icecream? [Y/N]: ");

            // Validated the user input to ensure that it is either y or n
            string repeat = Console.ReadLine().Trim().ToLower();
            if (repeat != "y" && repeat != "n")
            {
                throw new ArgumentException("\nPlease enter Y or N.");
            }
            if (repeat == "n")
            {
                Console.WriteLine("\nOrder has been created successfully.");
                Console.WriteLine($"The current total of your order stands at " +
                    $"${order.CalculateTotal().ToString("0.00")} for {order.IceCreamList.Count} icecream(s).");
                // This is to link the order to the customer as their customer order
                customer.CurrentOrder = order;

                // Adding the order object to the orderDict
                orderDict[orderId] = order;

                // Adding the order object to the queue based on the customer's tier using a new method
                // we created
                OrderQueue(customer, order);
                break;
            }
            Console.WriteLine();
        }
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine("Order creation failed.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("An unexpected error occurred while making order.");
    }
}

// Option 5 - Display order details of a customer.
void DisplayCustomerOrders()
{
    DisplayCustomerInfo();
    Console.Write("\nEnter ID of customer: ");

    // Using a try catch block to catch any errors that may occur from user input
    try
    {
        // Using TryParse to try to convert user input into an int, and that the id belongs to a
        // registered customer
        if (!int.TryParse(Console.ReadLine(), out int id) || !customerDict.ContainsKey(id))
        {
            throw new ArgumentException("Please enter a valid customer id.");
        }

        // Creating a bool beforehand for iterating through the orderDict and checking if the customer
        // made any orders past and present
        bool haveOrder = false;

        // Creating an int variable beforehand to be used for an extra output of the number of orders made by 
        // customer
        int currentOrder = 0;
        Customer customer = customerDict[id];
        if (customer.CurrentOrder != null)
        {
            currentOrder = 1;
        }

        // Checking if customer has made any order
        if (customer.OrderHistory.Count + currentOrder == 0)
        {
            throw new ArgumentException($"\nNo orders have been made by {customer.Name}.");
        }

        Console.WriteLine($"\n{customer.Name} has made {customer.OrderHistory.Count + currentOrder} " +
            $"order(s).");
        foreach (Order order in orderDict.Values)
        {
            if (order.MemberId == id)
            {
                // If there is an order, then haveOrder bool is true
                haveOrder = true;
                DisplayOrder(order);
            }
        }
    }
    catch (ArgumentException ex) 
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("An unexpected error occurred.");
    }
}

// Option 6 - Modify order details
void ModifyOrderDetails()
{
    DisplayCustomerInfo();
    Console.Write("\nEnter ID of customer: ");

    // Using a try catch block to catch any errors that may occur from user input
    try
    {
        // Using TryParse to try to convert user input into an int, and that the id belongs to a
        // registered customer
        if (!int.TryParse(Console.ReadLine(), out int id) || !customerDict.ContainsKey(id))
        {
            throw new ArgumentException("\nPlease enter a valid customer id.");
        }
        Customer selectedCustomer = customerDict[id];

        // If user has not made any order, then this method stops
        if (selectedCustomer.CurrentOrder == null)
        {
            throw new ArgumentException($"\nNo orders have been made by {selectedCustomer.Name}.");
        }
        Order order = selectedCustomer.CurrentOrder;
        Console.Write("\nYour current order is:");
        DisplayOrder(order);

        // Menu for modification of order
        Console.Write(
        "\n==============================" +
        "\n            Option" +
        "\n==============================" +
        "\n[1] Choose an existing ice cream object to modify" +
        "\n[2] Add an entirely new ice cream object to the order" +
        "\n[3] Choose an existing ice cream object to delete from the order" +
        "\n==============================" +
        "\n\nEnter your option: ");

        // Using TryParse to try to convert user input into an int
        if (!int.TryParse(Console.ReadLine(), out int option))
        {
            throw new ArgumentException("Please enter a valid option.");
        }
        // Checking if the option is within 1-3
        else if (option < 1 || option > 3)
        {
            throw new ArgumentException("Please enter a valid option between 1-3.");
        }

        Console.WriteLine();

        // Using a switch case block since it is faster and more efficient than if else block in this case
        switch (option)
        {
            case 1:

                // Created a for loop to display the icecreams in the order with its index so that it is
                // easier to know which icecream the user is selecting
                for (int i = 0; i < order.IceCreamList.Count; i++)
                {
                    IceCream icecreamOld = order.IceCreamList[i];
                    Console.WriteLine($"[{i + 1}]: {icecreamOld.Option} icecream" +
                        $", {icecreamOld.Flavours.Count} flavour(s)" +
                        $", {icecreamOld.Toppings.Count} topping(s)");
                }
                Console.Write("\nEnter the ice cream position for modification: ");

                // Using TryParse to try to convert user input into an int
                if (!int.TryParse(Console.ReadLine(), out int positionMod))
                {
                    throw new ArgumentException("Please enter a valid position.");
                }
                Console.WriteLine();

                // Calling the method to create a new icecream and replacing it with the icecream that
                // user chose
                IceCream icecreamNew = CreateIceCream();
                order.ModifyIceCream(positionMod - 1, icecreamNew);
                Console.WriteLine("Icecream has been modified.");
                break;

            case 2:
                // Using the method to create a new icecream and then adding it into the order
                IceCream icecream = CreateIceCream();
                order.AddIceCream(icecream);
                Console.WriteLine("Icecream has been added.");
                break;

            case 3:

                // This is to check if the order has more than 1 icecream. If the order only has 1
                // icecream, deletion of icecream would not be allowed as it would result in an empty
                // order
                if (order.IceCreamList.Count == 1)
                {
                    throw new ArgumentException("Sorry, you cannot have 0 icecreams in your order.");
                }
                for (int i = 0; i < order.IceCreamList.Count; i++)
                {
                    IceCream icecreamOld = order.IceCreamList[i];
                    Console.WriteLine($"[{i + 1}]: {icecreamOld.Option} icecream" +
                        $", {icecreamOld.Flavours.Count} flavour(s)" +
                        $", {icecreamOld.Toppings.Count} topping(s)");
                }

                Console.Write("\nEnter the ice cream position for deletion: ");

                // Using TryParse to try to convert user input into an int, and that the position entered
                // is within the available positions in the list
                if (!int.TryParse(Console.ReadLine(), out int positionDel) || 
                    positionDel > order.IceCreamList.Count)
                {
                    throw new ArgumentException("Please enter a valid position.");
                }
                Console.WriteLine();

                // Calling the class method to remove the icecream object from order.IceCreamList
                order.DeleteIceCream(positionDel - 1);
                Console.WriteLine("Icecream has been deleted.");
                break;

            default:
                throw new ArgumentException("Please enter an option from 1-3.");
        }

        // Displaying the updated order
        Console.Write("\nYour modified order is:");
        DisplayOrder(order);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("An unexpected error occurred while modifying order.");
    }
}

// Option 7 Process an order and checkout
void ProcessNCheckOut()
{
    // Creating backups in case an error occurs during the method runtime
    // This is to prevent the previous data from being lost
    Queue<Order> orderQueueCopy = new(orderQueue);
    Queue<Order> goldOrderQueueCopy = new(goldOrderQueue);
    // Using a try catch block to catch any errors that may occur from user input
    try
    {
        // This is to check if both queue collection classes are null, and if so, the method will end here
        if (goldOrderQueue.Count == 0 && orderQueue.Count == 0)
        {
            throw new NotImplementedException("No orders have been made.");
        }

        // Creating the order object first
        Order order = null;
        // Checking if there are gold orders in the queue first to be checked out
        if (goldOrderQueue.Count > 0)
        {
            // Dequeuing the object from the queue and then running the ProcessOrder method we created
            order = goldOrderQueue.Dequeue();
        }
        else if (orderQueue.Count > 0)
        {
            // Same process as the goldOrderQueue
            order = orderQueue.Dequeue();
        }

        Console.Write($"The order being checked out:");
        DisplayOrder(order);

        // Creating the variables to be used later on
        Customer customer = customerDict[order.MemberId];

        // Creating a copy of the punchcard in the case of an error during method runtime. 
        // This is to prevent the previous data from being lost
        int punchCardCopy = customer.Rewards.PunchCard;
        double totalPrice = order.CalculateTotal();

        Console.WriteLine($"{customer.Name}'s current membership status: {customer.Rewards.Tier}");
        Console.WriteLine($"{customer.Name}'s current membership points: {customer.Rewards.Points}");

        // Using customer class method to check if it is the customer's birthday
        if (customer.IsBirthday())
        {
            Console.WriteLine();
            // This is to check if the birthday promotion was already redeemed
            bool check = customer.OrderHistory.Any(order => order.TimeReceived.Date == DateTime.Now.Date);
            if (check)
            {
                Console.WriteLine("The customer has already redeemed the free birthday ice cream");
            }
            else
            {
                // Calculate the final bill while having the most expensive ice cream cost $0.00
                IceCream mostExpensiveIceCream = order.IceCreamList.OrderByDescending(icecream =>
                icecream.CalculatePrice()).First();
                totalPrice -= mostExpensiveIceCream.CalculatePrice();
                Console.WriteLine($"It's the customer's birthday! The most expensive icecream is free of charge!");
            }
        }

        // Checking if the birthday promotion has already been redeemed. If it has been redeemed, punch card
        // will remain the same
        if (customer.Rewards.PunchCard >= 10 && !customer.IsBirthday())
        {
            // Set the cost of the first ice cream to $0.00
            IceCream punchedIceCream = order.IceCreamList[0];
            totalPrice -= punchedIceCream.CalculatePrice();
            Console.WriteLine("\nSince customer's punch-card reached 10, first icecream is free!");

            // Reset the punch card back to 0
            customer.Rewards.PunchCard = 0;
        }

        // if punchcard promotion is unavailable, then just add a punch for every ice cream
        else
        {
            foreach (IceCream icecream in order.IceCreamList)
            {
                customer.Rewards.Punch();
            }

            // Ensure that the punch card is not over 10 and to remind that next order's icecream is free
            if (customer.Rewards.PunchCard >= 10)
            {
                customer.Rewards.PunchCard = 10;
                Console.WriteLine("\nCustomer will receive a free icecream next order.");
            }
        }

        // Created an external variable to store the amount of points redeemed to check
        // whether customer was eligible for promotion before redemption of points
        int redeemedPoints = 0;
        // Using a method to check if customer tier is silver or gold to redeem points
        if (IsSilverOrGoldMember(customer) && customer.Rewards.Points > 0)
        {
            double discount = 0;
            while (true)
            {
                Console.Write($"\nAvailable points: {customer.Rewards.Points}" +
                "\nHow many points would you like to redeem? ");
                if (!int.TryParse(Console.ReadLine(), out int redeemPoints) || redeemPoints < 0)
                {
                    customer.Rewards.PunchCard = punchCardCopy;
                    throw new ArgumentException("\nPlease enter a valid number of points.");
                }

                // Checking if the customer has enough points to redeem
                else if (redeemPoints > customer.Rewards.Points)
                {
                    customer.Rewards.PunchCard = punchCardCopy;
                    throw new ArgumentException("\nThere is not enough points to redeem.");
                }
                // Calculate the discount based on the redeemed points and subtract it from the customer object
                discount = redeemPoints * 0.02;
                redeemedPoints = redeemPoints;

                // Checking if the redeemed points exceed the order bill and if it does, prompt user 
                // for decision
                if (discount > totalPrice)
                {
                    redeemedPoints = Convert.ToInt32(Math.Ceiling(totalPrice / 0.02));
                    discount = totalPrice;
                    Console.Write("\nYour points redemption has exceeded the order bill." +
                        $"\nIf you proceed, only {redeemedPoints} will be deducted. If not, you can re-enter." +
                        "\nWould you like to proceed? [Y/N]: ");
                    string proceed = Console.ReadLine().Trim().ToLower();
                    if (proceed != "y" && proceed != "n")
                    {
                        Console.WriteLine("\nPlease enter a valid option.");
                    }
                    else if (proceed == "y") { break; }
                    continue;
                }
                else { break; }
            }

            customer.Rewards.RedeemPoints(redeemedPoints);

            // Adjust the total bill by subtracting the discount
            totalPrice -= discount;

            // Display information about redeemed points
            Console.WriteLine($"{redeemedPoints} points have been redeemed, saving ${discount:0.00}.");
        }

        // Display the final bill amount
        Console.WriteLine($"\nFinal bill amount: ${totalPrice:0.00}\n");

        // Prompt user to press any key to make payment
        Console.Write("Press any key to make payment...");
        Console.ReadKey();

        // Setting the time fulfilled to indicate order completion
        order.TimeFulfilled = DateTime.Now;

        // Calculate points based on the total amount paid
        int earnedPoints = Convert.ToInt32(Math.Floor(totalPrice * 0.72));

        // Increment customer's points
        customer.Rewards.AddPoints(earnedPoints);

        Console.WriteLine($"\n\nUpdated Points: {customer.Rewards.Points - earnedPoints} + {earnedPoints}");
        Console.WriteLine($"Updated Punch-Card: {customer.Rewards.PunchCard - order.IceCreamList.Count} + " +
            $"{order.IceCreamList.Count}");

        // Check for membership tier promotions
        if (customer.Rewards.Tier == "Gold")
        {
            // Customer is already a Gold member
            Console.WriteLine("You are already a Gold member! Thank you for your continuous support!");
        }
        else if ((customer.Rewards.Points + redeemedPoints) >= 100)
        {
            // Promote the customer to Gold member
            customer.Rewards.Tier = "Gold";
            Console.WriteLine("Congratulations! You are now a Gold member.");
        }
        else
        {
            if (customer.Rewards.Tier == "Silver")
            {
                // Customer is already a Silver member
                Console.WriteLine("Congratulations! You are already a Silver member.");
            }
            else if ((customer.Rewards.Points + redeemedPoints) >= 50)
            {
                // Promote the customer to Silver member
                customer.Rewards.Tier = "Silver";
                Console.WriteLine("Congratulations! You are now a Silver member.");
            }
        }
        UpdateCustomerCsvFile();
        UpdateOrderCsvFile();
    }
    catch (NotImplementedException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine("Order checkout failed.");

        // Restoring the backup data of the queues
        goldOrderQueue = goldOrderQueueCopy;
        orderQueue = orderQueueCopy;
    }
    catch (Exception ex)
    {
        Console.WriteLine("An unexpected error occured during checkout.");
        // Restoring the backup data of the queues
        goldOrderQueue = goldOrderQueueCopy;
        orderQueue = orderQueueCopy;
    }
}

// Option 8 Display monthly charged amounts breakdown & total charged amounts for the year
void DisplayMonthlyAndYearAmount()
{
    // Using a try catch block to catch any errors that may occur from user input
    try
    {
        Console.Write("Enter the year: ");
        // Using TryParse to try to convert user input into an int
        if (!int.TryParse(Console.ReadLine(), out int promptyear))
        {
            throw new ArgumentException("Please enter a valid year.");
        }

        // Fun little error message if user keys a year below 2000
        if (promptyear < 2000)
        {
            throw new ArgumentException("\nHaha, our shop is not that old!");
        }

        double totalAmount = 0;
        Console.WriteLine("======================================");
        Console.WriteLine("|   Year   |   Month   |   Amount    |");
        Console.WriteLine("======================================");

        // Using a for loop to iterate through the months
        for (int month = 1; month <= 12; month++)
        {
            double monthlyTotal = 0;

            // Using two foreach loops to receive the monthly total by checking whether the order was made
            // in that month
            foreach (Customer customer in customerDict.Values)
            {
                foreach (Order order in customer.OrderHistory)
                {
                    if (order.TimeFulfilled.Value.Year == promptyear && order.TimeFulfilled.Value.Month == month)
                    {
                        monthlyTotal += order.CalculateTotal();
                    }
                }
            }
            // Getting the name of the month based on the number of i which is being iterated with the for
            // loop
            string monthName = new DateTime(promptyear, month, 1).ToString("MMM");
            Console.WriteLine($"|   {promptyear,-4}   |   {monthName,-5}   |   ${monthlyTotal,-6:F2}   |");

            totalAmount += monthlyTotal;
        }
        Console.WriteLine("--------------------------------------");
        Console.WriteLine($"Total Charged Amount for {promptyear}: ${totalAmount:F2}");
        Console.WriteLine("--------------------------------------");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An unexpected error occurred");
    }
}


// New method for appending customer information into csv file
void AppendCustomerToCsvFile(Customer customer)
{
    // We used a relative path to make it so that the changes can occur to the file that is located
    // inside the solution folder, which is the original text file
    string relativePath = @"..\..\..\customers.csv";
    string filePath = Path.GetFullPath(relativePath, Directory.GetCurrentDirectory());
    string csvLine = $"{customer.Name},{customer.MemberId},{customer.Dob},{customer.Rewards.Tier}," +
        $"{customer.Rewards.Points},{customer.Rewards.PunchCard}";
    File.AppendAllLines(filePath, new[] { csvLine });
    return;
}

// New method to update customers.csv after checking out an order
void UpdateCustomerCsvFile()
{
    // We used a relative path to make it so that the changes can occur to the file that is located
    // inside the solution folder, which is the original text file
    string relativePath = @"..\..\..\customers.csv";
    string filePath = Path.GetFullPath(relativePath, Directory.GetCurrentDirectory());

    // We wiped the contents of customers.csv file so that updates can be
    // made such as tier, points and punch
    File.WriteAllText(filePath, string.Empty);

    // Re-adding the header of the file
    File.AppendAllLines(filePath, new[]
    { "Name,MemberId,DOB,MembershipStatus,MembershipPoints,PunchCard" });

    foreach (Customer customer in customerDict.Values)
    {
        string csvLine = $"{customer.Name},{customer.MemberId},{customer.Dob},{customer.Rewards.Tier}," +
        $"{customer.Rewards.Points},{customer.Rewards.PunchCard}";
        File.AppendAllLines(filePath, new[] { csvLine });
    }
    return;
}

// New method to update orders.csv after checking out an order
void UpdateOrderCsvFile()
{
    // We used a relative path to make it so that the changes can occur to the file that is located
    // inside the solution folder, which is the original text file
    string relativePath = @"..\..\..\orders.csv";
    string filePath = Path.GetFullPath(relativePath, Directory.GetCurrentDirectory());

    // We wiped the contents of the orders.csv file so that the file can be rewritten with the
    // new orders. We chose to do it this way to be consistent with the UpdateCustomerCsvFile method
    File.WriteAllText(filePath, string.Empty);

    // Re-adding the header of the file
    File.AppendAllLines(filePath, new[]
    { "Id,MemberId,TimeReceived,TimeFulfilled,Option,Scoops,Dipped,WaffleFlavour,Flavour1,Flavour2" +
    ",Flavour3,Topping1,Topping2,Topping3,Topping4" });
    
    foreach (Order order in orderDict.Values)
    {
        foreach (IceCream icecream in order.IceCreamList)
        {
            string flavoursString = "";
            string toppingsString = "";

            // We used a foreach loop to loop through the Flavours list to add into the string, and then
            // used a for loop to add empty spaces into the string as well
            foreach (Flavour flavour in icecream.Flavours)
            {
                flavoursString += $"{flavour.Type},";
            }
            // To ensure that there wont be a trailing comma
            if (icecream.Flavours.Count == 3)
            {
                flavoursString = flavoursString.Trim(',');
            }
            for (int i = 0; i < (3 - icecream.Flavours.Count); i++)
            {
                // This is to check if it is the last element in the string and if it is, then it will not add
                // another comma
                if (i == (3 - icecream.Flavours.Count - 1))
                {
                    break;
                }
                flavoursString += ",";
            }

            // Similarly to the Flavours ist, we used a foreach loop to loop through the Toppings list
            // to add into the string, and then used a for loop to add empty spaces into the string as well
            foreach (Topping topping in icecream.Toppings)
            {
                toppingsString += $"{topping.Type},";
            }
            // To ensure that there wont be a trailing comma
            if (icecream.Toppings.Count == 4)
            {
                toppingsString = toppingsString.Trim(',');
            }
            for (int i = 0; i < (4 - icecream.Toppings.Count); i++)
            {
                // This is to check if it is the last element in the string and if it is, then it will not add
                // another comma
                if (i == (4 - icecream.Toppings.Count - 1))
                {
                    break;
                }
                toppingsString += ",";
            }

            // Initialising the dipped and waffleFlavour strings beforehand as not all icecream objects 
            // will contain them.
            string dipped = "";
            string waffleFlavour = "";

            // Using if statement to check if the icecream object is cone or waffle flavour, and changing the
            // dipped and waffleflavour strings accordingly to their values
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
            
            // order.TimeFulfilled?.ToString() is to check if order.TimeFulfilled is null. If it is null,
            // the .ToString will not run and instead return a null string value which is ""
            string csvLine = $"{order.Id},{order.MemberId},{order.TimeReceived.ToString("dd/MM/yyyy HH:mm")}," +
                $"{order.TimeFulfilled?.ToString("dd/MM/yyyy HH:mm")},{icecream.Option},{icecream.Scoop},{dipped},{waffleFlavour}," +
                $"{flavoursString},{toppingsString}";
            File.AppendAllLines(filePath, new[] { csvLine });
        }
    }
    return;
}

// New Method to check whether it is premium or normal flavour
Flavour FlavourPremiumCheck(string flavourOrg)
{
    // In order to standardise any capitalisation, we lowered all characters in the string
    string flavour = flavourOrg.ToLower();

    // Using if statements to check if they are premium or normal flavours
    if (flavour == "vanilla" || flavour == "chocolate" || flavour == "strawberry")
    {
        // Creating and return the flavour object
        return new Flavour(CapitaliseStr(flavourOrg), false, 1);
    }
    else if (flavour == "durian" || flavour == "ube" || flavour == "sea salt")
    {
        // Creating and return the flavour object
        return new Flavour(CapitaliseStr(flavourOrg), true, 1);
    }
    return null;
}

// New method to display all the flavours
void DisplayFlavours()
{
    Console.WriteLine(
        "====================" +
        "\n      Flavours" +
        "\n====================");

    // Using a foreach loop to iterate through flavourOptions string list that was created at the start
    foreach (string flavour in flavourOptions)
    {
        // The flavourOptions string list was ordered so that the first 3 flavours are the normal ones, 
        // while the last 3 are the premium ones
        if (flavourOptions.IndexOf(flavour) < 3)
        {
            Console.WriteLine($"{CapitaliseStr(flavour),-10} |");
            continue;
        }
        Console.WriteLine($"{CapitaliseStr(flavour),-10} | +$2.00");
    }
    return;
}

// New method to display all the toppings
void DisplayToppings()
{
    Console.WriteLine(
        "====================" +
        "\n      Toppings" +
        "\n====================");
    // Simimlar to the DisplayFlavours, we used foreach loop to iterate through toppingOptions which was
    // created beforehand
    foreach (string topping in toppingOptions)
    {
        Console.WriteLine($"{CapitaliseStr(topping),-10} | +$1.00");
    }
    return;
}

// New method to display all the waffle flavours
void DisplayWaffleFlavours()
{
    Console.WriteLine(
        "====================" +
        "\n  Waffle Flavours" +
        "\n====================");
    // Same process as DisplayFlavour, except the first element in the list is the original one which does
    // not cost extra, the rest costing extra
    foreach (string waffleFlavour in waffleOptions)
    {
        if (waffleOptions.IndexOf(waffleFlavour) == 0)
        {
            Console.WriteLine($"{CapitaliseStr(waffleFlavour),-10} |");
            continue;
        }
        Console.WriteLine($"{CapitaliseStr(waffleFlavour),-10} | +$3.00");
    }
    return;
}

// New method to display all details of order
void DisplayOrder(Order order)
{
    Console.WriteLine("\n------------------------------------------------------------");
    Console.WriteLine(order.ToString());
    Console.WriteLine("|----------------------------------------------------------|");
    // Using a foreach loop to iterate through icecream list to print all icecreams using ToString
    foreach (IceCream icecream in order.IceCreamList)
    {
        if (order.IceCreamList.IndexOf(icecream) == (order.IceCreamList.Count-1))
        {
            Console.WriteLine(icecream.ToString());
            break;
        }
        Console.WriteLine(icecream.ToString()+$"\n|{" ",-58}|");
    }
    // We also included the order total
    Console.WriteLine("|----------------------------------------------------------|");
    Console.WriteLine($"|{$"The order total is ${order.CalculateTotal().ToString("0.00")}",-58}|");
    Console.WriteLine("------------------------------------------------------------");
}

// New method to enqueue order for gold or normal tier customer's order
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

// New method to create ice cream
IceCream CreateIceCream()
{
    IceCream icecream = null;
    List<Flavour> flavourList = new List<Flavour>();
    List<Topping> toppingsList = new List<Topping>();
    int scoops = 0;
    Console.Write("Enter your option of icecream (Cup, Cone, Waffle): ");
    string option = Console.ReadLine().Trim().ToLower();

    // Checking if the user input is a valid flavour by checking if it is inside the icecreamOptions list
    // initialised at the start
    if (!icecreamOptions.Contains(option))
    {
        throw new ArgumentException("\nPlease enter a valid icecream option.");
    }

    // Using if statements to create the proper icecream object
    if (option == "waffle")
    {
        Console.WriteLine();
        DisplayWaffleFlavours();
        Console.Write("\nEnter waffle flavour: ");
        string waffleFlavour = Console.ReadLine().Trim().ToLower();
        // Checking if user input is a valid waffle flavour by checking if it is inside the waffleOptions list
        // initialised at the start
        if (!waffleOptions.Contains(waffleFlavour))
        {
            throw new ArgumentException("\nPlease enter a valid waffle flavour.");
        }
        icecream = new Waffle(CapitaliseStr(option), scoops, flavourList, toppingsList,
            CapitaliseStr(waffleFlavour));
        Console.WriteLine();
    }
    else if (option == "cone")
    {
        Console.Write("Would you like to change the cone to chocolate-dipped? [Y/N]: ");
        string dippedInput = Console.ReadLine().Trim().ToLower();
        if (dippedInput != "y" && dippedInput != "n")
        {
            throw new ArgumentException("\nPlease enter Y or N.");
        }
        bool dipped = false;
        if (dippedInput == "y")
        {
            dipped = true;
        }
        icecream = new Cone(CapitaliseStr(option), scoops, flavourList, toppingsList, dipped);
        Console.WriteLine();
    }
    else
    {
        icecream = new Cup(CapitaliseStr(option), scoops, flavourList, toppingsList);
    }

    // Creating the rest of the icecream object
    Console.Write("Enter number of icecream scoops (1-3): ");
    // Using TryParse to try to convert user input into an int
    if (!int.TryParse(Console.ReadLine(), out scoops))
    {
        throw new ArgumentException("\nPlease enter a valid number.");
    }

    // Checking if the user input is within 1-3
    else if (scoops < 1 || scoops > 3)
    {
        throw new ArgumentException("\nPlease only enter 1-3 scoops.");
    }
    icecream.Scoop = scoops;
    Console.WriteLine();
    DisplayFlavours();

    // Using a for loop to allow user to enter the flavour based on the number of scoops
    for (int i = 0; i != scoops; i++)
    {
        Console.WriteLine($"\n------ Scoop {i + 1} ------");
        Console.Write("Enter flavour of icecream: ");
        string flavour = Console.ReadLine().Trim().ToLower();
        // Checking if user input is a valid icecream flavour by checking if it is inside the flavourOptions 
        // list initialised at the start
        if (!flavourOptions.Contains(flavour))
        {
            throw new ArgumentException("\nPlease enter a valid flavour.");
        }

        // Adding the flavour object into the flavours list
        icecream.Flavours.Add(FlavourPremiumCheck(flavour));
    }
    Console.WriteLine();
    DisplayToppings();
    Console.WriteLine("You can enter a maximum of 4 toppings (enter 0 for no toppings).\n");
    Console.Write("Enter number of toppings: ");
    
    // Same process as the flavours
    if (!int.TryParse(Console.ReadLine(), out int toppingsNumber))
    {
        throw new ArgumentException("\nPlease enter a valid number.");
    }
    else if (toppingsNumber < 0 || toppingsNumber > 4)
    {
        throw new ArgumentException("\nSorry, you can only choose a maximum of 4 toppings");
    }

    // Checking if user entered no toppings
    if (toppingsNumber != 0)
    {
        for (int i = 0; i != toppingsNumber; i++)
        {
            Console.WriteLine($"\n------ Topping {i + 1} ------");
            Console.Write($"Enter topping: ");
            string toppings = Console.ReadLine().Trim().ToLower();
            if (!toppingOptions.Contains(toppings))
            {
                throw new ArgumentException("\nPlease enter a valid topping.");
            }
            else
            {
                icecream.Toppings.Add(new Topping(CapitaliseStr(toppings)));
            }
        }
    }
    return icecream;
}

//New method to capitalise the string
string CapitaliseStr(string str)
{
    string str1 = char.ToUpper(str[0]) + str.Substring(1);
    return str1;
}

//New method to check if the customer is silver or gold and returns a bool
bool IsSilverOrGoldMember(Customer customer)
{
    // Check if the customer is silver or gold
    return customer.Rewards.Tier == "Silver" || customer.Rewards.Tier == "Gold";
}