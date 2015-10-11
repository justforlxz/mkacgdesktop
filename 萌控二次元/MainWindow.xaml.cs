using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
        static int themes = 0, bg_showorhide = 0;
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timer_noew = new DispatcherTimer();
        private void MenuItem_Click (object sender , RoutedEventArgs e)
        {
            //显示一句话
            timer.Stop();
            bg.Visibility = bg_label.Visibility = bg_source.Visibility = bg_text.Visibility = Visibility.Visible;
            mkacg_showhitokoto.Class1 mkacgclass = new mkacg_showhitokoto.Class1();
            String[] list = mkacgclass.hitokoto();
            bg_text.Text = "";
            bg_text.Text = list[0] + "\n";
            bg_source.Content = list[1] + "\n";
            timer.Start();
        }
       
        private void showorhide (object sender , EventArgs e)
        {
            //bg.Visibility = bg_label.Visibility = bg_source.Visibility = bg_text.Visibility = Visibility.Visible;
            bg.Visibility = bg_label.Visibility = bg_source.Visibility = bg_text.Visibility = Visibility.Hidden;
            timer.Stop();
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
            if (themes==0)
            {
                image.Source = new BitmapImage(new Uri("Images/Main.png" , UriKind.Relative));
                themes = 1;
            }
            timer.Interval = new TimeSpan(0 , 0 , 4);
            timer.Tick += new EventHandler(showorhide);
           
        }

        private void image_MouseLeftButtonDown (object sender , MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        
        private void MenuItem_Click_3 (object sender , RoutedEventArgs e)
        {
            if (themes == 1)
            {
                image.Source = new BitmapImage(new Uri("Images/Main1.png" , UriKind.Relative));
                themes = 0;
            }
           else if (themes == 0)
            {
                image.Source = new BitmapImage(new Uri("Images/Main.png" , UriKind.Relative));
                themes = 1;
            }
           
        }
    }
}
