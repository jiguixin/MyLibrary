using System; 

namespace Infrastructure.Crosscutting.IoC
{
    /// <summary>
    /// 此类用于，在运行层初始化了IOC之后，然后把IOC赋值到当前类，以供派生该层的类
    /// 得到Unity的实例
    /// </summary>
    public class InstanceLocator : IInstanceLocator
    {
        private static IInstanceLocator currentLocator;

        private InstanceLocator()
        { }

        public static IInstanceLocator Current
        {
            get
            {
                return currentLocator;
            }
        }

        public static void SetLocator(IInstanceLocator locator)
        {
            currentLocator = locator;
        }

        public T GetInstance<T>() where T : class
        {
            return Current.GetInstance<T>();
        }

        public object GetInstance(Type instanceType)
        {
            return Current.GetInstance(instanceType);
        }

        public void RegisterType<T>()
        {
            currentLocator.RegisterType<T>();
        }

        public void RegisterType(Type type)
        {
            currentLocator.RegisterType(type);
        }

        public bool IsTypeRegistered<T>()
        {
            return currentLocator.IsTypeRegistered<T>();
        }

        public bool IsTypeRegistered(Type type)
        {
            return currentLocator.IsTypeRegistered(type);
        }
    }
}
