using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.NetFramework.Logging.Log4Net;
using NUnit.Framework;
using Infrastructure.Crosscutting.Logging;

namespace Infrastructure.Crosscutting.Tests
{
   [TestFixture]
   public class Log4NetTest
   {
       private ILogger myLogger = null;

       [SetUp]
       public void Log4NetTestStart()
       {
           Console.WriteLine("Beging Start");
           Log4NetFactory factory = new Log4NetFactory();
           factory.Create();
           LoggerFactory.SetCurrent(factory);

           myLogger = LoggerFactory.CreateLog();
       }

       [Test]
       public void Begin()
       {
           Console.WriteLine("Invoke Begin");
           
           myLogger.LogInfo("this is LogInfo Method");

           myLogger.LogWarning("this is Warning Method");

           myLogger.LogError("this is Not Exception Method Parameter");

           try
           {
               throw new Exception("异常");
           }
           catch (Exception ex)
           {
               myLogger.LogError("捕获到的异常",ex);
           } 

       } 

    }
}
