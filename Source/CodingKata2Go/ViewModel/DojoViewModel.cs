using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingKata2Go.DataModel;
using CodingKata2Go.Services;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;

namespace CodingKata2Go.ViewModel
{
    public class DojoViewModel : ViewModelBase
    {
        public DojoViewModel()
        {
            m_dojoStateMachine = ServiceLocator.Current.GetInstance<DojoStateMachine>();
            m_dojoStateMachine.Add(new DojoState{ IsCodeEnabled = true, IsFightEnabled = true});
        }

        private string m_test;

        public string Test
        {
            get { return m_test; }
            set
            {
                if (m_test != value)
                {
                    m_test = value;
                    RaisePropertyChanged(() => Test);
                }
            }
        }

        private string m_code;
        private readonly DojoStateMachine m_dojoStateMachine;

        public string Code
        {
            get { return m_code; }
            set
            {
                if (m_code != value)
                {
                    m_code = value;
                    RaisePropertyChanged(() => Code);
                }
            }
        }
    }
}
