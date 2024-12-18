using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Labb2DataAcess.Services;
using NeliasBookManager.Infrastructure.Data;
using NeliasBookManager.presentation.Services;
using NeliasBookManager.presentation.Viewmodel;

namespace NeliasBookManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal MainWindow()
        {
            InitializeComponent();

            var storeRepository = new StoreRepository();
            var bookRepository = new BookRepository();
            var inventoryRepository = new InventoryRepository();

            var bookViewModel = new BookViewModel(bookRepository);
            var storeViewModel = new StoreViewModel(storeRepository, bookRepository, inventoryRepository, bookViewModel);
            var mainWindowViewModel = new MainWindowViewModel(bookRepository, bookViewModel, storeViewModel);


            DataContext = mainWindowViewModel;
            
        }

    }
}