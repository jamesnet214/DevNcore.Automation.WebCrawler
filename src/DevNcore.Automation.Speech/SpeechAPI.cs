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

        public SpeechAPI(bool ClearAllProcess = true)
        {            
            if(ClearAllProcess)
                CloseAllProcess();
        }
        
        public async Task Initialize()
        {
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            browser = await Puppeteer.LaunchAsync(
                new LaunchOptions {
                    Headless = true, 
                    // 마이크 허용
                    Args = new[] 
                    { 
                        "--use-fake-ui-for-media-stream",
                    },
                    // headless 모드에서 음소거하지 않게
                    IgnoredDefaultArgs = new[] {
                        "--mute-audio",
                    },
            });

            page = await browser.NewPageAsync();
            await page.GoToAsync("https://www.bing.com");
        }


        public async Task<string> Run()
        {
            string oldTitle = "";
            string step = "";
            Stopwatch w = new Stopwatch();
            w.Start();

            while (true)
            {
                Thread.Sleep(100);

                if (w.Elapsed.TotalSeconds >= 10)
                {
                    await page.GoToAsync("https://www.bing.com");
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
                            // 음성열기
                            await el[0].ClickAsync();
                            step = "wait";
                        }
                    }
                    else if (step == "wait")
                    {
                        string title = await page.EvaluateFunctionAsync<string>("()=> document.querySelector('head > title').text");
                        title = title.ToString().Replace(" - 검색", "");
                        // 닫기 버튼
                        if (title != oldTitle)
                        {
                            await page.GoToAsync("https://www.bing.com");
                            return title;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return null;

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
        private void CloseAllProcess()
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
