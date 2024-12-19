using System.Windows;

namespace NeliasBookManager.presentation.Dialogs
{
    /// <summary>
    /// Interaction logic for AddBooksDialog.xaml
    /// </summary>
    public partial class AddBooksDialog : Window
    {
        public AddBooksDialog()
        {
            InitializeComponent();
            DataContext = App.Current.MainWindow.DataContext;
        }

        private void Close_click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
