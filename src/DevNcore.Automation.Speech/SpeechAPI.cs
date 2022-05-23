using PuppeteerSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DevNcore.Automation.Speech
{
    public class SpeechAPI
    {
        Browser browser { get; set; }
        Page page { get; set; }
        string baseURL { get; set; } = "https://www.bing.com";

        public SpeechAPI(bool ClearAllChromium = true)
        {            
            if(ClearAllChromium)
                ClearAllProcess();
        }


        /// <summary>
        /// 음성인식을 하기 위한 초기화 작업 진행
        /// </summary>
        /// <returns></returns>
        public async Task Initialize()
        {
            // 크로미움 설치 확인
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            browser = await Puppeteer.LaunchAsync(
                new LaunchOptions {
                    // 숨김모드
                    Headless = true, 
                    // 마이크 허용
                    Args = new[] {  "--use-fake-ui-for-media-stream", },
                    // headless 모드에서 음소거하지 않게
                    IgnoredDefaultArgs = new[] { "--mute-audio" },
            });

            page = await browser.NewPageAsync();
            await page.GoToAsync(baseURL);
        }


        public async Task<string> Run()
        {
            string result = null;

            string oldTitle = "";
            string step = "";

            Stopwatch w = new Stopwatch();
            w.Start();

            while (true)
            {
                Thread.Sleep(100);

                // 10초 대기 제한
                if (w.Elapsed.TotalSeconds >= 10)
                {
                    break;
                }

                try
                {
                    if (step == "")
                    {
                        oldTitle = await page.EvaluateFunctionAsync<string>("()=> document.querySelector('head > title').text");
                        oldTitle = oldTitle.ToString().Replace(" - 검색", "");

                        var el = await page.XPathAsync("//*[@id='sb_form']/div[1]/div");
                        if(el != null)
                        {
                            // 음성인식 열기
                            await el[0].ClickAsync();
                            step = "wait";
                        }
                    }
                    else if (step == "wait")
                    {
                        string title = await page.EvaluateFunctionAsync<string>("()=> document.querySelector('head > title').text");
                        title = title.Replace(" - 검색", "");

                        // 음성인식 완료시 루프종료
                        if (title != oldTitle)
                        {                            
                            result = title;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            // 초기화 페이지로 이동해둠
            await page.GoToAsync(baseURL);

            return result;

        }

        public async void Close()
        {
            await page.CloseAsync();
            await browser.CloseAsync();

            page = null;
            browser = null;
        }

        /// <summary>
        /// 모든 크로니움 프로세스 닫기
        /// </summary>
        private void ClearAllProcess()
        {
            try
            {                
                var list = Process.GetProcessesByName("chrome");
                if (list != null)
                {
                    foreach (var row in list)
                    {
                        Process proc = row;
                        if(proc.MainModule != null)
                        {
                            string folder = Path.GetDirectoryName(proc.MainModule.FileName);
                            string checkFile = $"{folder}\\chrome_pwa_launcher.exe";
                            if(File.Exists(checkFile))
                            {
                                proc.Kill();
                            }
                        }                        
                    }
                }

                page = null;
                browser = null;
            }
            catch
            {
            }
        }






        ~SpeechAPI()
        {
            if(page != null)
                page.CloseAsync().ConfigureAwait(false);

            if(browser != null)
                browser.CloseAsync().ConfigureAwait(false);
        }
    }
}
