using LeQuangHuyWPF.Utils;
using LeQuangHuyWPF.ViewModels;
using LeQuangHuyWPF.Views;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private CustomerViewModel _customerViewModel;
        private RoomViewModel _roomViewModel;
        private BookingViewModel _bookingViewModel;

        // Dialog windows
        private CustomerDetailView _customerDialog;
        private RoomDetailView _roomDialog;
        private BookingDetailView _bookingDialog;

        public AdminWindow()
        {
            InitializeComponent();

            // Initialize ViewModels
            _customerViewModel = new CustomerViewModel();
            _roomViewModel = new RoomViewModel();
            _bookingViewModel = new BookingViewModel();

            // Create a view model for this window
            DataContext = new AdminWindowViewModel
            {
                CustomerViewModel = _customerViewModel,
                RoomViewModel = _roomViewModel,
                BookingViewModel = _bookingViewModel,

                // Bind dialog actions
                ShowCustomerDialog = ShowCustomerDialog,
                ShowRoomDialog = ShowRoomDialog,
                ShowBookingDialog = ShowBookingDialog
            };
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

        private void ShowCustomerDialog(bool show)
        {
            if (show)
            {
                if (_customerDialog == null || !_customerDialog.IsVisible)
                {
                    _customerDialog = new CustomerDetailView
                    {
                        Owner = this,
                        DataContext = _customerViewModel
                    };
                    _customerDialog.ShowDialog();
                }
                else
                {
                    _customerDialog.Activate();
                }
            }
            else if (_customerDialog != null && _customerDialog.IsVisible)
            {
                _customerDialog.Close();
            }
        }

        private void ShowRoomDialog(bool show)
        {
            if (show)
            {
                if (_roomDialog == null || !_roomDialog.IsVisible)
                {
                    _roomDialog = new RoomDetailView
                    {
                        Owner = this,
                        DataContext = _roomViewModel
                    };
                    _roomDialog.ShowDialog();
                }
                else
                {
                    _roomDialog.Activate();
                }
            }
            else if (_roomDialog != null && _roomDialog.IsVisible)
            {
                _roomDialog.Close();
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

    public class AdminWindowViewModel : ViewModelBase
    {
        public CustomerViewModel CustomerViewModel { get; set; }
        public RoomViewModel RoomViewModel { get; set; }
        public BookingViewModel BookingViewModel { get; set; }

        public Action<bool> ShowCustomerDialog { get; set; }
        public Action<bool> ShowRoomDialog { get; set; }
        public Action<bool> ShowBookingDialog { get; set; }
    }
}
