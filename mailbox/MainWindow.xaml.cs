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

namespace mailbox
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mail_ok_Click(object sender, RoutedEventArgs e)
        {
            /*判断一下邮箱的地址和尝试使用pop3和smtp。成功后写入xml配置文件 config/mailbox.ini 
            *每隔15分钟检测一次，如果有新邮件，就通知主程序进行消息推送。
            *判断关键字  [回复：]
            */

        }
    }
}
