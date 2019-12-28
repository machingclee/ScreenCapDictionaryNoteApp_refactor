using ImageProcessor.Imaging;
using ScreenCapDictionaryNoteApp.Model;
using ScreenCapDictionaryNoteApp.ViewModel.Command;
using ScreenCapDictionaryNoteApp.ViewModel.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ScreenCapDictionaryNoteApp.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {


        private ObservableCollection<Note> _Notes;

        public ObservableCollection<Note> Notes
        {
            get { return _Notes; }
            set
            {
                _Notes = value;
                onPropertyChanged("Notes");
            }
        }



        private bool _IsRenamingPage;

        public bool IsRenamingPage
        {
            get { return _IsRenamingPage; }
            set
            {
                _IsRenamingPage = value;
                onPropertyChanged("IsRenamingPage");
            }
        }




        private bool _PreviousCropButtonIsSelected;

        public bool PreviousCropButtonIsSelected
        {
            get { return _PreviousCropButtonIsSelected; }
            set
            {
                _PreviousCropButtonIsSelected = value;
                onPropertyChanged("PreviousCropButtonIsSelected");

                if (PreviousCropButtonIsSelected)
                {
                    DoUsePreviousSelection = true;
                }
                else
                {
                    DoUsePreviousSelection = false;
                }


            }
        }



        private bool _NoPreviousCropButtonIsSelected;

        public bool NoPreviousCropButtonIsSelected
        {
            get { return _NoPreviousCropButtonIsSelected; }
            set
            {
                _NoPreviousCropButtonIsSelected = value;
                onPropertyChanged("NoPreviousCropButtonIsSelected");


                if (NoPreviousCropButtonIsSelected)
                {
                    DoUsePreviousSelection = false;

                }
                else
                {
                    DoUsePreviousSelection = true;
                }

            }
        }




        public Page newPageViaScreenshot { get; set; }

        public string browserAddress { get; set; }

        public System.Windows.Forms.Screen CurrentDisplay { get; set; }

        private bool _DoUsePreviousSelection;

        public bool DoUsePreviousSelection
        {
            get { return _DoUsePreviousSelection; }
            set
            {
                _DoUsePreviousSelection = value;
                onPropertyChanged("DoUsePreviousSelection");
            }
        }

        private int _SelectedPageIndex;

        public int SelectedPageIndex
        {
            get { return _SelectedPageIndex; }
            set
            {
                if (value != -1)
                {
                    _SelectedPageIndex = value;
                    if (SelectedNote != null)
                    {
                        SelectedNote.LastViewedPageIndex = _SelectedPageIndex;
                        DatabaseHelper.Update(SelectedNote);
                    }

                }

            }
        }




        private string _SelectedTextInDectionContainer;

        public string SelectedTextInDectionContainer
        {
            get { return _SelectedTextInDectionContainer; }
            set
            {
                _SelectedTextInDectionContainer = value;
            }
        }

        public string DictionaryBaseUrl { get; set; }



        private string _DetectionContainerContent;

        public string DetectionContainerContent
        {
            get { return _DetectionContainerContent; }
            set
            {
                _DetectionContainerContent = value;
                onPropertyChanged("DetectionContainerContent");
            }
        }



        public string TranslationResult { get; set; }




        public ObservableCollection<Note> NotesDesign { get; set; }

        private CropLayer _CropLayer;

        public CropLayer CropLayer
        {
            get { return _CropLayer; }
            set
            {
                _CropLayer = value;
                CommandManager.InvalidateRequerySuggested();
                onPropertyChanged("CropLayer");
            }
        }





        private int _DisplayIndex;

        public int DisplayIndex
        {
            get { return _DisplayIndex; }
            set
            {
                _DisplayIndex = value;
                onPropertyChanged("DisplayIndex");
            }
        }


        private double[] _PrevSelectionRegion;

        public double[] PrevSelectionRegion
        {
            get { return _PrevSelectionRegion; }
            set { _PrevSelectionRegion = value; }
        }





        private ObservableCollection<Page> _Pages;

        public ObservableCollection<Page> Pages
        {
            get { return _Pages; }
            set
            {
                _Pages = value;
                onPropertyChanged("Pages");
            }
        }

        public string CurrentScreenshot { get; set; }
        public string DictionaryFilePath { get; set; }
        public string screenshotFilePath { get; set; }

        private BitmapImage _SelectedImage;

        public BitmapImage SelectedImage
        {
            get { return _SelectedImage; }
            set { _SelectedImage = value; }
        }

        public string TextCapturedByVisionAPI { get; set; }

        public string GoogleTranslation { get; set; }

        public string SelectedText { get; set; }

        public string SelectedTextExplanation { get; set; }


        private bool _IsEditingNoteName;

        public bool IsEditingNoteName
        {
            get { return _IsEditingNoteName; }
            set
            {
                _IsEditingNoteName = value;
                onPropertyChanged("IsEditingNoteName");
            }
        }



        private bool _IsEditingVocab;

        public bool IsEditingVocab
        {
            get { return _IsEditingVocab; }
            set
            {
                _IsEditingVocab = value;
                onPropertyChanged("IsEditingVocab");

            }
        }



        private ObservableCollection<Vocab> _Vocabs;
        public ObservableCollection<Vocab> Vocabs
        {
            get { return _Vocabs; }
            set
            {
                _Vocabs = value;
                onPropertyChanged("Vocabs");
            }
        }

        private Vocab _SelectedVocab;

        public Vocab SelectedVocab
        {
            get { return _SelectedVocab; }
            set
            {
                _SelectedVocab = value;
                onPropertyChanged("SelectedVocab");
            }
        }




        public NewNoteCommand NewNoteCommand { get; set; }
        public DeleteNoteCommand DeleteNoteCommand { get; set; }
        public NewPageCommand NewPageCommand { get; set; }
        public DeletePageCommand DeletePageCommand { get; set; }
        public RenameNoteCommand RenameNoteCommand { get; set; }
        public HasEditedNoteNameCommand HasEditedNoteNameCommand { get; set; }
        public ScreenCapCommand ScreenCapCommand { get; set; }
        public UsePreviousSelectionCommand UsePreviousSelectionCommand { get; set; }
        public DoNotUsePreviousSelectionCommand DoNotUsePreviousSelectionCommand { get; set; }
        public CheckDictionaryCommand CheckDictionaryCommand { get; set; }
        public GoogleTranslateCommand GoogleTranslateCommand { get; set; }
        public NewVocabCommand NewVocabCommand { get; set; }
        public DeleteVocabCommand DeleteVocabCommand { get; set; }
        public StartUpdateVocabCommand StartUpdateVocabCommand { get; set; }
        public EndUpdateVocabCommand EndUpdateVocabCommand { get; set; }
        public CropOriginalScreenshotCommand CropOriginalScreenshotCommand { get; set; }
        public NewPageViaScreenshotCommand NewPageViaScreenshotCommand { get; set; }
        public CaptureMoreTextCommand CaptureMoreTextCommand { get; set; }
        public StartRenamePageCommand StartRenamePageCommand { get; set; }
        public EndRenamePageCommand EndRenamePageCommand { get; set; }
        public ToggleUpdateVocabCommand ToggleUpdateVocabCommand { get; set; }

        private Note _SelectedNote;



        public Note SelectedNote
        {
            get { return _SelectedNote; }
            set
            {
                _SelectedNote = value;
                ReadPages();
                ReadVocabs();
                DetectionContainerContent = null;
                onPropertyChanged("SelectedNote");

                if (Pages.Count > 0)
                {
                    if (SelectedNote.LastViewedPageIndex < Pages.Count)
                    {
                        SelectedPage = Pages[SelectedNote.LastViewedPageIndex ?? 0];
                    }
                }



                //if (Pages.Count > 0)
                //{
                //    SelectedPage = Pages[0];
                //}

            }
        }




        private Page _SelectedPage;

        public Page SelectedPage
        {
            get { return _SelectedPage; }
            set
            {
                _SelectedPage = value;
                onPropertyChanged("SelectedPage");
                ReadVocabs();
                CommandManager.InvalidateRequerySuggested();
            }
        }



        private System.Windows.WindowState _WindowState;

        public System.Windows.WindowState WindowState
        {
            get { return _WindowState; }
            set
            {
                _WindowState = value;
                onPropertyChanged("WindowState");
            }
        }







        public MainVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                Notes = new ObservableCollection<Note>();
                Notes.Add(new Note() { Name = "note1", DateTime = DateTime.Now.ToShortDateString() });
                Notes.Add(new Note() { Name = "note2", DateTime = DateTime.Now.ToShortDateString() });
                Notes.Add(new Note() { Name = "note3", DateTime = DateTime.Now.ToShortDateString() });

                Pages = new ObservableCollection<Page>();
                Pages.Add(new Page() { Name = "Screenshot", DateTime = DateTime.Now.ToShortDateString() });
                Pages.Add(new Page() { Name = "Screenshot", DateTime = DateTime.Now.ToShortDateString() });
                Pages.Add(new Page() { Name = "Screenshot", DateTime = DateTime.Now.ToShortDateString() });
                IsEditingNoteName = false;
                IsEditingVocab = false;

                Vocabs = new ObservableCollection<Vocab>();
                Vocabs.Add(new Vocab() { PageId = 1, Word = "123", Explaination = "321", Pronounciation = "aaa" });
                Vocabs.Add(new Vocab() { PageId = 1, Word = "456", Explaination = "654", Pronounciation = "bbb" });
                Vocabs.Add(new Vocab() { PageId = 1, Word = "789", Explaination = "987", Pronounciation = "ccc" });
            }
            else
            {
                WindowState = System.Windows.WindowState.Normal;
                NewNoteCommand = new NewNoteCommand(this);
                DeleteNoteCommand = new DeleteNoteCommand(this);
                NewPageCommand = new NewPageCommand(this);
                DeletePageCommand = new DeletePageCommand(this);
                RenameNoteCommand = new RenameNoteCommand(this);
                ScreenCapCommand = new ScreenCapCommand(this);
                HasEditedNoteNameCommand = new HasEditedNoteNameCommand(this);
                UsePreviousSelectionCommand = new UsePreviousSelectionCommand(this);
                DoNotUsePreviousSelectionCommand = new DoNotUsePreviousSelectionCommand(this);
                CheckDictionaryCommand = new CheckDictionaryCommand(this);
                GoogleTranslateCommand = new GoogleTranslateCommand(this);
                DeleteVocabCommand = new DeleteVocabCommand(this);
                NewVocabCommand = new NewVocabCommand(this);
                StartUpdateVocabCommand = new StartUpdateVocabCommand(this);
                EndUpdateVocabCommand = new EndUpdateVocabCommand(this);
                CropOriginalScreenshotCommand = new CropOriginalScreenshotCommand(this);
                NewPageViaScreenshotCommand = new NewPageViaScreenshotCommand(this);
                CaptureMoreTextCommand = new CaptureMoreTextCommand(this);
                StartRenamePageCommand = new StartRenamePageCommand(this);
                EndRenamePageCommand = new EndRenamePageCommand(this);
                ToggleUpdateVocabCommand = new ToggleUpdateVocabCommand(this);

                Notes = new ObservableCollection<Note>();
                Pages = new ObservableCollection<Page>();
                Vocabs = new ObservableCollection<Vocab>();
                ReadNotes();
                ReadPages();
                IsEditingNoteName = false;
                IsEditingVocab = false;
                DisplayIndex = 0;
                DoUsePreviousSelection = false;
                browserAddress = "";
                DictionaryBaseUrl = "https://www.japandict.com/";
                SelectedPageIndex = 0;
                NoPreviousCropButtonIsSelected = true;
                IsRenamingPage = false;
            }
        }







        public void NewNote()
        {
            Note note = new Note()
            {
                Name = "New Note",
                DateTime = DateTime.Now.ToShortDateString()
            };
            DatabaseHelper.Insert(note);
            ReadNotes();
        }

        public void ReadNotes()
        {
            List<Note> notes = DatabaseHelper.Read<Note>();

            Notes.Clear();

            foreach (var note in notes)
            {
                Notes.Add(note);
            }


        }



        public void ReadPages()
        {
            List<Page> pages;
            if (SelectedNote != null)
            {
                using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
                {
                    connection.CreateTable<Page>();
                    pages = connection.Table<Page>().ToList<Page>().Where(p => p.NoteId == SelectedNote.Id).ToList();
                }

                Pages.Clear();


                foreach (var page in pages)
                {
                    Pages.Add(page);
                }




            }
            else
            {
                SelectedPage = null;
                Pages.Clear();
            }

        }



        public void ReadVocabs()
        {
            List<Vocab> vocabs;
            if (SelectedPage != null)
            {
                using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
                {
                    connection.CreateTable<Vocab>();
                    vocabs = connection.Table<Vocab>().ToList<Vocab>().Where(v => v.PageId == SelectedPage.Id).ToList();
                }

                Vocabs.Clear();

                foreach (var vocab in vocabs)
                {
                    Vocabs.Add(vocab);
                }
            }
            else
            {
                Vocabs.Clear();
            }

        }










        public void DeleteNote()
        {
            DatabaseHelper.Delete(SelectedNote);


            foreach (Page page in Pages)
            {
                if (page.ScreenshotByteArray != null)
                {
                    File.Delete(page.ScreenshotByteArray);
                }

                if (page.CroppedScreenshotByteArray != null)
                {
                    File.Delete(page.CroppedScreenshotByteArray);
                }

            }

            ReadNotes();
        }

        public void DeleteVocab()
        {
            DatabaseHelper.Delete(SelectedVocab);
            ReadVocabs();
        }






        public void NewPage()
        {

            if (SelectedNote != null)
            {
                Page page = new Page()
                {
                    Name = "Page",
                    NoteId = SelectedNote.Id,
                    DateTime = DateTime.Now.ToShortDateString()
                };
                DatabaseHelper.Insert(page);
            }
            Pages.Clear();
            ReadPages();
            SelectedPage = Pages.LastOrDefault();
        }


        public void NewVocab()
        {
            if (SelectedNote != null)
            {
                Vocab vocab = new Vocab()
                {
                    PageId = SelectedPage.Id,
                    Word = "New Word",
                    Name = "New Word",
                    Pronounciation = "Pronounciation",
                    Explaination = "Description."
                };
                DatabaseHelper.Insert(vocab);
            }
            Vocabs.Clear();
            ReadVocabs();
        }





        public void DeletePage()
        {
            DatabaseHelper.Delete(SelectedPage);

            if (SelectedPage.CroppedScreenshotByteArray != null)
            {
                File.Delete(SelectedPage.CroppedScreenshotByteArray);
            }

            if (SelectedPage.ScreenshotByteArray != null)
            {
                File.Delete(SelectedPage.ScreenshotByteArray);
            }




            ReadPages();
        }



        public event EventHandler RenameNoteStarted;

        public void RenameNote()
        {
            IsEditingNoteName = true;
            RenameNoteStarted(this, new EventArgs());
        }

        public void HasEditedNoteName()
        {
            DatabaseHelper.Update(SelectedNote);
            IsEditingNoteName = false;
        }






        public event PropertyChangedEventHandler PropertyChanged;

        public void onPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public static event EventHandler ScreenshotIsTaken;

        public static event EventHandler CheckApplicationLyingInWhichDisplay;



        public void TakeScreenShot()
        {

            CheckApplicationLyingInWhichDisplay(this, new EventArgs());

            int applicationLyingDisplayIndex = int.Parse(Regex.Match(CurrentDisplay.DeviceName, @"\d+").ToString()) - 1;


            if (applicationLyingDisplayIndex == DisplayIndex)
            {
                WindowState = System.Windows.WindowState.Minimized;
                CurrentScreenshot = BitmapHelper.SaveBitmapReturnPath(BitmapHelper.TakeScreenShot(DisplayIndex), SelectedPage);
                WindowState = System.Windows.WindowState.Normal;
            }
            else
            {
                CurrentScreenshot = BitmapHelper.SaveBitmapReturnPath(BitmapHelper.TakeScreenShot(DisplayIndex), SelectedPage);
            }

            SelectedPage.ScreenshotByteArray = CurrentScreenshot;
            SelectedPage.CroppedScreenshotByteArray = null;
            DatabaseHelper.Update(SelectedPage);
            ReadPages();
            ScreenshotIsTaken(this, new EventArgs());
        }

        public void UsePreviousSelection()
        {
            DoUsePreviousSelection = true;
        }

        public void DoNotUsePreviousSelection()
        {
            DoUsePreviousSelection = false;
        }



        public event EventHandler ConfirmJapanDictSelection;


        public void CheckViaDictionary()
        {
            string selection = SelectedTextInDectionContainer;
            ConfirmJapanDictSelection(this, new EventArgs());
        }


        public event EventHandler GoogleTranslateComplete;

        public void GoogleTranslate()
        {
            string selection = SelectedTextInDectionContainer;
            TranslationResult = TranslationAPIHelper.Translate(selection).TranslatedText;
            GoogleTranslateComplete(this, new EventArgs());
        }

        public event EventHandler UpdateVocabIsClicked;

        public void StartUpdateVocab()
        {
            IsEditingVocab = true;
            UpdateVocabIsClicked(this, new EventArgs());
        }

        public void EndUpdateVocab()
        {
            IsEditingVocab = false;
            DatabaseHelper.Update(SelectedVocab);

        }


        public event EventHandler CropOriginalScreenshot;

        public void CropOriginal()
        {
            CropOriginalScreenshot(this, new EventArgs());
        }




        public static event EventHandler NewPageViaScreenshotIsTaken;



        public void NewPageViaScreenshot()
        {

            CheckApplicationLyingInWhichDisplay(this, new EventArgs());

            int applicationLyingDisplayIndex = int.Parse(Regex.Match(CurrentDisplay.DeviceName, @"\d+").ToString()) - 1;


            if (applicationLyingDisplayIndex == DisplayIndex)
            {
                WindowState = System.Windows.WindowState.Minimized;
                CurrentScreenshot = BitmapHelper.SaveBitmapReturnPath(BitmapHelper.TakeScreenShot(DisplayIndex), SelectedPage);
                WindowState = System.Windows.WindowState.Normal;
            }
            else
            {
                CurrentScreenshot = BitmapHelper.SaveBitmapReturnPath(BitmapHelper.TakeScreenShot(DisplayIndex), SelectedPage);
            }




            var newPage = new Page()
            {
                NoteId = SelectedNote.Id,
                Name = "Page",
                DateTime = DateTime.Now.ToShortDateString(),
                ScreenshotByteArray = CurrentScreenshot
            };
            newPage.ScreenshotByteArray = CurrentScreenshot;
            DatabaseHelper.Insert(newPage);
            ReadPages();
            NewPageViaScreenshotIsTaken(this, new EventArgs());
        }

        public static event EventHandler CaptureFromScreenshotNow;

        public void CaptureMoreText()
        {
            CaptureFromScreenshotNow(this, new EventArgs());
        }

        public static event EventHandler RenamePageIsClicked;

        public void StartRenamePage()
        {
            IsRenamingPage = true;
            RenamePageIsClicked(this, new EventArgs());

        }

        public void EndRenamePage()
        {
            IsRenamingPage = false;
            DatabaseHelper.Update(SelectedPage);
            ReadPages();
            if (Pages.Count > 0)
            {
                int lastViewPage = SelectedNote.LastViewedPageIndex ?? 0;

                {
                    SelectedPage = Pages[lastViewPage];
                }

            }
        }


        public void toggleUpdateVocabCommand()
        {
            if (IsEditingVocab == false)
            {
                StartUpdateVocab();
            }
            else
            {
                EndUpdateVocab();
            }
        }



    }
}
