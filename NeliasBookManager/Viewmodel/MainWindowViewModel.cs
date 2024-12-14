using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookButikAdminLab2.Command;
using Labb2DataAcess.Services;
using Microsoft.EntityFrameworkCore;
using NeliasBookManager.Domain.ModelsDb;
using NeliasBookManager.Infrastructure.Data;
using NeliasBookManager.presentation.Dialogs;
using NeliasBookManager.presentation.Models;
using static System.Reflection.Metadata.BlobBuilder;

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

        private int? _quantityToAdd;
        public int? QuantityToAdd
        {
            get => _quantityToAdd;
            set
            {
                _quantityToAdd = value;
                RaisePropertyChanged(nameof(ActiveStore));
                GetBooksInStore();

            }
        }
        public BookModel? ActiveBook
        {
            get => _activeBook;
            set
            {
                _activeBook = value;
                RaisePropertyChanged();

                //TODO: Varje gång man byter bok så uppdateras boken? eller sätta propertychanged
            }
        }
        public StoreModel? ActiveStore
        {
            get => _activeStore;
            set
            {
                _activeStore = value;
                BooksInStore.Clear();
                TestGetBooksInStore(ActiveStore.Id);
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

        public RelayCommand OpenAddBooksCommand { get; }
        public RelayCommand OpenRemoveBooksDialogCommand { get; }
        public RelayCommand AddBookToStoreCommand { get; }
        public RelayCommand RemoveBookFromStoreCommand { get; }
        public MainWindowViewModel()
        {
            Stores = new ObservableCollection<StoreModel>(); //mina butiker, 3 st
            BooksInStore = new ObservableCollection<BookModel>();
            AllBooks = new ObservableCollection<BookModel>();
            Authors = new ObservableCollection<AuthorModel>();
            OpenAddBooksCommand = new RelayCommand(OpenAddBookDialog);
            OpenRemoveBooksDialogCommand = new RelayCommand(OpenRemoveBooksDialog);
            AddBookToStoreCommand = new RelayCommand(AddBookToSaldo);
            RemoveBookFromStoreCommand = new RelayCommand(RemoveBookFromStore);

            LoadStores(); //den här ska vara async sen
            
        }
       //private bool CanAddBookToSaldo(object? arg)
       // {
       //     return ActiveBook != null && ActiveStore != null;
       // }
        private void AddBookToSaldo(object obj)
        {
        //    var newBook = new BookModel() //Skapar en ny bok som är en kopia av activebook
        //    {
        //        Isbn13 = ActiveBook.Isbn13,
        //        Title = ActiveBook.Title,
        //        Price = ActiveBook.Price,
        //        PublishingDate = ActiveBook.PublishingDate,
        //        Authors = new ObservableCollection<AuthorModel>(
        //          ActiveBook.Authors.Select(a => new AuthorModel
        //          {
        //              Id = a.Id,
        //              FirstName = a.FirstName,
        //              LastName = a.LastName
        //          })
        //      ),
        //        AmountInStore = new ObservableCollection<InventoryBalanceModel>
        //{
        //    new InventoryBalanceModel
        //    {
        //        Isbn13 = ActiveBook.Isbn13,
        //        Quantity = QuantityToAdd,
        //        StoreId = ActiveStore.Id,
        //        Store = ActiveStore,
        //        Isbn13Navigation = ActiveBook
        //    }
        //}
        //    };

            using var context = new NeliasBokHandelContext();
            var existingSaldo = ActiveStore.IventoryBalances.FirstOrDefault(ib => ib.Isbn13 == ActiveBook.Isbn13);

            var bookInStore = BooksInStore.FirstOrDefault(b => b.Isbn13 == ActiveBook.Isbn13); //kolla om boken finns


            if (ActiveBook == null || QuantityToAdd <= 0) //Om det inte finns någon bok eller kvantiteten är underlikamed noll
            {
                return;
            }
            else if (bookInStore == null)
            {
                //AddNewBookAndSaldo(newBook);
            }
            else
            {
                existingSaldo.Quantity += QuantityToAdd;
                UpdateExistingSaldo(existingSaldo);// Uppdaterar saldot om den har ett saldot finns
            }

        }
        private void UpdateExistingSaldo(InventoryBalanceModel existingSaldo) //uppdaterar saldo, om det redan finns ett saldo
        {
            if (existingSaldo != null)
            {
                using var context = new NeliasBokHandelContext();

                // Hämta rätt saldo från databasen
                var dbSaldo = context.LagerSaldos
                    .FirstOrDefault(ls => ls.Isbn == existingSaldo.Isbn13 && ls.ButikId == ActiveStore.Id);

                if (dbSaldo != null)
                {
                    // Uppdatera både det lokala objektet och databasen
                   // existingSaldo.Quantity += QuantityToAdd;
                    dbSaldo.AntalBöcker = existingSaldo.Quantity;
                    context.SaveChanges();
                    GetBooksInStore();
                    TestGetBooksInStore(dbSaldo.ButikId);
                    RaisePropertyChanged(nameof(BooksInStore)); //kanske är överflödigt?
                }
            }
            
        }
        //private void AddNewBookAndSaldo(BookModel book)
        //{
        //    using var context = new NeliasBokHandelContext();

        //    var newInventoryBalance = new InventoryBalanceModel
        //    {
        //        Isbn13 = book.Isbn13,
        //        Quantity = QuantityToAdd,
        //        StoreId = ActiveStore.Id,
        //        Store = ActiveStore
        //    };

        //    var newBookDb = new Böcker
        //    {
        //        Isbn = book.Isbn13,
        //        Titel = book.Title,
        //        Pris = book.Price,
        //        Utgivningsår = book.PublishingDate
        //    };

        //    var newSaldoDb = new LagerSaldo
        //    {
        //        Isbn = book.Isbn13,
        //        ButikId = ActiveStore.Id,
        //        AntalBöcker = QuantityToAdd
        //    };

        //    // Lägg till i context
        //    context.Böckers.Add(newBookDb);
        //    context.LagerSaldos.Add(newSaldoDb);
        //    //context.SaveChanges();

        //    // Uppdatera lokalt
        //    ActiveStore.IventoryBalances.Add(newInventoryBalance);
        //    BooksInStore.Add(book);
        //    RaisePropertyChanged(nameof(BooksInStore));
        //}
        private void RemoveBookFromStore(object obj)
        {
            using var context = new NeliasBokHandelContext();

            // Hämta existerande saldo och bok
            var existingSaldo = ActiveStore.IventoryBalances.FirstOrDefault(ib => ib.Isbn13 == ActiveBook.Isbn13);
            var bookInStore = BooksInStore.FirstOrDefault(b => b.Isbn13 == ActiveBook.Isbn13);

            // Validering av input
            if (ActiveBook == null || QuantityToAdd <= 0 || existingSaldo == null || bookInStore == null)
            {
                return; 
            }

            // Uppdatera saldot
            if (bookInStore != null)
            {
                existingSaldo.Quantity -= QuantityToAdd;
                UpdateExistingSaldo(existingSaldo);
            }
            //else
            //{
            //    // Om kvantiteten når 0, ta bort boken
            //    ActiveStore.IventoryBalances.Remove(existingSaldo);
            //    BooksInStore.Remove(bookInStore);
            //    context.LagerSaldos.Remove(
            //        context.LagerSaldos.SingleOrDefault(ls => ls.Isbn == ActiveBook.Isbn13 && ls.ButikId == ActiveStore.Id));
            //    context.Böckers.Remove(
            //        context.Böckers.SingleOrDefault(b => b.Isbn == ActiveBook.Isbn13));

            //    RaisePropertyChanged(nameof(BooksInStore));
            //}

            // Spara ändringar i databasen

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
        public void LoadStores() //laddar in alla butiker och lägger dom i Stores
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
        public void GetBooksInStore() //TODO:lägga typ allBooks och något kriterie på butikID
        {
            using var context = new NeliasBokHandelContext();

            var CurrentStoreId = ActiveStore.Id;

            var BooksPerStore = context.Böckers.Where(b => b.LagerSaldos.Any(ls => ls.ButikId == CurrentStoreId)).
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
                b.LagerSaldos.Select(ls => new InventoryBalanceModel()
                {
                    StoreId = ls.ButikId,
                    Quantity = ls.AntalBöcker,
                    Isbn13 = ls.Isbn
                }).ToList()
                )
                
                }).ToList();
            
            BooksInStore = new ObservableCollection<BookModel>(BooksPerStore);
        }
        public void TestGetBooksInStore(int storeId) //TODO:lägga typ allBooks och något kriterie på butikID
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

