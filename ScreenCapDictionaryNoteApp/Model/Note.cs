using ScreenCapDictionaryNoteApp.Interface;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapDictionaryNoteApp.Model
{
    public class Note : IWithIdAndName, IHaveVersion
    {

        private int _Id;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string DateTime { get; set; }

        public int Version { get; set; }

        public bool IsNewerVersion { get; set; }

        public int? LastViewedPageIndex { get; set; }
    }
}
