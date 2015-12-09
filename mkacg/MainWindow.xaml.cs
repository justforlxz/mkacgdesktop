using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Windows.Interop;


namespace mkacg
{


    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>


    public partial class MainWindow : Window
    {

        public MainWindow ()
        {
            InitializeComponent();
            //单例模式
            bool requestInitialOwnership = true;
            bool mutexWasCreated;
            mut = new System.Threading.Mutex(requestInitialOwnership , "com.Application1.Ding" , out mutexWasCreated);
            if (!(requestInitialOwnership && mutexWasCreated))
            {
                // 随意什么操作啦~
                Application.Current.Shutdown();
            }
        }


        /// <summary>
        /// ////////////////////////
        /// </summary>
        static int themes = 0;
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timerToSendMessages = new DispatcherTimer();
        DispatcherTimer someTime_timer = new DispatcherTimer();
        redio redio_r = new redio();
        Random someTime_random = new Random();
        private void MenuItem_Click (object sender , RoutedEventArgs e)
        {
            //显示一句话
            timer.Stop();
            showorhidetrue();
            mkacg_showhitokoto mkacgclass = new mkacg_showhitokoto();
            String[] list = mkacgclass.hitokoto();
            bg_text.Text = "";
            bg_text.Text = list[0] + "\n";
            bg_source.Content = list[1] + "\n";
            timer.Start();
        }
        public void showorhidetrue ()
        {
            bg_white.Visibility = bg_source.Visibility = bg_text.Visibility = Visibility.Visible;

        }
        public void showorhide (object sender , EventArgs e)
        {
            bg_white.Visibility = bg_source.Visibility = bg_text.Visibility = Visibility.Hidden;

            timer.Stop();
            timerToSendMessages.Stop();
        }
        private void MenuItem_Click_2 (object sender , RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        settings setting = new settings();
        public String music_id;
        public int appfirst = 0;


        System.Threading.Mutex mut;
        private void Window_Loaded (object sender , RoutedEventArgs e)
        {
            
            bgmusicplayer.Play();
            appfirst = 1;

            //开机启动
            //配置文件
            if (File.Exists(@"config.xml"))
            {
                open_config();
            }
            else
            {

                setting.create_config("主人" , "38688170");
            }
            /*************************************/
            this.Topmost = true;
            //if (themes == 0)
            //{
            //    image.Source = new BitmapImage(new Uri("Images/1.png" , UriKind.Relative));
            //    themes = 1;
            //}
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 1.1;
            this.Left = (workWidth - this.Width) / 1;
            Class1.redio_sta = 0;
            timer.Interval = new TimeSpan(0 , 0 , 4);
            timer.Tick += new EventHandler(showorhide);
            timer.Start();
            timerToSendMessages.Interval = new TimeSpan(0 , 0 , 10);
            timerToSendMessages.Tick += new EventHandler(showorhide);
            timerToSendMessages.Start();
            someTime_timer.Interval = new TimeSpan(0 , 0 , someTime_random.Next(1 , 1800));  //随机事件进行消息提醒
            someTime_timer.Tick += new EventHandler(someTime);
            someTime_timer.Start();

            //  从alt tab中隐藏的代码来自  http://www.helplib.com/qa/494704
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);

            int exStyle = (int)GetWindowLong(wndHelper.Handle , (int)GetWindowLongFields.GWL_EXSTYLE);

            exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            SetWindowLong(wndHelper.Handle , (int)GetWindowLongFields.GWL_EXSTYLE , (IntPtr)exStyle);
        }

        //  从alt tab中隐藏的代码来自  http://www.helplib.com/qa/494704
        #region Window styles
        [Flags]
        public enum ExtendedWindowStyles
        {
            // ...
            WS_EX_TOOLWINDOW = 0x00000080,
            // ...
        }

        public enum GetWindowLongFields
        {
            // ...
            GWL_EXSTYLE = (-20),
            // ...
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong (IntPtr hWnd , int nIndex);

        public static IntPtr SetWindowLong (IntPtr hWnd , int nIndex , IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd , nIndex , IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd , nIndex , dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if (result == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll" , EntryPoint = "SetWindowLongPtr" , SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr (IntPtr hWnd , int nIndex , IntPtr dwNewLong);

        [DllImport("user32.dll" , EntryPoint = "SetWindowLong" , SetLastError = true)]
        private static extern Int32 IntSetWindowLong (IntPtr hWnd , int nIndex , Int32 dwNewLong);

        private static int IntPtrToInt32 (IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll" , EntryPoint = "SetLastError")]
        public static extern void SetLastError (int dwErrorCode);
        #endregion

        public void open_config ()
        {
            try
            {
                music_id = null;
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load("config.xml");
                XmlNode rootnode = xmldoc.SelectSingleNode("result");
                string innerXmlInnfo = rootnode.InnerXml.ToString();
                string outerxmlinfo = rootnode.OuterXml.ToString();
                XmlNodeList firstlevelnodelist = rootnode.ChildNodes;
                foreach (XmlNode node in firstlevelnodelist)
                {
                    XmlAttributeCollection attributecol = node.Attributes;
                    foreach (XmlAttribute attri in attributecol)
                    {
                        string name = attri.Name;
                        string value = attri.Value;
                        Console.WriteLine("{0}={1}" , name , value);
                        if (name == "musicid")
                        {
                            music_id = value;
                            Console.WriteLine(music_id);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private void image_MouseLeftButtonDown (object sender , MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void MenuItem_Click_3 (object sender , RoutedEventArgs e)
        {
            // image.Source = new BitmapImage(new Uri("Images/Main1.png" , UriKind.Relative));
            Random random = new Random();
            int i = random.Next(0 , 5);
            Console.WriteLine(i);
            switch (i)
            {
                case 1:
                    image.Source = new BitmapImage(new Uri("Images/1.png" , UriKind.Relative));
                    break;
                case 2:
                    image.Source = new BitmapImage(new Uri("Images/2.png" , UriKind.Relative));
                    break;
                case 3:
                    image.Source = new BitmapImage(new Uri("Images/3.png" , UriKind.Relative));
                    break;
                case 4:
                    image.Source = new BitmapImage(new Uri("Images/4.png" , UriKind.Relative));
                    break;
                default:
                    image.Source = new BitmapImage(new Uri("Images/1.png" , UriKind.Relative));
                    break;
            }
        }

        int count = 0;

        private void bgmusicplayer_MediaEnded (object sender , RoutedEventArgs e)
        {
            if (appfirst != 1)
            {
                play();
            }
        }

        String play_name_get;
        public void play ()
        {
            open_config();
            List<string> list = redio_r.ConnectTuLing();
            Console.WriteLine(music_id);
            list[0] = System.Web.HttpUtility.UrlDecode(list[0] , System.Text.Encoding.UTF8);
            Console.WriteLine(list[0]);
            bgmusicplayer.Source = new Uri(list[0]);
            Class1.redio_img = list[2];
            Console.WriteLine(list[2]);
            bg_text.Text = "";
            bg_source.Content = "";
            timer.Stop();
            showorhidetrue();

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
                    Class1.redio_sta = 1;
                    bgmusicplayer.Volume = Class1.redio_volume;

                }
                catch (Exception)
                {
                    timer.Stop();
                    bg_text.Text = "对不起，播放失败";
                    showorhidetrue();

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
                Class1.redio_sta = 0;
                bgmusicplayer.Volume = Class1.redio_volume;
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
                open_config();
                List<string> list = redio_r.ConnectTuLing();
                Console.WriteLine("下首播放操作->" + music_id + list);
                bgmusicplayer.Source = (new Uri(list[0]));
                bg_text.Text = "";
                bg_source.Content = "";
                timer.Stop();
                showorhidetrue();

                timer.Start();
                bgmusicplayer.Play();
                bg_text.Text = "正在播放:" + list[1];

                play_name_get = list[1];
                Class1.redio_img = list[2];
                Console.WriteLine("img->" + list[2]);
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
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "2":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "3":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "4":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "5":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "6":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "7":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "8":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "9":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "10":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "11":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "12":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "13":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "14":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "15":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "16":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "17":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "18":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "19":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "20":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "21":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "22":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "23":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;
                case "24":
                    bg_text.Text = "";
                    timer.Stop();
                    showorhidetrue();
                    bg_source.Content = "";
                    bg_text.Text = "现在是" + hour + "点了";
                    timer.Start();
                    break;

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
                talk_control talk_cont = new talk_control();
               talk_cont.play_next_click += new talk_control.play_next_Click(play_next_Click);
                talk_cont.cv += new talk_control.change_volume(change_volume);
                talk_cont.Show();
                
            }
        }

        void change_volume (double value)
        {
            bgmusicplayer.Volume = value;
        }
        private void someTime (object sender , EventArgs e)
        {
            someTime_timer.Stop();

            //显示一句话
            timer.Stop();
            showorhidetrue();

            mkacg_showhitokoto mkacgclass = new mkacg_showhitokoto();
            String[] list = mkacgclass.hitokoto();
            bg_text.Text = "";
            bg_text.Text = list[0] + "\n";
            bg_source.Content = list[1] + "\n";
            timer.Start();
            someTime_timer.Interval = new TimeSpan(0 , 0 , someTime_random.Next(1 , 1800));  //随机事件进行消息提醒
            someTime_timer.Tick += new EventHandler(someTime);
            someTime_timer.Start();
        }

        private void MenuItem_Click_5 (object sender , RoutedEventArgs e)
        {
            settings setting = new settings();
            setting.Show();

        }

        private void bg_text_LayoutUpdated (object sender , EventArgs e)
        {
            bg_text.Height = bg_text.ActualHeight;

        }

    }
}