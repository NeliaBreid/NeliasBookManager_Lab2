using System.Windows;

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
