using ScreenCapDictionaryNoteApp.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers
{
    public class Commands
    {
        public NewNoteCommand NewNoteCommand { get; set; }
        public DeleteNoteCommand DeleteNoteCommand { get; set; }
        public NewPageCommand NewPageCommand { get; set; }
        public DeletePageCommand DeletePageCommand { get; set; }
        public RenameNoteCommand RenameNoteCommand { get; set; }
        public HasEditedNoteNameCommand HasEditedNoteNameCommand { get; set; }
        public ScreenCapCommand ScreenCapCommand { get; set; }
        public UsePreviousSelectionCommand UsePreviousSelectionCommand { get; set; }
        public DoNotUsePreviousSelectionCommand DoNotUsePreviousSelectionCommand { get; set; }
        public CheckDictionaryJapanDictCommand CheckDictionaryCommand { get; set; }
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
        public CheckDictionaryLittleDCommand CheckDictionaryLittleDCommand { get; set; }
        public SyncVocabsCommand SyncVocabsCommand { get; set; }
        public CheckDictionaryWeblioCommand CheckDictionaryWeblioCommand { get; set; }
        public CheckDictionaryTanoshiiJapaneseCommand CheckDictionaryTanoshiiJapaneseCommand { get; set; }
        private MainVM MainVM;


        public Commands(MainVM mainVM)
        {
            this.MainVM = mainVM;
            NewNoteCommand = new NewNoteCommand(MainVM);
            DeleteNoteCommand = new DeleteNoteCommand(MainVM);
            NewPageCommand = new NewPageCommand(MainVM);
            DeletePageCommand = new DeletePageCommand(MainVM);
            RenameNoteCommand = new RenameNoteCommand(MainVM);
            ScreenCapCommand = new ScreenCapCommand(MainVM);
            HasEditedNoteNameCommand = new HasEditedNoteNameCommand(MainVM);
            UsePreviousSelectionCommand = new UsePreviousSelectionCommand(MainVM);
            DoNotUsePreviousSelectionCommand = new DoNotUsePreviousSelectionCommand(MainVM);

            GoogleTranslateCommand = new GoogleTranslateCommand(MainVM);
            DeleteVocabCommand = new DeleteVocabCommand(MainVM);
            NewVocabCommand = new NewVocabCommand(MainVM);
            StartUpdateVocabCommand = new StartUpdateVocabCommand(MainVM);
            EndUpdateVocabCommand = new EndUpdateVocabCommand(MainVM);
            CropOriginalScreenshotCommand = new CropOriginalScreenshotCommand(MainVM);
            NewPageViaScreenshotCommand = new NewPageViaScreenshotCommand(MainVM);
            CaptureMoreTextCommand = new CaptureMoreTextCommand(MainVM);
            StartRenamePageCommand = new StartRenamePageCommand(MainVM);
            EndRenamePageCommand = new EndRenamePageCommand(MainVM);
            ToggleUpdateVocabCommand = new ToggleUpdateVocabCommand(MainVM);

            SyncVocabsCommand = new SyncVocabsCommand(MainVM);

            CheckDictionaryCommand = new CheckDictionaryJapanDictCommand(MainVM);
            CheckDictionaryLittleDCommand = new CheckDictionaryLittleDCommand(MainVM);
            CheckDictionaryWeblioCommand = new CheckDictionaryWeblioCommand(MainVM);
            CheckDictionaryTanoshiiJapaneseCommand = new CheckDictionaryTanoshiiJapaneseCommand(MainVM);
        }


    }
}
