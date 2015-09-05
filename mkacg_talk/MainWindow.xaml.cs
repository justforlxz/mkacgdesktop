using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace mkacg_talk
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow ()
        {
            InitializeComponent();
         
        }
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState (out int connectionDescription , int reservedValue);
        HttpWebResponse Response = null;
        public string ConnectTuLing (string p_strMessage)
        {
            string result = null;
            try
            {
                String APIKEY = "c32ccaa805b6441be76bc18074f12e51";
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
                throw;
            }
            return result;
        }
        //判断网络连接
        static bool IsWebResourceAvailable (string webResourceAddress)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(webResourceAddress));
                req.Method = "HEAD";
                req.Timeout = 1000;
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                return (res.StatusCode == HttpStatusCode.OK);
            }
            catch (WebException wex)
            {
                System.Diagnostics.Trace.Write(wex.Message);
                return false;
            }
        }
        private void button_Click (object sender , RoutedEventArgs e)
        {
            bool net = IsWebResourceAvailable("http://www.baidu.com");
            if (net == true)
            {
                if (rtb_send.Text != "")
                {
                    //    Thread thread = new Thread(new ThreadStart(() => ConnectTuLing(rtb_send.Text)));   //提示别的线程已经拥有该对象
                    //        thread.Start();

                    string returnMess = ConnectTuLing(rtb_send.Text);
                    //   rtb_mess.Text = "\n \r";
                    rtb_mess.Text += ">>>" + rtb_send.Text + "\r";
                    rtb_mess.Text += returnMess + "\r";
                    rtb_send.Text = "";

                }
                else
                {
                    rtb_mess.Text = "不允许输入空值";
                }
            }
            else rtb_mess.Text = ">>>您的网络没有连接，请检查网络!<<<";
        }

        private void Window_Loaded (object sender , RoutedEventArgs e)
        {
            rtb_mess.Text = "欢迎使用萌控二次元对话机器人 \r \n";
        }
    }
}
