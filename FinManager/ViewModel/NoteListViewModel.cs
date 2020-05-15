using FinManager.Model;
using FinManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FinManager.ViewModel
{
    public class NoteListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<NoteViewModel> List { get; set; }
        public ObservableCollection<Grouping<string, NoteViewModel>> NoteGroups { get; set; }
        public List<Wallet> Wallets { get; set; }
        public List<Category> Categories { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }

        public ICommand CreateNoteCommand { protected set; get; }
        public ICommand SaveNoteCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        private NoteViewModel selectedNote;

        public NoteListViewModel()
        {
            List = new ObservableCollection<NoteViewModel>();
            GetItems();
            App.Wallets.BalanceSync();
            CreateNoteCommand = new Command(CreateNote);
            SaveNoteCommand = new Command(SaveNote);
            BackCommand = new Command(Back);
        }

        public void MakeGrouping()
        {
            if (List == null)
            {
                return;
            }    
            var group = List.GroupBy(p => p.Date)
                .Select(g => new Grouping<string, NoteViewModel>(g.Key, g));
            NoteGroups = new ObservableCollection<Grouping<string, NoteViewModel>>(group);
            OnPropertyChanged("NoteGroups");

        }

        public NoteViewModel SelectedNote
        {
            get { return selectedNote; }
            set
            {
                if (selectedNote != value)
                {
                    NoteViewModel tempNote = value;
                    tempNote.ListViewModel = this;
                    selectedNote = null;
                    OnPropertyChanged("SelectedNote");
                    Navigation.PushAsync(new NotePage(tempNote));
                }
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void CreateNote()
        {
            Navigation.PushAsync(new NotePage(new NoteViewModel() { ListViewModel = this }));
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        private void SaveNote(object noteObj)
        {
            NoteViewModel note = noteObj as NoteViewModel;
            if (note != null && note.IsValid)
            {
                if (note.Expense.ID == 0)
                {
                    List.Add(note);
                }
                App.Notes.SaveNote(note.Expense);
                OnPropertyChanged("Balance");
            }
            Back();
        }

        public void DeleteNote(NoteViewModel note)
        {
            if (note != null)
            {
                List.Remove(note);
                App.Notes.DeleteNote(note.Expense);
                MakeGrouping();
                OnPropertyChanged("Balance");
            }
        }

        public string Balance
        {
            get { return App.Wallets.Balance.ToString(); }
        }

        private async void GetItems()
        {
            var table = new ObservableCollection<Note>(await App.Notes.GetNotesAsync());
            foreach (var i in table)
            {
                List.Add(new NoteViewModel(i));
            }
        }
    }
}
