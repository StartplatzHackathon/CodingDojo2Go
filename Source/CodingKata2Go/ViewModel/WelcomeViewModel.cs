using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingKata2Go.DataModel;
using CodingKata2Go.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
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
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(DojoPage));
        }
    }
}
