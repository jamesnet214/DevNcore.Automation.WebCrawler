using DevNcore.Stopwatch.Models;
using DevNcore.Stopwatch.Views;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DevNcore.Stopwatch.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public MainViewModel(MainWindow win)
        {
            this.win = win;

            cfg = UserConfig.Load();
            UserConfig.MoveWindow(this.win, cfg);

            Thread th = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    Hour = $"{timer.Elapsed.Hours:D2}";
                    Minute = $"{timer.Elapsed.Minutes:D2}";
                    Second = $"{timer.Elapsed.Seconds:D2}";
                    Millisecond = $"{timer.Elapsed.Milliseconds / 10:D2}";
                }
            });
            th.IsBackground = true;
            th.Start();
        }

     

        #region Properties
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool TopMost { get; set; } = true;

        public string Hour { get; set; } = "00";
        public string Minute { get; set; } = "00";
        public string Second { get; set; } = "00";
        public string Millisecond { get; set; } = "00";


        public bool IsEnableStart { get; set; } = true;
        public bool IsEnableStop { get; set; } = false;
        public bool IsEnableReset { get; set; } = false;
        #endregion


        #region Values
        public System.Diagnostics.Stopwatch timer { get; set; } = new System.Diagnostics.Stopwatch();
        public UserConfig cfg { get; set; } = new UserConfig();
        private MainWindow win { get; set; }
        private HistoryWindow winHistory{ get; set; }

        #endregion



        #region Events
        private RelayCommand _BtnEvent;
        public RelayCommand BtnEvent
        {
            get { return _BtnEvent ?? (_BtnEvent = new RelayCommand(EventBtn)); }
        }

        private void EventBtn(object obj)
        {
            string msg = obj.ToString();
            switch (msg)
            {
                case "StartTimer":
                    StartTimer();
                    break;
                case "StopTimer":
                    StopTimer();
                    break;
                case "ResetTimer":
                    ResetTimer();
                    break;
                case "Close":
                    Close();                    
                    break;
                case "Setting":
                    Setting();
                    break;
            }
        }

        private void Setting()
        {
            if(winHistory == null)
            {
                winHistory = new HistoryWindow();
                winHistory.Owner = win;                
            }

            winHistory.ShowDialog();
            winHistory = null;
        }


        private void Close()
        {   
            UserConfig.Save(win, cfg);
            win.Close();
        }

        private void StartTimer()
        {
            if (timer.IsRunning)
                return;

            IsEnableStart = false;
            IsEnableStop = true;

            timer.Start();            
        }

        private void StopTimer()
        {
            IsEnableStart = true;
            IsEnableStop = false;

            timer.Stop();            
        }

        private void ResetTimer()
        {
            IsEnableStart = true;
            IsEnableStop = false;
            timer.Stop();
            timer.Reset();
        }

        #endregion




    }
}
