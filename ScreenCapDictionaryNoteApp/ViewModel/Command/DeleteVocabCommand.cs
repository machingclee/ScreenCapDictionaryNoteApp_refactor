using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ScreenCapDictionaryNoteApp.Model;

namespace ScreenCapDictionaryNoteApp.ViewModel.Command
{
    public class DeleteVocabCommand : ICommand
    {
        public MainVM MainVM { get; set; }
        public DeleteVocabCommand(MainVM vm)
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
            var page = parameter as Page;
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
            MainVM.DeleteVocab();
        }
    }
}
