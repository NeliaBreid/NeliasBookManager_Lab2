using System.Collections.ObjectModel;
using Labb2DataAcess.Services;
using NeliasBookManager.presentation.Models;

namespace NeliasBookManager.presentation.Viewmodel
{
    class BookViewModel:ViewModelBase
    {
        private readonly BookRepository _bookRepository;
        public ObservableCollection<BookModel> AllBooks { get; set; }

        private BookModel? _activeBook;
       
        public BookModel? ActiveBook
        {
            get => _activeBook;
            set
            {
                _activeBook = value;
                RaisePropertyChanged();
            }
        }
        public BookViewModel(BookRepository bookRepos)
        {
            _bookRepository = bookRepos;

            AllBooks = new ObservableCollection<BookModel>(_bookRepository.GetAllBooks());
        }

    }
}

