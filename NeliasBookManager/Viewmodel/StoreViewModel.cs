using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookButikAdminLab2.Command;
using Labb2DataAcess.Services;
using NeliasBookManager.Domain.ModelsDb;
using NeliasBookManager.Infrastructure.Data; //rensa upp usings
using NeliasBookManager.presentation.Models;
using NeliasBookManager.presentation.Services;

namespace NeliasBookManager.presentation.Viewmodel
{
    class StoreViewModel : ViewModelBase
    {
        public BookModel? ActiveBook { get => _bookViewModel?.ActiveBook; }
        public ObservableCollection<StoreModel> Stores { get; set; }

        private StoreModel? _activeStore;

        public ObservableCollection<BookModel> _booksInStore;

        private readonly NeliasBokHandelContext _context;

        private readonly BookViewModel _bookViewModel;

        private readonly StoreRepository _storeRepository;

        private readonly BookRepository _bookRepository;

        private readonly InventoryRepository _inventoryRepository;

        private int? _quantityToAlter = 1;
        public int? QuantityToAlter
        {
            get => _quantityToAlter;
            set
            {
                if (value < 0) _quantityToAlter = 0;
                else _quantityToAlter = value;
                RaisePropertyChanged(nameof(ActiveStore));
            }
        }
        public StoreModel? ActiveStore
        {
            get => _activeStore;
            set
            {
                _activeStore = value;
                GetBooksInActiveStore(ActiveStore.Id);

                RaisePropertyChanged(nameof(ActiveStore));
                RaisePropertyChanged(nameof(BooksInStore));
            }
        }

        public ObservableCollection<BookModel> BooksInStore
        {
            get => _booksInStore;
            set
            {
                _booksInStore = value;
                RaisePropertyChanged(nameof(BooksInStore));
            }
        }
        public RelayCommand AddBookToStoreCommand { get; }
        public RelayCommand RemoveBookFromStoreCommand { get; }
        public StoreViewModel(StoreRepository storeRepo, BookRepository bookRepo, InventoryRepository inventoryrepo, BookViewModel bookViewModel)
        {

            this._bookViewModel = bookViewModel;
            _inventoryRepository = inventoryrepo;
            _storeRepository = storeRepo;
            _bookRepository = bookRepo;
            _context = new NeliasBokHandelContext();

            Stores = new ObservableCollection<StoreModel>(_storeRepository.LoadStores());
            ActiveStore = Stores.First(); //TODO: inte jättesnyggt at ha den här 
            BooksInStore = new ObservableCollection<BookModel>(_bookRepository.GetBooksByIsbn(ActiveStore.Id));

            AddBookToStoreCommand = new RelayCommand(AddBookToSaldoCommand);
            RemoveBookFromStoreCommand = new RelayCommand(RemoveBookFromSaldoCommand);

        }
        private void GetBooksInActiveStore(int butikId) //Gör en metod som sätter propertyn BooksinStore
        {
            BooksInStore = new ObservableCollection<BookModel>(_bookRepository.GetBooksByIsbn(butikId));
            RaisePropertyChanged(nameof(BooksInStore));
        }

        private void AddBookToSaldoCommand(object obj)
        {
            if (ActiveBook != null)
            {
                var existingSaldo = _context.LagerSaldos.FirstOrDefault(ib => ib.Isbn == ActiveBook.Isbn13 && ib.ButikId == ActiveStore.Id);

                if (QuantityToAlter <= 0 || QuantityToAlter == null) //Om det inte finns någon bok eller kvantiteten är underlikamed noll
                {
                    return;
                }

                else if (existingSaldo == null) //om boken inte redan finns
                {
                    _inventoryRepository.AddSaldoToDataBase(ActiveBook, ActiveStore, QuantityToAlter);
                    GetBooksInActiveStore(ActiveStore.Id);
                }

                else
                {
                    existingSaldo.AntalBöcker += QuantityToAlter;
                    _inventoryRepository.UpdateExistingSaldo(existingSaldo, ActiveStore);
                    GetBooksInActiveStore(ActiveStore.Id);
                }
            }
        }
        private void RemoveBookFromSaldoCommand(object obj)
        {
            if (ActiveBook != null)
            {

                var existingSaldo = _context.LagerSaldos.FirstOrDefault(ib => ib.Isbn == ActiveBook.Isbn13 && ib.ButikId == ActiveStore.Id);


                if (QuantityToAlter <= 0 || QuantityToAlter == null || existingSaldo == null) //Om det inte finns någon bok eller kvantiteten är underlikamed noll
                {
                    return;
                }

                else if (existingSaldo.AntalBöcker - QuantityToAlter == 0) //om boken inte redan finns
                {
                    _inventoryRepository.RemoveSaldofromDataBase(ActiveBook, ActiveStore, QuantityToAlter);
                    GetBooksInActiveStore(ActiveStore.Id);
                }

                else if (existingSaldo.AntalBöcker - QuantityToAlter >= 0)//inte kunna ta minusvärden
                {
                    existingSaldo.AntalBöcker -= QuantityToAlter;
                    _inventoryRepository.UpdateExistingSaldo(existingSaldo, ActiveStore);
                    GetBooksInActiveStore(ActiveStore.Id);
                }
            }
        }
    }
}
