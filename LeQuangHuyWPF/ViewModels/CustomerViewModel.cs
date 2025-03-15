using BusinessObjects;
using LeQuangHuyWPF.Utils;
using Services.Interfaces;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LeQuangHuyWPF.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (SetProperty(ref _selectedCustomer, value))
                {
                    // When selection changes, copy values to edit properties
                    if (_selectedCustomer != null)
                    {
                        CustomerID = _selectedCustomer.CustomerID;
                        CustomerFullName = _selectedCustomer.CustomerFullName;
                        Telephone = _selectedCustomer.Telephone;
                        EmailAddress = _selectedCustomer.EmailAddress;
                        CustomerBirthday = _selectedCustomer.CustomerBirthday;
                        Password = _selectedCustomer.Password;
                    }

                    // Update command states
                    (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<Customer> _customers;
        public System.Collections.ObjectModel.ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }

        // Properties for adding/editing customer
        private int _customerID;
        public int CustomerID
        {
            get => _customerID;
            set => SetProperty(ref _customerID, value);
        }

        private string _customerFullName;
        public string CustomerFullName
        {
            get => _customerFullName;
            set => SetProperty(ref _customerFullName, value);
        }

        private string _telephone;
        public string Telephone
        {
            get => _telephone;
            set => SetProperty(ref _telephone, value);
        }

        private string _emailAddress;
        public string EmailAddress
        {
            get => _emailAddress;
            set => SetProperty(ref _emailAddress, value);
        }

        private System.DateTime _customerBirthday = System.DateTime.Now.AddYears(-18);
        public System.DateTime CustomerBirthday
        {
            get => _customerBirthday;
            set => SetProperty(ref _customerBirthday, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    (SearchCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // Commands
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand RefreshCommand { get; }

        public CustomerViewModel()
        {
            _customerService = new CustomerService();
            Customers = new System.Collections.ObjectModel.ObservableCollection<Customer>();

            AddCommand = new RelayCommand(ExecuteAdd);
            EditCommand = new RelayCommand(ExecuteEdit, CanExecuteEdit);
            DeleteCommand = new RelayCommand(ExecuteDelete, CanExecuteDelete);
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
            SearchCommand = new RelayCommand(ExecuteSearch, CanExecuteSearch);
            RefreshCommand = new RelayCommand(ExecuteRefresh);

            LoadCustomers();
        }

        private void LoadCustomers()
        {
            Customers.Clear();
            foreach (var customer in _customerService.GetCustomers())
            {
                Customers.Add(customer);
            }
        }

        private void ExecuteAdd(object obj)
        {
            // Clear form for new customer
            CustomerID = 0;
            CustomerFullName = string.Empty;
            Telephone = string.Empty;
            EmailAddress = string.Empty;
            CustomerBirthday = System.DateTime.Now.AddYears(-18);
            Password = string.Empty;

            // Show dialog to add new customer
            if (obj is System.Action<bool> showDialog)
            {
                showDialog(true);
            }
        }

        private bool CanExecuteEdit(object obj) => SelectedCustomer != null;

        private void ExecuteEdit(object obj)
        {
            // Values already copied in SelectedCustomer setter

            // Show dialog to edit customer
            if (obj is System.Action<bool> showDialog)
            {
                showDialog(true);
            }
        }

        private bool CanExecuteDelete(object obj) => SelectedCustomer != null;

        private void ExecuteDelete(object obj)
        {
            if (SelectedCustomer != null)
            {
                if (MessageBoxHelper.Confirm($"Are you sure you want to delete customer '{SelectedCustomer.CustomerFullName}'?"))
                {
                    try
                    {
                        _customerService.DeleteCustomer(SelectedCustomer.CustomerID);
                        Customers.Remove(SelectedCustomer);
                        MessageBoxHelper.ShowInfo("Customer deleted successfully.");
                    }
                    catch (System.Exception ex)
                    {
                        MessageBoxHelper.ShowError($"Error deleting customer: {ex.Message}");
                    }
                }
            }
        }

        private bool CanExecuteSave(object obj) =>
            !string.IsNullOrWhiteSpace(CustomerFullName) &&
            !string.IsNullOrWhiteSpace(Telephone) &&
            !string.IsNullOrWhiteSpace(EmailAddress) &&
            !string.IsNullOrWhiteSpace(Password);

        private void ExecuteSave(object obj)
        {
            try
            {
                var customer = new Customer
                {
                    CustomerID = CustomerID,
                    CustomerFullName = CustomerFullName,
                    Telephone = Telephone,
                    EmailAddress = EmailAddress,
                    CustomerBirthday = CustomerBirthday,
                    Password = Password,
                    CustomerStatus = 1 // Active
                };

                if (CustomerID == 0) // New customer
                {
                    _customerService.AddCustomer(customer);
                    Customers.Add(customer);
                    MessageBoxHelper.ShowInfo("Customer added successfully.");
                }
                else // Update existing customer
                {
                    _customerService.UpdateCustomer(customer);

                    // Find and update the customer in the collection
                    var index = Customers.IndexOf(SelectedCustomer);
                    if (index >= 0)
                    {
                        Customers[index] = customer;
                    }

                    MessageBoxHelper.ShowInfo("Customer updated successfully.");
                }

                // Close dialog
                if (obj is System.Action<bool> showDialog)
                {
                    showDialog(false);
                }
            }
            catch (System.Exception ex)
            {
                MessageBoxHelper.ShowError($"Error saving customer: {ex.Message}");
            }
        }

        private void ExecuteCancel(object obj)
        {
            // Close dialog
            if (obj is System.Action<bool> showDialog)
            {
                showDialog(false);
            }
        }

        private bool CanExecuteSearch(object obj) => !string.IsNullOrWhiteSpace(SearchText);

        private void ExecuteSearch(object obj)
        {
            Customers.Clear();
            foreach (var customer in _customerService.SearchCustomers(SearchText))
            {
                Customers.Add(customer);
            }
        }

        private void ExecuteRefresh(object obj)
        {
            SearchText = string.Empty;
            LoadCustomers();
        }
    }
}
