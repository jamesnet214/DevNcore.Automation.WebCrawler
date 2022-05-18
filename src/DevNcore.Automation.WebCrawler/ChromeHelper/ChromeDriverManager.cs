using Ressy;
using Ressy.HighLevel.Versions;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;

namespace DevNcore.Automation.WebCrawler
{
    public class ChromeDriverManager
    {
        private const string LatestReleaseVersionUrl = "https://chromedriver.storage.googleapis.com/LATEST_RELEASE";

        /// <summary>
        /// chromedriver.exe 파일이 최신버전과 같은지 확인하고 아니면 최신버전을 다운로드 받는다.
        /// Make sure that the chromedriver.exe file is the same as the latest version, or download the latest version.
        /// </summary>
        /// <param name="currentChromedriverPath">기존 드라이버 경로</param>
        /// <returns></returns>
        public bool SetUp(string currentDriverPath)
        {
            bool result = false;
            string lastVersion = GetLatestVersion();
            string fileVersion = "";

            if (File.Exists(currentDriverPath))
            {
                fileVersion = FileVersionInfo.GetVersionInfo(currentDriverPath).FileVersion;
            }

            // 파일버전이 다르면 최신버전을 다운받는다.
            // If the file version is different, download the latest version
            if (lastVersion != fileVersion)
            {
                string url = $"https://chromedriver.storage.googleapis.com/{lastVersion}/chromedriver_win32.zip";
                string tempZipPath = GetZipDestination(url);
                if (InstallDriver(url, tempZipPath, currentDriverPath))
                {
                    // chromedriver.exe 파일에 드라이버 버전을 입력한다.
                    // 여기서 버전을 입력하지 않으면 매번 다운받으려고 하는 문제가 있기 때문에
                    // Type the driver version in the chromedriver.exe file.
                    // If you don't enter the version here, there's a problem that you always try to download
                    var portableExecutable = new PortableExecutable(currentDriverPath);
                    portableExecutable.SetVersionInfo(v => v.SetFileVersion(new Version(lastVersion)));
                    result = true;
                }
            }
            else
            {
                result = true;
            }

            return result;
        }


        /// <summary>
        /// 크롬드라이버 최신버전정보를 얻는다.
        /// Get the latest version information of the chrome driver
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private string GetLatestVersion()
        {
            var uri = new Uri(LatestReleaseVersionUrl);
            var webRequest = WebRequest.Create(LatestReleaseVersionUrl);
            using (var response = webRequest.GetResponse())
            {
                using (var content = response.GetResponseStream())
                {
                    if (content == null)
                        throw new ArgumentNullException($"Can't get content from URL: {uri}");

                    using (var reader = new StreamReader(content))
                    {
                        var version = reader.ReadToEnd().Trim();
                        return version;
                    }
                }
            }
        }


        #region InstallDriver
        /// <summary>
        /// 최신 드라이버를 다운받고 압축을 푼 후 대상경로에 복사한다.
        /// Download and extract the latest driver and copy it to the destination path
        /// </summary>
        /// <param name="url">Download Url</param>
        /// <param name="zipPath">Download ZipPath</param>
        /// <param name="binaryPath">Unzip exePath</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private bool InstallDriver(string url, string zipPath, string binaryPath)
        {
            // 파일이 이미 있는경우 기존 파일을 삭제하고 받는다.
            // If the file already exists, delete the existing file and receive it
            if (File.Exists(binaryPath))
            {
                try
                {
                    File.Delete(binaryPath);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
                    return false;
                }
            }

            var zipDir = Path.GetDirectoryName(zipPath);
            var binaryName = Path.GetFileName(binaryPath);

            // 드라이버를 다운로드 받는다.
            // Download the driver
            Directory.CreateDirectory(zipDir);
            zipPath = DownloadZip(url, zipPath);

            // 설정된 경로에 압축을 푼다.
            // Extract to the set path
            var stagingDir = Path.Combine(zipDir, "staging");
            var stagingPath = Path.Combine(stagingDir, binaryName);

            Directory.CreateDirectory(stagingDir);

            if (zipPath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                File.Copy(zipPath, stagingPath);
            }
            else if (zipPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                UnZip(zipPath, stagingPath, binaryName);
            }

            var binaryDir = Path.GetDirectoryName(binaryPath);

            // 대상 디렉터리가 없는 경우 생성
            // Create the destination directory if it doesn't exist
            if (!Directory.Exists(binaryDir))
            {
                Directory.CreateDirectory(binaryDir);
            }


            Exception renameException = null;
            try
            {
                string[] files = Directory.GetFiles(stagingDir);

                // 파일을 복사하고 대상 파일이 이미 있는 경우 덮어쓰기
                // Copy the files and overwrite destination files if they already exist.
                foreach (string file in files)
                {
                    // 정적 경로 방법을 사용하여 경로에서 파일 이름만 추출
                    // Use static Path methods to extract only the file name from the path.
                    var fileName = Path.GetFileName(file);
                    var destFile = Path.Combine(binaryDir, fileName);
                    File.Copy(file, destFile, true);
                }
            }
            catch (Exception ex)
            {
                renameException = ex;
            }

            // 작업이 완료되면 필요없는 파일을 삭제한다
            // Delete unnecessary files when the operation is complete            
            try
            {
                if (Directory.Exists(stagingDir))
                    Directory.Delete(stagingDir, true);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
            try
            {
                RemoveZip(zipPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }

            // 대상이 여전히 존재하지 않으면 예기치 않은 방식으로 이름 변경이 실패했음을 의미
            // If the destination still doesn't exist, it means the rename failed in an unexpected way
            if (!Directory.Exists(binaryDir))
            {
                throw new Exception($"Error writing {binaryDir}", renameException);
            }

            return true;
        }


        private string GetZipDestination(string url)
        {
            var tempDirectory = Path.GetTempPath();
            var guid = Guid.NewGuid().ToString();
            var zipName = Path.GetFileName(url);
            if (zipName == null) throw new ArgumentNullException($"Can't get zip name from URL: {url}");
            return Path.Combine(tempDirectory, guid, zipName);
        }

        private string DownloadZip(string url, string destination)
        {
            if (File.Exists(destination)) return destination;
            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(new Uri(url), destination);
            }

            return destination;
        }

        private string UnZip(string path, string destination, string name)
        {
            var zipName = Path.GetFileName(path);
            if (zipName != null && zipName.Equals(name, StringComparison.CurrentCultureIgnoreCase))
            {
                File.Copy(path, destination);
                return destination;
            }

            using (var zip = ZipFile.Open(path, ZipArchiveMode.Read))
            {
                foreach (var entry in zip.Entries)
                {
                    if (entry.Name == name)
                    {
                        entry.ExtractToFile(destination, true);
                    }
                }
            }

            return destination;
        }

        private void RemoveZip(string path)
        {
            File.Delete(path);
        }

        #endregion

    }
}
