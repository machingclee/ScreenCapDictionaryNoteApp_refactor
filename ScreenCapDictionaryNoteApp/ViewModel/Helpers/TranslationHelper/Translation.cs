using ScreenCapDictionaryNoteApp.ViewModel.Helpers.TranslationHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers
{
    public class Translation
    {
        private ITranslator CurrentTranslator;


        public void SetTranslator(ITranslator translator)
        {
            this.CurrentTranslator = translator;
        }


        public void Translate()
        {
            CurrentTranslator.Translate();
        }
    }
}
