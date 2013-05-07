using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Infrastructure.Crosscutting.Core;
using Infrastructure.Crosscutting.Caching;

namespace Infrastructure.Crosscutting.Tests.EngineTest
{
    [TestFixture]
    class Start
    {
        [Test]
        public void EntryPoint()
        {
           var myEnginee = EngineContext.Initialize(false, false);

            var memoryCache = myEnginee.GetInstance<ICacheManager>();
            memoryCache.Set("Jim","safasdf",5000);

            var s = memoryCache.Get<string>("Jim");

            Console.WriteLine(s);
        }
    }
}
