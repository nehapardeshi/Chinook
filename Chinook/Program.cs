// See https://aka.ms/new-console-template for more information
using Chinook;
using Chinook.Entities;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
     .AddJsonFile($"appsettings.json");

var config = configuration.Build();
var connectionString = config.GetConnectionString("SqlConnection");

Console.WriteLine("Hello, Chinook!");
IDataRepository dataRepository = new SqlDataRepository(connectionString);

int option = -1;

do
{
    Console.WriteLine("");
    Console.WriteLine("========================================================");
    Console.WriteLine("Enter 1 to get customer by Id");
    Console.WriteLine("Enter 2 to get all customers");
    Console.WriteLine("Enter 3 to get customer(s) by name");
    Console.WriteLine("Enter 4 to add customer");
    Console.WriteLine("Enter 5 to get customers by limit and offset");
    Console.WriteLine("Enter 6 to update customer");
    Console.WriteLine("Enter 7 to get number of customers in each country");
    Console.WriteLine("Enter 8 to get customers who are the highest spenders");
    Console.WriteLine("Enter 9 to get a customer's most popular genre");
    Console.WriteLine("Enter 0 to exit");
    option = Convert.ToInt32(Console.ReadLine());

    switch (option)
    {
        case 1: // Get customer by Id
            Console.Write("Enter customer Id: ");
            var id = Console.ReadLine();
            var customer = dataRepository.GetCustomer(Convert.ToInt32(id));
            DisplayCustomer(customer);
            break;

        case 2:// Get customers
            var customers = dataRepository.GetCustomers();
            foreach (var c in customers)
            {
                DisplayCustomer(c);
            }
            break;

        case 3:// Get customer(s) by name
            Console.Write("Enter customer name: ");
            var name = Console.ReadLine();
            var customersByName = dataRepository.GetCustomers(name);
            foreach (var c in customersByName)
            {
                DisplayCustomer(c);
            }
            break;

        case 4:// Add new customer
            Console.Write("Enter customer first name: ");
            var firstName = Console.ReadLine();
            Console.Write("Enter customer last name: ");
            var lastName = Console.ReadLine();
            Console.Write("Enter customer country: ");
            var country = Console.ReadLine();
            Console.Write("Enter customer phone: ");
            var phone = Console.ReadLine();
            Console.Write("Enter customer postal code: ");
            var postalCode = Console.ReadLine();
            Console.Write("Enter customer email: ");
            var email = Console.ReadLine();

            dataRepository.AddCustomer(new Customer { FirstName = firstName, LastName = lastName, Country = country, Email = email, Phone = phone, PostalCode = postalCode });
            Console.WriteLine("Customer added successfully!");
            break; 
       
        case 5: // Get customers by limit and offset
            Console.Write("Enter limit: ");
            var limit = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter offset: ");
            var offset = Convert.ToInt32(Console.ReadLine());
            var customersByLimitOffset = dataRepository.GetCustomers(limit, offset);
            foreach (var c in customersByLimitOffset)
            {
                DisplayCustomer(c);
            }
            break;

        case 6: // Update customer
            Console.Write("Enter customer Id: ");
            var customerId = Console.ReadLine();
            var customerToUpdate = dataRepository.GetCustomer(Convert.ToInt32(customerId));
            DisplayCustomer(customerToUpdate);

            Console.Write("Enter customer first name (press enter to skip update): ");
            var firstNameToUpdate = Console.ReadLine();
            if(!string.IsNullOrWhiteSpace(firstNameToUpdate))
                customerToUpdate.FirstName = firstNameToUpdate;

            Console.Write("Enter customer last name (press enter to skip update): ");
            var lastNameToUpdate = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(lastNameToUpdate))
                customerToUpdate.LastName = lastNameToUpdate;

            Console.Write("Enter customer country (press enter to skip update): ");
            var countryToUpdate = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(countryToUpdate))
                customerToUpdate.Country = countryToUpdate;

            Console.Write("Enter customer phone (press enter to skip update): ");
            var phoneToUpdate = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(phoneToUpdate))
                customerToUpdate.Phone = phoneToUpdate;

            Console.Write("Enter customer postal code (press enter to skip update): ");
            var postalCodeToUpdate = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(postalCodeToUpdate))
                customerToUpdate.PostalCode = postalCodeToUpdate;

            Console.Write("Enter customer email (press enter to skip update): ");
            var emailToUpdate = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(emailToUpdate))
                customerToUpdate.Email = emailToUpdate;

            dataRepository.UpdateCustomer(customerToUpdate);
            Console.WriteLine("Customer updated successfully!");

            break;

        case 7: // Get number of customers in each country, ordered descending (high to low)
            var customersCountry = dataRepository.GetCustomersCountry();

            Console.WriteLine("Customers per Country");
            foreach (var cc in customersCountry)
            {
                Console.WriteLine($"{cc.Country}: {cc.CustomerCount}");
            }
            break;

        case 8: //get customers who are the highest spenders
            var customersSpender = dataRepository.GetCustomersSpender();

            Console.WriteLine("Customers who are the highest spenders");
            foreach (var cs in customersSpender)
            {
                Console.WriteLine($"{cs.CustomerId}: {cs.TotalAmount}");
            }
            break;

        case 9: //get customers most popular genre
            Console.Write("Enter customer Id: ");
            var cId = Convert.ToInt32(Console.ReadLine());
            var customerGenre = dataRepository.GetCustomerGenre(cId);

            Console.WriteLine($"Customer ID {cId} most popular genre");
            foreach (var cg in customerGenre)
            {
                Console.WriteLine($"Genre Id: {cg.GenreId}, Genre Name: {cg.GenreName}");
            }
            break;

        default:
            break;
    }

} while (option != 0);

static void DisplayCustomer(Customer customer)
{
    Console.WriteLine($"Customer ID = {customer.Id}, First Name = {customer.FirstName}, Last Name = {customer.LastName}, Country = {customer.Country}, Phone = {customer.Phone}, PostalCode = {customer.PostalCode}, Email = {customer.Email}");
}