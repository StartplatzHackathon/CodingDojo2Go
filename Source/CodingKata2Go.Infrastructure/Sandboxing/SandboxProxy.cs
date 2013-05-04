using System;

namespace CodingKata2Go.Infrastructure.Sandboxing
{
    public class SandboxProxy : MarshalByRefObject, ISandboxProxy
    {
        private readonly object _instance;

        public SandboxProxy(object instance)
        {
            _instance = instance;
        }

        public T InvokeMethod<T>(string method, params object[] parameters)
        {
            Type type = _instance.GetType();

            return (T)type.GetMethod(method).Invoke(_instance, parameters);
        }
    }
}