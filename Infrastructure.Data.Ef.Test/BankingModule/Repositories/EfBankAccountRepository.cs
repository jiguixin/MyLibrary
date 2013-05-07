using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates.BankAccountAgg;
using Infrastructure.Data.Ef.Test.UnitOfWork;

namespace Infrastructure.Data.Ef.Test.BankingModule.Repositories
{
    /// <summary>
    /// The bank account repository implementation
    /// </summary>
    public class EfBankAccountRepository
        : EfRepository<BankAccount>, IBankAccountRepository
    { 
        public EfBankAccountRepository(string connectionStringName) : base(connectionStringName)
        {

        }
        
    }
}
