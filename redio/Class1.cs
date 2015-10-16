using System;
using System.Runtime.InteropServices;
using System.Xml;
namespace redio
{
    public class redio
    {
        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState (out int connectionDescription , int reservedValue);
        public String[] redio_ (int list)
        {
            int i = 0;
            String[] source = new String[2];
            if (InternetGetConnectedState(out i , 0))
            {
                try
                {
                    String url = "http://www.m-acg.com/desktop/node.xml";
                    XmlElement root = null;
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(url);
                    root = xmldoc.DocumentElement;
                    XmlNodeList listNodes = null, listsource = null;
                    string listxml_url = "/result/music" + list + "/url";
                    string listxml_name = "/result/music" + list + "/name";
                    listNodes = root.SelectNodes(listxml_url);
                    listsource = root.SelectNodes(listxml_name);
                    foreach (XmlNode node in listNodes)
                    {
                        source[0] = node.InnerText.ToString();
                    }
                    foreach (XmlNode node in listsource)
                    {
                        source[1] = "来自：" + node.InnerText;
                    }
                }
                catch
                {
                    source[0] = "网络失败";
                    source[1] = "";
                }
                return source;
            }
            else
            {
                source[0] = "网络失败";
                source[1] = "";
                return source;
            }
        }
    }
}