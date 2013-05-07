//===================================================================================
// Microsoft Developer & Platform Evangelism
//================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================
			

namespace Infrastructure.Crosscutting.Tests.Classes
{
    using Infrastructure.Crosscutting.Adapters;

    public class CRMRegisterTypesMap
        :RegisterTypesMap
    {
        public CRMRegisterTypesMap()
        {
            //configure fake types
            var mapConfiguration = new TypeMapConfiguration<Customer,CustomerDTO>();
            mapConfiguration = mapConfiguration.Before((e)=>{})
                                               .Map((e)=>
                                                {
                                                    return new CustomerDTO()
                                                    {
                                                        CustomerId = e.Id,
                                                        FullName = string.Format("{0},{1}",e.LastName,e.FirstName)
                                                    };
                                                }).After((dto,sources)=>{});

            this.RegisterMap<Customer, CustomerDTO>(mapConfiguration);
        }
    }
}
