using System;

namespace Infrastructure.Crosscutting.IoC
{
    public interface IInstanceLocator
    {
        T GetInstance<T>() where T : class;
        object GetInstance(Type instanceType);
        bool IsTypeRegistered<T>();
        bool IsTypeRegistered(Type type);
        void RegisterType<T>();
        void RegisterType(Type type);
    }
}
