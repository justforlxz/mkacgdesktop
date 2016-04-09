using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

namespace mkacg
{
    public class Class1
    {

        public static int sta = 0;
        public static string redio_img;
        public static string redio_on_off;
        public static int redio_sta;
        public static double redio_volume=0.3;
        public static string redio_path;
        public static string music_name;
        public static double music_lenth;
        public static double music_now;
        public static double music_now_fen;
        public String main (String text)
        {
            String gettext = null;
            int i = 0;
            if (InternetGetConnectedState(out i , 0))
            {

                if (text != "")
                {
                    gettext = ConnectTuLing(text);

                }
                else
                {
                    gettext = "不允许输入空值";
                }
            }
            else
            {
                gettext = "网络连接失败";
            }
            return gettext;

        }
        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState (out int connectionDescription , int reservedValue);

        HttpWebResponse Response = null;
        public string ConnectTuLing (string p_strMessage)
        {
            string result = null;
            try
            {
                String APIKEY = "7319f41ea831612d94fb05c9de2cdaa3";
                String _strMessage = p_strMessage;
                String INFO = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(_strMessage));
                String getURL = "http://www.tuling123.com/openapi/api?key=" + APIKEY + "&info=" + INFO;
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
                    if (reader.Path == "text")
                    {
                        //结果赋值
                        result = reader.Value.ToString();
                    }
                    Console.WriteLine(reader.TokenType + "\t\t" + reader.ValueType + "\t\t" + reader.Value);
                }
            }
            catch (Exception)
            {
                result = "网络连接失败";
            }
            return result;
        }

    }
}