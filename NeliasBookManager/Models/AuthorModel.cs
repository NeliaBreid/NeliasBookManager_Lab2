﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeliasBookManager.presentation.Viewmodel;

namespace NeliasBookManager.presentation.Models
{
    class AuthorModel :ViewModelBase
    {
        //Kommer inte lägga så mycket här i G-uppgift, om ens något
        private string _firstName = "Egon";
        private string _lastName = "Egonsson";

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
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
