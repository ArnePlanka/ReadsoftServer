using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using System.Net;
using System.Xml;

namespace NavisionServiceApp
{
    class HttpUtils
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string jsonString = "application/json";
        const string xmlString = "application/xml";
        public const string CookieName = "x-rs-auth-v1";

        /// <summary>
        /// Gets the request Body from a POST inputstream. A Get has no reuqest body.
        /// </summary>
        /// <param name="io"></param>
        /// <returns></returns>
        public static string GetRequestBody(Stream stream)
        {
            StreamReader str = new StreamReader(stream, Encoding.UTF8);
            string result = "";
            result = str.ReadToEnd();

            log.Debug(result);
            return result??"";
        }

        
        /// <summary>
        /// Convert object to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string ConvertToString<T>(T model, bool json = true /* else xml */ )
        {
            string result;
            if (json)
            {
                result = JsonConvert.SerializeObject(model, Newtonsoft.Json.Formatting.Indented);
            }else
            {
                result = XmlToolkit.Serialize(model);
            }
            return result;
        }

        /// <summary>
        /// Convert data string to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ConvertToModel<T>(string data, bool json = true /* else xml */ ) {
            T model;
            if (json)
            {
                model = JsonConvert.DeserializeObject<T>(data);
            }
            else
            {
                model = XmlToolkit.Deserialize<T>(data);
            }

            return model;
                
        }

        /// <summary>
        /// Alternative methods.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="json"></param>
        /// <returns></returns>

        public static string ConvertToString2(dynamic model, bool json = true /* else xml */ )
        {
            string result;
            if (json)
            {
                result = JsonConvert.SerializeObject(model, Newtonsoft.Json.Formatting.Indented);
            }
            else
            {
                result = XmlToolkit.Serialize2(model);
            }
            return result;
        }


        public static dynamic ConvertToModel2(string data, bool json = true /* else xml */ )
        {
            dynamic model;
            if (json)
            {
                model = JsonConvert.DeserializeObject(data);
            }
            else
            {
                model = XmlToolkit.Deserialize2(data);
            }

            return model;

        }

        /// <summary>
        /// Check string[] if Json is requested.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static bool IsJson(string[] types)
        {
            if (types == null)
                return false;

            foreach(var t in types)
            {
                if (IsJson(t))
                {
                    return true;
                }
            }
            return false; // defaults to Not being Json. 
        } 

        /// <summary>
        /// returns contentType as string being json or xml.
        /// </summary>
        /// <param name="isjson"></param>
        public static string GetResponseType(bool isjson)
        {
            if(isjson)
            {
                return jsonString;
            } else
            {
                return xmlString;
            }
        }
        /// <summary>
        /// Check if json is sent foramt or requested response.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsJson(string type)
        {
            if (type.Equals(jsonString)  || type.Equals("*/*"))
            {
                return true;
            }
            else if (type.Equals(xmlString))
            {
                return false;
            }
            return false; // defaults to XML anyway.
        }

        private static string NewKey()
        {
            string resultKey = "";
            for (int i = 0; i < 14; i++)
            {
                resultKey += Guid.NewGuid().ToString("N");
            }
            return resultKey;
        }

        public static Cookie NewSessionCookie(string domain ="", double hours = 2.0)
        {
            Cookie SessionCookie = new Cookie(CookieName, NewKey(),"/",domain);
            SessionCookie.Expires.AddHours(hours);
            // SessionCookie.HttpOnly = true;
            log.Debug(SessionCookie.Value);
            return SessionCookie;
        }
    }
}
