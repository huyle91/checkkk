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

namespace LeQuangHuyWPF.Views
{
    public partial class BookingDetailView : Window
    {
        public Action<bool> ShowDialogAction { get; set; }

        public BookingDetailView()
        {
            InitializeComponent();

            Loaded += BookingDetailView_Loaded;

            // Create an action that this dialog passes to the parent's show dialog action
            ShowDialogAction = show =>
            {
                if (!show)
                {
                    DialogResult = true;
                }
            };
        }

        private void BookingDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            // Set admin status in viewmodel if needed
            if (DataContext is BookingViewModel viewModel)
            {
                viewModel.IsAdmin = SessionManager.IsAdmin;
            }
        }
    }
}
