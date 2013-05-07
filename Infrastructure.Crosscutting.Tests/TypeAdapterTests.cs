////===================================================================================
//// Microsoft Developer & Platform Evangelism
////=================================================================================== 
//// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
////===================================================================================
//// Copyright (c) Microsoft Corporation.  All Rights Reserved.
//// This code is released under the terms of the MS-LPL license, 
//// http://microsoftnlayerapp.codeplex.com/license
////===================================================================================
			

//namespace Infrastructure.Crosscutting.Tests
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;

//    using Infrastructure.Crosscutting.Tests;

//    using Microsoft.Practices.Unity;

//    using Infrastructure.Crosscutting.Adapters;
//    using Microsoft.VisualStudio.TestTools.UnitTesting;

//    [TestClass()]
//    public class TypeAdapterTests
//    {
//        [TestMethod()]
//        public void TypeAdapter_LoadRegisterTypesMapFromConfiguration()
//        {
//            //Arrange
//            TypeAdapter adapter = null;
//            RegisterTypesMap[] mapModules = new RegisterTypesMap[]
//            {
//                new CRMRegisterTypesMap(),
//                new SalesRegisterTypesMap()
//            };

//            //Act
//            adapter = new TypeAdapter(mapModules);
            
//            //Assert
//            Assert.IsNotNull(adapter.Maps);
//            Assert.IsTrue(adapter.Maps.Count == 2);

//            string keyCustomer = TypeMapConfigurationBase<Customer, CustomerDTO>.GetDescriptor();
//            string keyOrder = TypeMapConfigurationBase<Order, OrderDTO>.GetDescriptor();

//            Assert.IsNotNull(adapter.Maps[keyCustomer] !=null );
//            Assert.IsNotNull(adapter.Maps[keyOrder] != null);
//        }

//        [TestMethod()]
//        public void TypeAdapter_AdaptMappedTypes()
//        {
//            //Arrange
//            TypeAdapter adapter = null;

//            RegisterTypesMap[] mapModules = new RegisterTypesMap[]
//            {
//                new CRMRegisterTypesMap(),
//                new SalesRegisterTypesMap()
//            };

//            adapter = new TypeAdapter(mapModules);

//            Customer customer = new Customer()
//            {
//                Id = 1,
//                FirstName = "Unai",
//                LastName = "Zorrilla"
//            };

//            Order order = new Order()
//            {
//                Id = 1,
//                OrderDate = DateTime.UtcNow,
//                Total = 1000
//            };

//            //Act
            
//            var dtoCustomer = adapter.Adapt<Customer, CustomerDTO>(customer);
//            var dtoOrder = adapter.Adapt<Order, OrderDTO>(order);

//            //Assert
//            Assert.IsNotNull(dtoCustomer);
//            Assert.IsNotNull(dtoOrder);

//            Assert.IsTrue(dtoCustomer.CustomerId == 1);
//            Assert.IsTrue(dtoCustomer.FullName == string.Format("{0},{1}",customer.LastName,customer.FirstName));

//            Assert.IsTrue(dtoOrder.OrderId == 1);
//            Assert.IsTrue(dtoOrder.Description == string.Format("{0} - {1}",order.OrderDate,order.Total));
            
//        }
//        [TestMethod()]
//        public void TypeAdapter_AdaptMappedTypesWithMultipleSources()
//        {
//            //Arrange                                                                 
//            TypeAdapter adapter = null;
//            RegisterTypesMap[] mapModules = new RegisterTypesMap[]
//            {
//                new CRMRegisterTypesMap(),
//                new SalesRegisterTypesMap()
//            };
//            adapter = new TypeAdapter(mapModules);

//            Customer customer = new Customer()
//            {
//                Id = 1,
//                FirstName = "Unai",
//                LastName = "Zorrilla"
//            };

//            Order order = new Order()
//            {
//                Id = 1,
//                OrderDate = DateTime.UtcNow,
//                Total = 1000
//            }; 

//            //Act
            
//            var dtoCustomer = adapter.Adapt<Customer, CustomerDTO>(customer,null);
//            var dtoOrder = adapter.Adapt<Order, OrderDTO>(order,null);

//            //Assert
//            Assert.IsNotNull(dtoCustomer);
//            Assert.IsNotNull(dtoOrder);

//            Assert.IsTrue(dtoCustomer.CustomerId == 1);
//            Assert.IsTrue(dtoCustomer.FullName == string.Format("{0},{1}", customer.LastName, customer.FirstName));

//            Assert.IsTrue(dtoOrder.OrderId == 1);
//            Assert.IsTrue(dtoOrder.Description == string.Format("{0} - {1}", order.OrderDate, order.Total));

//        }
//        [TestMethod()]
//        [ExpectedException(typeof(InvalidOperationException))]
//        public void TypeAdapter_AdapUnmappedTypesThrowInvalidOperationException()
//        {
//            //Arrange                                                                 
//            TypeAdapter adapter = null;
//            RegisterTypesMap[] mapModules = new RegisterTypesMap[]
//            {
//                new CRMRegisterTypesMap(),
//                new SalesRegisterTypesMap()
//            };
//            adapter = new TypeAdapter(mapModules);

//            Product product = new Product()
//            {
//                ProductId = 1,
//                LaunchDate = DateTime.UtcNow,
//                ProductName ="Product Name"
//            };

//            //Act
//            adapter.Adapt<Product, ProductDTO>(product);
//        }
//        [TestMethod()]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void TypeAdapter_AdaptNullItemThrowArgumentException()
//        {
//            //Arrange                                                                 
//            TypeAdapter adapter = null;
//            RegisterTypesMap[] mapModules = new RegisterTypesMap[]
//            {
//                new CRMRegisterTypesMap(),
//                new SalesRegisterTypesMap()
//            };
//            adapter = new TypeAdapter(mapModules);


//            //Act
//            adapter.Adapt<Customer, CustomerDTO>(null);
//        }
//    }
//}
                             