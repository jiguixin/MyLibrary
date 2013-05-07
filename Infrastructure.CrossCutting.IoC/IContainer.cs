
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Infrastructure.CrossCutting.IoC
{
    public interface IContainer
    {
        TService Resolve<TService>();

        object Resolve(Type type);

        void RegisterType(Type type);

        TService Resolve<TService>(string name);

        void RegisterInstance(Type type,string name, object instance);

        void ConfigureRoot();

    }
}
