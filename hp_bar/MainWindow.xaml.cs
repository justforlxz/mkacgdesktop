using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
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

                create_config(this.Top , this.Left);
            }
        }
        public void open_config ()
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load("hp_config.xml");
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
                            hp_name.Content = value;
                        }
                        else if (name == "lv")
                        {
                            hp_level.Content = value;
                        }
                        else if (name == "x")
                        {
                            this.Top = double.Parse(value);
                        }
                        else if (name == "y")
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
        public void create_config (double x , double y)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0" , "UTF-8" , null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("result");  //一级
            doc.AppendChild(root);
            XmlElement element1 = doc.CreateElement("hp_bar");
            element1.SetAttribute("name" , "kirito");
            element1.SetAttribute("lv" , "Lv.1");
            element1.SetAttribute("x" , x.ToString());
            element1.SetAttribute("y" , y.ToString());

            root.AppendChild(element1);
            doc.AppendChild(root);
            doc.Save("hp_config.xml");
        }
        private void Window_Loaded (object sender , RoutedEventArgs e)
        {

            if (File.Exists(@"hp_config.xml"))
            {
                open_config();
            }
            else
            {
                double workHeight = SystemParameters.WorkArea.Height;
                double workWidth = SystemParameters.WorkArea.Width;
                double top = (workHeight - this.Height) / 15;
                double left = (workWidth - this.Width) / 13;
                create_config(Top , Left);
            }

        }

    }
}