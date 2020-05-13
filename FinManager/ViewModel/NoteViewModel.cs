using FinManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Schema;
using Xamarin.Essentials;

namespace FinManager.ViewModel
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        NoteListViewModel lvm;

        public Note Expense { get; set; }

        public NoteViewModel()
        {
            Expense = new Note
            {
                ID = 0,
                CatId = 1,
                Date = DateTime.Today,
                Cat = App.Categories.GetCategory(1).Name
            };

        }

        public NoteViewModel(Note note)
        {
            Expense = new Note
            {
                ID = note.ID,
                CatId = note.CatId,
                Sum = note.Sum,
                Date = note.Date,
            };

            Expense.Cat = App.Categories.GetCategory(note.CatId).Name;
        }

        public NoteListViewModel ListViewModel
        {
            get { return lvm; }
            set
            {
                if (lvm != value)
                {
                    lvm = value;
                    OnPropertyChanged("ListViewModel");
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Sum.Trim()));
            }
        }

        public string Sum
        {
            get { return Expense.Sum.ToString(); }
            set
            {
                if (Double.TryParse(value.Replace(',', '.'), out double ans))
                {
                    Expense.Sum = ans;
                }
            }
        }

        protected void OnPropertyChanged(string note)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(note));
        }

        public string Category
        {
            get { return Expense.Cat; }
        }

        public string Date
        {
            get { return Expense.Date.ToString("d"); }
            set {; }
        }
    }
}
