using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenCapDictionaryNoteApp.ViewModel.Command
{
    public class SyncVocabsCommand : ICommand
    {
        private MainVM mainVM { get; set; }
        public SyncVocabsCommand(MainVM vm)
        {
            mainVM = vm;
        }



        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mainVM.SyncToWebServer();
        }
    }
}
