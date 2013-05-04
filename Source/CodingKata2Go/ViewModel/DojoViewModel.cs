using System.Collections.ObjectModel;
using System.Linq;
using CodingKata2Go.DataModel;
using CodingKata2Go.Services;
using CodingKata2Go.WebServiceReferences;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;

namespace CodingKata2Go.ViewModel
{
    public class DojoViewModel : ViewModelBase
    {
        private readonly DojoStateMachine m_dojoStateMachine;
        private readonly UserService m_userService;
        private ObservableCollection<CompileError> _compileErrors;
        private ObservableCollection<TestError> _testErrors;
        private string m_code;
        private string m_test;

        public DojoViewModel()
        {
            m_dojoStateMachine = ServiceLocator.Current.GetInstance<DojoStateMachine>();
            m_dojoStateMachine.AddAndActivate(new DojoState
                {
                    IsTestEnabled = true,
                    IsFightEnabled = true
                });
            m_userService = ServiceLocator.Current.GetInstance<UserService>();
            FightCommand = new RelayCommand(Fight);
            FightBackCommand = new RelayCommand(FightBack);
            VerifyCommand = new RelayCommand(Verify);

            _compileErrors = new ObservableCollection<CompileError>();
            _testErrors = new ObservableCollection<TestError>();
        }

        public string Test
        {
            get { return m_test; }
            set { Set(() => Test, ref m_test, value); }
        }

        public string Code
        {
            get { return m_code; }
            set { Set(() => Code, ref m_code, value); }
        }

        public RelayCommand FightCommand { get; private set; }
        public RelayCommand FightBackCommand { get; private set; }
        public RelayCommand VerifyCommand { get; private set; }

        public ObservableCollection<CompileError> CompileErrors
        {
            get { return _compileErrors; }
            set { Set(() => CompileErrors, ref _compileErrors, value); }
        }

        public ObservableCollection<TestError> TestErrors
        {
            get { return _testErrors; }
            set { Set(() => TestErrors, ref _testErrors, value); }
        }

        public void Fight()
        {
            var nextState = new DojoState
                {
                    IsCodeEnabled = true
                };
            m_dojoStateMachine.AddAndActivate(nextState);
        }

        public void FightBack()
        {
            m_dojoStateMachine.AddAndActivate(new DojoState
                {
                    IsTestEnabled = true,
                    IsFightEnabled = true
                });
            m_userService.SwitchUsers();
        }

        public async void Verify()
        {
            var client = new ServiceClient();
            CompileAndTestResult result = await client.CompileAndTestAsync(new KataRequest());

            if (result.CompileErrors.Any() || result.TestErrors.Any())
            {
                var nextState = new DojoState
                    {
                        CodeState = CodeState.Fail,
                        IsCodeEnabled = true
                    };
                m_dojoStateMachine.AddAndActivate(nextState);
            }
            else
            {
                var nextState = new DojoState
                    {
                        CodeState = CodeState.Pass,
                        IsCodeEnabled = true,
                        IsFightBackEnabled = true
                    };
                m_dojoStateMachine.AddAndActivate(nextState);
            }

            CompileErrors = new ObservableCollection<CompileError>(result.CompileErrors ?? new CompileError[0]);
            TestErrors = new ObservableCollection<TestError>(result.TestErrors ?? new TestError[0]);
        }
    }
}