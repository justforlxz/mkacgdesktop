using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
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
            bgmusicplayer.Play();
        }

        static int themes = 0;
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timerToSendMessages = new DispatcherTimer();
        private void MenuItem_Click (object sender , RoutedEventArgs e)
        {
            //显示一句话
            timer.Stop();
            showorhidetrue();
            sendbox.Visibility = Visibility.Hidden;
            mkacg_showhitokoto.Class1 mkacgclass = new mkacg_showhitokoto.Class1();
            String[] list = mkacgclass.hitokoto();
            bg_text.Text = "";
            bg_text.Text = list[0] + "\n";
            bg_source.Content = list[1] + "\n";
            timer.Start();
        }
        public void showorhidetrue ()
        {
            bg_white.Visibility = bg_label.Visibility = bg_source.Visibility = bg_text.Visibility = Visibility.Visible;
            sendbox.Visibility = Visibility.Visible;
        }
        public void showorhide (object sender , EventArgs e)
        {
            bg_white.Visibility = bg_label.Visibility = bg_source.Visibility = bg_text.Visibility = Visibility.Hidden;
            sendbox.Visibility = Visibility.Hidden;
            timer.Stop();
            timerToSendMessages.Stop();
        }
        private void MenuItem_Click_2 (object sender , RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Loaded (object sender , RoutedEventArgs e)
        {
            this.Topmost = true;
            if (themes == 0)
            {
                image.Source = new BitmapImage(new Uri("Images/Main.png" , UriKind.Relative));
                themes = 1;
            }
            sendbox.Visibility = Visibility.Hidden;
            timer.Interval = new TimeSpan(0 , 0 , 4);
            timer.Tick += new EventHandler(showorhide);
            timer.Start();
            timerToSendMessages.Interval = new TimeSpan(0 , 0 , 10);
            timerToSendMessages.Tick += new EventHandler(showorhide);
            timerToSendMessages.Start();
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
        talk.Class1 _talk = new talk.Class1();
        private void sendbox_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                timer.Stop();
                timerToSendMessages.Stop();
                showorhidetrue();
                bg_text.Text = _talk.main(sendbox.Text);
                sendbox.Text = "";
                bg_source.Content = "";
                timerToSendMessages.Start();
            }
        }
        int count = 0;
        private void image_MouseDown (object sender , MouseButtonEventArgs e)
        {
            count += 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0 , 0 , 0 , 0 , 300);
            timer.Tick += (s , e1) => { timer.IsEnabled = false; count = 0; };
            timer.IsEnabled = true;
            if (count % 2 == 0)
            {
                timer.IsEnabled = false;
                count = 0;
                timerToSendMessages.Stop();
                showorhidetrue();
                timerToSendMessages.Start();
            }
        }
        private void sendbox_TextChanged (object sender , System.Windows.Controls.TextChangedEventArgs e)
        {
            timer.Stop();
            timerToSendMessages.Stop();
        }
        private void bgmusicplayer_MediaEnded (object sender , RoutedEventArgs e)
        {
            bgmusicplayer.Stop();
        }
    }
}