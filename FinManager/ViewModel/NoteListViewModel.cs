using FinManager.Model;
using FinManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void Notification(string msg);
        public event Notification UserNotify;
        public List<Wallet> Wallets { get; private set; }
        public List<Category> Categories { get; private set; }
        public INavigation Navigation { get; set; }

        public ICommand CreateNoteCommand { protected set; get; }
        public ICommand SaveNoteCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        private NoteViewModel selectedNote;

        public NoteListViewModel()
        {
            List = new ObservableCollection<NoteViewModel>();
            SyncInfo();
            App.Wallets.BalanceSync();
            CreateNoteCommand = new Command(CreateNote);
            SaveNoteCommand = new Command(SaveNote);
            BackCommand = new Command(Back);
        }

        public void MakeGrouping()
        {
            if (List.Count == 0)
            {
                NoteGroups?.Clear();
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

        protected void OnNotify(string msg)
        {
            UserNotify?.Invoke(msg);
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
            if (note != null)
            {
                if (Double.TryParse(note.Amount, out double ans))
                {
                    if (ans == 0)
                    {
                        OnNotify("Amount should be more than zero.");
                        return;
                    }
                    note.Expense.Sum = ans;
                }
                else
                {
                    OnNotify("Wrong amount input. Can't save this note.");
                    return;
                }
                if (note.Expense.ID == 0)
                {
                    List.Add(note);
                }
                App.Notes.SaveNote(note.Expense);
                SyncInfo();
                OnPropertyChanged("NotesGroups");
                OnPropertyChanged("List");
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

        private void GetItems()
        {
            List?.Clear();
            var table = new ObservableCollection<Note>(App.Notes.GetNotes());
            table.Reverse<Note>();
            foreach (var i in table)
            {
                List.Add(new NoteViewModel(i));
            }
        }

        public void SyncInfo()
        {
            List?.Clear();
            Wallets?.Clear();
            Categories?.Clear();
            Wallets = App.Wallets.GetWallets();
            Categories = App.Categories.GetCategories();
            var table = new ObservableCollection<Note>(App.Notes.GetNotes());
            foreach (var i in table)
            {
                List.Add(new NoteViewModel(i));
            }
            MakeGrouping();
            OnPropertyChanged("Balance");
        }
    }
}
