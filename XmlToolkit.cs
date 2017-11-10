using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json;

namespace NavisionServiceApp
{
    class XmlToolkit
    {
        /// <summary>
        /// Serialize an object to XML by JsonConvert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string Serialize<T>(T model)
        {
            string json = "";
            JsonSerializerSettings jsonSetting = new JsonSerializerSettings();
            jsonSetting.MaxDepth = 2;
            json = JsonConvert.SerializeObject(model,typeof(T),null);
            XNode node = JsonConvert.DeserializeXNode(json,typeof(T).Name,true);
            return node.ToString();
        }

        /// <summary>
        /// Deserialize XML to an object By JsonConvert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, true);
            T model = JsonConvert.DeserializeObject<T>(json);
            return model;
        }

        /// <summary>
        /// Deserialize by using binary formatter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// 
        public static dynamic Deserialize2(string xml)
        {
            dynamic result;

            try
            {
                var f_binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
                stream.Position = 0;

                result = f_binaryFormatter.Deserialize(stream);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return result;
        }
        /// <summary>
        /// serialize by using binary formatter.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize2(dynamic obj)
        {
            string result = "";
            try
            {
                using (MemoryStream stream = new MemoryStream(System.Runtime.InteropServices.Marshal.SizeOf(obj) * 100))
                {
                    var f_binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    f_binaryFormatter.Serialize(stream, obj);
                    result = stream.ToString();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return result;
        }
    }

}
