using ScreenCapDictionaryNoteApp.enums;
using ScreenCapDictionaryNoteApp.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers.TranslationHelper
{
    public abstract class AbstractTranslator : ITranslator
    {
        protected abstract string BASE_URL { get; }

        protected MainVM MainVM;

        public AbstractTranslator(MainVM mainVM)
        {
            this.MainVM = mainVM;
        }

        public abstract void Translate();
    }
}
