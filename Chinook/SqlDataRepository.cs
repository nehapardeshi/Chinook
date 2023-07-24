using Chinook.Entities;
using System.Data.SqlClient;

namespace Chinook
{
    public class SqlDataRepository : IDataRepository
    {
        private readonly string _connectionString;

        public SqlDataRepository(string connectionString) 
        { 
            _connectionString = connectionString;
        }
        public void AddCustomer(Customer customer)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                using SqlCommand command = new SqlCommand(
                    @"Insert into Customer (FirstName, LastName, Country, Phone, PostalCode, Email) 
                        Values (@FirstName, @LastName, @Country, @Phone, @PostalCode, @Email)", con);
                command.Parameters.Add(new SqlParameter("FirstName", customer.FirstName));
                command.Parameters.Add(new SqlParameter("LastName", customer.LastName));
                command.Parameters.Add(new SqlParameter("Country", customer.Country));
                command.Parameters.Add(new SqlParameter("Phone", customer.Phone));
                command.Parameters.Add(new SqlParameter("PostalCode", customer.PostalCode));
                command.Parameters.Add(new SqlParameter("Email", customer.Email));
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot insert new customer. Error: {ex.Message}");
            }
        }

        public Customer GetCustomer(int id)
        {
            var customer = new Customer();
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            using SqlCommand command = new SqlCommand(
                @"SELECT 
                    CustomerId, 
                    FirstName, 
                    LastName, 
                    Country, 
                    Phone, 
                    PostalCode, 
                    Email 
                FROM Customer 
                Where CustomerId = @id", con);

            command.Parameters.Add(new SqlParameter("id", id));

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                customer.Id = id;
                customer.FirstName = reader.GetSafeString(reader.GetOrdinal("FirstName"));
                customer.LastName = reader.GetSafeString(reader.GetOrdinal("LastName"));
                customer.Country = reader.GetSafeString(reader.GetOrdinal("Country"));
                customer.Phone = reader.GetSafeString(reader.GetOrdinal("Phone"));
                customer.PostalCode = reader.GetSafeString(reader.GetOrdinal("PostalCode"));
                customer.Email = reader.GetSafeString(reader.GetOrdinal("Email"));
            }

            return customer;
        }

        public List<Customer> GetCustomers(string name)
        {
            name = $"%{name}%";
            List<Customer> customers = new List<Customer>();
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            using SqlCommand command = new SqlCommand(
                @"SELECT 
                    CustomerId, 
                    FirstName, 
                    LastName, 
                    Country, 
                    Phone, 
                    PostalCode, 
                    Email 
                FROM Customer 
                where upper(firstname) like upper(@name) OR
                upper(lastname) like upper(@name)", con);

            command.Parameters.Add(new SqlParameter("name", name));

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                string firstName = reader.GetSafeString(reader.GetOrdinal("FirstName"));
                string lastname = reader.GetSafeString(reader.GetOrdinal("LastName"));
                string country = reader.GetSafeString(reader.GetOrdinal("Country"));
                string phone = reader.GetSafeString(reader.GetOrdinal("Phone"));
                string postalCode = reader.GetSafeString(reader.GetOrdinal("PostalCode"));
                string email = reader.GetSafeString(reader.GetOrdinal("Email"));

                customers.Add(new Customer
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastname,
                    Country = country,
                    Phone = phone,
                    PostalCode = postalCode,
                    Email = email
                });
            }

            return customers;
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            using SqlCommand command = new SqlCommand(
                @"SELECT 
                    CustomerId, 
                    FirstName, 
                    LastName, 
                    Country, 
                    Phone, 
                    PostalCode, 
                    Email 
                FROM Customer", con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                string firstName = reader.GetSafeString(reader.GetOrdinal("FirstName"));
                string lastname = reader.GetSafeString(reader.GetOrdinal("LastName"));
                string country = reader.GetSafeString(reader.GetOrdinal("Country"));
                string phone = reader.GetSafeString(reader.GetOrdinal("Phone"));
                string postalCode = reader.GetSafeString(reader.GetOrdinal("PostalCode"));
                string email = reader.GetSafeString(reader.GetOrdinal("Email"));

                customers.Add(new Customer
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastname,
                    Country = country,
                    Phone = phone,
                    PostalCode = postalCode,
                    Email = email
                });
            }

            return customers;
        }

        public List<Customer> GetCustomers(int limit, int offset)
        {
            List<Customer> customers = new List<Customer>();
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            using SqlCommand command = new SqlCommand(
                @"SELECT 
                    CustomerId, 
                    FirstName, 
                    LastName, 
                    Country, 
                    Phone, 
                    PostalCode, 
                    Email 
                FROM Customer 
                order by CustomerId
                OFFSET @offset ROWS
                FETCH NEXT @limit ROWS ONLY", con);

            command.Parameters.Add(new SqlParameter("limit", limit));
            command.Parameters.Add(new SqlParameter("offset", offset));

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                string firstName = reader.GetSafeString(reader.GetOrdinal("FirstName"));
                string lastname = reader.GetSafeString(reader.GetOrdinal("LastName"));
                string country = reader.GetSafeString(reader.GetOrdinal("Country"));
                string phone = reader.GetSafeString(reader.GetOrdinal("Phone"));
                string postalCode = reader.GetSafeString(reader.GetOrdinal("PostalCode"));
                string email = reader.GetSafeString(reader.GetOrdinal("Email"));

                customers.Add(new Customer
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastname,
                    Country = country,
                    Phone = phone,
                    PostalCode = postalCode,
                    Email = email
                });
            }

            return customers;
        }

        public List<CustomerCountry> GetCustomersCountry()
        {
            List<CustomerCountry> customersCountry = new List<CustomerCountry>();
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            using SqlCommand command = new SqlCommand(
                "Select Country, count(*) as customercount from Customer group by Country order by count(*) desc", con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string country = reader.GetSafeString(reader.GetOrdinal("Country"));
                int customerCount = reader.GetInt32(reader.GetOrdinal("customercount"));

                customersCountry.Add(new CustomerCountry
                {
                    Country = country,
                    CustomerCount = customerCount
                });
            }

            return customersCountry;
        }

        public List<CustomerSpender> GetCustomersSpender()
        {
            List<CustomerSpender> customersSpender = new List<CustomerSpender>();
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            using SqlCommand command = new SqlCommand(
                "Select CustomerId, sum(total) as TotalAmount from Invoice group by CustomerId order by sum(total) desc", con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int customerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                decimal totalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount"));

                customersSpender.Add(new CustomerSpender
                {
                    CustomerId = customerId,
                    TotalAmount = totalAmount
                });
            }

            return customersSpender;
        }

        public List<CustomerGenre> GetCustomerGenre(int customerId)
        {
            List<CustomerGenre> customerGenre = new List<CustomerGenre>();
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            using SqlCommand command = new SqlCommand(
                @";With cte_topgenre as
                    (Select g.GenreId, g.Name, count(*) as Total from 
                    Customer c join Invoice i on i.CustomerId = c.CustomerId
                    join InvoiceLine il on il.InvoiceId = i.InvoiceId
                    join Track t on t.TrackId = il.TrackId
                    join Genre g on g.GenreId = t.GenreId
                    where c.CustomerId = @id  
                    Group by g.GenreId, g.Name)
                    Select genreId, Name from cte_topgenre
                    where Total = (Select MAX(total) from cte_topgenre)", con);

            command.Parameters.Add(new SqlParameter("id", customerId));

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int genreId = reader.GetInt32(reader.GetOrdinal("GenreId"));
                string genreName = reader.GetSafeString(reader.GetOrdinal("Name"));

                customerGenre.Add(new CustomerGenre
                {
                    CustomerId = customerId,
                    GenreId = genreId,
                    GenreName = genreName
                });
            }

            return customerGenre;
        }

        public void UpdateCustomer(Customer customer)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                using SqlCommand command = new SqlCommand(
                    @"Update Customer 
                        Set FirstName = @FirstName, 
                            LastName = @LastName, 
                            Country = @Country, 
                            Phone = @Phone, 
                            PostalCode = @PostalCode, 
                            Email = @Email
                    Where CustomerId = @Id", con);

                command.Parameters.Add(new SqlParameter("Id", customer.Id));
                command.Parameters.Add(new SqlParameter("FirstName", customer.FirstName));
                command.Parameters.Add(new SqlParameter("LastName", customer.LastName));
                command.Parameters.Add(new SqlParameter("Country", customer.Country));
                command.Parameters.Add(new SqlParameter("Phone", customer.Phone));
                command.Parameters.Add(new SqlParameter("PostalCode", customer.PostalCode));
                command.Parameters.Add(new SqlParameter("Email", customer.Email));
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot update customer. Error: {ex.Message}");
            }
        }

        
    }
}
