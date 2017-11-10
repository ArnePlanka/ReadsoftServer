using messageParser;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;
using ReadSoft.Services.Client.Entities;
using NavisionServiceApp;

namespace controllWorker
{

    class ControllWorker
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HttpListenerContext context;
        private MessageParser messageParser = null;

        public ControllWorker(HttpListenerContext context, string system)
        {
            log.Info("Created");

            this.context = context;
            this.messageParser = new MessageParser(system);


        }

        /// <summary>
        /// Debug logging of http headers.
        /// </summary>
        /// <param name="what"></param>
        /// <param name="n"></param>
        private void DebugConsolePrintCollection(string what, NameValueCollection n)
        {
            string data = "\n" + what + ":\n";

            foreach (string x in n)
            {
                data += x.ToString() + "=" + n[x].ToString() + "\n";
            }
            log.Debug(data);
        }
        /// <summary>
        /// Remeber that this function is threaded.
        /// </summary>
        public void ProcessRequest()
        {
            try
            {
                log.Info("ControllWorker Process Request.");
                string msg = context.Request.HttpMethod + " " + context.Request.Url;
                log.Debug(msg);

                DebugConsolePrintCollection("Request Querystring", context.Request.QueryString);
                DebugConsolePrintCollection("Request-headers", context.Request.Headers);
                log.Debug(context.Request.Url.ToString());
                log.Info("Request:" + context.Request.HttpMethod + " " + context.Request.RawUrl);

                string contentType = context.Request.ContentType ?? "";
                string[] Accepts = context.Request.AcceptTypes ?? new string[] {};
                foreach (var s in Accepts)
                {
                    log.Debug("Accepts:" + s);
                }

                // Setup Response reply.
                context.Response.StatusCode = 200;
                context.Response.ContentType = HttpUtils.GetResponseType(HttpUtils.IsJson(Accepts));
                
                context.Response.ContentEncoding = context.Request.ContentEncoding;
                context.Response.KeepAlive = false;

                // Call the message Parser that calls the message handler.
                string result = messageParser.HandleRequest(context);

                log.Debug("Response Body:\n\n" + result);
                StringBuilder sb = new StringBuilder();
                sb.Append(result);
    
                DebugConsolePrintCollection("Respose Headers",context.Response.Headers);
                // Sending the data.
                SendBodyData(context, sb);

            }
            catch (Exception e)
            {
                log.Fatal("ControllWorker", e);
                SendErrorMessage(context, e.Message, HttpUtils.IsJson(context.Request.AcceptTypes));

            }

        }

        public void SendErrorMessage(HttpListenerContext ct, string message, bool isjson)
        {
            StringBuilder sb = new StringBuilder();
            ServiceError se = new ServiceError();
            se.Message = message;
            se.Code = "1001";
            string errorMessage = HttpUtils.ConvertToString<ServiceError>(se, isjson);
            SendBodyData(ct, errorMessage);
        }

        public void SendBodyData(HttpListenerContext ct, StringBuilder sb)
        {
            SendBodyData(ct, sb.ToString());
        }

        public void SendBodyData(HttpListenerContext ct, string str)
        { 
            byte[] b = Encoding.UTF8.GetBytes(str);
            ct.Response.ContentLength64 = b.Length;
            ct.Response.OutputStream.Write(b, 0, b.Length);
            ct.Response.OutputStream.Close();
        }

    }
}
