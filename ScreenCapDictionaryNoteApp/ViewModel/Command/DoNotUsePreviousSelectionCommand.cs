using ImageProcessor.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenCapDictionaryNoteApp.ViewModel.Command
{
    public class DoNotUsePreviousSelectionCommand : ICommand
    {
        public MainVM MainVM { get; set; }
        public DoNotUsePreviousSelectionCommand(MainVM vm)
        {
            MainVM = vm;
        }


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public bool CanExecute(object parameter)
        {
            CropLayer cropLayer = parameter as CropLayer;
            if (cropLayer != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            MainVM.DoNotUsePreviousSelection();
        }
    }
}
