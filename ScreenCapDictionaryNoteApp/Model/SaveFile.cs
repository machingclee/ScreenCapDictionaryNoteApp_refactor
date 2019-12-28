using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapDictionaryNoteApp.Model
{
    public class SaveFile
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PageId { get; set; }
        public string ImageFilePath { get; set; }
        public string RecordRichTextFilePath { get; set; }
        public string CapturedText { get; set; }
        public string FetchedDictionaryResult { get; set; }

    }
}
