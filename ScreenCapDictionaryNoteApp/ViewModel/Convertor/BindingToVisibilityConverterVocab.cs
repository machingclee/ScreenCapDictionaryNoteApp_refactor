﻿using ScreenCapDictionaryNoteApp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ScreenCapDictionaryNoteApp.ViewModel.Convertor
{
    public class BindingToVisibilityConverterVocab : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int currentSelectionId = (int)values[0];
            bool isEditing = (bool)values[1];
            Vocab selectedVocab = values[2] as Vocab;


            if (isEditing == false)
            {
                return Visibility.Collapsed;
            }
            else
            {
                if (selectedVocab == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return currentSelectionId == selectedVocab.Id ? Visibility.Visible : Visibility.Collapsed;
                }
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { null, null, null };
        }
    }
}
