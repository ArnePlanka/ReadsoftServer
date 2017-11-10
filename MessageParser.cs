using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Net;
using dataLogicsLayer;
using ReadSoft.Services.Client.Entities;
using messageHandlers;
using actionSelector;
using logger;

namespace messageParser
{
    
    class MessageParser
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string who;
        // private static ReadSoft.Services.Client.Entities.Buyer b = new ReadSoft.Services.Client.Entities.Buyer();
        // private static ReadSoft.Services.Client.Entities.BuyerCollection bc = new ReadSoft.Services.Client.Entities.BuyerCollection();
        // private static ReadSoft.Services.Client.Entities.Email p;


        public MessageParser(string forWho)
        {
            
            this.who = forWho;
            log.Info("MessageParser Constructor");

        }

        public string HandleRequest(HttpListenerContext ct) {
            log.Info("Handle Request");
            string result = "";
            try
            {
                ActionSelector actionselector = new ActionSelector();
                Handlers messageHandler = new Handlers();
                HandlerData handlerData =
                    actionselector.GetHandlerData(ct.Request.HttpMethod + ct.Request.RawUrl);
                string Name = handlerData.handler.Method.Name;
                if (Name.Equals("Authenticate") || Name.Equals("IsAuthenticated")  ||
                    Name.Equals("SignOut") || Name.Equals("Touch") ||
                    messageHandler.IsSessionIdOk(ct))
                {
                    result = handlerData.handler(ct, handlerData.actionInfo);
                }
                else
                {
                    throw (new Exception("Not Authenticated."));
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw (e);
            }
            return result??"";
        }

    }
}
