using ScreenCapDictionaryNoteApp.Interface;
using ScreenCapDictionaryNoteApp.ViewModel.Helpers;
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
using static ScreenCapDictionaryNoteApp.ViewModel.Helpers.WebServiceHelper;

namespace ScreenCapDictionaryNoteApp.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Notification : Window, IObserver
    {

        public Notification(string shortMessage)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;


            MessageTextBlock.Text = shortMessage;

            WebServiceHelper.UpdateMessageBoxHandler += UpdateMessageBox;




            this.Title = "Message";
            this.SizeToContent = SizeToContent.Height;
        }



        private void UpdateMessageBox(Object sender, EventArgs messageBoxEventArgs)
        {
            var message = (messageBoxEventArgs as MessageBoxEventArgs).Message;
            Update(message);
        }


        public void OpenMessageBox()
        {
            this.Show();
        }





        private void OkButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Update(Object msg)
        {

            MessageTextBlock.Text += "\n" + (msg as string);
        }
    }
}
