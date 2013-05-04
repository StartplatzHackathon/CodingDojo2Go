using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace CodingKata2Go.DataModel
{
    public class User : ObservableObject
    {
        private string m_title;
        private string m_username;

        public string Title
        {
            get { return m_title; }
            set
            {
                if (m_title != value)
                {
                    m_title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        public string Username
        {
            get { return m_username; }
            set
            {
                if (m_username != value)
                {
                    m_username = value;
                    RaisePropertyChanged(() => Username);
                }
            }
        }
    }
}