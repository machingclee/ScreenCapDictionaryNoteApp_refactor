﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenCapDictionaryNoteApp.ViewModel.Command
{
    public class CheckDictionaryLittleDCommand : ICommand
    {
        public MainVM MainVM { get; set; }
        public CheckDictionaryLittleDCommand(MainVM vm)
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
            return true;
        }

        public void Execute(object parameter)
        {
            MainVM.CheckViaDictionary_littleD();
        }
    }
}
