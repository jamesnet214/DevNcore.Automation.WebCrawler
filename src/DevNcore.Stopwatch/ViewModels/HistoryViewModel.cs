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
    public class HistoryViewModel : INotifyPropertyChanged
    {

        HistoryWindow win { get; set; }
        public HistoryViewModel(HistoryWindow win)
        {
            this.win = win;
            InitDataBase();
        }

     

        #region Properties
        public event PropertyChangedEventHandler? PropertyChanged;
       
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
                case "Close":
                    Close();                    
                    break;               
            }
        }

      


        private void Close()
        {               
            win.Close();
        }

      

        #endregion




        #region LightDB

        private string DataBaseFilePath { get; set; }

        private void InitDataBase()
        {
            string ProjectName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            string FolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{ProjectName}";
            if (Directory.Exists(FolderPath) == false)
                Directory.CreateDirectory(FolderPath);

            DataBaseFilePath = $"{FolderPath}\\Database.db";
        }

        private void SaveHitory(TimeSpan TimeSpan, string Memo)
        {
            try
            {
                using (var db = new LiteDatabase(DataBaseFilePath))
                {
                    var col = db.GetCollection<History>("history");

                    var history = new History
                    {
                        Created = DateTime.Now,
                        TimeSpan = TimeSpan,
                        Memo = Memo,
                    };

                    col.Insert(history);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] exception => {ex.Message}");
            }
        }


        private bool DeleteHitory(int id)
        {
            bool result = false;
            
            try
            {
                using (var db = new LiteDatabase(DataBaseFilePath))
                {
                    // Get customer collection
                    var table = db.GetCollection<History>("history");
                    var col = table.Find(x => x.Id == id).FirstOrDefault();
                    if (col != null)
                    {
                        table.Delete(col.Id);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] exception => {ex.Message}");
            }

            return result;            
        }






        #endregion



    }
}
