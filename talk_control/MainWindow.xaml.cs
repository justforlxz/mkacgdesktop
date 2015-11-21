using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Microsoft.Win32;

namespace talk_control
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow ()
        {
            InitializeComponent();
            open_config();
            source_text.Text = Name_+"  欢迎回来";
        }
        string Name_;
        private void Window_Loaded (object sender , RoutedEventArgs e)
        {
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 1;
            this.Left = 1/(workWidth - this.Width);
        }
        talk.Class1 _talk = new talk.Class1();
        private void textBox_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                switch (textBox.Text)
                {
                    case "你是谁的女朋友":
                        source_text.Text = "我是"+Name_+"可爱的女朋友";
                        textBox.Text = "";
                        break;
                    case "设置开机启动":
                        textBox.Text = "";
                        open_autorun();
                        break;
                    case "设置关闭开机启动":
                        textBox.Text = "";
                        delete_autorun();
                        break;
                    case "打开设置":
                        source_text.Text = "请在主窗体右键";
                        textBox.Text = "";
                        break;
                    default:
                        source_text.Text = _talk.main(textBox.Text) + "\n\n我如此如此这般这般说道";
                        textBox.Text = "";
                        break;
                }
            }
        }



        private void Window_Deactivated (object sender , EventArgs e)
        {
            this.Hide();
            textBox.Text = "";
            source_text.Text = "";
        }
        private void open_autorun ()
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey Run = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run" , true);
            try
            {
                Run.SetValue("MKACG" , AppDomain.CurrentDomain.BaseDirectory + "萌控二次元.exe");
                source_text.Text = "已开启开机项\n你可以通过“设置关闭开机启动”命令来关闭。";

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
                      
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
