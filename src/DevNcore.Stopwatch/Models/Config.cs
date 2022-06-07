using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevNcore.Stopwatch.Models
{
    public class UserConfig
    {
        public int winX { get; set; }
        public int winY { get; set; }
        //public int winWidth { get; set; }
        //public int winHeight { get; set; }





        public static void Save(MainWindow win, UserConfig cfg)
        {
            try
            {
                cfg.winX = (int)win.Left;
                cfg.winY = (int)win.Top;
                //cfg.winWidth = (int)win.Width;
                //cfg.winHeight = (int)win.Height;

                string ProjectName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                string FolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{ProjectName}";
                if (Directory.Exists(FolderPath) == false)
                    Directory.CreateDirectory(FolderPath);

                string 파일경로 = $"{FolderPath}\\cfg.json";

                var json = JsonConvert.SerializeObject(cfg);
                File.WriteAllText(파일경로, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] Exception => {ex.Message}");
            }
        }


        public static void MoveWindow(MainWindow win, UserConfig cfg)
        {
            //if (cfg.winWidth < 100)
            //    cfg.winWidth = 100;
            //if (cfg.winHeight < 100)
            //    cfg.winHeight = 100;

            win.Left = cfg.winX;
            win.Top = cfg.winY;
            //win.Width = cfg.winWidth;
            //win.Height = cfg.winHeight;            
        }


        public static UserConfig Load()
        {
            try
            {
                string ProjectName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                string FolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{ProjectName}";
                if (Directory.Exists(FolderPath) == false)
                    Directory.CreateDirectory(FolderPath);

                string FilePath = $"{FolderPath}\\cfg.json";
                if (File.Exists(FilePath) == false)
                    return new UserConfig();

                var json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<UserConfig>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] Exception => {ex.Message}");
            }

            return new UserConfig();
        }



    }
}
