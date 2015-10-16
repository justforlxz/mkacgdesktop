using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;

namespace Update
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow ()
        {
            InitializeComponent();
        }
        XmlDocument doc = new XmlDocument();
        WebClient client = new WebClient();
        private void update_Click (object sender , RoutedEventArgs e)
        {

            download();
        }
        String str = null, sou = null;
        private void download ()
        {
            try
            {
                doc.Load("http://www.m-acg.com/desktop/update.xml");
                XmlNodeList version = doc.GetElementsByTagName("Verson");
                XmlNodeList source = doc.GetElementsByTagName("DownLoad");
                str = version[0].InnerText;
                sou = source[0].InnerText;
                FileVersionInfo myFileVersionInfo1 = FileVersionInfo.GetVersionInfo("萌控二次元.exe");
                if (myFileVersionInfo1.FileVersion==str)
                {
                    update_info.Content = "已最新";
                }
                else
                {
                    string URLAddress = sou;
                    string receivePath = AppDomain.CurrentDomain.BaseDirectory;
                    client.DownloadFile(URLAddress , receivePath + System.IO.Path.GetFileName(URLAddress));
                    KillProcess("萌控二次元.exe");
                    UnZipFile(System.IO.Path.GetFileName(URLAddress));
                    Process.Start("萌控二次元.exe");
                    update_info.Content = str;
                }
               
            }
            catch (Exception)
            {
                update_info.Content = "错误";
                throw;
            }
        }
        private void KillProcess (string processName)
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            //得到所有打开的进程   
            try
            {
                foreach (Process thisproc in Process.GetProcessesByName(processName))
                {
                    //找到程序进程,kill之。
                    if (!thisproc.CloseMainWindow())
                    {
                        thisproc.Kill();
                    }
                }

            }
            catch (Exception Exc)
            {
                MessageBox.Show(Exc.Message);
            }
        }
        private static void UnZipFile (string zipFilePath)
        {
            if (!File.Exists(zipFilePath))
            {
                Console.WriteLine("Cannot find file '{0}'" , zipFilePath);
                return;
            }

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {

                    Console.WriteLine(theEntry.Name);

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(theEntry.Name))
                        {

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data , 0 , data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data , 0 , size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}