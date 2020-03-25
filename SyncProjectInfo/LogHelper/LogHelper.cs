﻿using System;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncProjectInfo
{

    public class LogHelper
    {
        public static void WriteLog(string message)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Info(message);
            }
            return;

        }
        static public ILog GetLog()
        {
            return LogManager.GetLogger("Main");
        }

        static public void Error(string message, Exception ex)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Error(message, ex);
            }
        }
    }
}
