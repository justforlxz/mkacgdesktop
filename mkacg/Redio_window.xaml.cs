using System;
using System.Collections.Generic;
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

namespace mkacg
{
    /// <summary>
    /// Redio_window.xaml 的交互逻辑
    /// </summary>
    public partial class Redio_window : Window
    {
        public Redio_window ()
        {
            InitializeComponent();
            this.slider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(slider_ValueChanged);//注册事件 
        }
        public delegate void play_next_Click (object sender , RoutedEventArgs e);
        public event play_next_Click play_next_click;
        private void button_Click (object sender , RoutedEventArgs e)
        {
            play_next_click(sender , e);
        }
        public delegate void change_volume (double value);
        public event change_volume cv;

        private void slider_ValueChanged (object sender , RoutedPropertyChangedEventArgs<double> e)
        {
            cv(slider.Value);
            Class1.redio_volume = slider.Value;
        }

        private void Window_Loaded (object sender , RoutedEventArgs e)
        {
            slider.Value = Class1.redio_volume;
        }
    }
}
