using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using FinManager.Model;
using System.Threading.Tasks;

namespace FinManager.Data
{
    public class NoteAsyncRep: INotifyPropertyChanged
    {
        readonly SQLiteAsyncConnection database;

        public event PropertyChangedEventHandler PropertyChanged;
        public NoteAsyncRep(string dBPath)
        {
            database = new SQLiteAsyncConnection(dBPath);
            database.CreateTableAsync<Note>().Wait();
        }
        public Task<List<Note>> GetNotesAsync()
        {
            var notes = database.Table<Note>().ToListAsync();
            return notes;
        }

        public Task<Note> GetNoteAsync(int id)
        {
            var notes = database.Table<Note>();
           
            return notes
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> DeleteNote(Note item)
        {
            App.Wallets.ChangeSum(item, false);
            return database.DeleteAsync(item);
        }

        public Task<int> SaveNote(Note note)
        {
            App.Wallets.ChangeSum(note);
            if (note.ID != 0)
            {
                return database.UpdateAsync(note);
            }
            else
            {
                return database.InsertAsync(note);
            }
        }
    }
}
