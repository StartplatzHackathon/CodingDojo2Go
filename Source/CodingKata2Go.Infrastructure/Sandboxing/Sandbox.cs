using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace CodingKata2Go.Infrastructure.Sandboxing
{
    public class Sandbox : MarshalByRefObject
    {
        const string BaseDirectory = "Untrusted";
        public const string DomainName = "Sandbox";

        public Sandbox()
        {
        }

        public AppDomain AppDomain
        {
            get { return AppDomain.CurrentDomain; }
        }

        public static Sandbox Create()
        {
            var setup = new AppDomainSetup()
            {
                ApplicationBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BaseDirectory),
                ApplicationName = DomainName,
                DisallowBindingRedirects = true,
                DisallowCodeDownload = true,
                DisallowPublisherPolicy = true
            };

            var permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

            var domain = AppDomain.CreateDomain(DomainName, null, setup, permissions,
                typeof(Sandbox).Assembly.Evidence.GetHostEvidence<StrongName>());

            return (Sandbox)Activator.CreateInstanceFrom(domain, typeof(Sandbox).Assembly.ManifestModule.FullyQualifiedName, typeof(Sandbox).FullName).Unwrap();
        }

        public string Execute(string assemblyPath, string typeName, string method, params object[] parameters)
        {
            new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, assemblyPath).Assert();
            var assembly = Assembly.LoadFile(assemblyPath);
            CodeAccessPermission.RevertAssert();

            Type type = assembly.GetType(typeName);
            if (type == null)
                return null;

            var instance = Activator.CreateInstance(type);
            return string.Format("{0}", type.GetMethod(method).Invoke(instance, parameters));
        }

        public SandboxProxy GetProxy(string assemblyPath, string typeName)
        {
            new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, assemblyPath).Assert();
            var assembly = Assembly.LoadFile(assemblyPath);
            CodeAccessPermission.RevertAssert();

            Type type = assembly.GetType(typeName);
            if (type == null)
                return null;

            var instance = Activator.CreateInstance(type);
            return new SandboxProxy(instance);
        }

        public SandboxProxy GetProxyForInterface(string assemblyPath, string interfaceName)
        {
            new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, assemblyPath).Assert();
            var assembly = Assembly.LoadFile(assemblyPath);
            CodeAccessPermission.RevertAssert();

            foreach (var type in assembly.GetTypes())
            {
                if (type.GetInterface(interfaceName) != null)
                {
                    var instance = Activator.CreateInstance(type);
                    return new SandboxProxy(instance);
                }
            }

            return null;
        }
    }
}
