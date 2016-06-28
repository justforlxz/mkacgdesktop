using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Windows.Interop;
using System.Net;
using System.Windows.Forms;

namespace mkacg
{


    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>


    public partial class MainWindow : Window
    {
        private NotifyIcon notifyIcon = null;
        public MainWindow ()
        {
            InitializeComponent();
            InitialTray();


            //单例模式
            bool requestInitialOwnership = true;
            bool mutexWasCreated;
            mut = new System.Threading.Mutex(requestInitialOwnership , "com.Application1.Ding" , out mutexWasCreated);
            if (!(requestInitialOwnership && mutexWasCreated))
            {
                // 随意什么操作啦~
                System.Windows.Application.Current.Shutdown();
            }
           
        }
        private void InitialTray ()
        {
            //隐藏主窗体  
            this.Visibility = Visibility.Hidden;

            //设置托盘的各个属性  
            notifyIcon = new NotifyIcon();
            notifyIcon.BalloonTipText = "systray runnning...";
            notifyIcon.Text = "systray";
            notifyIcon.Icon = new System.Drawing.Icon(@"C:\Users\zhuzi\Source\Repos\mkacgdesktop\mkacg\tray.ico");
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(2000);
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);

            //设置菜单项  
            //MenuItem setting1 = new MenuItem("setting1");
            //MenuItem setting2 = new MenuItem("setting2");
            //MenuItem setting = new MenuItem("setting" , new MenuItem[] { setting1 , setting2 });

            //帮助选项  
            // MenuItem help = new MenuItem("help");

            //关于选项  
            //MenuItem about = new MenuItem("about");

            //退出菜单项  
             MenuItem exit = new MenuItem("exit");
            exit.Click += new EventHandler(exit_Click);

            //关联托盘控件  
            //MenuItem[] childen = new MenuItem[] { setting , help , about , exit };
            // notifyIcon.ContextMenu = new ContextMenu(childen);

            //电台
            MenuItem redio = new MenuItem("开启电台");
            redio.Click += new EventHandler(redioplay);


            //关联托盘控件  
            MenuItem[] childen = new MenuItem[] {redio,exit};
            notifyIcon.ContextMenu = new ContextMenu(childen);

            //窗体状态改变时候触发  
            this.StateChanged += new EventHandler(SysTray_StateChanged);
        }
     
        /// <summary>  
        /// 鼠标单击  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void notifyIcon_MouseClick (object sender , System.Windows.Forms.MouseEventArgs e)
        {
            //如果鼠标左键单击  
            if (e.Button == MouseButtons.Left)
            {
                talk_control talk_cont = new talk_control();
                talk_cont.Show();

                //if (this.Visibility == Visibility.Visible)
                //{
                //    this.Visibility = Visibility.Hidden;
                //}
                //else
                //{
                //    this.Visibility = Visibility.Visible;
                //    this.Activate();
                //}
            }
        }

        /// <summary>  
        /// 窗体状态改变时候触发  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void SysTray_StateChanged (object sender , EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Visible;
            }
           
        }


        /// <summary>  
        /// 退出选项  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void exit_Click (object sender , EventArgs e)
        {
            if (System.Windows.MessageBox.Show("sure to exit?" ,
                                               "application" ,
                                                MessageBoxButton.YesNo ,
                                                MessageBoxImage.Question ,
                                                MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// ////////////////////////
        /// </summary>

        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timerToSendMessages = new DispatcherTimer();
        DispatcherTimer someTime_timer = new DispatcherTimer();
        DispatcherTimer redio_update = new DispatcherTimer();
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

            Class1.redio_volume = 0.3;
            //开机启动
            //配置文件
            if (File.Exists(@"config.xml"))
            {
                open_config();
            }
            else
            {

                setting.create_config("主人");
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
                        if (name == "name")
                        {
                            bg_text.Text = value + ",欢迎你";
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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


        private void image_MouseLeftButtonDown (object sender , MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        int count = 0;
        private void bgmusicplayer_MediaEnded (object sender , EventArgs e)
        {
            play(sender,e);
         }

        String play_name_get;
        public List<string> music_list = new List<string>();
        // 一共存四个，每次播放0，2作为缓冲，播放完毕，移除0 1，追加 2  3
       public void play (object sender , EventArgs e)
        {
            try
            {
                bgmusicplayer.Stop();
                List<String> list = redio_r.ConnectTuLing();
                bgmusicplayer.Source = new Uri(list[0]);
                bg_text.Text = "";
                bg_source.Content = "";
                timer.Stop();
                showorhidetrue();
                timer.Start();
                bgmusicplayer.Play();
                
                bg_text.Text = "正在播放:" + list[1];
                Console.WriteLine(list[0]);
                Console.WriteLine(list[2]);
                play_name_get = list[1];
                Class1.music_name = list[1];
                play_next.Visibility = Visibility.Visible;
                play_name.Visibility = Visibility.Visible;
                redioplayer.Header = "关闭电台模式";
             
                Class1.redio_sta = 1;
                bgmusicplayer.Volume = Class1.redio_volume;
                redio_update.Interval = new TimeSpan(0,0,1);
                redio_update.Tick += new EventHandler(redio_update_function);
                redio_update.Start();
            }
            catch (Exception)
            {
               play_next_Click(sender , e);
            }
        }
        
        public void redio_update_function (object sender , EventArgs e)
        {
            //设置媒体的分秒
            if (bgmusicplayer.NaturalDuration.HasTimeSpan)
            {
                //Class1.music_lenth = double.Parse(bgmusicplayer.NaturalDuration.TimeSpan.ToString());
                //Console.WriteLine(Class1.music_lenth);
               Class1.music_lenth = bgmusicplayer.NaturalDuration.TimeSpan.Ticks;
              Class1.music_now = bgmusicplayer.Position.Ticks;

            }
        }
        //电台文件下载
        public List<int> down_count = new List<int>();
        public List<string> HttpDownloadFile (string url , String name)
        {

            List<String> list = new List<string>();
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();

            //创建本地文件写入流
            string str = System.Environment.CurrentDirectory;
            str = str + @"\Music\" + name + ".mp3";
            Stream stream = new FileStream(str , FileMode.Create);

            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr , 0 , (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr , 0 , size);
                size = responseStream.Read(bArr , 0 , (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            list.Add(str);
            list.Add(name);
            return list;
        }

        private void add_music_list () {
            List<String> list = redio_r.ConnectTuLing();
            music_list.AddRange(HttpDownloadFile(list[0],list[1]));  //添加两行
        }

        public void redioplay (object sender , EventArgs e)
        {
            Redio_window redio_window = new Redio_window();
            redio_window.Show();
            redio_window.play_next_click += new Redio_window.play_next_Click(play_next_Click);
        }
        private void redioplayer_Click (object sender , EventArgs e)
        {

            //bgmusicplayer.Stop();

            //if (redioplayer.Header.ToString() == "电台模式")
            //{
            //    try
            //    {
            //        play(sender ,e);

            //        redio_window.play_next_click += new Redio_window.play_next_Click(play_next_Click);
            //        redio_window.cv += new Redio_window.change_volume(change_volume);
            //        redio_window.redioplayer_click += new Redio_window.redioplayer_Click(redioplayer_Click);
            //        redio_window.Show();
            //       }
            //    catch (Exception)
            //    {
            //        timer.Stop();
            //        bg_text.Text = "对不起，播放失败";
            //        showorhidetrue();

            //        timer.Start();
            //        bgmusicplayer.Stop();
            //    }
            //}
            //else if (redioplayer.Header.ToString() == "关闭电台模式")
            //{
            //    timer.Stop();
            //    bg_text.Text = "";
            //    bg_text.Text = "已关闭电台模式";
            //    bg_source.Content = "";
            //    redioplayer.Header = "电台模式";
            //    redio_window.Hide();
            //    showorhidetrue();
            //    play_name_get = null;
            //    Class1.redio_sta = 0;
            //    bgmusicplayer.Volume = Class1.redio_volume;
            //    play_next.Visibility = Visibility.Collapsed;
            //    play_name.Visibility = Visibility.Collapsed;
            //    timer.Start();
            //    bgmusicplayer.Stop();
            //    appfirst = 1;
            //}


        }
        private void play_next_Click (object sender , EventArgs e)
        {
            bgmusicplayer.Stop();
            try
            {
                play(sender , e);
                //List<String> list = redio_r.ConnectTuLing();
                //bgmusicplayer.Source = (new Uri(list[0]));
                //bg_text.Text = "";
                //bg_source.Content = "";
                //timer.Stop();
                //showorhidetrue();
                //Class1.music_name =list[1];
                //timer.Start();
                //bgmusicplayer.Play();
                //bg_text.Text = "正在播放:" +list[1];
                //play_name_get = list[1];
                //Class1.music_name = list[1];
                //play_next.Visibility = Visibility.Visible;
                //play_name.Visibility = Visibility.Visible;
                //redioplayer.Header = "关闭电台模式";

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

        private void play_name_Click (object sender , EventArgs e)
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