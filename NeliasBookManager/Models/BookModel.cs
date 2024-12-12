using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeliasBookManager.presentation.Viewmodel;

namespace NeliasBookManager.presentation.Models
{
    class BookModel: ViewModelBase
    {
        private int _price = 0;
        private string? _title = "Untitled";
        private string _isbn13 = null!;
        private string? _language = "unknown";
        private int _publishingDate = 1000;
        private int _amountInStore = 1;

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
        public int AmountInStore
        {
            get => _amountInStore;
            set
            {
                _amountInStore = value;
                RaisePropertyChanged(nameof(AmountInStore));
            }
        }

    }
}
