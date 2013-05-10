using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.IoC;
using Ninject;

namespace Infrastructure.CrossCutting.IoC.Ninject
{
    public class InstanceLocatorNinject : IInstanceLocator
    {
        private readonly IKernel kernel;

        public InstanceLocatorNinject(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T GetInstance<T>() where T : class
        {
            return kernel.Get<T>();
        }

        public object GetInstance(Type instanceType)
        {
            return kernel.Get(instanceType);
        }

        public bool IsTypeRegistered<T>()
        {
            throw new NotImplementedException();
        }

        public bool IsTypeRegistered(Type type)
        {
            throw new NotImplementedException();
        }

        public void RegisterType<T>()
        {
            kernel.Bind<T>();
        }

        public void RegisterType(Type type)
        {
            kernel.Bind(type);
        }
    }
}
