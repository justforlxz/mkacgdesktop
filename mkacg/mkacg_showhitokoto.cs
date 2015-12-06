using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace mkacg
{
    public class mkacg_showhitokoto
    {
        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState (out int connectionDescription , int reservedValue);

        public String[] hitokoto ()
        {
            int i = 0;
            String[] source = new String[2];
            if (InternetGetConnectedState(out i , 0))
            {
                String url = "http://api.hitokoto.us/rand?charset=utf-8&encode=xml";
                XmlElement root = null;
                XmlDocument xmldoc = new XmlDocument();
                try
                {
                    xmldoc.Load(url);
                    root = xmldoc.DocumentElement;
                    XmlNodeList listNodes = null, listsource = null;
                    listNodes = root.SelectNodes("/result/hitokoto");
                    listsource = root.SelectNodes("/result/source");
                    // /result/author 谁说的话
                    // /result/source 来自哪部
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