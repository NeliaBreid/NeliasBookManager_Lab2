using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeliasBookManager.Domain.ModelsDb;
using NeliasBookManager.presentation.Viewmodel;

namespace NeliasBookManager.presentation.Models
{
    class BookModel: ViewModelBase
    {
        private int _price ;
        private string? _title ;
        private string _isbn13 ;
        private string? _language;
        private int _publishingDate;
        private ObservableCollection<InventoryBalanceModel> _amountInStore;
        private ObservableCollection<AuthorModel> _authors;

        public string Isbn13
        {
            get => _isbn13;
            set
            {
                _isbn13 = value;
                RaisePropertyChanged(nameof(Isbn13));
            }
        }

        public string? Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        public string? Language //oklart om behövs
        {
            get => _language;
            set
            {
                _language = value;
                RaisePropertyChanged(nameof(Language));
            }
        }

        public int Price
        {
            get => _price;
            set
            {
                _price = value;
                RaisePropertyChanged(nameof(Price));
            }
        }

        public int PublishingDate
        {
            get => _publishingDate;
            set
            {
                _publishingDate = value;
                RaisePropertyChanged(nameof(PublishingDate));
            }
        }
        public ObservableCollection<InventoryBalanceModel> AmountInStore
        {
            get => _amountInStore;
            set
            {
                _amountInStore = value;
                RaisePropertyChanged(nameof(AmountInStore));
            }
        }
        public ObservableCollection<AuthorModel> Authors
        {
            get => _authors;
            set
            {
                _authors = value;
                RaisePropertyChanged(nameof(Isbn13));
            }
        }

    }
}
