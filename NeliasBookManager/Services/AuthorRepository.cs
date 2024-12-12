
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using NeliasBookManager.Infrastructure.Data;
using NeliasBookManager.presentation.Models;

namespace Labb2DataAcess.Services;

public class AuthorRepository
{
    private readonly NeliasBokHandelContext _context;

    public AuthorRepository(NeliasBokHandelContext context)
    {
        _context = context;
    }

    //public List<AuthorModel> GetAllAuthors()
    //{
    //    var authors = _context.Authors
    //        .Select
    //        (
    //            a => new AuthorModel()
    //            {
    //                Id = a.Id,
    //                FirstName = a.FirstName,
    //                LastName = a.LastName,
    //                Birthday = a.Birthday,
    //                Isbn13s = new ObservableCollection<BookModel>
    //                (
    //                    a.Isbn13s
    //                        .Select
    //                        (
    //                            b => new BookModel()
    //                            {
    //                                Isbn13 = b.Isbn13,
    //                                Title = b.Title,
    //                                Language = b.Language,
    //                                Price = b.Price,
    //                                PublishingDate = b.PublishingDate,
    //                                Authors = new ObservableCollection<AuthorModel>
    //                                (
    //                                    b.Authors
    //                                        .Select
    //                                        (
    //                                            a => new AuthorModel()
    //                                            {
    //                                                Id = a.Id,
    //                                                Birthday = a.Birthday,
    //                                                FirstName = a.FirstName,
    //                                                LastName = a.LastName
    //                                            }
    //                                        ).ToList()
    //                                )

    //                            }
    //                        )
    //                )
    //            }
    //        ).ToList();
    //    return authors;
    //}

}