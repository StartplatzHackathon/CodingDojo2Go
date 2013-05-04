namespace CodingKata2Go.Infrastructure.Model
{
    public class CompileError
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string ErrorNumber { get; set; }
        public string ErrorText { get; set; }
        public bool IsWarning { get; set; }
    }
}