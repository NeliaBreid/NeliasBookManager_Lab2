using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using NeliasBookManager.Infrastructure.Data;
using NeliasBookManager.presentation.Models;

namespace Labb2DataAcess.Services;

public class BookRepository
{
    public BookRepository()
    {
       
    }
    public List<BookModel> GetBooksByIsbn(int storeId) //TODO:lägga typ allBooks och något kriterie på butikID
    {
        using var context = new NeliasBokHandelContext();

        

        var booksPerStore = context.Böckers.Where(b => b.LagerSaldos.Any(ls => ls.ButikId == storeId)).
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
        return booksPerStore;
     
    }
    public List<BookModel> GetAllBooks()
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

        return books;

    }


}