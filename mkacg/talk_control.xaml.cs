using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Microsoft.Win32;
using System.Windows.Media;
using System.Windows.Interop;
namespace mkacg
{
    public class AeroHelper
    {
        public static bool ExtendGlassFrame (Window window , Thickness margin)
        {
            if (!DwmApi.DwmIsCompositionEnabled())
                return false;
            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("The Window must be shown before extending glass.");
            // Set the background to transparent from both the WPF and Win32 perspectives
            window.Background = Brushes.Transparent;
            HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;
            DwmApi.MARGINS margins = new DwmApi.MARGINS((int)margin.Left , (int)margin.Top , (int)margin.Right , (int)margin.Bottom);
            DwmApi.DwmExtendFrameIntoClientArea(hwnd , margins);
            return true;
        }
    }
    public class DwmApi
    {
        [DllImport("dwmapi.dll" , PreserveSig = false)]
        public static extern void DwmEnableBlurBehindWindow (IntPtr hWnd , DWM_BLURBEHIND pBlurBehind);
        [DllImport("dwmapi.dll" , PreserveSig = false)]
        public static extern void DwmExtendFrameIntoClientArea (IntPtr hWnd , MARGINS pMargins);
        [DllImport("dwmapi.dll" , PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled ();
        [DllImport("dwmapi.dll" , PreserveSig = false)]
        public static extern void DwmGetColorizationColor (
            out int pcrColorization ,
            [MarshalAs(UnmanagedType.Bool)]out bool pfOpaqueBlend);
        [DllImport("dwmapi.dll" , PreserveSig = false)]
        public static extern void DwmEnableComposition (bool bEnable);
        [DllImport("dwmapi.dll" , PreserveSig = false)]
        public static extern IntPtr DwmRegisterThumbnail (IntPtr dest , IntPtr source);
        [DllImport("dwmapi.dll" , PreserveSig = false)]
        public static extern void DwmUnregisterThumbnail (IntPtr hThumbnail);
        [DllImport("dwmapi.dll" , PreserveSig = false)]
        public static extern void DwmUpdateThumbnailProperties (IntPtr hThumbnail , DWM_THUMBNAIL_PROPERTIES props);
        [DllImport("dwmapi.dll" , PreserveSig = false)]
        public static extern void DwmQueryThumbnailSourceSize (IntPtr hThumbnail , out Size size);
        [StructLayout(LayoutKind.Sequential)]
        public class DWM_THUMBNAIL_PROPERTIES
        {
            public uint dwFlags;
            public RECT rcDestination;
            public RECT rcSource;
            public byte opacity;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fVisible;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fSourceClientAreaOnly;
            public const uint DWM_TNP_RECTDESTINATION = 0x00000001;
            public const uint DWM_TNP_RECTSOURCE = 0x00000002;
            public const uint DWM_TNP_OPACITY = 0x00000004;
            public const uint DWM_TNP_VISIBLE = 0x00000008;
            public const uint DWM_TNP_SOURCECLIENTAREAONLY = 0x00000010;
        }
        [StructLayout(LayoutKind.Sequential)]
        public class MARGINS
        {
            public int cxLeftWidth, cxRightWidth, cyTopHeight, cyBottomHeight;
            public MARGINS (int left , int top , int right , int bottom)
            {
                cxLeftWidth = left;
                cyTopHeight = top;
                cxRightWidth = right;
                cyBottomHeight = bottom;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public class DWM_BLURBEHIND
        {
            public uint dwFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fEnable;
            public IntPtr hRegionBlur;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fTransitionOnMaximized;
            public const uint DWM_BB_ENABLE = 0x00000001;
            public const uint DWM_BB_BLURREGION = 0x00000002;
            public const uint DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left, top, right, bottom;
            public RECT (int left , int top , int right , int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
        }
    }
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class talk_control : Window
    {
        public talk_control ()
        {
            InitializeComponent();
       
        }
        protected override void OnSourceInitialized (EventArgs e)
        {
            base.OnSourceInitialized(e);
            AeroHelper.ExtendGlassFrame(this , new Thickness(-1));
        }
        string Name_;
        string musicid_;

        private void Window_Loaded (object sender , RoutedEventArgs e)
        {
          
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 1;
            this.Left = 1 / (workWidth - this.Width);
      
            if (Class1.sta == 0)
            {
                open_config();
                Class1.sta = 1;
            }
            else
            {
                open_config();
                source_text.Text = Name_ + "  欢迎回来";
                //如果电台在播放，显示控件

            }
            try
            {
             //  image.Source = new BitmapImage(new Uri(Class1.redio_img , UriKind.Relative));
             Console.WriteLine(Class1.redio_img);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }


        Class1 _talk = new Class1();
        private void textBox_KeyDown (object sender , KeyEventArgs e)
       {
            if (e.Key == Key.Enter)
            {
                open_config();
                if (textBox.Text == "你是谁的女朋友")
                {
                    source_text.Text = "我是" + Name_ + "可爱的女朋友";
                    textBox.Text = "";
                }

                else if (textBox.Text == "设置开机启动")
                {
                    textBox.Text = "";
                    open_autorun();
                }
                else if (textBox.Text == "设置关闭开机启动")
                {
                    textBox.Text = "";
                    delete_autorun();
                }
                else if (textBox.Text.IndexOf("称呼我为") > -1)
                {
                    Name_ = "";
                    Console.WriteLine(textBox.Text.Remove(0 , 4));
                    Name_ = textBox.Text.Remove(0 , 4);
                    create_config(Name_ , musicid_);
                    textBox.Text = "";
                    source_text.Text = "对您的称呼已更改，" + Name_;
                }
                else if (textBox.Text == "录音测试")
                {
                    Talk_baidu baidu = new Talk_baidu();
                    // baidu.sound();
                    textBox.Text = "";
                }
                else if (textBox.Text == "录音开始")
                {
                    Talk_baidu baidu = new Talk_baidu();
                    baidu.luyin_on();
                    textBox.Text = "";
                }
                else if (textBox.Text == "录音停止")
                {
                    Talk_baidu baidu = new Talk_baidu();
                    //  baidu.luyin_save();
                    if (baidu.luyin_save())
                    {
                        httpRequest hR = new httpRequest();
                        string token_Access = hR.getStrAccess(hR.API_key , hR.API_secret_key);
                        string token_Text = hR.getStrText(hR.API_id , token_Access , "zh" , "1.wav" , "pcm" , "8000");
                        source_text.Text = token_Text;
                        // File.Delete("1.wav");
                    }
                    textBox.Text = "";
                }
                else
                {
                    source_text.Text = _talk.main(textBox.Text) + "\n\n我如此如此这般这般说道";
                    textBox.Text = "";
                }
            }
        }



        private void Window_Deactivated (object sender , EventArgs e)
        {
            /*
            this.Hide();
            textBox.Text = "";
            source_text.Text = "";
            */
            this.Close();
        }
        private void open_autorun ()
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey Run = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run" , true);
            try
            {
                Run.SetValue("MKACG" , AppDomain.CurrentDomain.BaseDirectory + "萌控二次元.exe");
                //source_text.Text = "已开启开机项\n你可以通过“设置关闭开机启动”命令来关闭。";

            }
            catch
            {

            }
            HKCU.Close();
        }
        private void delete_autorun ()
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey Run = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run" , true);
            try
            {
                Run.DeleteValue("MKACG");
                source_text.Text = "已关闭开机项\n你可以通过“设置开机启动”命令来启动。";
            }
            catch
            {
           source_text.Text = "未知的失败";
            }
        }
        public void open_config ()
        {
            try
            {

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
                            Name_ = value;
                        }
                        else if (name == "musicid")
                        {
                            musicid_ = value;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void create_config (string name , string musicid)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0" , "UTF-8" , null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("result");  //一级
            doc.AppendChild(root);
            XmlElement element1 = doc.CreateElement("system");
            element1.SetAttribute("name" , name);
            element1.SetAttribute("musicid" , musicid);
            root.AppendChild(element1);
            doc.AppendChild(root);
            doc.Save("config.xml");
        }

        private void Window_Activated (object sender , EventArgs e)
        {

        }

        private void source_text_LayoutUpdated (object sender , EventArgs e)
        {
            source_text.Height = source_text.ActualHeight;
        }


        private void label_MouseLeftButtonDown (object sender , MouseButtonEventArgs e)
        {
            frame.Visibility = Visibility.Hidden;
            body.Visibility = Visibility.Visible;

        }
    }
}
