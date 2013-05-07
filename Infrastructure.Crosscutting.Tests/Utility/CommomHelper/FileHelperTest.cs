using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Infrastructure.Crosscutting.Utility.CommomHelper;

namespace Infrastructure.Crosscutting.Tests.Utility.CommomHelper
{
    [TestFixture]
    class FileHelperTest
    {
        [Test]
        public void A()
        {

        }



        [Test]
        public void ClearDirectory()
        {
            FileHelper.ClearDirectory(@"D:\A");
        }


        [Test]
        public void DeleteDirectory()
        {
            FileHelper.DeleteDirectory(@"D:\A");
        }


          [Test]
        public void CopyFolder()
          {
              FileHelper.CopyFolder(@"E:\MyCode\MyCommonLibrary\Infrastructure.Data.Ado", @"D:\A");
          }


        [Test]
        public void GetDirectories()
        {
            var lst = FileHelper.GetDirectories(@"E:\MyCode\MyCommonLibrary");
            foreach (var s in lst)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("Count:" + lst.Count());
        }

        [Test]
        public void GetFileNames1()
        {
            var lst = FileHelper.GetFileNames(@"E:\MyCode\MyCommonLibrary","App.config",true);
            foreach (var s in lst)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("Count:" + lst.Count());
        }


        [Test]
        public void GetFileNames()
        {
            var lst = FileHelper.GetFileNames(@"E:\MyCode\MyCommonLibrary\TestOutput");
            foreach (var s in lst)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("Count:"+lst.Count());
        }
    }
}
