using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace redio
{
    public class redio
    {
        HttpWebResponse Response = null;
        public string ConnectTuLing ()
        {
            string result = null;
            try
            {
                string getURL = "http://music.163.com/api/playlist/detail?id=23075108";
                HttpWebRequest MyRequest = (HttpWebRequest)HttpWebRequest.Create(getURL);
                HttpWebResponse MyResponse = (HttpWebResponse)MyRequest.GetResponse();
                Response = MyResponse;
                using (Stream MyStream = MyResponse.GetResponseStream())
                {
                    long ProgMaximum = MyResponse.ContentLength;
                    long totalDownloadedByte = 0;
                    byte[] by = new byte[1024];
                    int osize = MyStream.Read(by , 0 , by.Length);
                    Encoding encoding = Encoding.UTF8;
                    while (osize > 0)
                    {
                        totalDownloadedByte = osize + totalDownloadedByte;
                        result += encoding.GetString(by , 0 , osize);
                        long ProgValue = totalDownloadedByte;
                        osize = MyStream.Read(by , 0 , by.Length);
                    }
                }
                JObject json = (JObject)JsonConvert.DeserializeObject(result);
               
                JArray tracks = (JArray)json["tracks"];
                JArray mp3Url = (JArray)tracks["mp3Url"];
                foreach (var jObject in mp3Url)
                {
                    Console.WriteLine(jObject);
                }

            }
            catch (Exception ex)
            {
                result=ex.ToString();
            }
           
            return result;

        }

        private string ConvertJsonString (string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented ,
                    Indentation = 4 ,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter , obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }

    }
}