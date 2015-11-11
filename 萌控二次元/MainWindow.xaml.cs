using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

namespace 萌控二次元
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

        /// <summary>
        /// ////////////////////////
        /// </summary>
        static int themes = 0;
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timerToSendMessages = new DispatcherTimer();
        DispatcherTimer someTime_timer = new DispatcherTimer();
        redio.redio redio_r = new redio.redio();
        Random someTime_random = new Random();
        private void MenuItem_Click (object sender , RoutedEventArgs e)
        {
            //显示一句话
            timer.Stop();
            showorhidetrue();
            sendbox.Visibility = Visibility.Hidden;
            mkacg_showhitokoto.Class1 mkacgclass = new mkacg_showhitokoto.Class1();
            String[] list = mkacgclass.hitokoto();
            bg_text.Text = "";
            bg_text.Text = list[0] + "\n";
            bg_source.Content = list[1] + "\n";
            timer.Start();
        }
        public void showorhidetrue ()
        {
            bg_white.Visibility = bg_label.Visibility = bg_source.Visibility = bg_text.Visibility = Visibility.Visible;
            sendbox.Visibility = Visibility.Visible;
        }
        public void showorhide (object sender , EventArgs e)
        {
            bg_white.Visibility = bg_label.Visibility = bg_source.Visibility = bg_text.Visibility = Visibility.Hidden;
            sendbox.Visibility = Visibility.Hidden;
            timer.Stop();
            timerToSendMessages.Stop();
        }
        private void MenuItem_Click_2 (object sender , RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private System.Threading.Mutex myMutex = null;
        private void Window_Loaded (object sender , RoutedEventArgs e)
        {


            //开机启动
            //配置文件
            if (File.Exists(@"config.xml"))
            {
                //read
            }
            else
            {
                create_config_file();
            }
            /*************************************/
            this.Topmost = true;
            if (themes == 0)
            {
                image.Source = new BitmapImage(new Uri("Images/3.png" , UriKind.Relative));
                themes = 1;
            }
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 1.1;
            this.Left = (workWidth - this.Width) / 1;
            sendbox.Visibility = Visibility.Hidden;
            timer.Interval = new TimeSpan(0 , 0 , 4);
            timer.Tick += new EventHandler(showorhide);
            timer.Start();
            timerToSendMessages.Interval = new TimeSpan(0 , 0 , 10);
            timerToSendMessages.Tick += new EventHandler(showorhide);
            timerToSendMessages.Start();
            someTime_timer.Interval = new TimeSpan(0 , 0 , someTime_random.Next(1 , 1800));  //随机事件进行消息提醒
            someTime_timer.Tick += new EventHandler(someTime);
            someTime_timer.Start();
            hp.Visibility = Visibility.Visible;
            hp_bar_show = 1;
        }

        public void create_config_file ()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0" , "UTF-8" , null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("result");  //一级
            doc.AppendChild(root);

            XmlElement element1 = doc.CreateElement("config");
            /* element1.SetAttribute("name" , "kirito");
             root.AppendChild(element1);
             doc.AppendChild(root);
               */
            /*  
           double workHeight = SystemParameters.WorkArea.Height;
         double workWidth = SystemParameters.WorkArea.Width;
         this.Top = (workHeight - this.Height) / 1.1;
         this.Left = (workWidth - this.Width) / 1;   

         */
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            double top = (workHeight - this.Height);
            double left = (workWidth - this.Width);
            element1.SetAttribute("top" , top.ToString());
            element1.SetAttribute("left" , left.ToString());
            root.AppendChild(element1);
            doc.AppendChild(root);
            doc.Save("config.xml");
        }

        private void image_MouseLeftButtonDown (object sender , MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        public void playplay (object obj)
        {

        }
        private void MenuItem_Click_3 (object sender , RoutedEventArgs e)
        {
            if (themes == 1)
            {
                image.Source = new BitmapImage(new Uri("Images/Main1.png" , UriKind.Relative));
                themes = 0;
            }
            else if (themes == 0)
            {
                image.Source = new BitmapImage(new Uri("Images/Main.png" , UriKind.Relative));
                themes = 1;
            }
        }
        talk.Class1 _talk = new talk.Class1();
        hp_bar.MainWindow hp = new hp_bar.MainWindow();
        private void sendbox_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                switch (sendbox.Text)
                {
                    case "你是谁的女朋友":
                        timer.Stop();
                        showorhidetrue();
                        sendbox.Visibility = Visibility.Hidden;
                        bg_text.Text = "我是小竹可爱的女朋友";
                        sendbox.Text = "";
                        bg_source.Content = "";
                        timerToSendMessages.Start();
                        timer.Start();
                        break;
                    case "升级":
                        Process.Start("Update.exe");
                        break;
                    case "关闭开机启动":
                        delete_autorun();
                        break;
                    case "开启开机启动":
                        open_autorun();
                        break;
                    default:
                        timer.Stop();
                        timerToSendMessages.Stop();
                        showorhidetrue();
                        bg_text.Text = "--->" + sendbox.Text + "\n" + _talk.main(sendbox.Text);
                        sendbox.Text = "";
                        bg_source.Content = "";
                        timerToSendMessages.Start();
                        break;
                }


            }
        }
        private void open_autorun ()
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey Run = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run" , true);
            try
            {
                Run.SetValue("MKACG" , AppDomain.CurrentDomain.BaseDirectory + "萌控二次元.exe");

            }
            catch
            {

            }
            HKCU.Close();
        }
        int count = 0;
        private void sendbox_TextChanged (object sender , System.Windows.Controls.TextChangedEventArgs e)
        {
            timer.Stop();
            timerToSendMessages.Stop();
        }
        private void bgmusicplayer_MediaEnded (object sender , RoutedEventArgs e)
        {
            play();
        }
        private void delete_autorun ()
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey Run = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run" , true);
            try
            {
                Run.DeleteValue("MKACG");
                timer.Stop();
                showorhidetrue();
                sendbox.Visibility = Visibility.Hidden;
                bg_text.Text = "已删除开机项";
                timer.Start();
            }
            catch
            {
                timer.Stop();
                showorhidetrue();
                sendbox.Visibility = Visibility.Hidden;
                bg_text.Text = "已删除开机项";
                timer.Start();
            }
        }
        String play_name_get;
        public void play ()
        {
            List<string> list = redio_r.ConnectTuLing();
            list[0] = System.Web.HttpUtility.UrlDecode(list[0] , System.Text.Encoding.UTF8);
            Console.WriteLine(list[0]);
            bgmusicplayer.Source = new Uri(list[0]);
            bg_text.Text = "";
            bg_source.Content = "";
            timer.Stop();
            showorhidetrue();
            sendbox.Visibility = Visibility.Hidden;
            timer.Start();
            bgmusicplayer.Play();
            bg_text.Text = "正在播放:" + list[1];
            play_name_get = list[1];
            play_next.Visibility = Visibility.Visible;
            play_name.Visibility = Visibility.Visible;
            redioplayer.Header = "关闭电台模式";
        }
        private void redioplayer_Click (object sender , RoutedEventArgs e)
        {
            bgmusicplayer.Stop();

            if (redioplayer.Header.ToString() == "电台模式")
            {
                try
                {
                    play();
                }
                catch (Exception)
                {
                    timer.Stop();
                    bg_text.Text = "对不起，播放失败";
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    timer.Start();
                    bgmusicplayer.Stop();
                }
            }
            else if (redioplayer.Header.ToString() == "关闭电台模式")
            {
                timer.Stop();
                bg_text.Text = "";
                bg_text.Text = "已关闭电台模式";
                bg_source.Content = "";
                redioplayer.Header = "电台模式";
                showorhidetrue();
                play_name_get = null;
                sendbox.Visibility = Visibility.Hidden;
                play_next.Visibility = Visibility.Collapsed;
                play_name.Visibility = Visibility.Collapsed;
                timer.Start();
                bgmusicplayer.Stop();
            }


        }
        private void play_next_Click (object sender , RoutedEventArgs e)
        {
            bgmusicplayer.Stop();
            try
            {
                List<string> list = redio_r.ConnectTuLing();
                bgmusicplayer.Source = (new Uri(list[0]));
                bg_text.Text = "";
                bg_source.Content = "";
                timer.Stop();
                showorhidetrue();
                sendbox.Visibility = Visibility.Hidden;
                timer.Start();
                bgmusicplayer.Play();
                bg_text.Text = "正在播放:" + list[1];
                play_name_get = list[1];
                play_next.Visibility = Visibility.Visible;
                play_name.Visibility = Visibility.Visible;
                redioplayer.Header = "关闭电台模式";
            }
            catch (Exception)
            {
                timer.Stop();
                bg_text.Text = "";
                bg_source.Content = "";
                bg_text.Text = "对不起，播放失败";
                showorhidetrue();
                sendbox.Visibility = Visibility.Hidden;
                timer.Start();
                bgmusicplayer.Stop();
            }

        }

        private void play_name_Click (object sender , RoutedEventArgs e)
        {

            bg_text.Text = "";
            bg_source.Content = "";
            timer.Stop();
            showorhidetrue();
            sendbox.Visibility = Visibility.Hidden;
            timer.Start();
            bgmusicplayer.Play();
            bg_text.Text = "正在播放:" + play_name_get;
            Console.WriteLine(play_name_get);
            play_next.Visibility = Visibility.Visible;
            play_name.Visibility = Visibility.Visible;
        }
        public void timetotalk ()
        {
            DateTime dt = DateTime.Now;
            String hour = dt.Hour.ToString();
            switch (hour)
            {
                case "1":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "2":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "3":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "4":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "5":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "6":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "7":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "8":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "9":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "10":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "11":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "12":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "13":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "14":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "15":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "16":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "17":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "18":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "19":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "20":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "21":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "22":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "23":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;
                case "24":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start(); break;

                default:
                    break;
            }
        }

        private void MenuItem_Click_4 (object sender , RoutedEventArgs e)
        {
            timetotalk();
        }

        private void image_MouseDown (object sender , MouseButtonEventArgs e)
        {
            count += 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0 , 0 , 0 , 0 , 100);
            timer.Tick += (s , e1) => { timer.IsEnabled = false; count = 0; };
            timer.IsEnabled = true;
            if (count % 2 == 0)
            {
                bg_text.Text = "你好啊";
                bg_source.Content = "";
                timer.IsEnabled = false;
                count = 0;
                timerToSendMessages.Stop();
                showorhidetrue();
                timerToSendMessages.Start();
            }
        }


        private void someTime (object sender , EventArgs e)
        {
            someTime_timer.Stop();

            //显示一句话
            timer.Stop();
            showorhidetrue();
            sendbox.Visibility = Visibility.Hidden;
            mkacg_showhitokoto.Class1 mkacgclass = new mkacg_showhitokoto.Class1();
            String[] list = mkacgclass.hitokoto();
            bg_text.Text = "";
            bg_text.Text = list[0] + "\n";
            bg_source.Content = list[1] + "\n";
            timer.Start();
            someTime_timer.Interval = new TimeSpan(0 , 0 , someTime_random.Next(1 , 1800));  //随机事件进行消息提醒
            someTime_timer.Tick += new EventHandler(someTime);
            someTime_timer.Start();
        }
        int hp_bar_show = 0;

        private void MenuItem_Click_1 (object sender , RoutedEventArgs e)
        {
            if (hp_bar_show == 0)
            {
                hp.Visibility = Visibility.Visible;
                hp_bar_show = 1;
            }
            else
            {
                hp.Visibility = Visibility.Hidden;
                hp_bar_show = 0;
            }
        }

        //语音处理
        [DllImport("winmm.dll" , EntryPoint = "mciSendString" , CharSet = CharSet.Auto)]
        public static extern int mciSendString (
         string lpstrCommand ,
         string lpstrReturnString ,
         int uReturnLength ,
         int hwndCallback
        );
        public void luyin_on ()
        {
            mciSendString("set wave bitpersample 8" , "" , 0 , 0);

            mciSendString("set wave samplespersec 20000" , "" , 0 , 0);
            mciSendString("set wave channels 2" , "" , 0 , 0);
            mciSendString("set wave format tag pcm" , "" , 0 , 0);
            mciSendString("open new type WAVEAudio alias movie" , "" , 0 , 0);

            mciSendString("record movie" , "" , 0 , 0);
        }
        public void luyin_save ()
        {
            mciSendString("stop movie" , "" , 0 , 0);
            mciSendString("save movie 1.wav" , "" , 0 , 0);
            mciSendString("close movie" , "" , 0 , 0);
        }

        private void sound_on_Click (object sender , RoutedEventArgs e)
        {
           luyin_on();
        
        }

        private void sound_save_Click (object sender , RoutedEventArgs e)
        {

            // StreamReader sr = File.OpenText(openFile.FileName);
            // luyin_save();

            httpRequest hR = new httpRequest();
            string token_Access = hR.getStrAccess(hR.API_key , hR.API_secret_key);
            string token_Text = hR.getStrText(hR.API_id , token_Access , "zh" , "1.wav" , "pcm" , "8000");
            MessageBox.Show(token_Text);
            
        }

     
    }
    class httpRequest
    {
        public string API_id = "7094596";
        public string API_key = "pxxHWz9KmNsleFBsuWHQ77Hd";
        public string API_secret_key = "b5a9a0eb50d410b0dccd3eec5fc30388";
        public string strJSON = "";

        public string getStrAccess (string para_API_key , string para_API_secret_key)
        {
            string access_html = null;
            string access_token = null;
            //需要到规定的URL获取access token

             string getAccessUrl = "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id=" + API_key+ "&client_secret=" +API_secret_key;
           // String getAccessUrl = "http://vop.baidu.com/server_api?lan=zh&cuid="+API_id+"";
            try
            {
                HttpWebRequest getAccessRequest = WebRequest.Create(getAccessUrl) as HttpWebRequest;
                getAccessRequest.ContentType = "multipart/form-data";
                getAccessRequest.Accept = "*/*";
                getAccessRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
                getAccessRequest.Timeout = 30000;
                getAccessRequest.Method = "post";

                HttpWebResponse response = getAccessRequest.GetResponse() as HttpWebResponse;
                using (StreamReader strHttpComback = new StreamReader(response.GetResponseStream() , Encoding.UTF8))
                {
                    access_html = strHttpComback.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            JObject jo = JObject.Parse(access_html);
            access_token = jo["access_token"].ToString();//得到返回的toke
            return access_token;
        }
        public string getStrText (string para_API_id , string para_API_access_token , string para_API_language , string para_API_record , string para_format , string para_Hz)
        {
            //方法参数说明:
            //para_API_id:API_id(你的ID)
            //para_API_access_token(getStrAccess(...)方法得到的access_token口令)
            //para_API_language(你要识别的语言, zh, en, ct)
            //para_API_record(语音文件的路径)
            //para_format(语音文件的格式)
            //para_Hz(语音文件的采样率 16000或者8000)

            //该方法返回值:
            //该方法执行正确返回值是语音翻译的文本,错误是错误号,可以去看百度语音文档,查看对应错误

            string strText = null;
            string error = null;
            FileInfo fi = new FileInfo(para_API_record);
            FileStream fs = new FileStream(para_API_record , FileMode.Open);
            byte[] voice = new byte[fs.Length];
            fs.Read(voice , 0 , voice.Length);
            fs.Close();

            string getTextUrl = "http://vop.baidu.com/server_api?lan=" + "zh" + "&cuid=" + para_API_id + "&token=" + para_API_access_token;
            HttpWebRequest getTextRequst = WebRequest.Create(getTextUrl) as HttpWebRequest;
            Console.WriteLine(para_API_id+"\n"+
                 para_API_access_token + "\n" +
               para_API_language + "\n" +
              para_API_record + "\n" + 
                para_format+"\n"+
                para_Hz);
            /* getTextRequst.Proxy = null;
             getTextRequst.ServicePoint.Expect100Continue = false;
             getTextRequst.ServicePoint.UseNagleAlgorithm = false;
             getTextRequst.ServicePoint.ConnectionLimit = 65500;
             getTextRequst.AllowWriteStreamBuffering = false;*/

            getTextRequst.ContentType = "audio/wav;rate=8000";
            getTextRequst.ContentLength = fi.Length;
            getTextRequst.Method = "post";
            getTextRequst.Accept = "*/*";
            getTextRequst.KeepAlive = true;
            getTextRequst.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
            getTextRequst.Timeout = 30000;//30秒连接不成功就中断 
            using (Stream writeStream = getTextRequst.GetRequestStream())
            {
                writeStream.Write(voice , 0 , voice.Length);
            }

            HttpWebResponse getTextResponse = getTextRequst.GetResponse() as HttpWebResponse;
            using (StreamReader strHttpText = new StreamReader(getTextResponse.GetResponseStream() , Encoding.UTF8))
            {
                strJSON = strHttpText.ReadToEnd();
                Console.WriteLine(strJSON);
            }
            JObject jsons = JObject.Parse(strJSON);//解析JSON
            if (jsons["err_msg"].Value<string>() == "success.")
            {
                strText = jsons["result"][0].ToString();
                Console.WriteLine(strText);
                return strText;
            }
            else
            {
                error = jsons["err_no"].Value<string>() + jsons["err_msg"].Value<string>();
                Console.WriteLine(error);
                return error;
            }
        }
    }
}