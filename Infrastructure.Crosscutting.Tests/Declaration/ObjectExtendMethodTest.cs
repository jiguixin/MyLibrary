/*
 *名称：ObjectExtendMethodTest
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-09-03 09:25:08
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Infrastructure.Crosscutting.Declaration;

namespace Infrastructure.Crosscutting.Tests.Declaration
{
    [TestFixture]
    public class ObjectExtendMethodTest
    {

        #region Init

        /// <summary>
        /// 为整个TestFixture初始化资源
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
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

        #endregion

        #region public Test

        [Test]
        public void GetNumberTest()
        {
            Console.WriteLine("asfasf123.256".GetNumberDecimal());
            Console.WriteLine("asfasf0.256".GetNumberDouble());
            Console.WriteLine("asfasf256".GetNumberInt());
            Console.WriteLine("020342428220".IsPhone());
             
        }

        [Test]
        public void ExistsTest()
        {
            IEnumerable<int> lst = new List<int>() {2, 12, 12, 34, 43, 22};
            Console.WriteLine(lst.Exists(a => a == 2));
        }

        #endregion


        #region Private Methods

        #endregion
    }
}