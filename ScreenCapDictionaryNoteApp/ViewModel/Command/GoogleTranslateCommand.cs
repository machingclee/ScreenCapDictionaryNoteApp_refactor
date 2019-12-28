using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenCapDictionaryNoteApp.ViewModel.Command
{
    public class GoogleTranslateCommand : ICommand
    {

        public MainVM MainVM { get; set; }
        public GoogleTranslateCommand(MainVM vm)
        {
            MainVM = vm;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;

        }

        public void Execute(object parameter)
        {
            MainVM.GoogleTranslate();
        }
    }
}
