using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenCapDictionaryNoteApp.Interface
{
    public interface IObserver
    {
        public void Update(Object obj);
    }
}
