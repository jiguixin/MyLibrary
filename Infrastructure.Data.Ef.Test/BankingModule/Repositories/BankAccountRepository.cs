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
    public class BankAccountRepository
        : Repository<BankAccount>, IBankAccountRepository
    {
        #region Members

        IMainBCUnitOfWork _currentUnitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        public BankAccountRepository(IMainBCUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _currentUnitOfWork = unitOfWork;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Get all bank accounts and the customer information
        /// </summary>
        /// <returns>Enumerable collection of bank accounts</returns>
        public override IEnumerable<BankAccount> GetAll()
        {
            var set = _currentUnitOfWork.CreateSet<BankAccount>();

            return set.Include(ba => ba.Customer)
                      .AsEnumerable();

        }
        #endregion
    }
}
