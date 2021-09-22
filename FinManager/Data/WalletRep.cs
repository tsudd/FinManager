using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using FinManager.Model;
using System.Collections.ObjectModel;

namespace FinManager.Data
{
    public class WalletRep
    {
        readonly SQLiteConnection database;

        public double balance;
        public WalletRep(string dBPath)
        {
            database = new SQLiteConnection(dBPath);
            database.CreateTable<Wallet>();

        }

        public List<Wallet> GetWallets()
        {
            return database.Table<Wallet>().ToList();
        }

        public Wallet GetWallet(int id)
        {
            var it = database.Get<Wallet>(id);
            return it;

        }

        public void ChangeSum(Note note, bool add = true, bool noteChanged = true)
        {
            var wal = GetWallet(note.WalId);
            var cat = App.Categories.GetCategory(note.CatId);
            var old = App.Notes.GetNote(note.ID);
            if (old == null)
            {
                wal.Sum += (-1.0) * Math.Pow(-1, cat.InCome ? 1.0 : 0) * note.Sum;
            } 
            else
            {
                var oldCat = App.Categories.GetCategory(old.CatId);
                if (oldCat.InCome && !cat.InCome)
                {
                    wal.Sum -= 2.0 * note.Sum;
                }
                else if (!oldCat.InCome && cat.InCome)
                {
                    wal.Sum += 2.0 * note.Sum;
                }
            }
            
            //if (note.ID != 0 && noteChanged)
            //{
            //    wal.Sum += Math.Pow(-1, cat.InCome?1.0:0) * note.Sum;
            //}
            //if (add)
            //    wal.Sum += (-1) * Math.Pow(-1, cat.InCome ? 1.0 : 0) * note.Sum;
            SaveWallet(wal);
            BalanceSync();
        }

        public void DeleteNote(Note note)
        {
            var wal = GetWallet(note.WalId);
            var cat = App.Categories.GetCategory(note.CatId);
            wal.Sum += Math.Pow(-1, cat.InCome ? 1.0 : 0) * note.Sum;
            SaveWallet(wal);
            BalanceSync();
        }

        public void CalcNotes(int id, bool add = true, bool changed = true)
        {
            var notes = App.Notes.GetNotesWithCat(id);
            foreach(var i in notes)
            {
                ChangeSum(i, add, changed);
            }
        }

        public int DeleteWallet(int id)
        {
            return database.Delete<Wallet>(id);
        }

        public int SaveWallet(Wallet wallet)
        {
            if (wallet.ID != 0)
            {
                database.Update(wallet);
                return wallet.ID;
            }
            else
            {
                return database.Insert(wallet);
            }
        }

        public void BalanceSync()
        {
            var table = new ObservableCollection<Wallet>(GetWallets());
            balance = 0;
            foreach (var i in table)
            {
                balance += i.Sum;
            }

        }

        public double Balance
        {
            get { return balance; }
            set { balance = value; }
        }
    }

}
