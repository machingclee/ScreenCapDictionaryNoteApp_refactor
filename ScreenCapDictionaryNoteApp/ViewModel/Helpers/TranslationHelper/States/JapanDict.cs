using ScreenCapDictionaryNoteApp.enums;
using ScreenCapDictionaryNoteApp.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers.TranslationHelper
{
    public class JapanDict : AbstractTranslator
    {
        public JapanDict(MainVM mainVM) : base(mainVM)
        {
        }

        protected override string BASE_URL
        {
            get
            {
                return "https://www.japandict.com/";
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
