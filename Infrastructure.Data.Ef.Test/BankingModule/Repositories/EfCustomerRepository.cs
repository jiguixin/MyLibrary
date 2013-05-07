using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates.BankAccountAgg;
using Infrastructure.Data.Ef.Test.UnitOfWork;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates;

namespace Infrastructure.Data.Ef.Test.BankingModule.Repositories
{
    /// <summary>
    /// The bank account repository implementation
    /// </summary>
    public class EfCustomerRepository
        : EfRepository<Customer>
    {
        public EfCustomerRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }
    }
}
