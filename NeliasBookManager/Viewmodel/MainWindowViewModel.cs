using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NeliasBookManager.Domain.ModelsDb;
using NeliasBookManager.Infrastructure.Data;
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

        public BookModel? _activeBook;
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
                RaisePropertyChanged(nameof(ActiveStore));
                GetBooksInStore();

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


        public MainWindowViewModel()
        {
            Stores = new ObservableCollection<StoreModel>(); //mina butiker, 3 st
            BooksInStore = new ObservableCollection<BookModel>();
            AllBooks = new ObservableCollection<BookModel>();
            Authors = new ObservableCollection<AuthorModel>();

            LoadStores(); //den här ska vara async sen
            
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
        public void GetBooksInStore()
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


    }
    }

