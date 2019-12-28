using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenCapDictionaryNoteApp.ViewModel.Command
{
    public class CropOriginalScreenshotCommand : ICommand
    {

        public MainVM MainVM { get; set; }
        public CropOriginalScreenshotCommand(MainVM vm)
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
            var selectedPage = parameter as Model.Page;
            if (selectedPage == null)
            {
                return false;
            }
            else
            {
                if (selectedPage.ScreenshotByteArray == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void Execute(object parameter)
        {
            MainVM.CropOriginal();
        }
    }
}
