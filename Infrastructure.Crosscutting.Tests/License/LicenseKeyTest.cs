using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Infrastructure.Crosscutting.License;

namespace Infrastructure.Crosscutting.Tests.License
{
    [TestFixture]
    class LicenseKeyTest
    {
        [Test]
        public void EntryPoint()
        {
            string s = LicenseKey.Value();

            Console.WriteLine(s);
        }
    }

}
