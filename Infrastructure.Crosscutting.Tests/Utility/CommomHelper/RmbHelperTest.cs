using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Infrastructure.Crosscutting.Utility.CommomHelper;

namespace Infrastructure.Crosscutting.Tests.Utility.CommomHelper
{
    [TestFixture]
    class RmbHelperTest
    {
        [Test]
        public void A()
        {
        }

        [Test]
        public void CmycurD()
        {
            string daxie = RmbHelper.CmycurD((decimal) 124.2);
            Console.WriteLine(daxie);

            Console.WriteLine(RmbHelper.CmycurD("123"));

        }
    }
}
