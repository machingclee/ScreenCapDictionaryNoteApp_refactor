using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers
{
    public class RtfHelper
    {
        public static string GetRtfString(TextRange range)
        {
            string rtfString;
            using (MemoryStream ms = new MemoryStream())
            {
                range.Save(ms, System.Windows.DataFormats.Rtf);
                rtfString = Encoding.UTF8.GetString(ms.ToArray());
            }
            return rtfString;
        }
    }
}
