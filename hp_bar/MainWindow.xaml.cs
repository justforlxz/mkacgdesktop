using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace hp_bar
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


        private void images_hp_bar_png_MouseDown (object sender , MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
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
                foreach (XmlNode  node in firstlevelnodelist)
                {
                    XmlAttributeCollection attributecol = node.Attributes;
                    foreach (XmlAttribute attri in attributecol)
                    {
                        string name = attri.Name;
                        string value = attri.Value;
                        Console.WriteLine("{0}={1}" , name , value);
                        if (name!="lv")
                        {
                            hp_name.Content = value;
                        }
                        
                        hp_level.Content = value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void create_config ()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0","UTF-8",null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("result");  //一级
            doc.AppendChild(root);

            XmlElement element1 = doc.CreateElement("hp_bar"); 
            element1.SetAttribute("name","kirito");
            element1.SetAttribute("lv","Lv.1");
            root.AppendChild(element1);
            doc.AppendChild(root);
            doc.Save("config.xml");
        }
        private void Window_Loaded (object sender , RoutedEventArgs e)
        {
            if (File.Exists(@"config.xml"))
            {
                open_config();
            }
            else
            {
                create_config();
            }

        }
    }
}