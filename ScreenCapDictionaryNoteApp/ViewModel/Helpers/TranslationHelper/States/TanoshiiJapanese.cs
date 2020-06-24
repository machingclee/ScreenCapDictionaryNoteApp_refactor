using ScreenCapDictionaryNoteApp.enums;
using ScreenCapDictionaryNoteApp.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers.TranslationHelper
{
    public class TanoshiiJapanese : AbstractTranslator
    {
        public TanoshiiJapanese(MainVM mainVM) : base(mainVM)
        {
        }

        protected override string BASE_URL
        {
            get
            {
                return "https://www.tanoshiijapanese.com/dictionary/index.cfm?j=";
            }
        }


        override public void Translate()
        {
            var selection = MainVM.SelectedTextInDectionContainer;
            string url = BASE_URL + HttpUtility.UrlEncode(selection);
            BrowsersHelper.Browse(url);
        }
    }
}
