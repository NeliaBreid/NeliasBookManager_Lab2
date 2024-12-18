using System.Collections.ObjectModel;
using BookButikAdminLab2.Command;
using Labb2DataAcess.Services;
using NeliasBookManager.Domain.ModelsDb;
using NeliasBookManager.Infrastructure.Data;
using NeliasBookManager.presentation.Dialogs;
using NeliasBookManager.presentation.Models;

namespace NeliasBookManager.presentation.Viewmodel
{//Här finns commands för att öppna dialogrutorna
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly BookRepository _bookRepository; //TODO:strukturera upp alla properties
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

