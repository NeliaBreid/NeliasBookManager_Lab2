using NeliasBookManager.Infrastructure.Data;
using NeliasBookManager.presentation.Models;

namespace Labb2DataAcess.Services;

public class StoreRepository
{
    public StoreRepository()
    {

    }

    public List<StoreModel> LoadStores()
    {
        using var context = new NeliasBokHandelContext();

        var storesFromDb = context.Butikers
            .Select(s => new StoreModel()
            {
                Name = s.ButikNamn,
                Id = s.ButikId,
                InventoryBalances = s.LagerSaldos
                 .Select(ib => new InventoryBalanceModel()
                 {
                     Isbn13 = ib.Isbn,
                     Quantity = ib.AntalBöcker,
                     StoreId = ib.ButikId
                 }).ToList()
            }
            ).ToList();

        return storesFromDb;
    }
}