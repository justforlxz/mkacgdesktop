using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace mkacg
{
    /// <summary>
    /// settings.xaml 的交互逻辑
    /// </summary>
    public partial class settings : Window
    {
        public settings ()
        {
            InitializeComponent();
        }

        private void button_Click (object sender , RoutedEventArgs e)
        {
            if (system_name.Text!="")
            {
                create_config(system_name.Text);
            }
            else
            {
                MessageBox.Show("注意不要留空");
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
                            system_name.Text = value;
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void create_config (String name)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0" , "UTF-8" , null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("result");  //一级
            doc.AppendChild(root);
            XmlElement element1 = doc.CreateElement("system");
            //  element1.SetAttribute("id" , "内容");
            element1.SetAttribute("name",name);
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
                create_config("主人" );
            }
        }
    }
}
