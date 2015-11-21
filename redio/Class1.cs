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
        List<string> list = new List<string>();
        List<string> name_music = new List<string>();
        List<string> music = new List<string>();
        public List<string> ConnectTuLing (String music_id)
        {
          
            string result = null;
            int count = 0;
            list.Clear();
            name_music.Clear();
            music.Clear();
            music.Clear();
            try
            {
                string getURL = "http://music.163.com/api/playlist/detail?id="+music_id;
                Console.WriteLine("来自redio-->"+getURL);
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
                var name =
              from p in json["result"]["tracks"]
              select (string)p["name"];
                foreach (var item in urlMp3)
                {
                    list.Add(item);
                    count += 1;
                }
                foreach (var item in name)
                {
                    name_music.Add(item);
                }
                Random random = new Random();
                int n = random.Next(0 , count);
                music.Add(list[n]);
                music.Add(name_music[n]);
            }
            catch (Exception)
            {
                List<string> error = new List<string>();
                error.Add("网络失败");
                return error;
            }

            //    return RandomSortList(list);
            return music;
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