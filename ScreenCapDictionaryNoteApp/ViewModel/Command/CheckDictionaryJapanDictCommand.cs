
using ScreenCapDictionaryNoteApp.ViewModel.Helpers;
using ScreenCapDictionaryNoteApp.ViewModel.Helpers.TranslationHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenCapDictionaryNoteApp.ViewModel.Command
{
    public class CheckDictionaryJapanDictCommand : ICommand
    {
        public MainVM MainVM;

        public CheckDictionaryJapanDictCommand(MainVM mainVM)
        {
            this.MainVM = mainVM;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var translator = new Translation();
            translator.SetTranslator(new JapanDict(MainVM));
            translator.Translate();
        }
    }
}
