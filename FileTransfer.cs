using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using utils;
using System.IO;

namespace NavisionServiceApp
{

    class FileTransfer
    {
        static string FTPPROTO = Utils.GetAppSetting("FTPPROTO");
        static string USER = Utils.GetAppSetting("FTPUSER");
        static string PASS = Utils.GetAppSetting("FTPPASS");
        static string HOST = Utils.GetAppSetting("FTPHOST");
        
        static string FTPHOST = FTPPROTO + HOST;
        static string REMOTEIN = Utils.GetAppSetting("FTPIN");
        static string REMOTEOUT = Utils.GetAppSetting("FTPOUT");
        static string FTPURL = FTPPROTO + USER + ":" + PASS + "@" + HOST;

        private WebClient client = null;
        private FtpWebRequest request = null;
        private FtpWebResponse response = null;

        public enum direction { NONE = 0, IN = 1, OUT = 2 };

        public FileTransfer()
        {
            client = new WebClient();
            client.Credentials = new NetworkCredential(USER, PASS);

            // Hmmm Verkar vara exakt samma funtion. 
            request = (FtpWebRequest)FtpWebRequest.Create(FTPHOST);
            request = (FtpWebRequest)WebRequest.Create(FTPHOST);

        }

        public void PutFile(string name, string custid, string data, direction dir)
        {

        }

        public string GetFile(string name,string custid, direction dir)
        {
            return "";
        }

        public bool DeleteFile(string name,string custid, direction dir)
        {
            // dele
            return true;
        } 

        public bool CheckCustomer(string custid, string path)
        {
            // Does Customer dir exist?
            return false;
        }

        public void CreateCustomer(string custid, string path)
        {
            // check dir custid.
            // Create the dir if not exists.
            if (!CheckCustomer(custid,path))
            {
                    
            }
 
        }

        public List<string> GetFilelist(string custid, direction dir)
        {
            List<string> result = new List<string>();
            switch(dir)
            {
                case direction.IN:
                    GetFileListIn(custid);
                    break;
                case direction.OUT:
                    GetFileListOut(custid);
                    break;
                default:
                    break;


            }
            return result;
        }

        public List<string> GetFileListIn(string custid)
        {
            // Get files going IN 
            List<string> result = new List<string>(); ;
            return result;

        }

        public List<string> GetFileListOut(string custid)
        {
            // Get files going OUT 
            List<string> result = new List<string>(); ;
            return result;

        }


        public List<string> GetFilelist(string custid, string path)
        {
            return null;
        }
        
            /// <summary>
            /// Rest is just for test.
            /// </summary>
            public void UploadFile()
        {


            try
            {
                //Settings required to establish a connection with the server
                this.request = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://ServerIP/FileName"));
                this.request.Method = WebRequestMethods.Ftp.UploadFile;
                this.request.Proxy = null;
                this.request.UseBinary = true;
                this.request.Credentials = new NetworkCredential("UserName", "Password");

                //Selection of file to be uploaded
                FileInfo ff = new FileInfo("File Local Path With File Name");//e.g.: c:\\Test.txt
                byte[] fileContents = new byte[ff.Length];

                //will destroy the object immediately after being used
                using (FileStream fr = ff.OpenRead())
                {
                    fr.Read(fileContents, 0, Convert.ToInt32(ff.Length));
                }

                using (Stream writer = request.GetRequestStream())
                {
                    writer.Write(fileContents, 0, fileContents.Length);
                }
                //Gets the FtpWebResponse of the uploading operation
                this.response = (FtpWebResponse)this.request.GetResponse();
                Console.WriteLine(this.response.StatusDescription); //Display response
            }
            catch (WebException webex)
            {
                string Message = webex.ToString();
            }
        }

        public List<string> getFilesList()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPURL);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = new NetworkCredential(USER, PASS);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            List<string> names = new List<string>();
            while (!reader.EndOfStream)
            {
                names.Add(reader.ReadLine());
            }

            reader.Close();
            response.Close();
            return names;
        }


        public bool IsFtpFileExists(string remoteUri, out long remFileSize)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPURL);
            FtpWebResponse response;

            request.Method = WebRequestMethods.Ftp.GetFileSize;
            request.Credentials = new NetworkCredential(USER, PASS);
            try
            {
                response = (FtpWebResponse)request.GetResponse();
                remFileSize = response.ContentLength;
                return true;
            }
            catch (WebException we)
            {
                response = we.Response as FtpWebResponse;
                if (response != null && response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    remFileSize = 0;
                    return false;
                }
                throw;
            }
        }

        public bool SendFiledata(string name, string custid)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://www.contoso.com/test.htm");
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");

            // Copy the contents of the file to the request stream.
            StreamReader sourceStream = new StreamReader("testfile.txt");
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

            response.Close();

            return false;
        }

        

        public void deleteFile(string name, string data, string id)
        {

            // Get the object used to communicate with the server
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPURL);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(USER, PASS);


            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            logger.Logger.Info(String.Format("Delete status: {0}", response.StatusDescription));
            response.Close();
        }


        public void SendFile(string name, string data, string id)
        {
            // request.Method = WebRequestMethods.Ftp.DeleteFile;
            // 
            byte[] array = Encoding.ASCII.GetBytes(data);
            client.UploadData(REMOTEIN + "/" + DateTime.Now.ToString() + Guid.NewGuid().ToString("N") + "_" + name, array);
            response = (FtpWebResponse)request.GetResponse();
        }

        public string GetFile(string name)
        {
            byte[] newFileData = client.DownloadData(REMOTEOUT + "/" + name);
            string fileString = System.Text.Encoding.UTF8.GetString(newFileData);

            return fileString;
        }

        public string downloadFileData(string name, string custid)
        {


            // Get the object used to communicate with the server.  
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://www.contoso.com/test.htm");
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.  
            request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string result = reader.ReadToEnd();
            Console.WriteLine(result);

            Console.WriteLine("Download Complete, status {0}", response.StatusDescription);

            reader.Close();
            response.Close();

            return result;
        }
    }
}
