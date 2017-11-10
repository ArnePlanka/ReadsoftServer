using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using messageParser;
using controllWorker;
using log4net;
using logger;
using actionSelector;
using messageHandlers;
using utils;

namespace httpserver
{
    
    public class HttpServer
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string[] prefixes;
        private string system;
        //There can only be one httplistener for a specific port.
        private HttpListener listener;
        Handlers messageHandler = new Handlers();
        
        //A singleton.
        static HttpServer server = null;

        public HttpServer(string[] adresses, string system)
        {
            this.prefixes = adresses;
            this.system = system;
        }

        private void SetupActionSelector()
        {

            ActionSelector.Add("Authenticate","POST", "/authentication/rest/authenticate","","", messageHandler.Authenticate);
            ActionSelector.Add("Touch", "GET", "/authentication/rest/touch", "", "",messageHandler.Touch);
            ActionSelector.Add("GetCurrentUser", "GET", "/users/rest/currentuser", "", "", messageHandler.GetCurrentUser);
            ActionSelector.Add("GetCurrentCustomer", "GET", "/accounts/rest/currentcustomer", "", "",messageHandler.GetCurrentCustomer);
            ActionSelector.Add("GetAllCustomers", "GET", "/accounts/rest/customers", "", "", messageHandler.GetAllCustomers);
            ActionSelector.Add("GetCurrentUserBuyers", "GET", "/accounts/rest/customers/currentuser/buyers", "", "", messageHandler.GetCurrentUserBuyers);
            ActionSelector.Add("GetBuyerExtractionConfiguration", "GET", "/accounts/rest/customers/buyers","/services/extraction", "/{buyerId}", messageHandler.GetAccountExtractionConfiguration);
            ActionSelector.Add("UploadImage2", "POST", "/files/rest/image2", "", "", messageHandler.UploadImage2);
            ActionSelector.Add("GetCustomerExtractionConfiguration", "GET", "/accounts/rest/customers", "/services/extraction", "/{customerId}", messageHandler.GetCustomerExtractionConfiguration);
            ActionSelector.Add("GetOutputDocumentsByCurrentCustomer", "GET", "/documents/rest/customers/outputdocumentsbycurrentcustomer", "", "", messageHandler.GetOutputDocumentsByCurrentCustomer);
            ActionSelector.Add("GetDocumentOutputImage", "GET", "/documents/rest/file", "/image", "/{documentId}", messageHandler.GetDocumentOutputImage);
            ActionSelector.Add("GetDocumentOutputData", "POST", "/documents/rest/file", "/data", "/{documentId}", messageHandler.GetDocumentOutputData);    
            ActionSelector.Add("DocumentStatus", "PUT", "/documents/rest", "/documentstatus", "/{documentId}", messageHandler.DocumentStatus);
            ActionSelector.Add("IsAuthenticated", "GET", "/authentication/rest/isauthenticated", "", "", messageHandler.IsAuthenticated);
            ActionSelector.Add("SignOut", "POST", "/authentication/rest/signout", "", "", messageHandler.SignOut);
            ActionSelector.Add("ActivateCustomer", "POST", "/accounts/rest/customers", "/activate", "/{customerId}", messageHandler.ActivateCustomer);
            ActionSelector.Add("CreateCustomer", "POST", "/accounts/rest/customers", "", "", messageHandler.CreateCustomer);
            ActionSelector.Add("GetOutputDocuments", "GET", "/documents/rest/customers","/outputdocuments", "/{customerId}", messageHandler.GetOutputDocuments);
            ActionSelector.Add("GetDocument", "GET", "/documents/rest", "", "/{customerId}", messageHandler.GetDocument);
            ActionSelector.Add("LearnDocument", "POST", "/documents/rest", "/learningdocument", "/{documentId}", messageHandler.LearnDocument);
            ActionSelector.Add("GetUserConfiguration", "GET", "/accounts/rest/customers", "/userconfiguration", "/{organizationId}", messageHandler.GetUserConfiguration);




            //Should always be the last handler.
            ActionSelector.Add("DummyHandler","", "/", "", "", messageHandler.Dummy);
        }

        public static void StartServer()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                log.Info("Application is starting");

                // get this one from appsetting
                string[] prefixes = Utils.GetAppSetting("HOSTLIST").Split(',');


                server = new HttpServer(prefixes, "Navision");
                log.Debug("adresser:");

                foreach (var p in prefixes)
                    log.Debug(p.ToString());

                new Thread(server.Start).Start();
            }
            catch (Exception e)
            {
                log.Fatal("Exception in Main", e);
                log.Fatal(e.Message);
            }
        }

        /// <summary>
        /// Stop the Http Server.
        /// </summary>
        public static void StopServer()
        {
            if (server != null)
            {
                server.Stop();
            }
            server = null;
           
        }

        /// <summary>
        /// Http/s Server creation
        /// </summary>
        public void Start()
        {
            try
            {
                this.SetupActionSelector();
                listener = new HttpListener();
                foreach (string s in prefixes)
                    listener.Prefixes.Add(s.Trim());
                listener.Start();
                log.Info("HTTP Server starting/listening...");
                log.Debug("HTTP Server starting/listening...");


                // Listen to incoming requests forever.
                for (;;)
                {
                    try
                    {
                        // This is where we wait for connection
                        HttpListenerContext ctx = listener.GetContext();
                        log.Info("New Connection...");
                        new Thread(new ControllWorker(ctx, system).ProcessRequest).Start();
                    }
                    catch (Exception e)
                    {
                        log.Fatal("HTTP Server New connection error", e);
                        log.Error(e.Message);

                    }
                }
            }catch(Exception e)
            {
                log.Fatal("HttpServer Start Error", e);
                throw e;
            }
        }

        public void Stop()
        {
            try
            {
                if (listener != null)
                {
                    listener.Stop();
                    listener = null;
                }
            }catch(Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }

    
}