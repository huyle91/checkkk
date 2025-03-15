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
    /// <summary>
    /// Interaction logic for RoomDetailView.xaml
    /// </summary>
    public partial class RoomDetailView : Window
    {
        public Action<bool> ShowDialogAction { get; set; }

        public RoomDetailView()
        {
            InitializeComponent();

            // Create an action that this dialog passes to the parent's show dialog action
            ShowDialogAction = show =>
            {
                if (!show)
                {
                    DialogResult = true;
                }
            };
        }
    }
}
