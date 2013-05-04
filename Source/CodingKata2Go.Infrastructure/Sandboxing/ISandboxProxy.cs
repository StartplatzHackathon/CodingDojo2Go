namespace CodingKata2Go.Infrastructure.Sandboxing
{
    public interface ISandboxProxy
    {
        T InvokeMethod<T>(string method, params object[] parameters);
    }
}