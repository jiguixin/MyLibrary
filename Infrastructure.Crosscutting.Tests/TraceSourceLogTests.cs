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


using Infrastructure.Crosscutting.NetFramework.Logging.TraceSource;

namespace Infrastructure.Crosscutting.Tests
{
    using System;
    using Infrastructure.Crosscutting.NetFramework.Logging;
    using Infrastructure.Crosscutting.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public partial class TraceSourceLogTest
    {
        #region Class Initialize

        [ClassInitialize()]
        public static void ClassInitialze(TestContext context)
        {
            // Initialize default log factory
            LoggerFactory.SetCurrent(new TraceSourceLogFactory());
        }

        #endregion

        [TestMethod()]
        public void LogInfo()
        {
            //Arrange
            ILogger log = LoggerFactory.CreateLog();

            //Act
            log.LogInfo("{0}","the info message"); 
        }
        [TestMethod()]
        public void LogWarning()
        {
            //Arrange
            ILogger log = LoggerFactory.CreateLog();

            //Act
            log.LogWarning("{0}","the warning message");
        }
        [TestMethod()]
        public void LogError()
        {
            //Arrange
            ILogger log = LoggerFactory.CreateLog();

            //Act
            log.LogError("{0}", "the error message"); 
        }
            
        [TestMethod()]
        public void LogErrorWithException()
        {
            //Arrange
            ILogger log = LoggerFactory.CreateLog();

            //Act
            log.LogError("{0}", new ArgumentNullException("param"), "the error message");
        }
    }
}
