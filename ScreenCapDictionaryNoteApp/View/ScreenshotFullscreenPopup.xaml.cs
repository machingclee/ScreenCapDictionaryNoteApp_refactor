using ImageProcessor.Imaging;
using ScreenCapDictionaryNoteApp.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ScreenCapDictionaryNoteApp.View
{
    /// <summary>
    /// Interaction logic for ScreenshotFullscreenPopup.xaml
    /// </summary>


    public partial class ScreenshotFullscreenPopup : Window
    {
        bool leftButtonIsPressed { get; set; }
        double[] cropStart { get; set; }
        double[] cropEnd { get; set; }
        byte[] screenshotBytes { get; set; }
        public BitmapImage CroppedImage { get; set; }
        public CropLayer CropLayer { get; set; }

        SolidColorBrush fillStyle = new SolidColorBrush(Color.FromArgb((byte)Math.Floor(0.4 * 255), 255, 0, 0));

        public ScreenshotFullscreenPopup(byte[] screenshot_bytes)
        {
            InitializeComponent();

            leftButtonIsPressed = false;
            cropStart = new double[] { 0, 0 };
            cropEnd = new double[] { 0, 0 };
            screenshotBytes = screenshot_bytes;
            using (MemoryStream stream = new MemoryStream(screenshot_bytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                fullscreenScreenshot.Source = bitmapImage;
            }

        }




        private void FullscreenScreenshot_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (leftButtonIsPressed == false)
            {
                leftButtonIsPressed = true;

                cropStart[0] = e.GetPosition(fullscreenScreenshot).X;
                cropStart[1] = e.GetPosition(fullscreenScreenshot).Y;

            }

        }


        private void FullscreenScreenshot_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftButtonIsPressed == true)
            {
                cropEnd[0] = e.GetPosition(fullscreenScreenshot).X;
                cropEnd[1] = e.GetPosition(fullscreenScreenshot).Y;

                recSelection.Margin = new Thickness(0, 0, 0, 0);
                recSelection.Width = Math.Abs(cropEnd[0] - cropStart[0]);
                recSelection.Height = Math.Abs(cropEnd[1] - cropStart[1]);
                recSelection.Fill = this.fillStyle;

                if (recSelection.Visibility != Visibility.Visible)
                { recSelection.Visibility = Visibility.Visible; }

                Canvas.SetTop(recSelection, Math.Min(cropEnd[1], cropStart[1]));
                Canvas.SetLeft(recSelection, Math.Min(cropEnd[0], cropStart[0]));
            }
        }



        //public delegate void ImageCroppedHandler(BitmapImage bitmapImage);
        //public event ImageCroppedHandler ImageCropped;


        public event Action<BitmapImage> imageCroppedHandler;


        public void FullscreenScreenshot_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (leftButtonIsPressed == true) leftButtonIsPressed = false;
            // (left, top, right, bottom, cropmode)
            CropLayer cropLayer = new CropLayer(
                Math.Min((float)cropStart[0], (float)cropEnd[0]),
                Math.Min((float)cropStart[1], (float)cropEnd[1]),
                (float)Math.Abs(cropEnd[0] - cropStart[0]),
                (float)Math.Abs(cropEnd[1] - cropStart[1]),
                CropMode.Pixels
            );

            CropLayer = cropLayer;

            CroppedImage = BitmapHelper.CropImage(screenshotBytes, cropLayer);
            this.Close();
            //if (ImageCropped != null)
            //{
            //    ImageCropped(result);
            //    this.Close();
            //}



        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
    }
}


