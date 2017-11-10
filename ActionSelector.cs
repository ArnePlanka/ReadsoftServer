using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using logger;

namespace actionSelector
{
    public class ActionInfo : IDisposable
    {
        public string argument { get; private set; }
        public string method { get; private set; }
        public string action { get; private set; }
        public string name { get; private set; }
        public string variables { get; private set; }

        public ActionInfo(string argument,string method, string action, string name,string variables)
        {
            this.argument = argument;
            this.method = method;
            this.action = action;
            this.name = name;
            this.variables = variables;
        }

        /// <summary>
        /// Gets a string value from the data string with respect to what to get. 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="what"></param>
        /// <returns></returns>
        public string GetParameter(string data, string whatToGet)
        {
            string result = "";
            var Valuelist = data.Split(new char[] { '/' });
            bool returnNext = false;

            foreach(var v in Valuelist)
            {
                if (returnNext)
                {
                    return v;
                }
                //Getting next or getting previous?
                if(whatToGet.ToUpper().Contains(v.ToUpper())) 
                {
                    returnNext = true;
                }

            }
            return ""; // Couldn't be found.
        }

        /// <summary>
        /// Return a list of pairs for this Objects varibles and their correlating value from the request string argument 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="what"></param>
        /// <param name="identifiers"></param>
        /// <returns></returns>
        public  Dictionary<string,long> GetParameterList(string data)
        {
            Dictionary<string, long> result = new Dictionary<string, long>();
            try
            {
                var Variablelist = this.variables.Split(new char[] { '{', '}', '/' });

                var Valuelist = data.Split(new char[]{'/' });

                List<long> Values = new List<long>();

                foreach (var v in Valuelist)
                {
                    if (v.Length > 1)
                    {

                        try
                        {
                            long val;
                            if (Int64.TryParse(v, out val))
                            {
                                Values.Add(val);
                            }
                        }
                        catch
                        {
                            // just try again with the next value.
                        }
                    }
                }

                int i = 0;
                foreach (var v in Variablelist)
                {
                    if (v.Length > 1)
                    {
                        result.Add(v,Values[i]);
                        i++;
                    }
                }
            }catch(Exception ex)
            {
                Logger.Fatal(ex.Message);
            }

            return result;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    delegate string MessageHandler(HttpListenerContext context, ActionInfo hd); 

    class HandlerData
    {
        public MessageHandler handler { get; private set; }
        public ActionInfo actionInfo { get; private set; }

        public HandlerData(MessageHandler mh, string name, string method, string action, string argument, string variables)
        {
            handler = mh;
            actionInfo = new ActionInfo(argument, method, action, name,variables);
        }
    }

    class ActionSelector
    {

        private static Dictionary<ActionInfo, HandlerData> HandlerList = new Dictionary<ActionInfo, HandlerData>();

        public ActionSelector()
        {
        }
        /// <summary>
        /// Add a handler for message
        /// </summary>
        /// <param name="action"></param>
        /// <param name="handler"></param>
        public static void Add(string name, string method, string action, string argument, string variables, MessageHandler handler)
        {
            HandlerData hd = new HandlerData(handler, name, method, action, argument,variables);
            HandlerList.Add(hd.actionInfo, hd);
        }
        /// <summary>
        /// Gets the HandlerData Object;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public HandlerData GetHandlerData(string key)
        {
           
            Exception ex = new Exception("Funtion not implemented yet");
            HandlerData hd = null;
            try
            {
                foreach (KeyValuePair<ActionInfo, HandlerData> pair in HandlerList)
                {
                    using (ActionInfo ai = pair.Value.actionInfo)
                    {
                        if (key.StartsWith(ai.method + ai.action) && key.Contains(ai.argument))
                        {
                            //Found a  match.
                            hd = pair.Value;
                            break;
                        }

                    }
                }
            }
            catch
            {
                throw (ex);
            }

            // Found the function.
            return hd;
        }

    }
}
