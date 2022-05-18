using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevNcore.Automation.WebCrawler
{
    public partial class Chrome
    {
        public IntPtr GetWindowHanlde()
        {
            IntPtr hwnd = IntPtr.Zero;

            if (driver == null)
                return hwnd;

            if (mainProcess == null)
                return hwnd;

            try
            {
                hwnd = mainProcess.MainWindowHandle;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }

            return hwnd;
        }

        public bool MoveWindow(Point position)
        {
            if (driver == null)
                return false;

            try
            {
                driver.Manage().Window.Position = position;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }

            return false;
        }


        public bool SetSize(Size size)
        {
            if (driver == null)
                return false;
            try
            {
                driver.Manage().Window.Size = size;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }

            return false;
        }

        public Size GetSize()
        {
            if (driver == null)
                return new Size();

            try
            {
                return driver.Manage().Window.Size;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }

            return new Size();
        }

        public bool HideWindow()
        {
            if (driver == null)
                return false;

            if (mainProcess == null)
                return false;

            Win32.SetWindowPos(mainProcess.MainWindowHandle, IntPtr.Zero, -2000, 0, 0, 0, 
                Win32.SWP_NOZORDER | Win32.SWP_NOSIZE | Win32.SWP_SHOWWINDOW);

            return true;
        }


        public bool ShowWindow()
        {
            if (driver == null)
                return false;

            if (mainProcess == null)
                return false;

            Win32.SetWindowPos(mainProcess.MainWindowHandle, IntPtr.Zero, 0, 0, 0, 0, 
                Win32.SWP_NOZORDER | Win32.SWP_NOSIZE | Win32.SWP_SHOWWINDOW);

            return true;
        }

        public bool Maximized()
        {
            if (driver == null)
                return false;

            try
            {
                driver.Manage().Window.Maximize();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Maximized: exception: " + ex.Message);
            }

            return true;
        }

        public bool Minimized()
        {
            if (driver == null)
                return false;

            try
            {
                driver.Manage().Window.Minimize();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }

            return true;
        }


        public bool IsWindow()
        {
            if (mainProcess == null)
                return false;

            if (Win32.IsWindow(mainProcess.MainWindowHandle) == false)
                return false;

            return true;
        }





    }
}
