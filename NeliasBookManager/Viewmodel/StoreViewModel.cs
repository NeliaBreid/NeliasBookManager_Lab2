using System.Collections.ObjectModel;
using BookButikAdminLab2.Command;
using Labb2DataAcess.Services;
using NeliasBookManager.Infrastructure.Data;
using NeliasBookManager.presentation.Models;
using NeliasBookManager.presentation.Services;

namespace NeliasBookManager.presentation.Viewmodel
{
    class StoreViewModel : ViewModelBase
    {
        private readonly StoreRepository _storeRepository;
        private readonly BookRepository _bookRepository;
        private readonly InventoryRepository _inventoryRepository;

        private readonly BookViewModel _bookViewModel;

        public BookModel? ActiveBook { get => _bookViewModel?.ActiveBook; }
        public ObservableCollection<StoreModel> Stores { get; set; }

        private StoreModel? _activeStore;

        public ObservableCollection<BookModel> _booksInStore;

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

            Stores = new ObservableCollection<StoreModel>(_storeRepository.LoadStores());
            ActiveStore = Stores.First();
            BooksInStore = new ObservableCollection<BookModel>(_bookRepository.GetBooksByIsbn(ActiveStore.Id));

            AddBookToStoreCommand = new RelayCommand(AddBookToSaldoCommand);
            RemoveBookFromStoreCommand = new RelayCommand(RemoveBookFromSaldoCommand);
        }
        private void GetBooksInActiveStore(int butikId)
        {
            BooksInStore = new ObservableCollection<BookModel>(_bookRepository.GetBooksByIsbn(butikId));
            RaisePropertyChanged(nameof(BooksInStore));
        }

        private void AddBookToSaldoCommand(object obj)
        {
            using var context = new NeliasBokHandelContext();

            if (ActiveBook != null)
            {
                var existingSaldo = context.LagerSaldos.FirstOrDefault(ib => ib.Isbn == ActiveBook.Isbn13 && ib.ButikId == ActiveStore.Id);

                if (QuantityToAlter <= 0 || QuantityToAlter == null) 
                {
                    return;
                }

                else if (existingSaldo == null) 
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
            using var context = new NeliasBokHandelContext();

            if (ActiveBook != null)
            {
                var existingSaldo = context.LagerSaldos.FirstOrDefault(ib => ib.Isbn == ActiveBook.Isbn13 && ib.ButikId == ActiveStore.Id);

                if (QuantityToAlter <= 0 || QuantityToAlter == null || existingSaldo == null)
                {
                    return;
                }

                else if (existingSaldo.AntalBöcker - QuantityToAlter == 0)
                {
                    _inventoryRepository.RemoveSaldofromDataBase(ActiveBook, ActiveStore, QuantityToAlter);
                    GetBooksInActiveStore(ActiveStore.Id);
                }

                else if (existingSaldo.AntalBöcker - QuantityToAlter >= 0)
                {
                    existingSaldo.AntalBöcker -= QuantityToAlter;
                    _inventoryRepository.UpdateExistingSaldo(existingSaldo, ActiveStore);
                    GetBooksInActiveStore(ActiveStore.Id);
                }
            }
        }
    }
}
