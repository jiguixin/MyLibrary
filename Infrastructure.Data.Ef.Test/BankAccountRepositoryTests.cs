using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates.BankAccountAgg;
using Infrastructure.Data.Ef.Test.BankingModule.Repositories;
using Infrastructure.Data.Ef.Test.Initializers;
using Infrastructure.Data.Ef.Test.UnitOfWork;
using NUnit.Framework;

namespace Infrastructure.Data.Ef.Test
{
    [TestFixture]
    public class BankAccountRepositoryTests
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
            IBankAccountRepository bankAccountRepository = new BankAccountRepository(unitOfWork);

            Guid selectedBankAccount = new Guid("0343C0B0-7C40-444A-B044-B463F36A1A1F");

            //Act
            var bankAccount = bankAccountRepository.Get(selectedBankAccount);

            //Assert
            Assert.IsNotNull(bankAccount);
            Assert.IsTrue(bankAccount.Id == selectedBankAccount);
        }
    }
}
