﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Infrastructure.Crosscutting.Configuration;

namespace Infrastructure.Crosscutting.Core
{
    public class EngineContext
    {
        #region Initialization Methods
        /// <summary>Initializes a static instance of the Nop factory.</summary>
        /// <param name="forceRecreate">Creates a new factory instance even though the factory has been previously initialized.</param>
        /// <param name="databaseIsInstalled">A value indicating whether database is installed</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate, bool databaseIsInstalled)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                var config = ConfigurationManager.GetSection("NopConfig") as NopConfig;
                Debug.WriteLine("Constructing engine " + DateTime.Now);
                Singleton<IEngine>.Instance = CreateEngineInstance(config);
                Debug.WriteLine("Initializing engine " + DateTime.Now);
                Singleton<IEngine>.Instance.Initialize(config, databaseIsInstalled);
            }
            return Singleton<IEngine>.Instance;
        }

        /// <summary>Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.</summary>
        /// <param name="engine">The engine to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        /// <summary>
        /// Creates a factory instance and adds a http application injecting facility.
        /// </summary>
        /// <returns>A new factory</returns>
        public static IEngine CreateEngineInstance(NopConfig config)
        {
            if (config != null && !string.IsNullOrEmpty(config.EngineType))
            {
                var engineType = Type.GetType(config.EngineType);
                if (engineType == null)
                    throw new ConfigurationErrorsException("The type '" + engineType + "' could not be found. Please check the configuration at /configuration/nop/engine[@engineType] or check for missing assemblies.");
                if (!typeof(IEngine).IsAssignableFrom(engineType))
                    throw new ConfigurationErrorsException("The type '" + engineType + "' doesn't implement 'Nop.Core.Infrastructure.IEngine' and cannot be configured in /configuration/nop/engine[@engineType] for that purpose.");
                return Activator.CreateInstance(engineType) as IEngine;
            }

            throw new Exception("初始化Engine 出错");
        }

        #endregion

        /// <summary>Gets the singleton Nop engine used to access Nop services.</summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false, true);
                }
                return Singleton<IEngine>.Instance;
            }
        }
    }
}
