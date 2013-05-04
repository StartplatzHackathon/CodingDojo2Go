using CodingKata2Go.DataModel;
using CodingKata2Go.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;

namespace CodingKata2Go.ViewModel
{
    public class DojoViewModel : ViewModelBase
    {
        private readonly DojoStateMachine m_dojoStateMachine;
        private string m_code;
        private string m_test;

        public DojoViewModel()
        {
            m_dojoStateMachine = ServiceLocator.Current.GetInstance<DojoStateMachine>();
            m_dojoStateMachine.AddAndActivate(new DojoState{ IsTestEnabled = true, IsFightEnabled = true});
            m_userService = ServiceLocator.Current.GetInstance<UserService>();
            FightCommand = new RelayCommand(Fight);
            FightBackCommand = new RelayCommand(FightBack);
            VerifyCommand = new RelayCommand(Verify);
        }

        public string Test
        {
            get { return m_test; }
            set { Set(() => Test, ref m_test, value); }
        }

        private readonly UserService m_userService;
        public string Code
        {
            get { return m_code; }
            set { Set(() => Code, ref m_code, value); }
        }

        public RelayCommand FightCommand { get; private set; }
        public RelayCommand FightBackCommand { get; private set; }
        public RelayCommand VerifyCommand { get; private set; }

        public void Fight()
        {
            var nextState = new DojoState { IsCodeEnabled = true };
            m_dojoStateMachine.AddAndActivate(nextState);
        }

        public void FightBack()
        {
            m_dojoStateMachine.AddAndActivate(new DojoState { IsTestEnabled = true, IsFightEnabled = true });
            m_userService.SwitchUsers();
        }

        public void Verify()
        {
            // Todo: Webrequest.
            var nextState = new DojoState { CodeState = CodeState.Pass, IsCodeEnabled = true, IsFightBackEnabled = true };
            m_dojoStateMachine.AddAndActivate(nextState);
        }
    }
}