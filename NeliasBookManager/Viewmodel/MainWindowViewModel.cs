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

namespace NeliasBookManager.presentation.Viewmodel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<StoreModel> Stores { get; set; }
        public ObservableCollection<BookModel> AllBooks { get; set; }
        public ObservableCollection<AuthorModel> Authors { get; set; }

        private ObservableCollection<BookModel> _booksInStore;

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

        public MainWindowViewModel()
        {
            Stores = new ObservableCollection<StoreModel>(); //mina butiker, 3 st
            BooksInStore = new ObservableCollection<BookModel>();
            AllBooks = new ObservableCollection<BookModel>();
            Authors = new ObservableCollection<AuthorModel>();

            LoadStores(); //den här ska vara async sen
        }

    }
    }

