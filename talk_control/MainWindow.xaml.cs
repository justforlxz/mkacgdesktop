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
        }

        private void Window_Loaded (object sender , RoutedEventArgs e)
        {
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 1;
            this.Left = 1/(workWidth - this.Width);
        }
        talk.Class1 _talk = new talk.Class1();
        private void sumbit_MouseLeftButtonDown (object sender , MouseButtonEventArgs e)
        {

        }

        private void textBox_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                switch (textBox.Text)
                {
                    case "你是谁的女朋友":
                        source_text.Text = "我是小竹可爱的女朋友";
                        break;
                    case "关闭":
                        this.Hide();
                        textBox.Text ="";
                        source_text.Text = "";
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
        }
    }
}
