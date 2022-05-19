using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevNcore.Automation.WebCrawler
{
    public partial class Chrome
    {
        private ChromeDriver driver { get; set; } 

        /// <summary>
        /// 크롬 드라이버 매니저 (최신버전 체크용)
        /// </summary>
        private ChromeDriverManager manager { get; set; } = new ChromeDriverManager();

        /// <summary>
        /// 크롬 설정 (생성시 각종 설정값 적용)
        /// </summary>
        private ChromeDriverSetting setting { get; set; }

        /// <summary>
        /// 크롬 프로세스 (윈도우 제어용)
        /// </summary>
        private Process mainProcess { get; set; }


        public Chrome(ChromeDriverSetting setting = null)
        {
            this.setting = setting;
            Initialize();
        }


        private void Initialize()
        {
            ChromeOptions options = new ChromeOptions();

            if (setting == null)
                setting = new ChromeDriverSetting();

            // 크롬드라이버 최신버전 확인 및 다운로드
            if (setting.updateLatestVersion)
            {
                string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string driverPath = $"{folder}\\chromedriver.exe";
                if (manager.SetUp(driverPath) == false)
                {
                    throw new FileNotFoundException("chromedriver.exe를 업데이트 하지 못했습니다.");
                }
            }

            // 언어 설정
            if (!string.IsNullOrEmpty(setting.lang))
                options.AddArgument($"--lang={setting.lang}");

            // 숨김모드
            if(setting.headless)
                options.AddArgument("--headless");

            // 자동화 문구 제거
            if (setting.disableAutoMessage)
                options.AddExcludedArgument("enable-automation");

            // 이미지 표시 안함
            if (setting.disableImage)
                options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);

            // 창크기 설정
            if (setting.size != null)
                options.AddArgument($"--window-size={setting.size.Value.Width},{setting.size.Value.Height}");

            // 샌드박스 사용안함
            if (setting.disableSandbox)
                options.AddArgument("--no-sandbox");

            // 크롬 프로필 경로 사용시 
            if (!string.IsNullOrEmpty(setting.userDataDir))
                options.AddArgument($"user-data-dir={setting.userDataDir}");

            // 스트림 사용시
            if(setting.enableMediaStream)
                options.AddArgument("use-fake-ui-for-media-stream");

            // 크롬 콘솔창 설정
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = setting.hideCommandPromptWindow;

           

            // 크롬 드라이버 생성
            driver = new ChromeDriver(chromeDriverService, options, TimeSpan.FromSeconds(setting.commandTimeoutDelay));

            // 크롬 제어를 위해 윈도우 핸들을 찾는다.
            GetMainProcess();
        }




        /// <summary>
        /// driver.CurrentWindowHandle을 이용해 타이틀명을 바꾼후 프로세스 검색을 통해
        /// 해당 크롬의 Process를 얻는다.
        /// </summary>
        private void GetMainProcess(int findHandleWaitDelaySecond = 20)
        {
            try
            {
                string old_title = driver.Title;

                mainProcess = null;

                string guid = driver.CurrentWindowHandle;
                driver.ExecuteScript($"document.title = '{guid}'");

                Stopwatch watch = new Stopwatch();
                watch.Start();
                while (true)
                {
                    if (watch.Elapsed.TotalSeconds >= findHandleWaitDelaySecond)
                    {
                        break;
                    }

                    var processes = Process.GetProcessesByName("chrome").FirstOrDefault(x => x.MainWindowHandle.Equals(IntPtr.Zero) == false
                                && x.MainWindowTitle.Contains(guid));
                    if (processes != null)
                    {
                        mainProcess = processes;
                        break;
                    }

                    if (findHandleWaitDelaySecond == 0)
                        break;

                    Thread.Sleep(100);
                }

                driver.ExecuteScript($"document.title = '{old_title}'");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }
        }








        public void Exit()
        {
            if (driver == null)
                return;

            driver.Quit();
        }














        ~Chrome()
        {
            try
            {
                if (driver != null)
                {
                    driver.Quit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }
        }


    }
}
