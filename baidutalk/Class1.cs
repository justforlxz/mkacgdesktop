using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace baidutalk
{
    public class Talk_baidu
    {
        string token = "7094596";
        string filename = "1.wav";
        string apiKey = "pxxHWz9KmNsleFBsuWHQ77Hd";//对应百度云界面基本信息的API Key
        string secretKey = "b5a9a0eb50d410b0dccd3eec5fc30388";//对应百度云界面基本信息的Secret Key
        string cuid = "mkacg";//这个随便写  不过尽量写唯一的，比如自己创建个guid，或者你手机号码什么的都可以
        string getTokenURL = "";
        string serverURL = "http://vop.baidu.com/server_api";
        //这个方法得到一个密钥，这个密钥可以使用1个月，1个月之后要重新请求一次获得一个
        private void getToken ()
        {
            getTokenURL = "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials" +
            "&client_id=" + apiKey + "&client_secret=" + secretKey;
            token = GetValue("access_token");
        }

        private string GetValue (string key)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(getTokenURL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader1 = new StreamReader(response.GetResponseStream() , Encoding.UTF8);
            string ssss = reader1.ReadToEnd().Replace("\"" , "").Replace("{" , "").Replace("}" , "").Replace("\n" , "");
            string[] indexs = ssss.Split(',');
            foreach (string index in indexs)
            {
                string[] _indexs = index.Split(':');
                if (_indexs[0] == key)
                    return _indexs[1];
            }
            return "";
        }
        private void Post ()
        {

            serverURL += "?lan=en&cuid=kwwwvagaa&token=" + token;
            FileStream fs = new FileStream(filename , FileMode.Open);
            byte[] voice = new byte[fs.Length];
            fs.Read(voice , 0 , voice.Length);
            fs.Close();
            fs.Dispose();

            HttpWebRequest request = null;

            Uri uri = new Uri(serverURL);
            request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = 10000;
            request.Method = "POST";
            request.ContentType = "audio/wav; rate=8000";
            request.ContentLength = voice.Length;
            try
            {
                using (Stream writeStream = request.GetRequestStream())
                {
                    writeStream.Write(voice , 0 , voice.Length);
                    writeStream.Close();
                    writeStream.Dispose();
                }
            }
            catch
            {
                return;
            }
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader readStream = new StreamReader(responseStream , Encoding.UTF8))
                    {
                        string line = string.Empty;
                        StringBuilder sb = new StringBuilder();
                        while (!readStream.EndOfStream)
                        {
                            line = readStream.ReadLine();
                            sb.Append(line);
                            sb.Append("\r");
                        }

                        // result = readStream.ReadToEnd();

                        result = sb.ToString();

                        //message = result.Substring(result.IndexOf("utterance") + 12);
                        //message = message.Substring(0, message.IndexOf("\""));
                        readStream.Close();
                        readStream.Dispose();
                        System.Console.WriteLine(result);
                    }
                    responseStream.Close();
                    responseStream.Dispose();
                }
                response.Close();
            }
                    //result

        }
    }
}
