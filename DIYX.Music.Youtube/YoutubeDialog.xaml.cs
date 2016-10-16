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
using System.Windows.Shapes;

namespace DIYX.Music.Youtube
{
    /// <summary>
    /// Interaction logic for YoutubeDialog.xaml
    /// </summary>
    public partial class YoutubeDialog : Window
    {
        public string ResponseText
        {
            get { return boxYoutubeUrl.Text; }
            set { boxYoutubeUrl.Text = value; }
        }

        public YoutubeDialog()
        {
            InitializeComponent();
        }

        private void btnYoutubeConfirm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
