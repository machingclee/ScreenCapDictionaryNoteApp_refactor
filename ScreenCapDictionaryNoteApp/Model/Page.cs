using ScreenCapDictionaryNoteApp.Interface;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ScreenCapDictionaryNoteApp.Model
{
    public class Page : IWithIdAndName, IHaveVersion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }


        private int _NoteId;

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string DateTime { get; set; }


        public int NoteId
        {
            get { return _NoteId; }
            set { _NoteId = value; }
        }

        public string DetectionResult { get; set; }



        private string _ScreenshotByteArray;

        public string ScreenshotByteArray
        {
            get { return _ScreenshotByteArray; }
            set { _ScreenshotByteArray = value; }
        }

        private string _CroppedScreenshotByteArray;

        public string CroppedScreenshotByteArray
        {
            get { return _CroppedScreenshotByteArray; }
            set { _CroppedScreenshotByteArray = value; }
        }

        public int Version { get; set; }

        public bool IsNewerVersion { get; set; }

        public bool IsImgNewerVersion { get; set; }

        public bool IsSyncToS3 { get; set; }
    }
}
