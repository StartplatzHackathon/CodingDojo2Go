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
            set { Set(() => Title, ref m_title, value); }
        }

        public string Username
        {
            get { return m_username; }
            set { Set(() => Username, ref m_username, value); }
        }
    }
}