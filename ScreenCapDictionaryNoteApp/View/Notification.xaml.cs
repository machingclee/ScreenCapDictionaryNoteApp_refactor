using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Notification : Window
    {
        public string ShortMessage { get; set; }


        public Notification(string shortMessage)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            ShortMessage = shortMessage;
            MessageTextBlock.Text = ShortMessage;

            this.Title = "Message";
            this.SizeToContent = SizeToContent.Height;
        }

        private void OkButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
