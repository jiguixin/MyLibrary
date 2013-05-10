/*
 *名称：NinjectContainer
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-10 17:32:40
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Infrastructure.Crosscutting.IoC;
using Ninject;

namespace Infrastructure.CrossCutting.IoC.Ninject
{
    public sealed class NinjectContainer
    {
        public IInstanceLocator Locator;

        private readonly object lockObject = new object();

        public IKernel Resolver { get; private set; }

        public NinjectContainer WireDependenciesInAssemblyMatching(string assemblyName)
        {
            ScanAssembliesUsing(x => x.Load(Assembly.Load(assemblyName)));
            return this;
        }

        public NinjectContainer WireDependenciesInAssemblies(params string[] assemblies)
        {
            ScanAssembliesUsing(x => x.Load(assemblies.Select(Assembly.Load)));
            return this;
        }

        public NinjectContainer WireDependenciesInAssemblies(IEnumerable<string> assemblies)
        {
            ScanAssembliesUsing(x => x.Load(assemblies.Select(Assembly.Load)));
            return this;
        }

        private void ScanAssembliesUsing(Action<StandardKernel> applytoKernel)
        {
            if (DontHaveAResolverYet)
            {
                lock (lockObject)
                {
                    if (DontHaveAResolverYet)
                    {
                        var kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false, InjectNonPublic = true });

                        applytoKernel(kernel);

                        SetKernel(kernel);
                    }
                }
            }
        }

        private void SetKernel(IKernel kernel)
        {
            Locator = new InstanceLocatorNinject(kernel); 
            Resolver = kernel;
        }

        private bool DontHaveAResolverYet
        {
            get { return Resolver == null; }
        } 
    }
}