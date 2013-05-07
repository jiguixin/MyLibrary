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

namespace Infrastructure.Crosscutting.Adapters
{

    /// <summary>
    /// Base contract type map configurations
    /// </summary>
    public interface ITypeMapConfigurationBase
    {
        /// <summary>
        /// Get descriptor for this instance. 
        /// <remarks>
        /// This descriptor is not unique string.
        /// </remarks>
        /// </summary>
        string Descriptor { get; }
    }
}
