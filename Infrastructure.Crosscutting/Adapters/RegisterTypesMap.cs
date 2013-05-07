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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Base class from ModuleTypesAdapter. This class
    /// is intended to subclasing for each module in 
    /// solution to configure specific maps
    /// </summary>
    public abstract class RegisterTypesMap
    {
        #region Properties

        Dictionary<string, ITypeMapConfigurationBase> _maps;

        /// <summary>
        /// Get the dictionary of type maps
        /// </summary>
        public Dictionary<string, ITypeMapConfigurationBase> Maps
        {
            get
            {
                return _maps;
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of ModulesTypeAdapter
        /// </summary>
        public RegisterTypesMap()
        {
            //create a new instance of type maps dictionary
            _maps = new Dictionary<string, ITypeMapConfigurationBase>();
        }

        #endregion

        #region Public Abstract Methods

        /// <summary>
        /// Register map into this register types map
        /// </summary>
        /// <typeparam name="TSource">The source type</typeparam>
        /// <typeparam name="KTarget">The target type</typeparam>
        /// <param name="map">The map to register</param>
        public void RegisterMap<TSource, TTarget>(TypeMapConfigurationBase<TSource, TTarget> map)
            where TSource : class
            where TTarget : class,new()
        {
            if (map == (TypeMapConfigurationBase<TSource, TTarget>)null)
                throw new ArgumentNullException("map");

            _maps.Add(map.Descriptor, map);
        }

        #endregion

    }
}
