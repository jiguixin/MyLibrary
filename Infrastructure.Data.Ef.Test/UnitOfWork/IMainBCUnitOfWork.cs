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

using Infrastructure.Data.Ef.Test.BankingModule.Aggregates;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates.BankAccountAgg;

namespace Infrastructure.Data.Ef.Test.UnitOfWork
{

    using System.Data.Entity;

    /// <summary>
    /// This is a contract for 'Main Bounded-Context' Unit Of Work,
    /// you can use this contract for implementing the real
    /// dependency to your O/RM or, for creating a mock... 
    /// Also, setting this abstraction adds more information about 
    /// the existent sets in non generic repository methods.
    /// 
    /// This is not the contract for switching  
    /// your persistent infrastructure, of course....
    /// </summary>
    public interface IMainBCUnitOfWork
        :IQueryableUnitOfWork
    {
        IDbSet<Customer> Customers { get; }
         
        IDbSet<BankAccount> BankAccounts { get; }
    }
}
