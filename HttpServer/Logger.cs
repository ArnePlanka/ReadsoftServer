using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Diagnostics;

namespace ConsoleApp3.HttpServer
{    
    class Logger
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // private static long starttime = DateTime.Now.Ticks;
        // private static StackFrame sf = new StackFrame();

        public static void Info(string message)
        {
            StackFrame sf = new StackFrame(1);
            string Method = sf.GetMethod().ToString();
            log.Info(Method + " " + message);
        }

        public static void Debug(string message)
        {
            StackFrame sf = new StackFrame(1);
            string Method = sf.GetMethod().ToString();
            log.Debug(Method + " " + message);
        }
        public static void Warn(string message)
        {
            StackFrame sf = new StackFrame(1);
            string Method = sf.GetMethod().ToString();
            log.Warn(Method + " " + message);
        }
        public static void Fatal(string message)
        {
            StackFrame sf = new StackFrame(1);
            string Method = sf.GetMethod().ToString();
            log.Fatal(Method + " " + message);
        }

        public static void Error(string message)
        {
            StackFrame sf = new StackFrame(1);
            string Method = sf.GetMethod().ToString();
            log.Error(Method + " " + message);
        }


    }
}
