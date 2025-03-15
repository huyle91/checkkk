using BusinessObjects;
using DataAccessObjects;
using Repositories.Interfaces;
using System.Collections.Generic;

namespace Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public void AddCustomer(Customer customer) => CustomerDAO.Instance.AddCustomer(customer);

        public void DeleteCustomer(int id) => CustomerDAO.Instance.DeleteCustomer(id);

        public Customer GetCustomerByID(int id) => CustomerDAO.Instance.GetCustomerByID(id);

        public Customer GetCustomerByEmail(string email) => CustomerDAO.Instance.GetCustomerByEmail(email);

        public List<Customer> GetCustomers() => CustomerDAO.Instance.GetCustomerList();

        public void UpdateCustomer(Customer customer) => CustomerDAO.Instance.UpdateCustomer(customer);

        public Customer Login(string email, string password) => CustomerDAO.Instance.Login(email, password);

        public List<Customer> SearchCustomers(string searchValue) => CustomerDAO.Instance.SearchCustomers(searchValue);
    }
}