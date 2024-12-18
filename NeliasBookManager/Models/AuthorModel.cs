using NeliasBookManager.presentation.Viewmodel;

namespace NeliasBookManager.presentation.Models
{
    public class AuthorModel :ViewModelBase
    {
        private string _firstName;
        private string _lastName;
        public int Id { get; set; }
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }
        public string FullName => $"{FirstName} {LastName}";
    }
}
