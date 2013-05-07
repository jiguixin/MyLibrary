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
    /// The type map configuration base. With this class
    /// you can specify the map for convert a source type into target type.
    /// This maps pec supports before and after injection points.
    /// </summary>
    /// <typeparam name="TSource">The source </typeparam>
    /// <typeparam name="TTarget">The target type</typeparam>
    public abstract class TypeMapConfigurationBase<TSource, TTarget>
        :ITypeMapConfigurationBase
        where TSource : class
        where TTarget : class,new()
    {
        #region Members

        int? _requestedHashCode = null;

        #endregion

        #region Properties

        /// <summary>
        /// Get descriptor for this instance. 
        /// <remarks>
        /// This descriptor is not unique string.
        /// </remarks>
        /// </summary>
        public string Descriptor
        {
            get
            {
                return TypeMapConfigurationBase<TSource, TTarget>.GetDescriptor();
            }
        }

        #endregion

        #region Abstract

        /// <summary>
        /// Excuted action before map source to target.
        /// <remarks>
        /// If you use a framework for mappings you can use this method
        /// for preparing or setup the map.
        /// </remarks>
        /// </summary>
        /// <param name="source">The source to adapt</param>
        protected abstract void BeforeMap(ref TSource source);
        
        /// <summary>
        /// Executed action after map.
        /// <remarks>
        /// You can use this method for set more sources into adapted object
        /// </remarks>
        /// </summary>
        /// <param name="target">The item adapted </param>
        /// <param name="nestedData">Nested data to use in post filter</param>
        protected abstract void AfterMap(ref TTarget target, params object[] moreSources);

        /// <summary>
        /// Map a source to a target
        /// </summary>
        /// <remarks>
        /// If you use a framework, use this method for setup or resolve mapping.
        /// <example>Automapper.Map{TSource,KTarget}</example>
        /// </remarks>
        /// <param name="source">The source to map</param>
        /// <returns>A new instance of <typeparamref name="TTarget"/></returns>
        protected abstract TTarget Map(TSource source);


        #endregion

        #region Public Static Methods

        /// <summary>
        /// Get MapSpec descriptor
        /// </summary>
        /// <returns>Associated descriptor</returns>
        public static string GetDescriptor()
        {
            return string.Format("{0}<->{1}", typeof(TSource).FullName, typeof(TTarget).FullName);
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// The resolver. Adapt a {TSource} instance into a new instance of type {KTarget}
        /// <remarks>
        /// This is the template method for translate a source type into a destination type. The template
        /// is BeforeMap -> Map -> AfterMap
        /// </remarks>
        /// </summary>
        /// <param name="source">The source item to adapt</param>
        /// <param name="nestedData">Nested data to use in pipeline</param>
        /// <returns>The target instance</returns>
        public TTarget Resolve(TSource source, params object[] moreSources)
        {
            /*
             * Resolve adapter pipeline
             * pre -> map -> post
             */

            //execute prefilter
            BeforeMap(ref source);

            //map from source to target using conventions or specific things ( for each framework )
            TTarget target = Map(source);

            //execute postfilter
            AfterMap(ref target, moreSources);

            //return adapted object
            return target;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// <see cref="M:System.Object.Equals"/>
        /// </summary>
        /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
        /// <returns><see cref="M:System.Object.Equals"/></returns>
        public override bool Equals(object obj)
        {
            bool equals = true;

            if (obj == null) equals = false;

            TypeMapConfigurationBase<TSource, TTarget> spec = obj as TypeMapConfigurationBase<TSource, TTarget>;

            if (spec == null) equals = false;

            return equals;
        }

        public override int GetHashCode()
        {
            if (!_requestedHashCode.HasValue)    //Why 31 ( factor for scrambling function)?--> "distributed better over the buckets"
                _requestedHashCode = typeof(TSource).GetHashCode() ^ typeof(TTarget).GetHashCode() * 31;

            return _requestedHashCode.Value;
        }

        #endregion
    }
}
