using ScreenCapDictionaryNoteApp.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ScreenCapDictionaryNoteApp.ViewModel.Convertor
{
    public class ByteArrayToBitmapConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedPage = value as Model.Page;
            if (selectedPage == null)
            {
                return null;
            }
            else
            {
                if (selectedPage.ScreenshotByteArray == null)
                {

                    return null;
                }
                else
                {
                    if (selectedPage.CroppedScreenshotByteArray != null)
                    {
                        return BitmapHelper.BitmapImageFromPath(selectedPage.CroppedScreenshotByteArray);
                    }
                    else
                    {
                        return BitmapHelper.BitmapImageFromPath(selectedPage.ScreenshotByteArray);
                    }

                }
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
