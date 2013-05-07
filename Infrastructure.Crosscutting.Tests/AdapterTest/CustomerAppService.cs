using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Adapters;
using Infrastructure.Crosscutting.Tests.DTOAdapters;
using Infrastructure.Crosscutting.Tests.Model;
using Infrastructure.Crosscutting.Tests.Dtos;
using NUnit.Framework;

namespace Infrastructure.Crosscutting.Tests.AdapterTest
{
    [TestFixture]
    public class CustomerAppService
    {
        private ITypeAdapter typeAdapter;
         [SetUp] 
        public void  BeginCustomerAppService ()
        {
            typeAdapter = new TypeAdapter(new RegisterTypesMap[] { new  ModuleRegisterTypesMap() }); 
        }

        [Test]
        public void AdapterModel()
        {
            Customer cus = new Customer(){Company = "科泰地理",FirstName = "jim",LastName = "ji",Telephone = "13880535849"};
            cus.CusAddress = new Address() { AddressLine1 = "中坝街29号", AddressLine2 = "三汇镇", City = "成都", ZipCode = "61000" };
            cus.Picture = new Picture(){ImgPath = @"F:\DDD\项目源码\NLayerAppV2"};
            cus.OrderList = new List<Order>() { new Order() { OrderDate = DateTime.Now.AddDays(-1), DeliveryDate = DateTime.Now }, new Order() { OrderDate = DateTime.Now.AddDays(-10), DeliveryDate = DateTime.Now.AddDays(-5) } };

            //将Model转换为DTO
            var CusDto = typeAdapter.Adapt<Customer,CustomerDto>(cus);

            //实现将DTO反向转换为Model
            var cusModel = typeAdapter.Adapt<CustomerDto, Customer>(CusDto);

            //将DTO转换成Address
            var adsModel = typeAdapter.Adapt<CustomerDto, Address>(CusDto);

        }
         

    }
}
