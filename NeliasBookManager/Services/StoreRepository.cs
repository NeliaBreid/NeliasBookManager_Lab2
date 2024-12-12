using NeliasBookManager.Infrastructure.Data;
using NeliasBookManager.presentation.Models;

namespace Labb2DataAcess.Services;

public class StoreRepository
{
    private readonly NeliasBokHandelContext _context;

    public StoreRepository(NeliasBokHandelContext context)
    {
        _context = context;
    }


    public void GetAllStores()
    {
       
        var stores = _context.Butikers
            .Select
            (
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

    }

    


}