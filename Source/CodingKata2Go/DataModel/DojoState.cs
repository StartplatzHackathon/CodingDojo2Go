using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingKata2Go.DataModel
{
    public enum CodeState
    {
        Pass,
        Fail
    }

    public class DojoState : ObservableObject
    {
        private string m_testName;
        private string m_codeName;
        private bool m_isCodeEnabled;
        private bool m_isUserEnabled;
        private bool m_isFightEnabled;
        private bool m_isFightBackEnabled;
        private CodeState? m_codeState;

        public string TestName
        {
            get { return m_testName; }
            set
            {
                if (m_testName != value)
                {
                    m_testName = value;
                    RaisePropertyChanged(() => TestName);
                }
            }
        }

        public string CodeName
        {
            get { return m_codeName; }
            set
            {
                if (m_codeName != value)
                {
                    m_codeName = value;
                    RaisePropertyChanged(() => CodeName);
                }
            }
        }


        public bool IsCodeEnabled
        {
            get { return m_isCodeEnabled; }
            set
            {
                if (m_isCodeEnabled != value)
                {
                    m_isCodeEnabled = value;
                    RaisePropertyChanged(() => IsCodeEnabled);
                }
            }
        }

        public bool IsUserEnabled
        {
            get { return m_isUserEnabled; }
            set
            {
                if (m_isUserEnabled != value)
                {
                    m_isUserEnabled = value;
                    RaisePropertyChanged(() => IsUserEnabled);
                }
            }
        }

        public bool IsFightEnabled
        {
            get { return m_isFightEnabled; }
            set
            {
                if (m_isFightEnabled != value)
                {
                    m_isFightEnabled = value;
                    RaisePropertyChanged(() => IsFightEnabled);
                }
            }
        }

        public bool IsFightBackEnabled
        {
            get { return m_isFightBackEnabled; }
            set
            {
                if (m_isFightBackEnabled != value)
                {
                    m_isFightBackEnabled = value;
                    RaisePropertyChanged(() => IsFightBackEnabled);
                }
            }
        }

        public CodeState? CodeState
        {
            get { return m_codeState; }
            set
            {
                if (m_codeState != value)
                {
                    m_codeState = value;
                    RaisePropertyChanged(() => CodeState);
                }
            }
        }

    }
}
