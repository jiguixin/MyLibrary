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
    using System.Configuration;
    using System.Reflection; 
    using Infrastructure.Crosscutting.Resources;

    /// <summary>
    /// ITypeAdatper implementation. 
    /// <remarks>
    /// Really, this implementation only load RegisterTypesMapConfiguration 
    /// and create a global dictionary of mappers.
    /// </remarks>
    /// </summary>
    public sealed class TypeAdapter
        : ITypeAdapter
    {
        #region Properties

        Dictionary<string, ITypeMapConfigurationBase> _maps;

        /// <summary>
        /// Get the collection of ITypeMapConfigurationBase elements
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
        /// Create a instance of TypeAdapter
        /// </summary>
        public TypeAdapter(RegisterTypesMap[] mapModules)
        {
            InitializeAdapter(mapModules);
        }

        #endregion

        #region Private Method

        void InitializeAdapter(RegisterTypesMap[] mapsModules)
        {
            //create map adapters dictionary
            _maps = new Dictionary<string, ITypeMapConfigurationBase>();

            if (mapsModules != null)
            {
                //foreach adapter's module in solution load mapping
                foreach (var module in mapsModules)
                {
                    foreach (var map in module.Maps)
                        _maps.Add(map.Key, map.Value);
                }
            }
        }

        #endregion

        #region ITypeAdapter Implementation

        /// <summary>
        /// <see cref="Infrastructure.Crosscutting.Adapters.ITypeAdapter"/>
        /// </summary>
        /// <typeparam name="TSource"><see cref="Infrastructure.CrossCutting.Adapters.ITypeAdapter"/></typeparam>
        /// <typeparam name="TTarget"><see cref="Infrastructure.CrossCutting.Adapters.ITypeAdapter"/></typeparam>
        /// <param name="source"><see cref="Infrastructure.CrossCutting.Adapters.ITypeAdapter"/></param>
        /// <returns><see cref="Infrastructure.CrossCutting.Adapters.ITypeAdapter"/></returns>
        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class, new()
        {
            if (source == (TSource)null)
                throw new ArgumentNullException("source");

            var descriptor = TypeMapConfigurationBase<TSource, TTarget>.GetDescriptor();

            if (_maps.ContainsKey(descriptor))
            {
                var spec = _maps[descriptor] as TypeMapConfigurationBase<TSource, TTarget>;

                return spec.Resolve(source);
            }
            else
                throw new InvalidOperationException(string.Format(Messages.exception_NotMapFoundForTypeAdapter,
                                                                typeof(TSource).FullName, 
                                                                typeof(TTarget).FullName));
        }

        /// <summary>
        /// <see cref="Infrastructure.CrossCutting.Adapters.ITypeAdapter"/>
        /// </summary>
        /// <typeparam name="TSource"><see cref="Infrastructure.CrossCutting.Adapters.ITypeAdapter"/></typeparam>
        /// <typeparam name="TTarget"><see cref="Infrastructure.CrossCutting.Adapters.ITypeAdapter"/></typeparam>
        /// <param name="source"><see cref="Infrastructure.CrossCutting.Adapters.ITypeAdapter"/></param>
        /// <param name="moreSources"><see cref="Infrastructure.CrossCutting.Adapters.ITypeAdapter"/></param>
        /// <returns><see cref="Infrastructure.CrossCutting.Adapters.ITypeAdapter"/></returns>
        public TTarget Adapt<TSource, TTarget>(TSource source, params object[] moreSources)
            where TSource : class
            where TTarget : class, new()
        {

            if (source == (TSource)null)
                throw new ArgumentNullException("source");

            string descriptor = TypeMapConfigurationBase<TSource, TTarget>.GetDescriptor();

            if (_maps.ContainsKey(descriptor))
            {
                var spec = _maps[descriptor] as TypeMapConfigurationBase<TSource, TTarget>;

                return spec.Resolve(source, moreSources);
            }
            else
                throw new InvalidOperationException(string.Format(Messages.exception_NotMapFoundForTypeAdapter,
                                                                typeof(TSource).FullName,
                                                                typeof(TTarget).FullName));
        }

        #endregion
    }
}
