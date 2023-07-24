using Chinook.Entities;

namespace Chinook
{
    public interface IDataRepository
    {
        List<Customer> GetCustomers();
        Customer GetCustomer(int id);
        List<Customer> GetCustomers(string name);
        List<Customer> GetCustomers(int limit, int offset);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        List<CustomerCountry> GetCustomersCountry();
        List<CustomerSpender> GetCustomersSpender();
        List<CustomerGenre> GetCustomerGenre(int customerId);

    }
}
