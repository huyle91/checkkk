using BusinessObjects;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ICustomerService
    {
        List<Customer> GetCustomers();
        Customer GetCustomerByID(int id);
        Customer GetCustomerByEmail(string email);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
        Customer Login(string email, string password);
        List<Customer> SearchCustomers(string searchValue);
    }
}