using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNcore.Automation.WebCrawler
{
    public class ChromeDriverSetting
    {
        /// <summary>
        /// 언어 설정
        /// </summary>
        public string lang { get; set; } = "ko_KR";


        /// <summary>
        /// 크롬 드라이버 생성전 최신버전으로 업데이트
        /// </summary>
        public bool updateLatestVersion { get; set; } = true;


        /// <summary>
        /// 크롬 콘솔창 숨기기
        /// </summary>
        public bool hideCommandPromptWindow { get; set; } = true;

        /// <summary>
        /// 명령 대기 시간 (초단위)
        /// </summary>
        public int commandTimeoutDelay { get; set; } = 120;

        /// <summary>
        /// 샌드박스 사용안함
        /// </summary>
        public bool disableSandbox { get; set; } = true;

        /// <summary>
        /// 자동화된 크롬에서 동작중입니다 문구 제거
        /// </summary>
        public bool disableAutoMessage { get; set; } = true;

        /// <summary>
        /// 이미지를 표시하지 않음
        /// </summary>
        public bool disableImage { get; set; }

        /// <summary>
        /// 창크기
        /// </summary>
        public Size? size { get; set; } = new Size(1920, 1080);

        /// <summary>
        /// 숨김모드
        /// </summary>
        public bool headless { get; set; }

        /// <summary>
        /// 창위치
        /// </summary>
        public Point? position { get; set; }

        /// <summary>
        /// 최대화 여부
        /// </summary>
        public bool maximized { get; set; }

        /// <summary>
        /// 크롬 프로파일 경로
        /// 기본경로 => $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Google\\Chrome\\User Data";        
        /// </summary>
        public string? userDataDir { get; set; }


        /// <summary>
        /// media stream 사용
        /// </summary>
        public bool enableMediaStream { get; set; }
    }
}
