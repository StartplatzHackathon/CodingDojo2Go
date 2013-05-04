using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingKata2Go.DataModel;

namespace CodingKata2Go.Services
{
    public class UserService
    {
        private int m_userIndex = 3;

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
    }
}
