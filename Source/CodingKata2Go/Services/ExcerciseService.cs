using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace CodingKata2Go.Services
{
    public class ExcerciseService : ObservableObject
    {
        private string m_excercise;

        public string Excercise
        {
            get { return m_excercise; }
            set
            {
                if (m_excercise != value)
                {
                    m_excercise = value;
                    RaisePropertyChanged(() => Excercise);
                }
            }
        }
    }
}
