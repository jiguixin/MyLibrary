using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Configuration;

namespace Infrastructure.Crosscutting.Core
{
    public interface IEngine
    {
        /// <summary>
        /// Initialize components and plugins in the nop environment.
        /// 通过继承该接口，可以完成IOC容器的注入，以及可以实现插件初始化。
        /// </summary>
        /// <param name="config">Config</param>
        /// <param name="databaseIsInstalled">A value indicating whether database is installed</param>
        void Initialize(NopConfig config, bool databaseIsInstalled);

        T GetInstance<T>() where T : class;
        object GetInstance(Type instanceType);
        bool IsTypeRegistered<T>();
        bool IsTypeRegistered(Type type);
        void RegisterType<T>();
        void RegisterType(Type type);
    }
}
