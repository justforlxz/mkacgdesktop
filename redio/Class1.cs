using System;
using System.Collections;
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

        public List<string>  ConnectTuLing ()
        {

            List<string> list = new List<string>();
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
                var urlMp3 =
             from p in json["result"]["tracks"]
             select (string)p["mp3Url"];

                foreach (var item in urlMp3)
                {
                  
                    list.Add(item);
               
                }
             
            }
            catch (Exception ex)
            {
                List<string> error = new List<string>();
                error.Add("网络失败");
                return error;
            }

            return RandomSortList(list);

        }
        public List<T> RandomSortList<T>(List<T> ListT)
        {
            Random random = new Random();
            List<T> newList = new List<T>();
            foreach (T item in ListT)
            {
                newList.Insert(random.Next(newList.Count + 1) , item);
            }
            return newList;
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