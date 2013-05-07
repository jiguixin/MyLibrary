//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
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

    public class SalesRegisterTypesMap
        :RegisterTypesMap
    {
        public SalesRegisterTypesMap()
        {
            //configure fake types
            var mapConfiguration = new TypeMapConfiguration<Order, OrderDTO>();
            mapConfiguration = mapConfiguration.Before((o) => { })
                                               .Map((o) =>
                                               {
                                                   return new OrderDTO()
                                                   {
                                                       OrderId = o.Id,
                                                       Description = string.Format("{0} - {1}", o.OrderDate,o.Total)
                                                   };
                                               }).After((dto, sources) => { });

            this.RegisterMap<Order, OrderDTO>(mapConfiguration);
        }
    }
}
