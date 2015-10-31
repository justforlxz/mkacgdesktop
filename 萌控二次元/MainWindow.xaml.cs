using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;
using System.IO;
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
            bgmusicplayer.Play();

        }
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
            this.Close();
        }
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
                image.Source = new BitmapImage(new Uri("Images/Main.png" , UriKind.Relative));
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
            someTime_timer.Interval = new TimeSpan(0,0, someTime_random.Next(1,1800));  //随机事件进行消息提醒
            someTime_timer.Tick += new EventHandler(someTime);
            someTime_timer.Start();
        }
        private void create_config_file ()
        {
            var xmlDoc = new XmlDocument();
            //Create the xml declaration first 
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0" , "utf-8" , null));
            //Create the root node and append into doc 
            var el = xmlDoc.CreateElement("Contacts");
            xmlDoc.AppendChild(el);
            /*

            XmlElement elementName = xmlDoc.CreateElement("Name");   //子节点
            elementName.InnerText = "Daisy Abbey"; //子节点值
            elementContact.AppendChild(elementName);  


    */
            // Contact 
            XmlElement elementContact = xmlDoc.CreateElement("Contact");
            XmlAttribute attrID = xmlDoc.CreateAttribute("id");
            attrID.Value = "01";
            elementContact.Attributes.Append(attrID);
            el.AppendChild(elementContact);
            // Contact Name 
            XmlElement elementName = xmlDoc.CreateElement("Name");
            elementName.InnerText = "Daisy Abbey";
            elementContact.AppendChild(elementName);
            // Contact Gender 
            XmlElement elementGender = xmlDoc.CreateElement("Gender");
            elementGender.InnerText = "female";
            elementContact.AppendChild(elementGender);
            xmlDoc.Save("config.xml");
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
        private void sendbox_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                switch (sendbox.Text)
                {
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
                        break;
                }
                timer.Stop();
                timerToSendMessages.Stop();
                showorhidetrue();
                bg_text.Text = _talk.main(sendbox.Text);
                sendbox.Text = "";
                bg_source.Content = "";
                timerToSendMessages.Start();
            }
        }
        private void open_autorun()
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey Run = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            try
            {
                Run.SetValue("MKACG", AppDomain.CurrentDomain.BaseDirectory + "萌控二次元.exe");

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
            bgmusicplayer.Stop();
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
        private void redioplayer_Click (object sender , RoutedEventArgs e)
        {
            bgmusicplayer.Stop();
            Random num = new Random(); int a = num.Next(1 , 10);

            if (redioplayer.Header.ToString() == "电台模式")
            {
                try
                {
                    String[] list = redio_r.redio_(a);
                    list[0]= System.Web.HttpUtility.UrlDecode(list[0], System.Text.Encoding.UTF8);
                    Console.WriteLine(list[0]);
                    bgmusicplayer.Source =new Uri(list[0]);  //服务器方面文件404，转码问题导致未找到，正在准备改成使用其他电台的数据。
                    bg_text.Text = "";
                    bg_source.Content = "";
                    timer.Stop();
                    showorhidetrue();
                    sendbox.Visibility = Visibility.Hidden;
                    timer.Start();
                    bgmusicplayer.Play();
                    bg_text.Text = "正在播放:" + list[1];
                    play_next.Visibility = Visibility.Visible;
                    redioplayer.Header = "关闭电台模式";
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
                sendbox.Visibility = Visibility.Hidden;
                play_next.Visibility = Visibility.Collapsed;
                play_name.Visibility = Visibility.Collapsed;
                timer.Start();
                bgmusicplayer.Stop();
            }


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
        String[] list;
        private void play_next_Click (object sender , RoutedEventArgs e)
        {
            bgmusicplayer.Stop();
            Random num = new Random(); int a = num.Next(1 , 10);
            try
            {
                list = redio_r.redio_(a);
                bgmusicplayer.Source = (new Uri(list[0]));
                bg_text.Text = "";
                bg_source.Content = "";
                timer.Stop();
                showorhidetrue();
                sendbox.Visibility = Visibility.Hidden;
                timer.Start();
                bgmusicplayer.Play();
                bg_text.Text = "正在播放:" + list[1];
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
            bg_text.Text = "正在播放:" + list[1];
            play_next.Visibility = Visibility.Visible;
            play_name.Visibility = Visibility.Visible;
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

    }
}