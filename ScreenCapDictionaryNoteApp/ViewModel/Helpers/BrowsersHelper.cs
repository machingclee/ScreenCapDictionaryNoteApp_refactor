using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers
{
    public class BrowsersHelper
    {
        public BrowsersHelper() { }

        public static void Browse(string URL)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = URL,
                UseShellExecute = true
            };

            Process.Start(psi);
        }
    }
}
