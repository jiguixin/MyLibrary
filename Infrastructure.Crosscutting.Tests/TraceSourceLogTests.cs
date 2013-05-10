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


using System.Diagnostics;
using Infrastructure.Crosscutting.Logging.TraceSource;
using NUnit.Framework;

namespace Infrastructure.Crosscutting.Tests
{
    using System;
    using Infrastructure.Crosscutting.NetFramework.Logging;
    using Infrastructure.Crosscutting.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestFixture]
    public partial class TraceSourceLogTest
    {
        #region Class Initialize

       [SetUp]
        public static void ClassInitialze()
        {
            TextWriterTraceListener tr1 = new TextWriterTraceListener(System.Console.Out); 
            // Initialize default log factory
           // LoggerFactory.SetCurrent(new TraceSourceLogFactory(tr1));
            LoggerFactory.SetCurrent(new TraceSourceLogFactory());
        }

        #endregion

        [Test]
        public void LogInfo()
        {
            //Arrange
            ILogger log = LoggerFactory.CreateLog();

            //Act
            log.LogInfo("{0}","the info message"); 
        }
        [Test]
        public void LogWarning()
        {
            //Arrange
            ILogger log = LoggerFactory.CreateLog();

            //Act
            log.LogWarning("{0}","the warning message");
        }
        [Test]
        public void LogError()
        {
            //Arrange
            ILogger log = LoggerFactory.CreateLog();

            //Act
            log.LogError("{0}", "the error message"); 
        }

        [Test]
        public void LogErrorWithException()
        {
            //Arrange
            ILogger log = LoggerFactory.CreateLog();

            //Act
            log.LogError("{0}", new ArgumentNullException("param"), "the error message");
        }
    }
}
