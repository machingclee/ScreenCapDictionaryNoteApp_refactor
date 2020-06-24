using CefSharp;
using ImageProcessor.Imaging;
using Newtonsoft.Json.Linq;
using ScreenCapDictionaryNoteApp.Interface;
using ScreenCapDictionaryNoteApp.Model;
using ScreenCapDictionaryNoteApp.ViewModel;
using ScreenCapDictionaryNoteApp.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static ScreenCapDictionaryNoteApp.ViewModel.Helpers.WebServiceHelper;
using static ScreenCapDictionaryNoteApp.ViewModel.MainVM;
using Application = System.Windows.Application;

namespace ScreenCapDictionaryNoteApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainVM MainVM { get; set; }

        bool LeftButtonIsPressed { get; set; }
        double[] CropStart { get; set; }
        double[] CropEnd { get; set; }

        SolidColorBrush fillStyle = new SolidColorBrush(Color.FromArgb((byte)Math.Floor(0.4 * 255), 255, 0, 0));

        public MainWindow()
        {
            InitializeComponent();

            mainWindow.MouseLeftButtonDown += delegate
            {
                try
                {
                    DragMove();
                }
                catch (Exception err)
                {
                    Debug.WriteLine(err.Message);
                }
            };
            if (Screen.AllScreens.Length < 2)
            {
                Display2Option.IsEnabled = false;
            }




            //MainVM = mainWindow.DataContext as MainVM;
            MainVM = Resources["mainVM"] as MainVM;


            //event 
            MainVM.ScrollToSelectedPagePlz += (sender, arg) => { pagesList.ScrollIntoView(MainVM.SelectedPage); };



            LeftButtonIsPressed = false;
            CropStart = new double[] { 0, 0 };
            CropEnd = new double[] { 0, 0 };
            //browser.Address = "https://www.japandict.com/";
            MainVM.browserAddress = "https://www.japandict.com/";





            //event 
            MainVM.CheckApplicationLyingInWhichDisplay += ExtractLyingDisplayToVM;


            //event
            MainVM.ScreenshotIsTaken += InsertScreenshot;
            MainVM.ScreenshotIsTaken += HandleScreenshot;
            MainVM.ScreenshotIsTaken += RestoreSelectedItemToPrevious;

            //event
            MainVM.NewPageViaScreenshotIsTaken += RestoreSelectedItemToLast;
            MainVM.NewPageViaScreenshotIsTaken += HandleScreenshot;
            MainVM.NewPageViaScreenshotIsTaken += RestoreSelectedItemToLast;

            //event
            MainVM.CropOriginalScreenshot += HandleOriginalScreenshot;
            MainVM.CropOriginalScreenshot += RestoreSelectedItemToPrevious;

            //event
            MainVM.GoogleTranslateComplete += InsertTranslationToRichTextBox;

            //event
            MainVM.CaptureFromScreenshotNow += ScrapMoreFromOriginalScreenshot;



            //event
            MainVM.RenameNoteStarted += (sender, arg) =>
            {
                var listViewItem = notesList.ItemContainerGenerator.ContainerFromItem(MainVM.SelectedNote) as System.Windows.Controls.ListViewItem;
                var textBoxes = FindVisualChildren<System.Windows.Controls.TextBox>(listViewItem);
                textBoxes.FirstOrDefault().Focus();
                foreach (var textBox in textBoxes)
                {
                    textBox.Focus();
                    textBox.SelectAll();
                }
            };



            MainVM.RenamePageIsClicked += (sender, arg) =>
            {

                var listViewItem = pagesList.ItemContainerGenerator.ContainerFromItem(MainVM.SelectedPage) as System.Windows.Controls.ListViewItem;
                var textBox = FindVisualChildren<System.Windows.Controls.TextBox>(listViewItem).FirstOrDefault();

                textBox.Focus();
                textBox.SelectAll();
            };


            MainVM.UpdateVocabIsClicked += (sender, arg) =>
            {
                var listViewItem = jotNotes.ItemContainerGenerator.ContainerFromItem(MainVM.SelectedVocab) as System.Windows.Controls.ListViewItem;
                var textBoxes = FindVisualChildren<System.Windows.Controls.TextBox>(listViewItem);

                if (textBoxes.Count() > 0)
                {
                    textBoxes.FirstOrDefault().Focus();
                    foreach (var textBox in textBoxes)
                    {

                        textBox.SelectAll();
                    }
                }
            };

        }

        private void ExtractLyingDisplayToVM(object sender, EventArgs args)
        {
            var screen = System.Windows
            .Forms
            .Screen
            .FromHandle(
                new System.Windows.Interop.WindowInteropHelper(this).Handle
                );
            MainVM.CurrentDisplay = screen;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }




        private void InsertTranslationToRichTextBox(object sender, EventArgs args)
        {
            detectionContainer.Document.Blocks.Add(new Paragraph(new Run(MainVM.TranslationResult)));
        }















        private void RestoreSelectedItemToLast(object sender, EventArgs args)
        {
            var latestPage = MainVM.Pages.LastOrDefault();
            pagesList.SelectedItem = latestPage;
        }


        private void RestoreSelectedItemToPrevious(object sender, EventArgs args)
        {
            var lastVisited = MainVM.Pages[MainVM.SelectedPageIndex];
            pagesList.SelectedItem = lastVisited;

        }








        private async void HandleScreenshot(object sender, EventArgs args)
        {
            if (MainVM.DoUsePreviousSelection == false)
            {
                var screenshotPopup = new ScreenshotFullscreenPopup(File.ReadAllBytes(MainVM.CurrentScreenshot));
                screenshotPopup.BorderThickness = new Thickness(0, 0, 0, 0);
                System.Drawing.Rectangle workingArea = Screen.AllScreens[MainVM.DisplayIndex].WorkingArea;
                screenshotPopup.Left = workingArea.Left;
                screenshotPopup.SourceInitialized += screenshotPopup_SourceInitialized;

                void screenshotPopup_SourceInitialized(object Sender, EventArgs Args)
                {
                    screenshotPopup.Top = workingArea.Top;
                    screenshotPopup.Width = workingArea.Width;
                    screenshotPopup.Height = workingArea.Height;
                    screenshotPopup.WindowState = WindowState.Maximized;
                    screenshotPopup.WindowStyle = WindowStyle.None;
                    screenshotPopup.Topmost = true;
                }


                screenshotPopup.ShowDialog();
                MainVM.CropLayer = screenshotPopup.CropLayer;
                this.WindowState = WindowState.Normal;
                try
                {
                    var response = await VisionAPIHelper.TextDetectionAsync(BitmapHelper.BitmapImageToBytes(screenshotPopup.CroppedImage));
                    var result = response[0].ToString();
                    var json = JObject.Parse(result);
                    string text = json["description"].ToString();
                    detectionContainer.Document.Blocks.Clear();
                    detectionContainer.Document.Blocks.Add(new Paragraph(new Run(text)));
                    var range = new TextRange(detectionContainer.Document.ContentStart, (detectionContainer.Document.ContentEnd));
                    SaveDectionResult(range);




                }
                catch (Exception err)
                {
                    detectionContainer.Document.Blocks.Clear();
                    detectionContainer.Document.Blocks.Add(new Paragraph(new Run("Please try again")));
                    var range = new TextRange(detectionContainer.Document.ContentStart, (detectionContainer.Document.ContentEnd));
                    SaveDectionResult(range);


                }

            }
            else
            {
                BitmapImage cropImageByPreviousSelection = BitmapHelper.CropImage(File.ReadAllBytes(MainVM.CurrentScreenshot), MainVM.CropLayer);
                try
                {
                    var result = (await VisionAPIHelper.TextDetectionAsync(BitmapHelper.BitmapImageToBytes(cropImageByPreviousSelection)))[0].ToString();
                    var json = JObject.Parse(result);
                    string text = json["description"].ToString();
                    detectionContainer.Document.Blocks.Clear();
                    detectionContainer.Document.Blocks.Add(new Paragraph(new Run(text)));
                    var range = new TextRange(detectionContainer.Document.ContentStart, (detectionContainer.Document.ContentEnd));
                    SaveDectionResult(range);
                }
                catch (Exception err)
                {
                    detectionContainer.Document.Blocks.Clear();
                    detectionContainer.Document.Blocks.Add(new Paragraph(new Run("mo text ar dai gor")));
                }
            }



        }



        private async void ScrapMoreFromOriginalScreenshot(object sender, EventArgs args)
        {
            var screenshotPopup = new ScreenshotFullscreenPopup(File.ReadAllBytes(MainVM.SelectedPage.ScreenshotByteArray));
            screenshotPopup.BorderThickness = new Thickness(0, 0, 0, 0);
            System.Drawing.Rectangle workingArea = Screen.AllScreens[MainVM.DisplayIndex].WorkingArea;
            screenshotPopup.Left = workingArea.Left;
            screenshotPopup.Top = workingArea.Top;
            screenshotPopup.Width = workingArea.Width;
            screenshotPopup.Height = workingArea.Height;
            screenshotPopup.WindowState = WindowState.Maximized;
            screenshotPopup.WindowStyle = WindowStyle.None;
            screenshotPopup.Topmost = true;
            screenshotPopup.ShowDialog();


            try
            {
                var result = (await VisionAPIHelper
                    .TextDetectionAsync(BitmapHelper.BitmapImageToBytes(screenshotPopup.CroppedImage)))[0]
                    .ToString();

                var json = JObject.Parse(result);
                string text = json["description"].ToString();
                detectionContainer.Document.Blocks.Add(new Paragraph(new Run(text)));
                var range = new TextRange(detectionContainer.Document.ContentStart, (detectionContainer.Document.ContentEnd));
                SaveDectionResult(range);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                detectionContainer.Document.Blocks.Add(new Paragraph(new Run("please try again")));
            }




        }


        private void HandleOriginalScreenshot(object sender, EventArgs args)
        {

            var screenshotPopup = new ScreenshotFullscreenPopup(File.ReadAllBytes(MainVM.SelectedPage.ScreenshotByteArray));
            screenshotPopup.BorderThickness = new Thickness(0, 0, 0, 0);
            System.Drawing.Rectangle workingArea = Screen.AllScreens[MainVM.DisplayIndex].WorkingArea;
            screenshotPopup.Left = workingArea.Left;
            screenshotPopup.Top = workingArea.Top;
            screenshotPopup.Width = workingArea.Width;
            screenshotPopup.Height = workingArea.Height;
            screenshotPopup.WindowState = WindowState.Maximized;
            screenshotPopup.WindowStyle = WindowStyle.None;
            screenshotPopup.Topmost = true;
            screenshotPopup.ShowDialog();
            MainVM.CropLayer = screenshotPopup.CropLayer;
            screenshotContainer.Source = null;

            var selectedPage = MainVM.Pages[MainVM.SelectedPageIndex];

            selectedPage.CroppedScreenshotByteArray = BitmapHelper.SaveCroppedBitmapReturnPath(screenshotPopup.CroppedImage, selectedPage);
            selectedPage.Version++;
            selectedPage.IsImgNewerVersion = true;

            DatabaseHelper.Update(selectedPage);
            if (pagesList.Items.Count > 0)
            {

                pagesList.SelectedIndex = MainVM.SelectedPageIndex;


            }

        }











        private void InsertScreenshot(object sender, EventArgs args)
        {



            var bitmap = BitmapHelper.BitmapImageFromPath(MainVM.CurrentScreenshot);
            screenshotContainer.Source = bitmap;

        }

        private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            MainVM.TakeScreenShot();

        }

        private void DetectionContainer_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedText = new TextRange(detectionContainer.Selection.Start, detectionContainer.Selection.End);
            if (!string.IsNullOrEmpty(selectedText.Text))
            {
                MainVM.SelectedTextInDectionContainer = selectedText.Text;
            }


        }

        private void ChromiumWebBrowser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
                 {
                     MainVM.browserAddress = e.Url;

                 }));

        }

        //private void BackButton_Click(object sender, RoutedEventArgs e)
        //{

        //    browser.Back();

        //}

        //private void ForwardButton_Click(object sender, RoutedEventArgs e)
        //{

        //    browser.Forward();

        //}

        private void DetectionContainer_LostFocus(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(detectionContainer.Document.ContentStart,
                   detectionContainer.Document.ContentEnd);
            SaveDectionResult(range);
        }

        private void SaveDectionResult(TextRange range)
        {

            if (MainVM.Pages.Count > 0)
            {
                string RtfString = RtfHelper.GetRtfString(range);
                MainVM.DetectionContainerContent = RtfString;
                MainVM.Pages[MainVM.SelectedPageIndex].DetectionResult = RtfString;
                MainVM.Pages[MainVM.SelectedPageIndex].Version++;
                MainVM.Pages[MainVM.SelectedPageIndex].IsNewerVersion = true;
                DatabaseHelper.Update(MainVM.Pages[MainVM.SelectedPageIndex]);

            }

        }





        /// <summary>
        /// CloseButton_Clicked
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// MaximizedButton_Clicked
        /// </summary>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
            mainWindow.Focus();
        }

        /// <summary>
        /// Minimized Button_Clicked
        /// </summary>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Focus();
            this.WindowState = WindowState.Minimized;

        }

        /// <summary>
        /// Adjusts the WindowSize to correct parameters when Maximize button is clicked
        /// </summary>
        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                mainWindow.Margin = new Thickness(10);

            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                mainWindow.Margin = new Thickness(0);


            }

        }

        private void jotNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Image_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {

        }
    }
}
