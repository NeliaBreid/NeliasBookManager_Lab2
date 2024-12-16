using System.Collections.ObjectModel;
using BookButikAdminLab2.Command;
using NeliasBookManager.Domain.ModelsDb;
using NeliasBookManager.Infrastructure.Data;
using NeliasBookManager.presentation.Dialogs;
using NeliasBookManager.presentation.Models;

namespace NeliasBookManager.presentation.Viewmodel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<StoreModel> Stores { get; set; }
        public ObservableCollection<BookModel> AllBooks { get; set; }
        public ObservableCollection<AuthorModel> Authors { get; set; }

        public ObservableCollection<BookModel> _booksInStore;

        private StoreModel? _activeStore; 

        private BookModel? _activeBook;

        private int? _quantityToAdd = 0;
        public int? QuantityToAlter
        {
            get => _quantityToAdd;
            set
            {
                if (value < 0) _quantityToAdd = 0;
                else _quantityToAdd = value;
                RaisePropertyChanged(nameof(ActiveStore));
            }
        }
        public BookModel? ActiveBook
        {
            get => _activeBook;
            set
            {
                _activeBook = value;
                RaisePropertyChanged();
            }
        }
        public StoreModel? ActiveStore
        {
            get => _activeStore;
            set
            {
                _activeStore = value;
                BooksInStore.Clear();
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

        public RelayCommand OpenAddBooksDialogCommand { get; }
        public RelayCommand OpenRemoveBooksDialogCommand { get; }
        public RelayCommand AddBookToStoreCommand { get; }
        public RelayCommand RemoveBookFromStoreCommand { get; }
        public MainWindowViewModel()
        {
            Stores = new ObservableCollection<StoreModel>(); //mina butiker, 3 st
            BooksInStore = new ObservableCollection<BookModel>();
            AllBooks = new ObservableCollection<BookModel>();
            Authors = new ObservableCollection<AuthorModel>();
            OpenAddBooksDialogCommand = new RelayCommand(OpenAddBookDialog);
            OpenRemoveBooksDialogCommand = new RelayCommand(OpenRemoveBooksDialog);
            AddBookToStoreCommand = new RelayCommand(AddBookToSaldo);
            RemoveBookFromStoreCommand = new RelayCommand(RemoveBookFromStoreSaldo);

            LoadStores(); //den här ska vara async sen
            
        }

        private void AddBookToSaldo(object obj)
        {
            if (ActiveBook != null)
            {
                var existingSaldo = ActiveStore.IventoryBalances.FirstOrDefault(ib => ib.Isbn13 == ActiveBook.Isbn13);
                var bookInStore = BooksInStore.FirstOrDefault(b => b.Isbn13 == ActiveBook.Isbn13); //kolla om boken finns

                if (QuantityToAlter <= 0 || QuantityToAlter == null) //Om det inte finns någon bok eller kvantiteten är underlikamed noll
                {
                    return;
                }
                else if (bookInStore == null) //om boken inte redan finns
                {
                    AddNewBookToDataBase();
                }
                else
                {
                    existingSaldo.Quantity += QuantityToAlter;
                    UpdateExistingSaldo(existingSaldo);// Uppdaterar saldot om den har ett saldot finns
                }
            }
        }
        private void UpdateExistingSaldo(InventoryBalanceModel existingSaldo) //uppdaterar saldo, om det redan finns ett saldo
        {
            using var context = new NeliasBokHandelContext();

            var dbSaldo = context.LagerSaldos
                .FirstOrDefault(ls => ls.Isbn == existingSaldo.Isbn13 && ls.ButikId == ActiveStore.Id);

            if (dbSaldo != null)
            {
                dbSaldo.AntalBöcker = existingSaldo.Quantity;
                context.SaveChanges();
                GetBooksInActiveStore(dbSaldo.ButikId); //hämtar böckerna
            }
        }
        private void AddNewBookToDataBase()
        {
            using var context = new NeliasBokHandelContext();

            var newSaldoDb = new LagerSaldo
            {
                Isbn = ActiveBook.Isbn13,
                ButikId = ActiveStore.Id,
                AntalBöcker = QuantityToAlter
            };

            var newInventoryBalance = new InventoryBalanceModel
            {
                Isbn13 = ActiveBook.Isbn13,
                Quantity = QuantityToAlter,
                StoreId = ActiveStore.Id,
                Store = ActiveStore
            };

            ActiveStore.IventoryBalances.Add(newInventoryBalance);

            context.LagerSaldos.Add(newSaldoDb);
            context.SaveChanges();

            GetBooksInActiveStore(ActiveStore.Id);
        }
        private void RemoveBookFromStoreSaldo(object obj)
        {
            if (ActiveBook != null)
            {
                var existingSaldo = ActiveStore.IventoryBalances.FirstOrDefault(ib => ib.Isbn13 == ActiveBook.Isbn13);
               // var bookInStore = BooksInStore.FirstOrDefault(b => b.Isbn13 == ActiveBook.Isbn13);

                // Validering av input
                if (QuantityToAlter <= 0 || QuantityToAlter == null || existingSaldo == null) //Om det inte finns någon bok eller kvantiteten är underlikamed noll
                {
                    return; //fångar om booken inte finns i butiken, om man inte väljer kvantitet
                }

                else if (existingSaldo.Quantity - QuantityToAlter == 0) //om boken inte redan finns
                {
                    RemoveBookfromDataBase();
                }
                else if (existingSaldo.Quantity - QuantityToAlter >= 0)//inte kunna ta minusvärden
                {
                    existingSaldo.Quantity -= QuantityToAlter;
                    UpdateExistingSaldo(existingSaldo);
                }

            }
        }

        public void RemoveBookfromDataBase()
        {
            using var context = new NeliasBokHandelContext();

            var newSaldoDb = new LagerSaldo
            {
                Isbn = ActiveBook.Isbn13,
                ButikId = ActiveStore.Id,
                AntalBöcker = QuantityToAlter
            };
            var newInventoryBalance = new InventoryBalanceModel
            {
                Isbn13 = ActiveBook.Isbn13,
                Quantity = QuantityToAlter,
                StoreId = ActiveStore.Id,
                Store = ActiveStore
            };

            ActiveStore.IventoryBalances.Remove(newInventoryBalance);

            context.LagerSaldos.Remove(newSaldoDb);
            context.SaveChanges();

            GetBooksInActiveStore(ActiveStore.Id);
        }

        
        private void OpenAddBookDialog(object obj)
        {
            AddBooksDialog createNewDialog = new AddBooksDialog();
            GetAllBooks();
            createNewDialog.ShowDialog();

        }
        private void OpenRemoveBooksDialog(object obj)
        {
            RemoveBooksDialog createNewDialog = new RemoveBooksDialog();
            GetAllBooks();
            createNewDialog.ShowDialog();

        }
        public void LoadStores() //laddar in alla butiker och lägger dom i Stores, sätt dit en await
        {
            using var context = new NeliasBokHandelContext();

            var storesFromDb = context.Butikers.Select(
                s => new StoreModel()
                {
                    Name = s.ButikNamn,
                    Id = s.ButikId,
                    IventoryBalances = s.LagerSaldos
                     .Select(ib => new InventoryBalanceModel()
                     {
                         Isbn13 = ib.Isbn,
                         Quantity = ib.AntalBöcker,
                         StoreId = ib.ButikId
                     }).ToList()
                }
            ).ToList();
            Stores = new ObservableCollection<StoreModel>(storesFromDb);
            if (Stores.Any()) ActiveStore = Stores.First();
        }

        public void GetAllBooks()
        {
            using var context = new NeliasBokHandelContext();

            var books = context.Böckers.Select(
                b => new BookModel()
                {
                    Isbn13 = b.Isbn,
                    Title = b.Titel,
                    Price = b.Pris,
                    PublishingDate = b.Utgivningsår,
                    Authors = new ObservableCollection<AuthorModel>(
                        b.Författars.Select(a => new AuthorModel()
                        {
                            Id = a.FörfattarId,
                            FirstName = a.Förnamn,
                            LastName = a.Efternamn
                        }).ToList()
                      ),
                    AmountInStore = new ObservableCollection<InventoryBalanceModel>(
                b.LagerSaldos.Select(ls => new InventoryBalanceModel()
                {
                    StoreId = ls.ButikId,
                    Quantity = ls.AntalBöcker,
                    Isbn13 = ls.Isbn
                }).ToList()
            )
                }
            ).ToList();

            // Här kan du hantera 'books', till exempel tilldela det till en property
            AllBooks = new ObservableCollection<BookModel>(books);

        }
        
        public void GetBooksInActiveStore(int storeId) //TODO:lägga typ allBooks och något kriterie på butikID
        {
            using var context = new NeliasBokHandelContext();

            var BooksPerStore = context.Böckers.Where(b => b.LagerSaldos.Any(ls => ls.ButikId == storeId)).
                Select(
                b => new BookModel()
                {
                    Isbn13 = b.Isbn,
                    Title = b.Titel,
                    Price = b.Pris,
                    PublishingDate = b.Utgivningsår,
                    Authors = new ObservableCollection<AuthorModel>(
                        b.Författars.Select(a => new AuthorModel()
                        {
                            Id = a.FörfattarId,
                            FirstName = a.Förnamn,
                            LastName = a.Efternamn
                        }).ToList()),
                    AmountInStore = new ObservableCollection<InventoryBalanceModel>(
                b.LagerSaldos
                .Where(ls => ls.ButikId == storeId)
                .Select(ls => new InventoryBalanceModel()
                {
                    StoreId = ls.ButikId,
                    Quantity = ls.AntalBöcker,
                    Isbn13 = ls.Isbn
                }).ToList()
                )

                }).ToList();

            BooksInStore = new ObservableCollection<BookModel>(BooksPerStore);
        }


    }

}

