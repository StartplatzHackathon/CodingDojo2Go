using System.Collections.ObjectModel;
using CodingKata2Go.DataModel;
using CodingKata2Go.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CodingKata2Go.ViewModel
{
    public class WelcomeViewModel : ViewModelBase
    {
        private readonly UserService m_userService;

        public WelcomeViewModel()
        {
            m_userService = ServiceLocator.Current.GetInstance<UserService>();
            AddUserCommand = new RelayCommand(m_userService.AddUser);
            StartExerciseCommand = new RelayCommand(StartExercise);
        }

        public ObservableCollection<User> Users
        {
            get { return m_userService.Users; }
        }

        public RelayCommand AddUserCommand { get; private set; }

        public RelayCommand StartExerciseCommand { get; private set; }


        public void StartExercise()
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof (DojoPage));
        }
    }
}