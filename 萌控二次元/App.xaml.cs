using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace 萌控二次元
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup (StartupEventArgs e)
        {
            Process process = Process.GetCurrentProcess();
            foreach (Process p in Process.GetProcessesByName(process.ProcessName))
            {
                if (p.Id != process.Id)
                {
                    Application.Current.Shutdown();
                    return;
                }
            }
            base.OnStartup(e);
        }
    }
}