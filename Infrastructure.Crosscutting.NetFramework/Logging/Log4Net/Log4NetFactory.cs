using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Logging;

namespace Infrastructure.Crosscutting.NetFramework.Logging.Log4Net
{
    public class Log4NetFactory : ILoggerFactory
    {
        #region Implementation of ILoggerFactory

        /// <summary>
        /// Create a new ILog
        /// </summary>
        /// <returns>The ILog created</returns>
        public ILogger Create()
        {
            return new Log4NetProvider();
        }

        #endregion
    }
}
