using ScreenCapDictionaryNoteApp.ViewModel.Helpers;
using ScreenCapDictionaryNoteApp.ViewModel.Helpers.TranslationHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ScreenCapDictionaryNoteApp.ViewModel.Command
{
    public class CheckDictionaryTanoshiiJapaneseCommand : ICommand
    {
        public MainVM MainVM { get; set; }

        public CheckDictionaryTanoshiiJapaneseCommand(MainVM mainVM)
        {
            this.MainVM = mainVM;
        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var translator = new Translation();
            translator.SetTranslator(new TanoshiiJapanese(MainVM));
            translator.Translate();
        }
    }
}
