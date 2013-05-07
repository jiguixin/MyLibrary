using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit;
using Rhino;
using Rhino.Mocks;
using Infrastructure.CrossCutting;
using Infrastructure.Crosscutting.Caching;

namespace Infrastructure.Crosscutting.Tests
{
    [TestFixture]
    public class RhinoMockTest
    {
        private ICacheManager _cacheManager;
        [SetUp]
        public void EnterPoint()
        {
             _cacheManager = MockRepository.GenerateMock<ICacheManager>();
             _cacheManager.Expect(e => e.Get<string>("test")).Return("sdfsdf");
            
        }

        [Test]
        public void MockInterface()
        {
            Console.WriteLine(_cacheManager.IsSet("test"));
            Console.WriteLine(_cacheManager.Get<string>("test"));

        }
    }
}
    