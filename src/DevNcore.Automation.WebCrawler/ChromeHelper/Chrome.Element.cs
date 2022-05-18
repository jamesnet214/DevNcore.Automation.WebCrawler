using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNcore.Automation.WebCrawler
{
    public partial class Chrome
    {
        public IWebElement GetElement(string xpath)
        {
            try
            {
                return driver.FindElement(By.XPath(xpath));
            }
            catch
            {
                return null;
            }
        }

        public IWebElement GetElement(By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch
            {
                return null;
            }
        }
       

        public IWebElement GetElement(IWebElement element, By by)
        {
            try
            {
                return element.FindElement(by);
            }
            catch
            {
                return null;
            }
        }

        public IWebElement GetElement(IWebElement element, string xpath)
        {
            try
            {
                return element.FindElement(By.XPath(xpath));
            }
            catch
            {
                return null;
            }
        }




        public ReadOnlyCollection<IWebElement> GetElementList(string xpath)
        {
            try
            {
                return driver.FindElements(By.XPath(xpath));
            }
            catch
            {
                return null;
            }
        }


        public ReadOnlyCollection<IWebElement> GetElementList(By by)
        {
            try
            {
                return driver.FindElements(by);
            }
            catch
            {
                return null;
            }
        }
       

        public ReadOnlyCollection<IWebElement> GetElementList(IWebElement element, By by)
        {
            try
            {
                return element.FindElements(by);
            }
            catch
            {
                return null;
            }
        }

        public ReadOnlyCollection<IWebElement> GetElementList(IWebElement element, string xpath)
        {
            try
            {
                return element.FindElements(By.XPath(xpath));
            }
            catch
            {
                return null;
            }
        }


        public IWebElement GetParentElement(IWebElement element)
        {
            try
            {
                IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
                IWebElement parent = (WebElement)executor.ExecuteScript("return arguments[0].parentNode;", element);
                return parent;
            }
            catch (Exception ex)
            {
            }

            return null;
        }














    }
}
