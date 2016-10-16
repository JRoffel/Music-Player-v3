using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIYX.Music.Youtube
{
    class MainController
    {
        YoutubeController youtubeController = new YoutubeController();

        public string DownloadYoutubeAudio(string link)
        {
            return youtubeController.DownloadYoutubeAudio(link);
        }
    }
}