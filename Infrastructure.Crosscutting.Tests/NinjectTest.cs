/*
 *名称：NinjectTest
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-10 05:50:56
 *修改时间：
 *备注：
 */

using System;
using Infrastructure.CrossCutting.IoC.Ninject;
using Infrastructure.Crosscutting.IoC;
using NUnit.Framework;
using Ninject;
using Ninject.Modules;

namespace Infrastructure.Crosscutting.Tests
{
    [TestFixture]
    public class NinjectTest
    {

        /// <summary>
        /// 为整个TestFixture初始化资源
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InstanceLocator.SetLocator(
                new NinjectContainer().WireDependenciesInAssemblyMatching("Infrastructure.Crosscutting.Tests").Locator);
        }

        /// <summary>
        /// 为整个TestFixture释放资源
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
        }

        /// <summary>
        /// 为每个Test方法创建资源
        /// </summary>
        [SetUp]
        public void Initialize()
        {
        }

        /// <summary>
        /// 为每个Test方法释放资源
        /// </summary>
        [TearDown]
        public void TearDown()
        {
        }

        [Inject] private ICustomer1 cus;
        
        [Test]
        public void NinjectTestCustomer()
        {

            cus = InstanceLocator.Current.GetInstance<ICustomer1>();
            Console.WriteLine(cus.GetName());
            //Console.WriteLine(f.GetFoo());


        }

    }

    public interface ICustomer1
    {
        string GetName();
    }

    public interface IFoo
    {
        int GetFoo();

    }

    public class Customer1 : ICustomer1
    {
        public string GetName()
        {
            return "jim ji";
        }
    }

    class Foo:IFoo
    {
        public int GetFoo()
        {
            return 1;
        }
    }


    public class MyModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICustomer1>().To<Customer1>();
            this.Bind<IFoo>().To<Foo>();
        }
    }
}