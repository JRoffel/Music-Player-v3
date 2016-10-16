using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YoutubeExtractor;

namespace DIYX.Music.Youtube
{
    public class YoutubeController
    {
        public static double downloadProgress = 0;
        public string pathToFile;
        public string DownloadYoutubeAudio(string link)
        {
            bool isCustomStrip = false;

            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);

            VideoInfo video = videoInfos.Where(info => info.CanExtractAudio).OrderByDescending(info => info.AudioBitrate).FirstOrDefault();

            if(video == null)
            {
                MessageBox.Show("Failed to normally extract audio, custom extract now");
                video = videoInfos.Where(info => info.VideoType == VideoType.Mp4).OrderByDescending(info => info.AudioBitrate).FirstOrDefault();
                isCustomStrip = true;

                if (video == null)
                {
                    MessageBox.Show("Unable to download video and/or to extract audio", "Error parsing video", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }

            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            if(isCustomStrip == false)
            {
                pathToFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), video.Title + video.AudioExtension);
                var audioDownloader = new AudioDownloader(video, pathToFile);

                audioDownloader.Execute();
            }
            else
            {
                pathToFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), video.Title + ".mp4");
                var videoDownloader = new VideoDownloader(video, pathToFile);

                videoDownloader.Execute();

                pathToFile = ConvertToAudio(pathToFile, video.Title);
            }

            MessageBox.Show("Download Finished!");

            return pathToFile;
        }

        private string ConvertToAudio(string videoPath, string title)
        {
            var ffmpeg = new Process
            {
                StartInfo = { UseShellExecute = false, RedirectStandardError = true, FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"\ffmpeg\bin\ffmpeg.exe") }
            };

            string audioPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), title + ".mp3");

            var arguments = String.Format(@"-i ""{0}"" -c:a mp3 ""{1}""", videoPath, audioPath);

            ffmpeg.StartInfo.Arguments = arguments;

            try
            {
                if (!ffmpeg.Start())
                {
                    Debug.WriteLine("Error Starting");
                    MessageBox.Show("Error Starting ffmpeg!");
                    return null;
                }
                var reader = ffmpeg.StandardError;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Debug.WriteLine(line);
                }
            }
            catch(Exception exception)
            {
                Debug.WriteLine(exception.ToString());
                return null;
            }

            ffmpeg.Close();

            return audioPath;
        }
    }
}
