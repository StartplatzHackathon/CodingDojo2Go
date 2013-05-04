using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingKata2Go.DataModel;
using GalaSoft.MvvmLight;

namespace CodingKata2Go.Services
{
    public class UserService : ObservableObject
    {
        private int m_userIndex = 3;
        private int m_actionIndex;

        public UserService()
        {
            Users = new ObservableCollection<User> {new User {Title = "User 1"}, new User {Title = "User 2"}};
        }

        public ObservableCollection<User> Users { get; private set; }

        public void AddUser()
        {
            Users.Add(new User { Title = "User " + m_userIndex });
            m_userIndex++;
        }

        public User TestUser
        {
            get { return GetUser(m_actionIndex); }
        }

        public User CodeUser
        {
            get { return GetUser(m_actionIndex + 1); }
        }

        private User GetUser(int index)
        {
            var i = index;
            while (i >= Users.Count)
            {
                i -= Users.Count;
            }
            return Users[i];
        }

        public void SwitchUsers()
        {
            m_actionIndex++;
        }
    }
}
