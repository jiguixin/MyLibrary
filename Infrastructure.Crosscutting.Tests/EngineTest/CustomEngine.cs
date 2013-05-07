using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Caching;
using Infrastructure.Crosscutting.Configuration;
using Infrastructure.Crosscutting.Core;
using Microsoft.Practices.Unity;

namespace Infrastructure.Crosscutting.Tests.EngineTest
{
    public class CustomEngine:IEngine
    {
        private IUnityContainer container;
        public CustomEngine()
        {
            //初始化IoC
            container = new UnityContainer();
            container.RegisterType<ICacheManager, MemoryCacheManager>(); 
        }

        #region Implementation of IEngine

        /// <summary>
        /// Initialize components and plugins in the nop environment.
        /// </summary>
        /// <param name="config">Config</param>
        /// <param name="databaseIsInstalled">A value indicating whether database is installed</param>
        public void Initialize(NopConfig config, bool databaseIsInstalled)
        {
            //干一些如后台线程的东西。
             
        }

        public T GetInstance<T>() where T : class
        {
            return container.Resolve<T>();
        }

        public object GetInstance(Type instanceType)
        {
            return container.Resolve(instanceType);
        }

        public bool IsTypeRegistered<T>()
        {
            return container.IsRegistered<T>();

        }

        public bool IsTypeRegistered(Type type)
        {
            return container.IsRegistered(type);

        }

        public void RegisterType<T>()
        {
             container.RegisterType<T>(); 
        }

        public void RegisterType(Type type)
        {
             container.RegisterType(type);

        }

        #endregion

        #region helper
         
        #endregion
    }
}
