using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Logging;

namespace Infrastructure.Crosscutting.NetFramework.Logging.Log4Net
{
    internal sealed class Log4NetProvider : ILogger
    {
        // 指定的 <logger name="MyLogger"> 
        private static log4net.ILog myLogger = log4net.LogManager.GetLogger("MyLogger");

        //这是不指定Logger所以一般会读取<root>
        //private static log4net.ILog myLogger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         
        #region Implementation of ILogger

        /// <summary>
        /// Log message information 
        /// </summary>
        /// <param name="message">The information message to write</param>
        /// <param name="args">The arguments values</param>
        public void LogInfo(string message, params object[] args)
        {
            if (!String.IsNullOrEmpty(message)
                &&
                !String.IsNullOrEmpty(message))
            {
                var traceData = string.Format(CultureInfo.InvariantCulture, message, args);

                myLogger.Info(traceData);
            }
        }

        /// <summary>
        /// Log warning message
        /// </summary>
        /// <param name="message">The warning message to write</param>
        /// <param name="args">The argument values</param>
        public void LogWarning(string message, params object[] args)
        {
            if (!String.IsNullOrEmpty(message)
               &&
               !String.IsNullOrEmpty(message))
            {
                var traceData = string.Format(CultureInfo.InvariantCulture, message, args);

                myLogger.Warn(traceData);
            }
        }

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="message">The error message to write</param>
        /// <param name="args">The arguments values</param>
        public void LogError(string message, params object[] args)
        {
            if (!String.IsNullOrEmpty(message)
               &&
               !String.IsNullOrEmpty(message))
            {
                var traceData = string.Format(CultureInfo.InvariantCulture, message, args);

                myLogger.Error(traceData);
            }
        }

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="message">The error message to write</param>
        /// <param name="exception">The exception associated with this error</param>
        /// <param name="args">The arguments values</param>
        public void LogError(string message, Exception exception, params object[] args)
        {
            if (!String.IsNullOrEmpty(message)
                &&
                !String.IsNullOrEmpty(message))
            {
                var traceData = string.Format(CultureInfo.InvariantCulture, message, args);

                myLogger.Error(traceData,exception);
            }
        }

        #endregion
    }
}
