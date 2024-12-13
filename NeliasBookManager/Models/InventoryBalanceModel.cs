using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NeliasBookManager.presentation.Viewmodel;

namespace NeliasBookManager.presentation.Models
{
    class InventoryBalanceModel: ViewModelBase
    {
        private int? _quantity;
        public int StoreId { get; set; }
        public string Isbn13 { get; set; } = null!;

        public int? Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                RaisePropertyChanged(nameof(Quantity));

            }
        }

        public virtual BookModel Isbn13Navigation { get; set; } = null!;
        public virtual StoreModel Store { get; set; } = null!;

    }
}
