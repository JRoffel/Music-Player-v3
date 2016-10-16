using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DIYX.Music.Youtube
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainController mainController = new MainController();
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private bool shouldUpdateSlider = true;
        private string youtubeUrl;

        public MainWindow()
        {
            InitializeComponent();
            sldrVolume.Value = mediaPlayer.Volume;
        }

        private void btnOpenAudioFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Music files (*.mp3, *.wav)|*.mp3;*.wav|All files (*.*)|*.*";

            if(openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Open(new Uri(openFileDialog.FileName));
            }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (shouldUpdateSlider == true)
            {
                if ((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan))
                {
                    lblStatus.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                    lblStatus.Foreground = new SolidColorBrush(Colors.White);
                    sldrProgress.Minimum = 0;
                    sldrProgress.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    sldrProgress.Value = mediaPlayer.Position.TotalSeconds;
                }
                else
                {
                    lblStatus.Content = "Not Playing...";
                    lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mediaPlayer.Volume += (e.Delta > 0) ? 0.05 : -0.05;
            sldrVolume.Value = mediaPlayer.Volume;
        }

        private void sldrProgress_DragStarted(object sender, RoutedEventArgs e)
        {
            shouldUpdateSlider = false;
        }

        private void sldrProgress_DragCompleted(object sender, RoutedEventArgs e)
        {
            shouldUpdateSlider = true;
            mediaPlayer.Position = TimeSpan.FromSeconds(sldrProgress.Value);
        }

        private void btnOpenYoutubeDialog_Click(object sender, RoutedEventArgs e)
        {
            YoutubeDialog youtubeDialog = new YoutubeDialog();

            if(youtubeDialog.ShowDialog() == true)
            {
                youtubeUrl = youtubeDialog.ResponseText;
                mainController.DownloadYoutubeAudio(youtubeUrl);
            }
        }
    }
}
