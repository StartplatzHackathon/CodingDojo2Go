using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using NUnit.Framework;

namespace CodingKata2Go.Infrastructure.Sandboxing
{
    public class Sandbox : MarshalByRefObject
    {
        private readonly string _sandboxApplicationBase;
        private static readonly string SandboxApplicationBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BaseDirectory);

        private const string BaseDirectory = "Untrusted";
        public const string DomainName = "Sandbox";

        public AppDomain AppDomain
        {
            get { return AppDomain.CurrentDomain; }
        }

        public Sandbox(string sandboxApplicationBase)
        {
            _sandboxApplicationBase = sandboxApplicationBase;
        }

        public static Sandbox Create()
        {
            var setup = new AppDomainSetup
                {
                    ApplicationBase = SandboxApplicationBase,
                    ApplicationName = DomainName,
                    DisallowBindingRedirects = true,
                    DisallowCodeDownload = true,
                    DisallowPublisherPolicy = true,
                };

            var permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

            AppDomain domain = AppDomain.CreateDomain(DomainName, null, setup, permissions, typeof (Sandbox).Assembly.Evidence.GetHostEvidence<StrongName>());

            var sandbox = (Sandbox)Activator.CreateInstanceFrom(domain, typeof(Sandbox).Assembly.ManifestModule.FullyQualifiedName, typeof(Sandbox).FullName, false, BindingFlags.CreateInstance, null, new[] { SandboxApplicationBase }, null, null).Unwrap();

            string nunitPath = typeof (TestAttribute).Assembly.Location;

            Directory.CreateDirectory(setup.ApplicationBase);
            CopyIfNotExists(nunitPath, Path.Combine(SandboxApplicationBase, Path.GetFileName(nunitPath)));
                
            return sandbox;
        }

        private static void CopyIfNotExists(string oldFileName, string newFileName)
        {
            if (!File.Exists(newFileName))
            {
                File.Copy(oldFileName, newFileName);
            }
        }

        public string Execute(string assemblyPath, string typeName, string method, params object[] parameters)
        {
            new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, assemblyPath).Assert();
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            CodeAccessPermission.RevertAssert();

            Type type = assembly.GetType(typeName);
            if (type == null)
                return null;

            object instance = Activator.CreateInstance(type);
            return string.Format("{0}", type.GetMethod(method).Invoke(instance, parameters));
        }

        public SandboxProxy GetProxy(string assemblyPath, string typeName)
        {
            new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, assemblyPath).Assert();
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            CodeAccessPermission.RevertAssert();

            Type type = assembly.GetType(typeName);
            if (type == null)
                return null;

            object instance = Activator.CreateInstance(type);
            return new SandboxProxy(instance);
        }

        public SandboxProxy GetProxyForInterface(string assemblyPath, string interfaceName)
        {
            new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, assemblyPath).Assert();
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            CodeAccessPermission.RevertAssert();

            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetInterface(interfaceName) != null)
                {
                    object instance = Activator.CreateInstance(type);
                    return new SandboxProxy(instance);
                }
            }

            return null;
        }

        public string RunNunitTest(string assemblyPath)
        {
            //var builder = new TestSuiteBuilder();
            //var testPackage = new TestPackage(filename);
            //var basePath = _sandboxApplicationBase;
            //testPackage.BasePath = basePath;

            //new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, filename).Assert();
            //TestSuite suite = builder.Build(testPackage);
            //CodeAccessPermission.RevertAssert();

            //TestResult result = suite.Run(new NullListener(), TestFilter.Empty);

            //return result.Message;

            new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, assemblyPath).Assert();
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            CodeAccessPermission.RevertAssert();

            var errors = new List<Exception>();

            foreach (Type type in assembly.GetTypes().Where(_ => _.GetCustomAttribute<TestFixtureAttribute>() != null))
            {
                try
                {
                    object instance = Activator.CreateInstance(type);

                    foreach (var method in type.GetMethods().Where(_ => _.GetCustomAttribute<TestAttribute>() != null))
                    {
                        try
                        {
                            method.Invoke(instance, null);
                        }
                        catch (TargetInvocationException invocationException)
                        {
                            if (invocationException.InnerException == null)
                            {
                                errors.Add(invocationException);
                            }
                            else
                            {
                                errors.Add(invocationException.InnerException);
                            }
                        }
                        catch (Exception testException)
                        {
                            errors.Add(testException);
                        }
                    }
                }
                catch (Exception testFixtureException)
                {
                    errors.Add(testFixtureException);
                }
            }

            return string.Join("\n\n", errors.Select(_ => _.ToString()));
        }
    }
}