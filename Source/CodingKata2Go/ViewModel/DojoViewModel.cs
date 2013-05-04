using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace CodingKata2Go.ViewModel
{
    public class DojoViewModel : ViewModelBase
    {
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
