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

    /// <summary>
    /// Fluent Type map configuration
    /// <remarks>
    /// This is the base class intented to use in "agile mapping", for 
    /// example for mapping test 
    /// <example>
    /// var spec = new MapSpec{Customer,CustomerDTO}().Pre((s)=>{})
    ///                                               .Map((s)=>Mapper.Map(s))
    ///                                               .Post((t,items)=>{});
    /// </example>
    /// </remarks>
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <typeparam name="TTarget">The target type</typeparam>
    public sealed class TypeMapConfiguration<TSource, TTarget>
        : TypeMapConfigurationBase<TSource, TTarget>
        where TSource : class
        where TTarget : class,new()
    {
        #region Members

        Action<TSource> _beforeMapAction = null;
        Func<TSource, TTarget> _mapFunction = null;
        Action<TTarget, object[]> _afterMapAction = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Configure before map action
        /// </summary>
        /// <param name="beforeMapAction">The pre action</param>
        /// <returns>This</returns>
        public TypeMapConfiguration<TSource, TTarget> Before(Action<TSource> beforeMapAction)
        {
            _beforeMapAction = beforeMapAction;

            return this;
        }
        /// <summary>
        /// Configure map function
        /// </summary>
        /// <param name="mapFunction">The map function to use</param>
        /// <returns>This</returns>
        public TypeMapConfiguration<TSource, TTarget> Map(Func<TSource, TTarget> mapFunction)
        {
            _mapFunction = mapFunction;

            return this;
        }

        /// <summary>
        /// Configure the after map action
        /// </summary>
        /// <param name="afterMapAction">The post function</param>
        /// <returns>This</returns>
        public TypeMapConfiguration<TSource, TTarget> After(Action<TTarget, object[]> afterMapAction)
        {
            _afterMapAction = afterMapAction;

            return this;
        }

        #endregion

        #region TypeMapConfigurationBase Members

        protected override void BeforeMap(ref TSource source)
        {
            //call to befor map action
            if (_beforeMapAction != null)
                _beforeMapAction(source);
        }

        protected override void AfterMap(ref TTarget target, params object[] moreSources)
        {
            //call to after map action
            if (_afterMapAction != null)
                _afterMapAction(target, moreSources);
        }

        protected override TTarget Map(TSource source)
        {
            //call map function
            if (_mapFunction != null)
                return _mapFunction(source);
            else
                throw new InvalidOperationException("Map is not specified!");
        }

        #endregion
    }
}
