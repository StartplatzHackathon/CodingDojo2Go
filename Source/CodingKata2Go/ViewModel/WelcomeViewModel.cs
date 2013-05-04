using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingKata2Go.DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CodingKata2Go.ViewModel
{
    public class WelcomeViewModel : ViewModelBase
    {
        private int m_userIndex = 3;

        private string m_excercise;

        public WelcomeViewModel()
        {
            Users = new ObservableCollection<User>();
            Users.Add(new User { Title = "User 1" });
            Users.Add(new User { Title = "User 2" });

            AddUserCommand = new RelayCommand(AddUser);
        }

        public ObservableCollection<User> Users { get; private set; }

        public string Excercise
        {
            get { return m_excercise; }
            set
            {
                if (m_excercise != value)
                {
                    m_excercise = value;
                    RaisePropertyChanged(() => Excercise);
                }
            }
        }

        public RelayCommand AddUserCommand { get; private set; }

        public void AddUser()
        {
            Users.Add(new User{ Title = "User " + m_userIndex});
            m_userIndex++;
        }
    }
}
