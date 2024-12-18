using BookButikAdminLab2.Command;
using Labb2DataAcess.Services;
using NeliasBookManager.presentation.Dialogs;

namespace NeliasBookManager.presentation.Viewmodel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly BookRepository _bookRepository;
       
        public BookViewModel BookViewModel { get; set; }
        public StoreViewModel StoreViewModel { get; set; }

        public RelayCommand OpenAddBooksDialogCommand { get; }
        public RelayCommand OpenRemoveBooksDialogCommand { get; }

        public MainWindowViewModel(BookRepository bookRepo, BookViewModel bookViewModel, StoreViewModel storeViewModel)
        {   
            BookViewModel = bookViewModel; 
            StoreViewModel = storeViewModel;

            _bookRepository = bookRepo;

            OpenAddBooksDialogCommand = new RelayCommand(OpenAddBookDialog);
            OpenRemoveBooksDialogCommand = new RelayCommand(OpenRemoveBooksDialog);
        }

        private void OpenAddBookDialog(object obj)
        {
            AddBooksDialog createNewDialog = new AddBooksDialog();
            _bookRepository.GetAllBooks();
            createNewDialog.ShowDialog();
        }
        private void OpenRemoveBooksDialog(object obj)
        {
            RemoveBooksDialog createNewDialog = new RemoveBooksDialog();
            _bookRepository.GetAllBooks();
            createNewDialog.ShowDialog();
        }
    }
}

