using System.Windows;
using Labb2DataAcess.Services;
using NeliasBookManager.presentation.Services;
using NeliasBookManager.presentation.Viewmodel;

namespace NeliasBookManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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