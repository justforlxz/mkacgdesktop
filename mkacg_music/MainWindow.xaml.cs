using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mkacg_music
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public class myListBox : System.Windows.Controls.ListBox
    {
        protected override DependencyObject GetContainerForItemOverride ()
        {
            return new myListBoxItem();
        }

    }
    public class myListBoxItem : System.Windows.Controls.ListBoxItem
    {
        protected override void OnSelected (System.Windows.RoutedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is ListBoxItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            ListBoxItem item = (ListBoxItem)dep;

            if (item.IsSelected)
            {
                item.IsSelected = !item.IsSelected;
                //e.Handled = true;
            }
            base.OnSelected(e);
        }
    }
    public partial class MainWindow : Window
    {
        public MainWindow ()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown (object sender , MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        HttpWebResponse Response = null;
        List<string> list = new List<string>();
        List<string> name_msuic = new List<string>();
        List<string> music = new List<string>();

        public object Newtonsoft { get; private set; }
        public List<string> ConnectTuLing ()
        {

            string result = null;
            int count = 0;
            music.Clear();
            try
            {
                string getURL = "http://music.163.com/api/playlist/detail?id=23075108";
                HttpWebRequest MyRequest = (HttpWebRequest)HttpWebRequest.Create(getURL);
                HttpWebResponse MyResponse = (HttpWebResponse)MyRequest.GetResponse();
                Response = MyResponse;
                using (Stream MyStream = MyResponse.GetResponseStream())
                {
                    long ProgMaximum = MyResponse.ContentLength;
                    long totalDownloadedByte = 0;
                    byte[] by = new byte[1024];
                    int osize = MyStream.Read(by , 0 , by.Length);
                    Encoding encoding = Encoding.UTF8;
                    while (osize > 0)
                    {
                        totalDownloadedByte = osize + totalDownloadedByte;
                        result += encoding.GetString(by , 0 , osize);
                        long ProgValue = totalDownloadedByte;
                        osize = MyStream.Read(by , 0 , by.Length);
                    }
                }
                JObject json = (JObject)JsonConvert.DeserializeObject(result);
                var urlMp3 =
               from p in json["result"]["tracks"]
               select (string)p["mp3Url"];
                var name =
              from p in json["result"]["tracks"]
              select (string)p["name"];
                foreach (var item in urlMp3)
                {
                    list.Add(item);
                    count += 1;
                }
                foreach (var item in name)
                {
                    name_msuic.Add(item);
                }
                Random random = new Random();
                int n = random.Next(0 , count);
             //   music.Add(list[n]);
             //   music.Add(name_msuic[n]);
            }
            catch (Exception)
            {
                List<string> error = new List<string>();
                error.Add("网络失败");
                return error;
            }

            //    return RandomSortList(list);
            return list;
        }

        private void button_Click (object sender , RoutedEventArgs e)
        {
            //ConnectTuLing ();

        }

    }
}