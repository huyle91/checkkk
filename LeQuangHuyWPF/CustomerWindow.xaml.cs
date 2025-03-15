using BusinessObjects;
using LeQuangHuyWPF.Utils;
using LeQuangHuyWPF.ViewModels;
using Services.Interfaces;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LeQuangHuyWPF
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private readonly ICustomerService _customerService;
        private BookingViewModel _bookingViewModel;
        private BookingDetailView _bookingDialog;

        public CustomerWindow()
        {
            InitializeComponent();

            _customerService = new CustomerService();
            _bookingViewModel = new BookingViewModel();

            // Create a view model for this window
            var viewModel = new CustomerWindowViewModel
            {
                BookingViewModel = _bookingViewModel,
                ShowBookingDialog = ShowBookingDialog
            };

            // Load customer data
            if (SessionManager.CurrentUser != null)
            {
                viewModel.CustomerID = SessionManager.CurrentUser.CustomerID;
                viewModel.CustomerFullName = SessionManager.CurrentUser.CustomerFullName;
                viewModel.EmailAddress = SessionManager.CurrentUser.EmailAddress;
                viewModel.Telephone = SessionManager.CurrentUser.Telephone;
                viewModel.CustomerBirthday = SessionManager.CurrentUser.CustomerBirthday;
                viewModel.CurrentUserName = SessionManager.CurrentUser.CustomerFullName;

                // Set the password box
                txtPassword.Password = SessionManager.CurrentUser.Password;
            }

            DataContext = viewModel;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxHelper.Confirm("Are you sure you want to log out?"))
            {
                // Clear current session
                SessionManager.CurrentUser = null;

                // Show login window
                var loginWindow = new LoginWindow();
                loginWindow.Show();

                // Close this window
                Close();
            }
        }

        private void ShowBookingDialog(bool show)
        {
            if (show)
            {
                if (_bookingDialog == null || !_bookingDialog.IsVisible)
                {
                    _bookingDialog = new BookingDetailView
                    {
                        Owner = this,
                        DataContext = _bookingViewModel
                    };
                    _bookingDialog.ShowDialog();
                }
                else
                {
                    _bookingDialog.Activate();
                }
            }
            else if (_bookingDialog != null && _bookingDialog.IsVisible)
            {
                _bookingDialog.Close();
            }
        }
    }

    public class CustomerWindowViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;

        public CustomerWindowViewModel()
        {
            _customerService = new CustomerService();
            SaveProfileCommand = new RelayCommand(ExecuteSaveProfile, CanExecuteSaveProfile);
        }

        // Profile properties
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
            set
            {
                if (SetProperty(ref _customerFullName, value))
                {
                    (SaveProfileCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _emailAddress;
        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                if (SetProperty(ref _emailAddress, value))
                {
                    (SaveProfileCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _telephone;
        public string Telephone
        {
            get => _telephone;
            set
            {
                if (SetProperty(ref _telephone, value))
                {
                    (SaveProfileCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private DateTime _customerBirthday;
        public DateTime CustomerBirthday
        {
            get => _customerBirthday;
            set => SetProperty(ref _customerBirthday, value);
        }

        private string _currentUserName;
        public string CurrentUserName
        {
            get => _currentUserName;
            set => SetProperty(ref _currentUserName, value);
        }

        // Booking ViewModel
        public BookingViewModel BookingViewModel { get; set; }

        // Commands
        public ICommand SaveProfileCommand { get; }

        // Dialog actions
        public Action<bool> ShowBookingDialog { get; set; }

        private bool CanExecuteSaveProfile(object obj)
        {
            return !string.IsNullOrWhiteSpace(CustomerFullName) &&
                   !string.IsNullOrWhiteSpace(EmailAddress) &&
                   !string.IsNullOrWhiteSpace(Telephone);
        }

        private void ExecuteSaveProfile(object obj)
        {
            try
            {
                // Get the password from the PasswordBox (passed as parameter)
                string password = "";
                if (obj is string pass)
                {
                    password = pass;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBoxHelper.ShowError("Password cannot be empty.");
                    return;
                }

                // Update customer data
                var customer = new Customer
                {
                    CustomerID = CustomerID,
                    CustomerFullName = CustomerFullName,
                    EmailAddress = EmailAddress,
                    Telephone = Telephone,
                    CustomerBirthday = CustomerBirthday,
                    Password = password,
                    CustomerStatus = 1 // Active
                };

                _customerService.UpdateCustomer(customer);

                // Update session data
                SessionManager.CurrentUser = customer;

                // Update display name
                CurrentUserName = customer.CustomerFullName;

                MessageBoxHelper.ShowInfo("Profile updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError($"Error updating profile: {ex.Message}");
            }
        }
    }
}
