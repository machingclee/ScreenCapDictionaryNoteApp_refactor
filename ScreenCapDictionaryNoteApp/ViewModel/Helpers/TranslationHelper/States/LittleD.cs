using ScreenCapDictionaryNoteApp.enums;
using ScreenCapDictionaryNoteApp.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers.TranslationHelper
{
    public class LittleD : AbstractTranslator
    {
        public LittleD(MainVM mainVM) : base(mainVM)
        {
        }

        protected override string BASE_URL
        {
            get
            {
                return "http://dict.hjenglish.com/jp/jc/";
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
