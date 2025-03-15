using BusinessObjects;
using LeQuangHuyWPF.Utils;
using LeQuangHuyWPF.ViewModels;
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
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginWindow()
        {
            InitializeComponent();

            _viewModel = new LoginViewModel();
            DataContext = _viewModel;

            // Handle password box (can't bind directly with MVVM)
            txtPassword.PasswordChanged += TxtPassword_PasswordChanged;

            // Handle login success
            _viewModel.OnLoginSuccess += ViewModel_OnLoginSuccess;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = txtPassword.Password;
        }

        private void ViewModel_OnLoginSuccess(Customer user)
        {
            // Open appropriate window based on user role
            if (SessionManager.IsAdmin)
            {
                // Open admin window
                var adminWindow = new AdminWindow();
                adminWindow.Show();
            }
            else
            {
                // Open customer window
                var customerWindow = new CustomerWindow();
                customerWindow.Show();
            }

            // Close login window
            Close();
        }
    }
}
