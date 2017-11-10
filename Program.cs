using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Net.Http;
using log4net;
using System.Diagnostics;
using NavisionServiceApp;
using httpserver;

namespace program
{
    class Program
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        static void Main(string[] args)
        {
            Console.WriteLine("Server start");
            HttpServer.StartServer();
        }
    }
}

