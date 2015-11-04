using System;
using System.Collections.Generic;
using System.IO;
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
                string getURL = "D:\\Documents\\Untitled-1.json";
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
                //解析json
                JsonReader reader = new JsonTextReader(new StringReader(result));
                while (reader.Read())
                {
                    //text中的内容才是你需要的
                    if (reader.Path == "mp3Url")
                    {
                        //结果赋值
                        result = reader.Value.ToString();
                        Console.WriteLine(result);
                    }
                    Console.WriteLine(reader.TokenType + "\t\t" + reader.ValueType + "\t\t" + reader.Value);
                }
            }
            catch (Exception ex)
            {
                result=ex.ToString();
            }
            return result;
        }



    }
}