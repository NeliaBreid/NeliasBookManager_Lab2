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

namespace NeliasBookManager.presentation.Dialogs
{
    /// <summary>
    /// Interaction logic for RemoveBooksDialog.xaml
    /// </summary>
    public partial class RemoveBooksDialog : Window
    {
        public RemoveBooksDialog()
        {
            InitializeComponent();
            DataContext = App.Current.MainWindow.DataContext;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
