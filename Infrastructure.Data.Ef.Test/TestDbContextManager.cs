using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Data.Ef.DbContextProvider;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates.BankAccountAgg;
using Infrastructure.Data.Ef.Test.BankingModule.Repositories;
using Infrastructure.Data.Ef.Test.Initializers;
using Infrastructure.Data.Ef.Test.UnitOfWork;
using NUnit.Framework;
using System.Data.Entity;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates; 

namespace Infrastructure.Data.Ef.Test
{
    [TestFixture]
    public class TestDbContextManager
    { 
        [SetUp]
        public void Initial()
        {
             AssemblyTestsInitialize.RebuildUnitOfWork();

        }

        [Test]
        public void BankAccountRepositoryGetMethodReturnMaterializedEntityById()
        {

            //Arrange
            var unitOfWork = new MainBCUnitOfWork();
            //DbContextManager.DbContextBuilder<TestDbContext> builder = new DbContextBuilder<TestDbContext>("Infrastructure.Data.Ef.Test.UnitOfWork.MainBCUnitOfWork", @"E:\MyCode\MyCommonLibrary\TestOutput\Infrastructure.Data.Ef.Test", "Infrastructure.Data.Ef.Test.UnitOfWork.Mapping");

            //var unitOfWork = builder.BuildDbContext();

            IBankAccountRepository bankAccountRepository = new BankAccountRepository(unitOfWork);

            Guid selectedBankAccount = new Guid("0343C0B0-7C40-444A-B044-B463F36A1A1F");

            //Act
            var bankAccount = bankAccountRepository.Get(selectedBankAccount);

            //Assert
            Assert.IsNotNull(bankAccount);
            Assert.IsTrue(bankAccount.Id == selectedBankAccount);
        }

        [Test]
        public void GenerateDbContext()
        {
            //目前没有发现可测试性
 
            
        }

    }
     
}
