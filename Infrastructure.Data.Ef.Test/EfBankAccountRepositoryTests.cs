using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Data.Ef.DbContextProvider;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates.BankAccountAgg;
using Infrastructure.Data.Ef.Test.BankingModule.Repositories;
//using Infrastructure.Data.Ef.Test.Initializers;
//using Infrastructure.Data.Ef.Test.UnitOfWork;
using NUnit.Framework;

namespace Infrastructure.Data.Ef.Test
{
    [TestFixture]
    public class EfBankAccountRepositoryTests
    {
       
        [SetUp]
        public void Initial()
        {
            //AssemblyTestsInitialize.RebuildUnitOfWork();
            // 初始化数据库
            DbContextManager.InitStorage(new DbContextStorageBase());
            // 由于这里是测试项目，因此其dll路径有所不同
            DbContextManager.InitDbContext("Infrastructure.Data.Ef.Test.UnitOfWork.MainBCUnitOfWork", @"E:\MyCode\MyCommonLibrary\TestOutput\Infrastructure.Data.Ef.Test.dll", "Infrastructure.Data.Ef.Test.UnitOfWork.Mapping");
             
            //DbContextManager.InitDbContext("ProductDB", "../Debug/EFPlusDemo.Data.dll", "EFPlusDemo.Data.Mapping.ProductDB");
        }

        [Test]
        public void BankAccountRepositoryGetMethodReturnMaterializedEntityById()
        {
            //Arrange 
            IBankAccountRepository _bankAccountRepository = new EfBankAccountRepository("Infrastructure.Data.Ef.Test.UnitOfWork.MainBCUnitOfWork");
           
            Guid selectedBankAccount = new Guid("0343C0B0-7C40-444A-B044-B463F36A1A1F");

            //Act
            var bankAccount = _bankAccountRepository.Get(selectedBankAccount);

            bankAccount.CreateTime = DateTime.Now;

            //_bankAccountRepository.UnitOfWork.SaveChanges();

            //Assert
            Assert.IsNotNull(bankAccount);
            Assert.IsTrue(bankAccount.Id == selectedBankAccount);
        }
    }
}
