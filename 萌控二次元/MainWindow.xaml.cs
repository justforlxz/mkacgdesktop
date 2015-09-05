using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml;
using mkacg_talk;
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

        private void MenuItem_Click (object sender , RoutedEventArgs e)
        {
        
            //开辟线程吧！
            Thread thread = new Thread(new ThreadStart(gethitokoto));
            //启动线程
            thread.Start();
           
            //  gethitokoto();

        }

        private void gethitokoto ()
        {

            //http://api.hitokoto.us/rand?cat=a&charset=utf-8&encode=xml
            String url = "http://api.hitokoto.us/rand?cat=a&charset=utf-8&encode=xml";
            bool net = IsWebResourceAvailable(url);
            if (true == true)
            {

                XmlElement root = null;
                XmlDocument xmldoc = new XmlDocument();
                try
                {
                    bg_text.Dispatcher.Invoke(new Action(() =>
                    {
                        bg_text.Text = "";
                    }));
                
                    xmldoc.Load("rand.xml");
                    root = xmldoc.DocumentElement;
                    XmlNodeList listNodes = null, listsource = null;
                    listNodes = root.SelectNodes("/result/hitokoto");
                    listsource = root.SelectNodes("/result/source");
                    // /result/author 谁说的话
                    // /result/source 来自哪部
                    foreach (XmlNode node in listNodes)
                    {
                        bg_text.Dispatcher.Invoke(new Action(() =>
                        {
                            bg_text.Text += node.InnerText + "\n";
                        }));
                    }
                    foreach (XmlNode node in listsource)
                    {
                        bg_source.Dispatcher.Invoke(new Action(() =>
                        {
                            bg_source.Content = "来自：" + node.InnerText + "\n";
                        }));
                    }
                    bg_source.Dispatcher.Invoke(new Action(() =>
                    {
                        bg_source.Content = "hhh";
                    }));
                }

                catch
                {
                    bg_text.Dispatcher.Invoke(new Action(() =>
                    {
                        bg_text.Text = "出错啦";
                    }));
                }
            }
            else
            {
                bg_text.Dispatcher.Invoke(new Action(() =>
                {
                    bg_text.Text= "网络连接失败";
                }));
            }
        }
        private void MenuItem_Click_1 (object sender , RoutedEventArgs e)
        {
            mkacg_talk.MainWindow mw = new mkacg_talk.MainWindow();
            mw.Show();
          
        }

        private void MenuItem_Click_2 (object sender , RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded (object sender , RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void image_MouseLeftButtonDown (object sender , MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
