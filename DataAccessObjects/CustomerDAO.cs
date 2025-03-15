using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class CustomerDAO
    {
        // Singleton pattern
        private static CustomerDAO instance = null;
        private static readonly object instanceLock = new object();
        private CustomerDAO() { }
        public static CustomerDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CustomerDAO();
                    }
                    return instance;
                }
            }
        }

        // Create a static list to store customer data (in-memory database)
        private static List<Customer> customerList = new List<Customer>
        {
            new Customer
            {
                CustomerID = 1,
                CustomerFullName = "Admin User",
                EmailAddress = "admin@FUMiniHotelSystem.com",
                Password = "@@abc123@@",
                CustomerStatus = 1,
                Telephone = "0123456789",
                CustomerBirthday = new DateTime(1990, 1, 1)
            },
            new Customer
            {
                CustomerID = 2,
                CustomerFullName = "John Doe",
                EmailAddress = "john@example.com",
                Password = "john123",
                CustomerStatus = 1,
                Telephone = "0987654321",
                CustomerBirthday = new DateTime(1995, 5, 15)
            }
        };

        // Implement CRUD operations for Customer
        public List<Customer> GetCustomerList() => customerList.Where(c => c.CustomerStatus == 1).ToList();

        public Customer GetCustomerByID(int customerID)
        {
            return customerList.SingleOrDefault(c => c.CustomerID == customerID);
        }

        public Customer GetCustomerByEmail(string email)
        {
            return customerList.SingleOrDefault(c => c.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void AddCustomer(Customer customer)
        {
            // Auto-generate ID if list is not empty
            if (customerList.Count > 0)
            {
                customer.CustomerID = customerList.Max(c => c.CustomerID) + 1;
            }
            else
            {
                customer.CustomerID = 1;
            }
            customerList.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            Customer existingCustomer = GetCustomerByID(customer.CustomerID);
            if (existingCustomer != null)
            {
                existingCustomer.CustomerFullName = customer.CustomerFullName;
                existingCustomer.Telephone = customer.Telephone;
                existingCustomer.EmailAddress = customer.EmailAddress;
                existingCustomer.CustomerBirthday = customer.CustomerBirthday;
                existingCustomer.CustomerStatus = customer.CustomerStatus;
                existingCustomer.Password = customer.Password;
            }
        }

        public void DeleteCustomer(int customerID)
        {
            Customer customer = GetCustomerByID(customerID);
            if (customer != null)
            {
                // Soft delete by changing status to 2 (Deleted)
                customer.CustomerStatus = 2;
            }
        }

        public Customer Login(string email, string password)
        {
            return customerList.SingleOrDefault(c => c.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase)
                                              && c.Password.Equals(password)
                                              && c.CustomerStatus == 1);
        }

        public List<Customer> SearchCustomers(string searchValue)
        {
            return customerList.Where(c =>
                (c.CustomerFullName != null && c.CustomerFullName.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                (c.EmailAddress != null && c.EmailAddress.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                (c.Telephone != null && c.Telephone.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
    }
}