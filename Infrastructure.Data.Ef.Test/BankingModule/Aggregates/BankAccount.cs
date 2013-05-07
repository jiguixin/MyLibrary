//===================================================================================
// Microsoft Developer and Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================


using System.ComponentModel.DataAnnotations;
using Domain.Seedwork;

namespace Infrastructure.Data.Ef.Test.BankingModule.Aggregates.BankAccountAgg
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The bank account representation (Domain Entity)
    /// </summary>
    public class BankAccount
        :Entity,IValidatableObject
    {
        #region Properties

        /// <summary>
        /// Get or set the bank account number
        /// </summary>
        public BankAccountNumber BankAccountNumber { get; set; }

        /// <summary>
        /// Get the IBAN  (International Bank Account Number)
        /// <remarks>
        /// http://en.wikipedia.org/wiki/International_Bank_Account_Number
        /// Spanish format
        /// </remarks>
        /// </summary>
        public string Iban
        {
            get
            {
                return string.Format("ES{0} {1} {2} {0}{3}", 
                                    this.BankAccountNumber.CheckDigits,
                                    this.BankAccountNumber.NationalBankCode, 
                                    this.BankAccountNumber.OfficeNumber, 
                                    this.BankAccountNumber.AccountNumber);
            }
            set
            {
            }
        }

        public int BackAccountState { get; set; }

        public BackAccountState State { 
            get { return (BankAccountAgg.BackAccountState) BackAccountState; }
            set { BackAccountState = (int) value; }

        }

        public DateTime CreateTime { get; set; }
         
        /// <summary>
        /// Get the current balance of this account
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Get the state of this account
        /// </summary>
        public bool Locked { get; private set; }

        /// <summary>
        /// Get or set the customer id associated with this bank account
        /// </summary>
        public Guid CustomerId { get; set; }
       
        /// <summary>
        /// The related customer
        /// </summary>
        public virtual Customer Customer { get; private set; }

        HashSet<BankAccountActivity> _bankAccountActivity;

        /// <summary>
        /// Get the bank account activity into this account
        /// </summary>
        public virtual ICollection<BankAccountActivity> BankAccountActivity
        {
            get
            {
                if (_bankAccountActivity == null)
                    _bankAccountActivity = new HashSet<BankAccountActivity>();

                return _bankAccountActivity;
            }
            set
            {
                _bankAccountActivity = new HashSet<BankAccountActivity>(value);
            }
        }

       
        #endregion

        #region Public Methods

        
        /// <summary>
        /// Lock current bank account
        /// </summary>
        public void Lock()
        {
            if (!Locked)
                Locked = true;
        }

        /// <summary>
        /// Un lock current bank account
        /// </summary>
        public void UnLock()
        {
            if (Locked)
                Locked = false;
        }

        /// <summary>
        /// Deposit momey into this bank account
        /// </summary>
        /// <param name="amount">the amount of money to deposit </param>
        public void DepositMoney(decimal amount,string reason)
        {
            if (amount < 0) throw new ArgumentException("exception_BankAccountInvalidWithdrawAmount");

            //DepositMoney is a term of our Ubiquitous Language. Means adding money to this account
            if (!this.Locked)
            {
                //set balance
                checked
                {
                    Balance += amount;

                    //anotate activity
                    this.BankAccountActivity.Add(new BankAccountActivity()
                    {
                        Date = DateTime.UtcNow,
                        Id = IdentityGenerator.NewSequentialGuid(),
                        Amount = amount,
                        ActivityDescription = reason
                    });
                }
            }
            else
                throw new InvalidOperationException("报错");
        }

        /// <summary>
        /// WithdrawMoney operation over this bankaccount
        /// </summary>
        /// <param name="amount">The amount of money for this withdraw operation</param>
        public void WithdrawMoney(decimal amount,string reason)
        {
            if (amount < 0) throw new ArgumentException("exception_BankAccountInvalidWithdrawAmount");

            //WithdrawMoney is a term of our Ubiquitous Language. Means deducting money to this account
            if (CanBeWithdrawed(amount))
            {
                checked
                {
                    this.Balance -= amount;

                    //anotate activity
                    this.BankAccountActivity.Add(new BankAccountActivity()
                    {
                        Date = DateTime.UtcNow,
                        Id = IdentityGenerator.NewSequentialGuid(),
                        Amount = -amount,
                        ActivityDescription = reason
                    });
                }
            }
            else
                throw new InvalidOperationException("");
        }

        /// <summary>
        /// Check if in this bankaccount is posible withdraw <paramref name="amount"/>
        /// </summary>
        /// <param name="amount">The amount of money </param>
        /// <returns>True if is posible perform the operation, else false</returns>
        public bool CanBeWithdrawed(decimal amount)
        {
            return !Locked && (this.Balance >= amount);
        }
         
        /// <summary>
        /// Set customer for this bank account
        /// </summary>
        /// <param name="customer"></param>
        public void SetCustomer(Customer customer)
        {
            if (customer == null
                ||
                customer.IsTransient())
            {
                throw new ArgumentException("");
            }

            //fix id and set reference
            this.CustomerId = customer.Id;
            this.Customer = customer;
        }

        #endregion

        #region IValidatableObject Members

        /// <summary>
        /// <see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/>
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (this.BankAccountNumber == null)
                validationResults.Add(new ValidationResult("validation_BankAccountNumberCannotBeNull", new string[] { "BankAccountNumber" }));
            else
            {
                if (String.IsNullOrEmpty(this.BankAccountNumber.AccountNumber) || String.IsNullOrWhiteSpace(this.BankAccountNumber.AccountNumber))
                    validationResults.Add(new ValidationResult("validation_BankAccountBankAccountNumberCannotBeNull", new string[] { "AccountNumber" }));

                if (String.IsNullOrEmpty(this.BankAccountNumber.CheckDigits) || String.IsNullOrWhiteSpace(this.BankAccountNumber.CheckDigits))
                    validationResults.Add(new ValidationResult("validation_BankAccountBankCheckDigitsCannotBeNull", new string[] { "CheckDigits" }));

                if (String.IsNullOrEmpty(this.BankAccountNumber.NationalBankCode) || String.IsNullOrWhiteSpace(this.BankAccountNumber.NationalBankCode))
                    validationResults.Add(new ValidationResult("validation_BankAccountBankNationalBankCodeCannotBeNull", new string[] { "NationalBankCode" }));

                if (String.IsNullOrEmpty(this.BankAccountNumber.OfficeNumber) || String.IsNullOrWhiteSpace(this.BankAccountNumber.OfficeNumber))
                    validationResults.Add(new ValidationResult("validation_BankAccountBankOfficeNumberCannotBeNull", new string[] { "OfficeNumber" }));
            }
            if(CustomerId == Guid.Empty)
                validationResults.Add(new ValidationResult("validation_BankAccountCustomerIdIsEmpty", new string[] { "CustomerId" }));

            return validationResults;
        }

        #endregion
    }

    public enum BackAccountState:int 
    {
        Error=1,
        Success
    }
}
