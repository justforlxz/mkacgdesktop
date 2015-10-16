using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
                    String url = "http://192.168.0.15/node.xml";
                    //  String url = "C:/Users/zhuzi/Documents/GitHubVisualStudio/mkacgdesktop/萌控二次元/node.xml";
                    XmlElement root = null;
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(url);
                        root = xmldoc.DocumentElement;
                        XmlNodeList listNodes = null, listsource = null;
                        string listxml_url = "/result/music" + list + "/url";
                        string listxml_name = "/result/music"+list+"/name";
                        listNodes = root.SelectNodes(listxml_url);
                        listsource = root.SelectNodes(listxml_name);
                        foreach (XmlNode node in listNodes)
                        {

                            // node.InnerText
                            source[0] = node.InnerText;
                        }
                        foreach (XmlNode node in listsource)
                        {
                            //node.InnerText
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