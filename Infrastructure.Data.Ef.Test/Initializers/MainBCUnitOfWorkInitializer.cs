using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Domain.Seedwork;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates.BankAccountAgg;
using Infrastructure.Data.Ef.Test.UnitOfWork;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates;

namespace Infrastructure.Data.Ef.Test.Initializers
{
    public class MainBCUnitOfWorkInitializer
       : DropCreateDatabaseIfModelChanges<MainBCUnitOfWork>
    {
        protected override void Seed(MainBCUnitOfWork unitOfWork)
        {
          
 
            /*
             * Customers agg
             */

            var customerJhon = new Customer()
                                   {
                                       Company = "科泰",
                                       CreditLimit = (decimal) 12223.3,
                                       FirstName = "Jim",
                                       FullName = "jimji", 
                                       LastName = "ji",
                                       Telephone = "13880535849"
                                   };
            customerJhon.Id = new Guid("43A38AC8-EAA9-4DF0-981F-2685882C7C45");


            var customerMay = new Customer()
            {
                Company = "创思奇",
                CreditLimit = (decimal)122233,
                FirstName = "Jim",
                FullName = "jimji",
                LastName = "ji",
                Telephone = "13880535849"
            };
            customerMay.Id = new Guid("0CD6618A-9C8E-4D79-9C6B-4AA69CF18AE6");


            unitOfWork.Customers.Add(customerJhon);
            unitOfWork.Customers.Add(customerMay);


           
            /*
             * Bank Account agg
             */

            var bankAccountNumberJhon = new BankAccountNumber("1111", "2222", "3333333333", "01");
            BankAccount bankAccountJhon = BankAccountFactory.CreateBankAccount(customerJhon, bankAccountNumberJhon);
            bankAccountJhon.Id = new Guid("0343C0B0-7C40-444A-B044-B463F36A1A1F");
            bankAccountJhon.DepositMoney(1000, "Open BankAccount");

            var bankAccountNumberMay = new BankAccountNumber("4444", "5555", "3333333333", "02");
            BankAccount bankAccountMay = BankAccountFactory.CreateBankAccount(customerMay, bankAccountNumberMay);
            bankAccountMay.Id = IdentityGenerator.NewSequentialGuid();
            bankAccountJhon.DepositMoney(2000, "Open BankAccount");

            unitOfWork.BankAccounts.Add(bankAccountJhon);
            unitOfWork.BankAccounts.Add(bankAccountMay);

        }
    }
}
