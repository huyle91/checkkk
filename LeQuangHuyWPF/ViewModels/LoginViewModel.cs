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
    public class LoginViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand CancelCommand { get; }

        public delegate void LoginSuccessHandler(Customer user);
        public event LoginSuccessHandler OnLoginSuccess;

        public LoginViewModel()
        {
            _customerService = new CustomerService();
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private bool CanExecuteLogin(object obj)
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
        }

        private void ExecuteLogin(object obj)
        {
            Customer user = _customerService.Login(Email, Password);

            if (user != null)
            {
                SessionManager.CurrentUser = user;
                OnLoginSuccess?.Invoke(user);
            }
            else
            {
                ErrorMessage = "Invalid email or password. Please try again.";
            }
        }

        private void ExecuteCancel(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
