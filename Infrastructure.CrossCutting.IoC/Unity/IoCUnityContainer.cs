
namespace Infrastructure.CrossCutting.IoC.Unity
{
    using System;
    using Microsoft.Practices.Unity;
    using Infrastructure.CrossCutting.IoC.Resources;
    using System.Configuration;

    using Microsoft.Practices.Unity.Configuration;

    /// <summary>
    /// Implemented container in Microsoft Practices Unity
    /// </summary>
    sealed class IoCUnityContainer
        :IContainer
    {
        #region Members

        IUnityContainer container;

        #endregion

        #region Constructor

        public IoCUnityContainer()
        {
            container = new UnityContainer();
        }

        #endregion


        #region IServiceFactory Members

        public TService Resolve<TService>()
        {
            if (container == null)
                throw new InvalidOperationException(Messages.Exception_ContainerNotFound);

            return container.Resolve<TService>();
        }
       
        public object Resolve(Type type)
        {
            if (container == null)
                throw new InvalidOperationException(Messages.Exception_ContainerNotFound);

            return container.Resolve(type, null);
        }

        public TService Resolve<TService>(string name)
        {
            if (container == null)
                throw new InvalidOperationException(Messages.Exception_ContainerNotFound);

            return container.Resolve<TService>(name);
        }

        public void RegisterType(Type type)
        {
            if (container != null)
                container.RegisterType(type, new TransientLifetimeManager());
        }

        public void RegisterInstance(Type type,string name, object instance)
        {
            if (container != null)
                container.RegisterInstance(type, name, instance, new ContainerControlledLifetimeManager());
        }


        public void ConfigureRoot()
        {
            UnityConfigurationSection section = ConfigurationManager.GetSection("unity") as UnityConfigurationSection;
            section.Configure(container); 
        }

        #endregion
    }
}
