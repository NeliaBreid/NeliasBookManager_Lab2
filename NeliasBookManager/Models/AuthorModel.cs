using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeliasBookManager.presentation.Viewmodel;

namespace NeliasBookManager.presentation.Models
{
    public class AuthorModel :ViewModelBase
    {
        //Kommer inte lägga så mycket här i G-uppgift, om ens något
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
        //public override string ToString()
        //{
        //    return $"{FirstName} {LastName}";
        //}
    }
}
