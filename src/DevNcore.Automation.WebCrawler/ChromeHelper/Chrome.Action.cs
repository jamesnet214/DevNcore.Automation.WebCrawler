using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevNcore.Automation.WebCrawler.ChromeHelper
{
    public partial class Chrome
    {
        public bool Url(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            if (!url.StartsWith("https://") && !url.StartsWith("http://") && !url.StartsWith("file:///"))
            {
                return false;
            }

            driver.Navigate().GoToUrl(url);
            return true;
        }



        #region Click
        public bool Click(IWebElement element, int delayMillisecond)
        {
            try
            {
                element.Click();
                Thread.Sleep(delayMillisecond);
                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public bool Click(By by, int delayMillisecond)
        {
            try
            {
                IWebElement element = driver.FindElement(by);
                element.Click();
                Thread.Sleep(delayMillisecond);
                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public bool Click(string xpath, int delayMillisecond)
        {
            try
            {
                if (string.IsNullOrEmpty(xpath))
                    return false;

                IWebElement element = driver.FindElement(By.XPath(xpath));
                element.Click();
                Thread.Sleep(delayMillisecond);
                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }
        #endregion



        #region Scroll
        public bool ScrollToTopParent(int y1, int y2)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("scroll({0}, {1})", y1, y2));
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }


        public bool ScrollToElement(IWebElement element, bool 스크롤아래방향)
        {
            try
            {
                if (스크롤아래방향) // 아래로
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(false);", element);
                else
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public bool ScrollToElement(By by, bool 스크롤아래방향)
        {
            try
            {
                IWebElement element = driver.FindElement(by);
                if (스크롤아래방향) // 아래로
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(false);", element);
                else
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public bool ScrollToElement(string xpath, bool 스크롤아래방향)
        {
            try
            {
                IWebElement element = driver.FindElement(By.XPath(xpath));
                if (스크롤아래방향) // 아래로
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(false);", element);
                else
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        #endregion




        #region Input
        public bool SendInput(IWebElement element, string text, int delayMillisecond = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(text) || text == "")
                    element.Clear();
                else
                {
                    element.SendKeys(text);
                }
                Thread.Sleep(delayMillisecond);
                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public bool SendInput(By by, string text, int delayMillisecond = 0)
        {
            try
            {
                IWebElement element = driver.FindElement(by);
                if (string.IsNullOrEmpty(text) || text == "")
                    element.Clear();
                else
                    element.SendKeys(text);

                Thread.Sleep(delayMillisecond);
                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public bool SendInput(string xpath, string text, int delayMillisecond = 0)
        {
            try
            {
                IWebElement element = driver.FindElement(By.XPath(xpath));
                if (string.IsNullOrEmpty(text) || text == "")
                {
                    element.Clear();
                }
                else
                {
                    element.SendKeys(text);
                }
                Thread.Sleep(delayMillisecond);
                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        #endregion






        #region ComboBox

        public bool SelectComboBoxText(IWebElement element, string text, int delayMillisecond = 0)
        {
            try
            {
                SelectElement select = new SelectElement(element);
                if (select == null)
                {
                    return false;
                }

                select.SelectByText(text);
                Thread.Sleep(delayMillisecond);

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public bool SelectComboBoxText(By by, string text, int delayMillisecond = 0)
        {
            try
            {
                IWebElement element = driver.FindElement(by);
                SelectElement select = new SelectElement(element);
                if (select == null)
                {
                    return false;
                }

                select.SelectByText(text);
                Thread.Sleep(delayMillisecond);

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public bool SelectComboBoxText(string xpath, string text, int delayMillisecond = 0)
        {
            try
            {
                IWebElement element = driver.FindElement(By.XPath(xpath));

                SelectElement select = new SelectElement(element);
                if (select == null)
                {
                    return false;
                }

                select.SelectByText(text);
                Thread.Sleep(delayMillisecond);

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }



        public bool SelectComboBoxIndex(IWebElement element, int index, int delayMillisecond = 0)
        {
            try
            {
                SelectElement select = new SelectElement(element);
                if (select == null)
                {
                    var el = GetParentElement(element);
                    if (el != null)
                        select = new SelectElement(el);
                    else
                        return false;
                }

                select.SelectByIndex(index);
                Thread.Sleep(delayMillisecond);

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }



        public bool SelectComboBoxIndex(By by, int index, int delayMillisecond = 0)
        {
            try
            {
                IWebElement element = driver.FindElement(by);
                SelectElement select = new SelectElement(element);
                if (select == null)
                {
                    var el = GetParentElement(element);
                    if (el != null)
                        select = new SelectElement(el);
                    else
                        return false;
                }

                select.SelectByIndex(index);
                Thread.Sleep(delayMillisecond);

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }


        public bool SelectComboBoxIndex(string xpath, int index, int delayMillisecond = 0)
        {
            try
            {
                IWebElement element = driver.FindElement(By.XPath(xpath));

                SelectElement select = new SelectElement(element);
                if (select == null)
                {
                    var el = GetParentElement(element);
                    if (el != null)
                        select = new SelectElement(el);
                    else
                        return false;
                }

                select.SelectByIndex(index);
                Thread.Sleep(delayMillisecond);

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }


        public bool SelectComboBoxValue(IWebElement element, string value, int delayMillisecond = 0)
        {
            try
            {
                SelectElement select = new SelectElement(element);
                if (select == null)
                {
                    var el = GetParentElement(element);
                    if (el != null)
                        select = new SelectElement(el);
                    else
                        return false;
                }

                select.SelectByValue(value);
                Thread.Sleep(delayMillisecond);

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public bool SelectComboBoxValue(By by, string value, int delayMillisecond = 0)
        {
            try
            {
                IWebElement element = driver.FindElement(by);
                SelectElement select = new SelectElement(element);
                if (select == null)
                {
                    return false;
                }

                select.SelectByValue(value);
                Thread.Sleep(delayMillisecond);

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public bool SelectComboBoxValue(string xpath, string value, int delayMillisecond = 0)
        {
            try
            {
                IWebElement element = driver.FindElement(By.XPath(xpath));

                SelectElement select = new SelectElement(element);
                if (select == null)
                {
                    return false;
                }

                select.SelectByValue(value);
                Thread.Sleep(delayMillisecond);

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }


        public bool SelectComboBox(IWebElement el, int delayMillisecond = 0)
        {
            try
            {
                // 선택할 노드의 Value값
                var value = el.GetAttribute("value");
                if (string.IsNullOrEmpty(value))
                    return false;

                // 선택할 노드의 부모 : 콤보박스 본체
                var parent = GetParentElement(el);
                if (parent == null)
                    return false;

                SelectElement select = new SelectElement(parent);
                if (select == null)
                {
                    return false;
                }

                select.SelectByValue(value);
                Thread.Sleep(delayMillisecond);

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        #endregion








    }
}
