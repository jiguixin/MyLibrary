using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Seedwork;

namespace Infrastructure.Data.Ef.Test.BankingModule.Aggregates
{
    public class Customer:Entity
    {
        #region Members

        bool _IsEnabled;

        #endregion

        #region Properties


        /// <summary>
        /// Get or set the Given name of this customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Get or set the surname of this customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Get or set the full name of this customer
        /// </summary>
        public string FullName
        {
            get
            {
                return string.Format("{0}, {1}", this.LastName, this.FirstName);
            }
            set { }

        }

        /// <summary>
        /// Get or set the telephone 
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Get or set the company name
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Get or set the current credit limit for this customer
        /// </summary>
        public decimal CreditLimit { get; set; }

        /// <summary>
        /// Get or set if this customer is enabled
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            private set
            {
                _IsEnabled = value;
            }
        }

        #endregion

    }
}
