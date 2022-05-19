using CodingVoice.Views;
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
        MainWindow win { get; set; }
        Chrome cm { get; set; }
        string step { get; set; }
        bool IsReady { get; set; }

        private Visibility _imgVisibility = Visibility.Collapsed;
        public Visibility imgVisibility
        {
            get { return _imgVisibility; }
            set { _imgVisibility = value; OnPropertyChanged(); }
        }

        private string _Status = "음성인식 준비중입니다...";
        public string Status
        {
            get { return _Status; }
            set { _Status = value; OnPropertyChanged(); }
        }



        public VM(MainWindow _win)
        {
            win = _win;
            win.Closed += Win_Closed;
            Init();
        }

        private void Win_Closed(object? sender, EventArgs e)
        {
            if (cm != null)
            {                
                cm.Exit();
                cm = null;
            }
        }


        void Init()
        {
            BackgroundWorker w = new BackgroundWorker();
            w.DoWork += W_DoWork;
            w.RunWorkerAsync();
        }

        private void W_DoWork(object? sender, DoWorkEventArgs e)
        {
            ChromeDriverSetting s = new ChromeDriverSetting();
            s.size = new System.Drawing.Size(800, 800);
            s.headless = true;
            s.enableMediaStream = true; // 마이크 허용하려면

            cm = new Chrome(s);
            cm.Url("https://www.bing.com");
            IsReady = true;
            Status = "F1을 누른후 말하세요.";

            string oldTitle = "";
            Stopwatch w = new Stopwatch();

            while(true)
            {
                Thread.Sleep(10);
                if (step == "")
                    continue;

                if (cm == null)
                    continue;

                try
                {
                    IWebElement el = null;

                    if (step == "열기")
                    {
                        w.Stop();
                        w.Restart();

                        oldTitle = cm.ExcuteJS("return document.querySelector('head > title').text")?.ToString().Replace(" - 검색", "");

                        // 음성열기
                        cm.Click("//*[@id='sb_form']/div[1]/div", 100);
                        step = "대기";                        
                    }
                    else if (step == "대기")
                    {
                        if (w.Elapsed.TotalSeconds >= 10)
                        {
                            w.Stop();
                            cm.Url("https://www.bing.com");
                            step = "";
                            imgVisibility = Visibility.Collapsed;
                            Status = "F1을 누른후 말하세요.";

                            // 닫기
                            cm.Click("/html/body/ytd-app/ytd-popup-container/tp-yt-paper-dialog/ytd-voice-search-dialog-renderer/div[1]/div[2]/ytd-button-renderer/a/yt-icon-button/button", 1000);
                            continue;
                        }

                        string title = cm.ExcuteJS("return document.querySelector('head > title').text")?.ToString().Replace(" - 검색", "").Replace(" ", "");
                        // 닫기 버튼
                        if (title != oldTitle)
                        {
                            cm.Url("https://www.bing.com");
                            step = "";
                            imgVisibility = Visibility.Collapsed;
                            Status = "F1을 누른후 말하세요.";

                            AddClass(title);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        

        void AddClass(string name)
        {
            win.Dispatcher.Invoke(() =>
            {
                win.editor.AppendText($"public class {name}\n{{\n\n}}\n");
            });
        }

        void Voice()
        {
            if(IsReady == false)
            {
                return;
            }

            if(imgVisibility == Visibility.Collapsed)
            {
                step = "열기";
                imgVisibility = Visibility.Visible;
                Status = "듣는중....";
            }
            else
            {
                step = "";
                imgVisibility = Visibility.Collapsed;
                Status = "F1을 누른후 말하세요.";
            }
        }


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
                case "Voice":
                    Voice();
                    break;
            }
        }
    }





    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null) throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter ?? "<N/A>");
        }

    }
}
