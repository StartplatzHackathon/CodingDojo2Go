﻿using GalaSoft.MvvmLight;

namespace CodingKata2Go.DataModel
{
    public enum CodeState
    {
        Pass,
        Fail
    }

    public class DojoState : ObservableObject
    {
        private CodeState? m_codeState;
        private bool m_isCodeEnabled;

        private bool m_isTestEnabled;
        private bool m_isFightEnabled;
        private bool m_isFightBackEnabled;

        public bool IsCodeEnabled
        {
            get { return m_isCodeEnabled; }
            set { Set(() => IsCodeEnabled, ref m_isCodeEnabled, value); }
        }

        public bool IsTestEnabled
        {
            get { return m_isTestEnabled; }
            set { Set(() => IsTestEnabled, ref m_isTestEnabled, value); }
        }

        public bool IsFightEnabled
        {
            get { return m_isFightEnabled; }
            set { Set(() => IsFightEnabled, ref m_isFightEnabled, value); }
        }

        public bool IsFightBackEnabled
        {
            get { return m_isFightBackEnabled; }
            set { Set(() => IsFightBackEnabled, ref m_isFightBackEnabled, value); }
        }

        public CodeState? CodeState
        {
            get { return m_codeState; }
            set { Set(() => CodeState, ref m_codeState, value); }
        }
    }
}