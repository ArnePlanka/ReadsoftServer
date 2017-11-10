using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadSoft.Services.Client.Entities;

namespace NavisionServiceApp.models
{
    public class Cookie
    {
        public string id { get; set; } 
        public string UserName  { get; set; }
  
        public Cookie(string cookieValue, string username)
        {
            id = cookieValue;
            UserName = username;
        }
    }

    public class Document
    {
        public  string id { get; set; }
        public string idField  { get; set; }
        public string versionField { get; set; }
        public string typeField { get; set; }
        public string originalFilenameField { get; set; }
        public string filenameField  { get; set; }
        public string partiesField  { get; set; }
        public string headerFieldsField  { get; set; }
        public string tablesField  { get; set; }
        public string processMessagesField  { get; set; }
        public string systemFieldsField  { get; set; }
        public string erpCorrelationDataField  { get; set; }
        public string baseTypeField  { get; set; }
        public string permalinkField  { get; set; }
        public string historyField  { get; set; }
        public string trackIdField  { get; set; }
        public string documentTypeField  { get; set; }
        public string validationInfoCollectionField { get; set; }
        public string originField { get; set; }
        // public DocumentAccountingInformation[] accountingInformationField  { get; set; }
        public string codingLinesField { get; set; }
        public string parkDocumentField { get; set; }
    }
}
