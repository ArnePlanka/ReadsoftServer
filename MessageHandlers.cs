using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ReadSoft.Services.Client.Entities;
using ReadSoft.Services.Client;
using actionSelector;
using NavisionServiceApp;
using dataLogicsLayer;
using System.IO;
using logger;

namespace messageHandlers
{
    class Handlers
    {
        // public static ReadSoft.Services.Client.Entities.AuthenticationCredentials ac = new AuthenticationCredentials();
        SessionCookieHandler sessionCookies = new SessionCookieHandler();
        const string CookieName = HttpUtils.CookieName;

        public bool IsSessionIdOk(HttpListenerContext ct)
        {
            try
            {

                CookieCollection cookieCollection = ct.Request.Cookies;
                Cookie c = cookieCollection[CookieName];
                if (c == null)
                {
                    return false;
                }
                else
                {
                    return sessionCookies.IsValid(c);
                }
            }
            catch
            {
                return false;
            }
        }

        public string GetCurrentUserBuyers(HttpListenerContext ct, ActionInfo hi)
        {
            BuyerCollection Buyers = new BuyerCollection();
            Buyer buyer = new Buyer() { Id = "1", Name = "jag", ExternalId = "2" };
            Buyers.Add(buyer);

            return GetResponseString<BuyerCollection>(Buyers, ct);
        }

        public string GetAllCustomers(HttpListenerContext ct, ActionInfo hi)
        {

            DataLogics logicsLayer = new DataLogics();
            var result = logicsLayer.SelectAllCustomers();

            CustomerCollection cc = new CustomerCollection();

            foreach(var cust in result)
            {
                cc.Add(cust);
            }

            return GetResponseString<CustomerCollection>(cc, ct);
        }


        public string GetUserConfiguration(HttpListenerContext ct, ActionInfo hi)
        {
            UserConfiguration uc = new UserConfiguration();
            return GetString(uc, ct);
        }


        public string LearnDocument(HttpListenerContext ct, ActionInfo hi)
        {
            Document doc = GetRequestObject<Document>(ct);

            BoolValue bv = new BoolValue();
            bv.Value = true;

            return GetResponseString<BoolValue>(bv, ct);
        }

        public string GetDocument(HttpListenerContext ct, ActionInfo hi)
        {
            Document doc = new Document();
            doc.Filename = "Data.txt";
            doc.DocumentType = "text";
            doc.Id = "22";
            Party p = new Party();
            p.Name = "Sven";
            p.Type = "supplier";
            Party p2 = new Party();
            p2.Name = "7";
            p2.Type = "invoicenumber";
            doc.Parties.Add(p);
            doc.Parties.Add(p2);


            HeaderField hf = new HeaderField();
            hf.Type = "invoicenumber";
            hf.Text = "5";

            HeaderField hf2 = new HeaderField();
            hf2.Type = "invoicetotalvatincludedamount";
            hf2.Text = "55";

            doc.HeaderFields.Add(hf2);
            doc.HeaderFields.Add(hf);
            return GetResponseString<Document>(doc, ct);
        }


        public string GetOutputDocuments(HttpListenerContext ct, ActionInfo hi)
        {
            var inputData = hi.GetParameterList(ct.Request.RawUrl);
            long CustomerId = inputData["customerId"];

            DocumentReferenceCollection drc = new DocumentReferenceCollection();
            DocumentReference dr = new DocumentReference();
            dr.CustomerId = "1";
            dr.DocumentId = "23";
            dr.ImagePageCount = 1;
            dr.BuyerId = "3";
            dr.BatchId = "34";
            dr.DocumentUri = new Uri("http://localhost:9090/test22.pdf");
            // dr.OutputOperation = OutputOperationType.Export;

            drc.Add(dr);

            return GetResponseString<DocumentReferenceCollection>(drc, ct);
        }

        public string ActivateCustomer(HttpListenerContext ct, ActionInfo hi)
        {
            //activate customer.
            var paramList = hi.GetParameterList(ct.Request.RawUrl);
            long customerId = paramList["customerId"];
            DataLogics logicsLayer = new DataLogics();
            bool result = logicsLayer.ActivateCustomer(customerId.ToString());

            return GetResponseString<Boolean>(result, ct);
        }

        public string CreateCustomer(HttpListenerContext ct, ActionInfo hi)
        {
            
            Customer cust = GetRequestObject<Customer>(ct);
            Logger.Debug(cust.ToString());

            DataLogics logicsLayer = new DataLogics();
    
            Customer CustResult = logicsLayer.CreateCustomer(cust);

            return GetResponseString<Customer>(CustResult, ct);
          
        }


        public string GetCurrentCustomer(HttpListenerContext ct, ActionInfo hi)
        {
            // Dummy customer for now.
            Customer cust = new Customer { Name = "AB AB", ActivationStatus = OrganizationActivationStatus.Active, Id = "1", ExternalId = "1", ClassificationValue = "1" };
            return GetResponseString<Customer>(cust, ct);
        }

        public string GetCurrentUser(HttpListenerContext ct, ActionInfo hi)
        {
            // returns a dummy.
            User2 user = new User2() { FullName = "Arne" };
            return GetResponseString<User2>(user, ct);
        }

        public string SignOut(HttpListenerContext ct, ActionInfo hi)
        {
            AuthenticationResult ar = new AuthenticationResult();

            CookieCollection cookieCollection = ct.Request.Cookies;
            Cookie c = cookieCollection[CookieName];

            if (c == null)
            {
                sessionCookies.Remove(c);
            }

            ar.Status = AuthenticationStatus.Success;
            return GetResponseString<AuthenticationResult>(ar, ct);
        }

        public string IsAuthenticated(HttpListenerContext ct, ActionInfo hi)
        {
            BoolValue bv = new BoolValue();
            bv.Value = false;
            if (IsSessionIdOk(ct))
            {
                bv.Value = true;
            }
            return GetResponseString<BoolValue>(bv, ct);
        }

        public string Touch(HttpListenerContext ct, ActionInfo hi)
        {
            string result = "";
            foreach (Cookie c in ct.Request.Cookies)
            {
                if (sessionCookies.UpdateCookie(c, 2))
                {
                    return result;
                }

            }

            ServiceError se = new ServiceError();
            se.Code = "1002";
            se.Message = "Can't Touch up cookie.";
            return GetResponseString<ServiceError>(se, ct);
        }

        /// <summary>
        /// Handles Authenticate message
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public string Authenticate(HttpListenerContext ct, ActionInfo hi)
        {
            // Get an Object from the request.
            AuthenticationCredentials AuthCredentials = GetRequestObject<AuthenticationCredentials>(ct);

            DataLogics DataBase = new DataLogics();

            bool loginSucess = DataBase.Authenticate(AuthCredentials.UserName, AuthCredentials.Password);

            // The return result.
            AuthenticationResult result = new AuthenticationResult();

            if (!loginSucess)
            {
                result.Status = AuthenticationStatus.Failed;
                return GetResponseString<AuthenticationResult>(result, ct);
            }

            // Login is okej. Lets continue.
            result.Status = AuthenticationStatus.Success;
            // Always create a new sessionCookie on Authenticate. 
            Cookie SessionCookie = HttpUtils.NewSessionCookie(ct.Request.Url.Host);

            // Add Cookie to Cookies list.
            sessionCookies.Add(SessionCookie);
            // Send cookie to client.
            ct.Response.SetCookie(SessionCookie);
            // result.ClientDeviceSecret = guid.ToString();
            if(DataBase.SaveCookie(SessionCookie.Value,AuthCredentials.UserName))
            {
                Logger.Error(String.Format("Couldn't save sessionCookie {0} for user {1} to database", SessionCookie.ToString(), AuthCredentials.UserName));
            }

            return GetResponseString<AuthenticationResult>(result, ct);
        }

        public string GetAccountExtractionConfiguration(HttpListenerContext ct, ActionInfo hi)
        {
            ExtractionConfiguration Ec = new ExtractionConfiguration();
            Ec.DefaultDocumentSpecification = "Stuff";
            Ec.OrientationDetectionMode = OrientationDetectionMode.Off;
            DocumentType dt = new DocumentType();
            dt.Country = "SE";
            dt.DisplayName = "PDF";
            dt.ExtractionName = "Thefil.pdf";
            FieldGroup Fg = new FieldGroup();
            Fg.IsTableGroup = true;
            Fg.Name = "My Group";
            Fg.Order = 23;
            dt.FieldGroups.Add(Fg);
            DocumentTypeField dtf = new DocumentTypeField();
            dtf.FormatType = FormatType.RegularExpression;

            ExtractionType et = new ExtractionType();
            dtf.ExtractionType = et;

            dt.Fields.Add(dtf);
            dt.InternalName = "hejsan";
            dt.SystemName = "Thesystemname";
            dt.IsSelected = true;

            Ec.SelectedDocumentTypes.Add(dt);

            return GetResponseString<ExtractionConfiguration>(Ec, ct);
        }

        public string UploadImage2(HttpListenerContext ct, ActionInfo hi)
        {
            NameValueCollection querystring = ct.Request.QueryString;
            string custid = "";

            string thefileData = HttpUtils.GetRequestBody(ct.Request.InputStream);
            Logger.Debug("Received file data:\n" + thefileData);
            FileTransfer ft = new FileTransfer();
            ft.SendFile("test", thefileData, custid);
            BoolValue result = new BoolValue();
            result.Value = true;

            return GetResponseString<BoolValue>(result, ct);
        }


        public string GetDocumentOutputImage(HttpListenerContext ct, ActionInfo hi)
        {
            NameValueCollection querystring = ct.Request.QueryString;
            string result;
            ct.Response.ContentType = "image/jpeg";
            FileTransfer ft = new FileTransfer();
            var data = ft.GetFile("small.jpg");

            using (var stream = File.Open("small.jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                StreamReader reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }

            return result;
        }

        public string GetDocumentOutputData(HttpListenerContext ct, ActionInfo hi)
        {
            MetaDataCollection mdc = GetRequestObject<MetaDataCollection>(ct);
            NameValueCollection querystring = ct.Request.QueryString;
            string result;
            ct.Response.ContentType = "image/jpeg";

            using (var stream = File.Open("small.jpg", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                StreamReader reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }

            return result;
        }

        public string GetOutputDocumentsByCurrentCustomer(HttpListenerContext ct, ActionInfo hi)
        {
            DocumentReferenceCollection drc = new DocumentReferenceCollection();
            DocumentReference dr = new DocumentReference();
            dr.CustomerId = "1";
            dr.DocumentId = "1";
            dr.BuyerId = "1";
            dr.DocumentUri = new Uri("File://test.pdf");
            dr.ImageUri = new Uri("File://Test.jpg");
            DocumentReference dr2 = new DocumentReference();
            dr2.CustomerId = "2";
            dr2.DocumentId = "1";
            dr2.BuyerId = "1";
            dr2.DocumentUri = new Uri("File://test.pdf");
            dr2.ImageUri = new Uri("File://Test.jpg");
            drc.Add(dr2);

            return GetResponseString<DocumentReferenceCollection>(drc, ct);
        }

        public string GetCustomerExtractionConfiguration(HttpListenerContext ct, ActionInfo hi)
        {
            ExtractionConfiguration Ec = new ExtractionConfiguration();
            Ec.DefaultDocumentSpecification = "A spec";
            Ec.OrientationDetectionMode = OrientationDetectionMode.Off;
            DocumentType dt = new DocumentType();
            dt.IsSelected = true;
            dt.Languages.Add("SE");
            dt.ExtractionName = "TEst.pdf";
            dt.InternalName = "TeSt.pdf";
            Ec.SelectedDocumentTypes.Add(dt);
            return GetResponseString<ExtractionConfiguration>(Ec, ct);
        }

        public string DocumentStatus(HttpListenerContext ct, ActionInfo hi)
        {

            OutputResult output = GetRequestObject<OutputResult>(ct);
            BoolValue bv = new BoolValue();
            bv.Value = true;
            return GetResponseString<BoolValue>(bv, ct);
        }


        public string Dummy(HttpListenerContext ct, ActionInfo hi)
        {
            Logger.Debug("DummyHandler");
            return "";
        }

        // helpers

        /// <summary>
        /// Returns the wanted object given the format sent in the context request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ct"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public T GetRequestObject<T>(HttpListenerContext ct)
        {
            return HttpUtils.ConvertToModel<T>(
                 HttpUtils.GetRequestBody(ct.Request.InputStream), HttpUtils.IsJson(ct.Request.AcceptTypes));
        }

        /// <summary>
        /// Returns Response String from the given resposen Object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public string GetResponseString<T>(T obj, HttpListenerContext ct)
        {
            return HttpUtils.ConvertToString<T>(obj, HttpUtils.IsJson(ct.Request.AcceptTypes)) ?? "";
        }

        public string GetString(dynamic obj, HttpListenerContext ct)
        {
            return HttpUtils.ConvertToString(obj, HttpUtils.IsJson(ct.Request.AcceptTypes)) ?? "";
        }
    }
}
