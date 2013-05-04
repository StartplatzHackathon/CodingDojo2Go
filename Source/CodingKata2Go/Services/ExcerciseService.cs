using GalaSoft.MvvmLight;

namespace CodingKata2Go.Services
{
    public class ExcerciseService : ObservableObject
    {
        private string m_excercise;

        public string Excercise
        {
            get { return m_excercise; }
            set { Set(() => Excercise, ref m_excercise, value); }
        }
    }
}