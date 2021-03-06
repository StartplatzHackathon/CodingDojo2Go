﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using CodingKata2Go.Sandbox.Model;
using NUnit.Framework;

namespace CodingKata2Go.Sandbox
{
    public class Sandboxer : MarshalByRefObject
    {
        private static readonly string SandboxApplicationBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                             BaseDirectory);

        private const string BaseDirectory = "Untrusted";
        public const string DomainName = "Sandboxer";

        public AppDomain AppDomain
        {
            get { return AppDomain.CurrentDomain; }
        }

        public static Sandboxer Create()
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

            AppDomain domain = AppDomain.CreateDomain(DomainName, null, setup, permissions,
                                                      typeof (Sandboxer).Assembly.Evidence.GetHostEvidence<StrongName>());

            var sandbox =
                (Sandboxer)
                Activator.CreateInstanceFrom(domain, typeof (Sandboxer).Assembly.ManifestModule.FullyQualifiedName, typeof (Sandboxer).FullName)
                .Unwrap();

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
            new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, assemblyPath)
                .Assert();
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            CodeAccessPermission.RevertAssert();

            Type type = assembly.GetType(typeName);
            if (type == null)
                return null;

            object instance = Activator.CreateInstance(type);
            return string.Format("{0}", type.GetMethod(method).Invoke(instance, parameters));
        }

        public List<TestError> RunNunitTest(string assemblyPath)
        {
            new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, assemblyPath)
                .Assert();
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            CodeAccessPermission.RevertAssert();

            var errors = new List<TestError>();

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
                                errors.Add(invocationException.ToTestError(type.Name, method.Name));
                            }
                            else
                            {
                                errors.Add(invocationException.InnerException.ToTestError(type.Name, method.Name));
                            }
                        }
                        catch (Exception testException)
                        {
                            errors.Add(testException.ToTestError(type.Name, method.Name));
                        }
                    }
                }
                catch (Exception testFixtureException)
                {
                    errors.Add(testFixtureException.ToTestError(type.Name));
                }
            }

            return errors;
        }
    }

    public static class ExceptionExtension
    {
        public static TestError ToTestError(this Exception exc, string className, string methodName = null)
        {
            return new TestError
                {
                    ErrorMessage = exc.Message,
                    StackTrace = exc.StackTrace,
                    TestClass = className,
                    TestMethod = methodName
                };
        }

    }
}