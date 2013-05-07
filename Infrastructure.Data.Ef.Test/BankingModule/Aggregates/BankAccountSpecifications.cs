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


using Domain.Seedwork.Specification;

namespace Infrastructure.Data.Ef.Test.BankingModule.Aggregates.BankAccountAgg
{
    using System;

    /// <summary>
    /// A list of bank account specifications. You can learn
    /// about Specifications, Enhanced Query Objects or repository methods
    /// in DesignNotes.txt in Domain.Seedwork project
    /// </summary>
    public static class BankAccountSpecifications
    {
        /// <summary>
        /// Specification for bank accounts with number like to <paramref name="bankAccountNumber"/>
        /// </summary>
        /// <param name="bankAccountNumber">The bank account number</param>
        /// <returns>Associated specification</returns>
        public static ISpecification<BankAccount> BankAccountWithNumber(string bankAccountNumber)
        {
            Specification<BankAccount> specification = new TrueSpecification<BankAccount>();

            if (!String.IsNullOrEmpty(bankAccountNumber)
                &&
                !String.IsNullOrWhiteSpace(bankAccountNumber))
            {
                specification &= new DirectSpecification<BankAccount>((b) => b.Iban
                                                                              .ToLower()
                                                                              .Contains(bankAccountNumber.ToLower()));
            }

            return specification;
        }
    }
}
