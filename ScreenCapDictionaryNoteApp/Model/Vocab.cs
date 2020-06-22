using ScreenCapDictionaryNoteApp.Interface;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapDictionaryNoteApp.Model
{
    public class Vocab : IWithIdAndName, IHaveVersion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PageId { get; set; }

        public string Name { get; set; }

        public string Word { get; set; }

        public string Pronounciation { get; set; }

        public string Explaination { get; set; }

        public int Version { get; set; }

        public bool IsNewerVersion { get; set; }
    }
}
