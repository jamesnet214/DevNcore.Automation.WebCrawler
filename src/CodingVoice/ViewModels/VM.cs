using CodingVoice.Views;
using DevNcore.Automation.Speech;
using DevNcore.Automation.WebCrawler;
using DevNcore.UI.Foundation.Mvvm;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CodingVoice.ViewModels
{
    public class VM : ObservableObject
    {
        MainWindow Win { get; set; }
        SpeechAPI Speech { get; set; }
        bool IsReady { get; set; }


        private string _Status = "음성인식 준비중입니다...";
        public string Status
        {
            get { return _Status; }
            set { _Status = value; OnPropertyChanged(); }
        }

        public VM(MainWindow _win)
        {
            Win = _win;
            Win.Closed += Win_Closed;
            Win.Loaded += Win_Loaded;
        }

        private async void Win_Loaded(object sender, RoutedEventArgs e)
        {
            Speech = new SpeechAPI();
            await Speech.Initialize();
            IsReady = true;
            Status = "F1을 누른후 말하세요.";
        }

        private void Win_Closed(object? sender, EventArgs e)
        {
            Speech.Close();
        }


        private async Task Voice()
        {
            if(IsReady == false)
            {
                MessageBox.Show("아직 준비되지 않았습니다.\n잠시만 기다려 주세요.", "준비중", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            BackgroundWorker w = new BackgroundWorker();
            w.DoWork += (ss, ee) =>
            {
                Status = "듣는중....";

                var text = Speech.Run().Result;
                Win.Dispatcher.Invoke(() =>
                {
                    Win.editor.AppendText(text + "\n");
                });

                Status = "F1을 누른후 말하세요...";
            };
            w.RunWorkerAsync();
        }



        private RelayCommand<string> _BtnEvent;
        public RelayCommand<string> BtnEvent
        {
            get { return _BtnEvent ?? (_BtnEvent = new RelayCommand<string>(EventBtn)); }
        }

        private void EventBtn(object obj)
        {
            string msg = obj.ToString();
            switch (msg)
            {
                case "Voice":
                    Voice();                  
                    break;
            }
        }
    }


}
