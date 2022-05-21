using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevNcore.Automation.WebCrawler
{
    public partial class Chrome
    {
        public object ExcuteJS(string script)
        {
            try
            {
                if (driver == null)
                    return null;

                return driver.ExecuteScript(script);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }

            return null;
        }




    }
}
