using CodingKata2Go.DataModel;
using CodingKata2Go.Services;
using GalaSoft.MvvmLight;
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
            m_dojoStateMachine.Add(new DojoState
                {
                    IsCodeEnabled = true,
                    IsFightEnabled = true
                });
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
    }
}