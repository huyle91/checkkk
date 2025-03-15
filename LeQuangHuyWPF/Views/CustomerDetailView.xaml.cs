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

namespace LeQuangHuyWPF.Views
{
    public partial class CustomerDetailView : Window
    {
        public Action<bool> ShowDialogAction { get; set; }

        public CustomerDetailView()
        {
            InitializeComponent();

            Loaded += CustomerDetailView_Loaded;

            // Create an action that this dialog passes to the parent's show dialog action
            ShowDialogAction = show =>
            {
                if (!show)
                {
                    DialogResult = true;
                }
            };
        }

        private void CustomerDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the password box value from the viewmodel when loaded
            if (DataContext is CustomerViewModel viewModel)
            {
                txtPassword.Password = viewModel.Password;

                // Handle password changes
                txtPassword.PasswordChanged += (s, args) =>
                {
                    viewModel.Password = txtPassword.Password;
                };
            }
        }
    }
}
