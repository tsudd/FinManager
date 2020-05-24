using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using FinManager.Model;
using System.Threading.Tasks;
using System.Linq;

namespace FinManager.Data
{
    public class NoteRep
    {
        readonly SQLiteConnection database;
        public NoteRep(string dBPath)
        {
            database = new SQLiteConnection(dBPath, true);
            database.CreateTable<Note>();
        }
        public List<Note> GetNotes()
        {
            var notes = database.Table<Note>().ToList();
            return notes;
        }

        public Note GetNote(int id)
        {
            var notes = database.Table<Note>();
           
            return notes
                .Where(i => i.ID == id)
                .FirstOrDefault();
        }

        public int DeleteNote(Note item)
        {
            App.Wallets.ChangeSum(item, false);
            return database.Delete(item);
        }

        public int SaveNote(Note note)
        {
            App.Wallets.ChangeSum(note);
            if (note.ID != 0)
            {
                return database.Update(note);
            }
            else
            {
                return database.Insert(note);
            }
        }

        public  void AdjustNotes(int id)
        {
            var notes = database.Table<Note>().Where(x => x.CatId == id).ToList();
            foreach(var i in notes)
            {
                if (i.CatId == id)
                {
                    i.CatId = 1;
                    SaveNote(i);
                }    
            }
        }

        public  List<Note> GetExactNotes(DateTime dt)
        {
            var lst = database.Table<Note>().ToList().Where(x => x.Date.Month == dt.Month).ToList();
            return lst;
        }

        public List<string> GetDateTimes(SortedDictionary<string, DateTime> lst)
        {
            var table = database.Table<Note>().ToList();
            List<string> ans = new List<string>();
            foreach (var i in table)
            {
                if (!lst.ContainsKey(i.Date.ToString("Y")))
                {
                    var str = i.Date.ToString("Y");
                    lst.Add(str, i.Date);
                    ans.Add(str);
                }
            }
            return ans;
        }
    }
}
