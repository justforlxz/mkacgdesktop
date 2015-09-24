using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
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
        private void hide (object sender , EventArgs e)
        {
           
                bg.Visibility = Visibility.Hidden;
                bg_label.Visibility = Visibility.Hidden;
                bg_source.Visibility = Visibility.Hidden;
                bg_text.Visibility = Visibility.Hidden;


            /*


            
                bg.Dispatcher.Invoke(new Action(() =>
                {
                    bg.Visibility = Visibility.Visible;
                }));
                bg_label.Dispatcher.Invoke(new Action(() =>
                {
                    bg_label.Visibility = Visibility.Visible;
                }));
                bg_source.Dispatcher.Invoke(new Action(() =>
                {
                    bg_source.Visibility = Visibility.Visible;
                }));
                bg_text.Dispatcher.Invoke(new Action(() =>
                {
                    bg_text.Visibility = Visibility.Visible;
                }));

    */
        }
        private void MenuItem_Click (object sender , RoutedEventArgs e)
        {
           
            //开辟线程吧！
            Thread thread = new Thread(new ThreadStart(gethitokoto));
            //启动线程
            thread.IsBackground = true;
            thread.Start();


        }
        private void gethitokoto ()
        {
            String url = "http://api.hitokoto.us/rand?cat=a&charset=utf-8&encode=xml";
            XmlElement root = null;
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                bg_text.Dispatcher.Invoke(new Action(() =>
                {
                    bg_text.Text = "";
                }));
                Thread.Sleep(200);
                bg.Dispatcher.Invoke(new Action(() =>
                {
                    bg.Visibility = Visibility.Visible;
                }));
                bg_label.Dispatcher.Invoke(new Action(() =>
                {
                    bg_label.Visibility = Visibility.Visible;
                }));
                bg_source.Dispatcher.Invoke(new Action(() =>
                {
                    bg_source.Visibility = Visibility.Visible;
                }));
                bg_text.Dispatcher.Invoke(new Action(() =>
                {
                    bg_text.Visibility = Visibility.Visible;
                }));

                xmldoc.Load(url);
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
                    Thread.Sleep(2000);
                    /*   DispatcherTimer timer = new DispatcherTimer();
                       timer.Interval = TimeSpan.FromMilliseconds(1500);
                       timer.Tick += new EventHandler(hide);  //你的事件
                       timer.Start();   */

                    bg.Dispatcher.Invoke(new Action(() =>
                    {
                        bg.Visibility = Visibility.Hidden;
                    }));
                    bg_label.Dispatcher.Invoke(new Action(() =>
                    {
                        bg_label.Visibility = Visibility.Hidden;
                    }));
                    bg_source.Dispatcher.Invoke(new Action(() =>
                    {
                        bg_source.Visibility = Visibility.Hidden;
                    }));
                    bg_text.Dispatcher.Invoke(new Action(() =>
                    {
                        bg_text.Visibility = Visibility.Hidden;

                    }));
                    Thread.Sleep(300);
                }
            }
            catch
            {
                bg_text.Dispatcher.Invoke(new Action(() =>
                {
                    bg_text.Text = "似乎网络出错啦";
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
         /*   DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1500);
            timer.Tick += new EventHandler(hide);  //你的事件
            timer.Start();
            */
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
