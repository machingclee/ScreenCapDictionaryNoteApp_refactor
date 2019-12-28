using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenCapDictionaryNoteApp.ViewModel.Command
{
    public class CaptureMoreTextCommand : ICommand
    {

        public MainVM MainVM { get; set; }
        public CaptureMoreTextCommand(MainVM vm)
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
            var imageByte = parameter as string;
            if (imageByte == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            MainVM.CaptureMoreText();
        }
    }
}
