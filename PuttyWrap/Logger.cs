using System;
using System.Collections.Generic;
using System.Text;

namespace PuttyWrap
{
    public delegate void LogCallback(string logLine);
    public static class Logger
    {
        public static event LogCallback OnLog;
        
        public static void Log(string logLine)
        {
            if (OnLog != null)
                OnLog(logLine);
        }
        public static void Log(Exception e)
        {
            if (OnLog != null)
                OnLog(e.Message);
        }

        public static void Log(string format, params object[] arg)
        {
            if (OnLog != null)
                OnLog(String.Format(format, arg));
        }
    }
}
