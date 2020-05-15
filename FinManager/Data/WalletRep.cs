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
    public class WalletRep : INotifyPropertyChanged
    {
        readonly SQLiteConnection database;

        public double balance;

        public event PropertyChangedEventHandler PropertyChanged;
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

        public async void ChangeSum(Note note, bool add = true)
        {
            var wal = GetWallet(note.WalId);
            var cat = App.Categories.GetCategory(note.CatId);
            if (note.ID != 0)
            {
                var oldNote = await App.Notes.GetNoteAsync(note.ID);
                wal.Sum += Math.Pow(-1, (double)cat.InCome) * oldNote.Sum;
            }
            if (add)
                wal.Sum += (-1)*Math.Pow(-1, (double)cat.InCome) * note.Sum;
            SaveWallet(wal);
            BalanceSync();
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
            OnPropertyChanged("Balance");

        }

        public double Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }

}
