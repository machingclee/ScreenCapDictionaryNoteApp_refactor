using ScreenCapDictionaryNoteApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Page = ScreenCapDictionaryNoteApp.Model.Page;

namespace ScreenCapDictionaryNoteApp.ViewModel.Command
{
    public class ScreenCapCommand : ICommand
    {


        public MainVM MainVM { get; set; }

        public ScreenCapCommand(MainVM vm)
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
            Page page = parameter as Page;
            if (page != null)
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
            MainVM.TakeScreenShot();
        }
    }
}
