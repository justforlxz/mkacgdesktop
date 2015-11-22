using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using NAudio.Wave;
using static System.Net.Mime.MediaTypeNames;
using System.Timers;
namespace baidutalk
{
    public class Talk_baidu
    {
        Timer t = new Timer(10000);   //实例化Timer类，设置间隔时间为10000毫秒； 
        public void sound ()
        {
            t.Elapsed += new ElapsedEventHandler(luyin_save); //到达时间的时候执行事件；   
            t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；
            luyin_on();
            t.Start();
            Console.WriteLine("录音开始");   
        }
        //语音处理
        [DllImport("winmm.dll" , EntryPoint = "mciSendString" , CharSet = CharSet.Auto)]
        public static extern int mciSendString (
         string lpstrCommand ,
         string lpstrReturnString ,
         int uReturnLength ,
         int hwndCallback
        );
        public void luyin_on ()
        {

            mciSendString("set wave bitpersample 8" , "" , 0 , 0);
            mciSendString("set wave samplespersec 8000" , "" , 0 , 0);
            mciSendString("set wave channels 2" , "" , 0 , 0);
            mciSendString("set wave format tag pcm" , "" , 0 , 0);
            mciSendString("open new type WAVEAudio alias movie" , "" , 0 , 0);

            mciSendString("record movie" , "" , 0 , 0);
        }
        public void luyin_save (object source , ElapsedEventArgs e)
        {
            mciSendString("stop movie" , "" , 0 , 0);
            mciSendString("save movie D:\\Documents\\GitHubVisualStudio\\mkacgdesktop\\萌控二次元\\bin\\Debug\\1.wav" , "" , 0 , 0);
            mciSendString("close movie" , "" , 0 , 0);
            // return true;
            Console.WriteLine("录音结束");
        }


    }
}