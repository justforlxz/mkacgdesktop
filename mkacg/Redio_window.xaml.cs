using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace mkacg
{
    /// <summary>
    /// Redio_window.xaml 的交互逻辑
    /// </summary>
    public partial class Redio_window : Window
    {
        public Redio_window ()
        {
            InitializeComponent();
            this.slider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(slider_ValueChanged);//注册事件 
         
        }
        public delegate void play_next_Click (object sender , EventArgs e);
        public event play_next_Click play_next_click;
        
        public delegate void change_volume (double value);
        public event change_volume cv;

      //  public delegate void redioplayer_Click (object sender , EventArgs e);
      //  public event redioplayer_Click redioplayer_click;
        private void slider_ValueChanged (object sender , RoutedPropertyChangedEventArgs<double> e)
        {
            Class1.redio_volume = slider.Value;
            bgmusicplayer.Volume = slider.Value;
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
        DispatcherTimer redio_update = new DispatcherTimer();
        private void Window_Loaded (object sender , RoutedEventArgs e)
        {
           slider.Value = Class1.redio_volume;
            progressbar.Value = 0;
            if (File.Exists(@"redio_config.xml"))
            {
                open_config();
            }
            else
            {
                double workHeight = SystemParameters.WorkArea.Height;
                double workWidth = SystemParameters.WorkArea.Width;
                this.Top = (workHeight - this.Height) / 10;
                this.Left = (workWidth - this.Width) / 1.05;
                create_config(Top , Left);
            }
             play(sender , e);
              redio_update.Interval = new TimeSpan(0 , 0 , 1);
              redio_update.Tick += new EventHandler(redio_update_function);
              redio_update.Start();

        }
        public void redio_update_function (object sender , EventArgs e)
        {
            //设置媒体的分秒
            progressbar.Maximum = Class1.music_lenth;
            progressbar.Value = Class1.music_now;
            MusicName.Content = Class1.music_name;
        }
        public void open_config ()
        {
            try
            {

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load("redio_config.xml");
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
                        if (name=="x")
                        {
                            this.Top = double.Parse(value) ;
                        }
                        else if (name=="y")
                        {
                            this.Left = double.Parse(value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void create_config (double x,double y)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0" , "UTF-8" , null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("result");  //一级
            doc.AppendChild(root);
            XmlElement element1 = doc.CreateElement("system");
            //  element1.SetAttribute("id" , "内容");
            element1.SetAttribute("x" ,x.ToString());
            element1.SetAttribute("y" , y.ToString());
            root.AppendChild(element1);
            doc.AppendChild(root);
            doc.Save("redio_config.xml");
        }



        private void Grid_MouseLeftButtonDown (object sender , MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
                create_config(this.Top,this.Left);
            }
        }

        private void label_MouseLeftButtonDown (object sender , EventArgs e)
        {
            this.Close();
            //bgmusicplayer.Stop();
          //  redioplayer_click(sender,e);
        }
        //////////////////
        redio redio = new redio();

        private void bgmusicplayer_MediaEnded (object sender , EventArgs e)
        {
            progressbar.Value = 0;
            play(sender , e);
        }
        private void next_MouseLeftButtonDown (object sender , EventArgs e)
        {
        
            bgmusicplayer.Stop();
            try
            {
                play(sender , e);
            }
            catch (Exception)
            {
                bgmusicplayer.Stop();
            }

        }


        String play_name_get;
        public List<string> music_list = new List<string>();
        // 一共存四个，每次播放0，2作为缓冲，播放完毕，移除0 1，追加 2  3
        public void play (object sender , EventArgs e)
        {
            try
            {
                bgmusicplayer.Stop();
                List<String> list = redio.ConnectTuLing();
                bgmusicplayer.Source = new Uri(list[0]);
           
                bgmusicplayer.Play();

                Console.WriteLine(list[0]);
                Console.WriteLine(list[2]);
                play_name_get = list[1];
                Class1.music_name = list[1];

                Class1.redio_sta = 1;
                bgmusicplayer.Volume = Class1.redio_volume;
                redio_update.Interval = new TimeSpan(0 , 0 , 1);
                redio_update.Tick += new EventHandler(this.redio_update_进度);
                redio_update.Start();
            }
            catch (Exception)
            {
                next_MouseLeftButtonDown(sender,e);
            }
        }
        public void redio_update_进度 (object sender , EventArgs e)
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
    }
}
