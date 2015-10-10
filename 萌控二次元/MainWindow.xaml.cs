using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
namespace 萌控二次元
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
        private void MenuItem_Click (object sender , RoutedEventArgs e)
        {
            mkacg_showhitokoto.Class1 mkacgclass = new mkacg_showhitokoto.Class1();
            String[] list = mkacgclass.hitokoto();
            bg_text.Text = "";
            bg_text.Text = list[0] + "\n";
            bg_source.Content = list[1] + "\n";
        }
        private void MenuItem_Click_1 (object sender , RoutedEventArgs e)
        {
            mkacg_talk.MainWindow mw = new mkacg_talk.MainWindow();
            mw.Show();
        }

        private void MenuItem_Click_2 (object sender , RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded (object sender , RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void image_MouseLeftButtonDown (object sender , MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

    }
}
