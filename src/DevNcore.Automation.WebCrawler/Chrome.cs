using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevNcore.Automation.WebCrawler
{
    public class Chrome
    {
        private ChromeDriver driver { get; set; }
        private ChromeDriverManager manager = new ChromeDriverManager();

        public bool Start()
        {
            if (driver != null)
                return false;
            
            // 크롬드라이버 최신버전 확인 및 다운로드
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string driverPath = $"{folder}\\chromedriver.exe";
            if (manager.SetUp(driverPath) == false)
                return false;

            // 크롬 콘솔화면을 숨긴다.
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            // 크롬 드라이버 옵션 설정
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--lang=ko_KR");
            options.AddArgument("--no-sandbox");
            // 자동화된 크롬에서 동작중입니다 문구 제거
            options.AddExcludedArgument("enable-automation");

            driver = new ChromeDriver(chromeDriverService, options, TimeSpan.FromSeconds(120));
            return true;
        }

        public bool Url(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            if(!url.StartsWith("https://") && !url.StartsWith("http://") && !url.StartsWith("file:///"))
            {
                return false;
            }
            
            driver.Navigate().GoToUrl(url);
            return true;
        }


        public void Exit()
        {
            if (driver == null)
                return;

            driver.Quit();
        }



    }
}
