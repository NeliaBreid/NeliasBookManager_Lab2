using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeliasBookManager.presentation.Viewmodel;

namespace NeliasBookManager.presentation.Models
{
    class StoreModel: ViewModelBase
    {
        public string Name { get; set; } = null!;

        public int Id { get; set; }

        public virtual List<InventoryBalanceModel> IventoryBalances { get; set; } = new();
    }
}
