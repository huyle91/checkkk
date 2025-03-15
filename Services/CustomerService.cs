using BusinessObjects;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;

namespace Services
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository customerRepository;

        public CustomerService()
        {
            customerRepository = new CustomerRepository();
        }

        public void AddCustomer(Customer customer) => customerRepository.AddCustomer(customer);

        public void DeleteCustomer(int id) => customerRepository.DeleteCustomer(id);

        public Customer GetCustomerByID(int id) => customerRepository.GetCustomerByID(id);

        public Customer GetCustomerByEmail(string email) => customerRepository.GetCustomerByEmail(email);

        public List<Customer> GetCustomers() => customerRepository.GetCustomers();

        public void UpdateCustomer(Customer customer) => customerRepository.UpdateCustomer(customer);

        public Customer Login(string email, string password) => customerRepository.Login(email, password);

        public List<Customer> SearchCustomers(string searchValue) => customerRepository.SearchCustomers(searchValue);
    }
}
