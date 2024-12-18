using NeliasBookManager.Domain.ModelsDb;
using NeliasBookManager.Infrastructure.Data;
using NeliasBookManager.presentation.Models;

namespace NeliasBookManager.presentation.Services
{
    public class InventoryRepository
    {
        public InventoryRepository()
        { 
        
        }
    
    
       public void AddSaldoToDataBase(BookModel book, StoreModel store, int? quantityToAlter)
        {
            using var context = new NeliasBokHandelContext();

            var newSaldoDb = new LagerSaldo
            {
                Isbn = book.Isbn13,
                ButikId = store.Id,
                AntalBöcker = quantityToAlter
            };

            context.LagerSaldos.Add(newSaldoDb);
            context.SaveChanges();
            
            
        }
        public void RemoveSaldofromDataBase(BookModel book, StoreModel store, int? quantityToAlter)
        {
            using var context = new NeliasBokHandelContext();

            var newSaldoDb = new LagerSaldo
            {
                Isbn = book.Isbn13,
                ButikId = store.Id,
                AntalBöcker = quantityToAlter
            };

            context.LagerSaldos.Remove(newSaldoDb);
            context.SaveChanges();

        }
        public void UpdateExistingSaldo(LagerSaldo existingSaldo, StoreModel store) //uppdaterar saldo, om det redan finns ett saldo
        {
            using var context = new NeliasBokHandelContext();
            var dbSaldo = context.LagerSaldos
                .FirstOrDefault(ls => ls.Isbn == existingSaldo.Isbn && ls.ButikId == store.Id);

            if (dbSaldo != null)
            {
                dbSaldo.AntalBöcker = existingSaldo.AntalBöcker;
                context.SaveChanges();
            }
        }
    }
}
