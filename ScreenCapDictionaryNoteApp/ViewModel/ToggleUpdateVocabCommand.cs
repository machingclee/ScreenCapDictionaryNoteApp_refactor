using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenCapDictionaryNoteApp.ViewModel
{
    public class ToggleUpdateVocabCommand : ICommand
    {

        public MainVM MainVM { get; set; }
        public ToggleUpdateVocabCommand(MainVM vm)
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
            var selectedVocab = parameter as Model.Vocab;
            if (selectedVocab == null)
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
            MainVM.toggleUpdateVocabCommand();
        }
    }
}
