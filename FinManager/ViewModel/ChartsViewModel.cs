using FinManager.Model;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Xamarin.Forms.PlatformConfiguration;

namespace FinManager.ViewModel
{
    public class ChartsViewModel : INotifyPropertyChanged
    {
        private List<Note> notes;
        private List<Wallet> wallets;
        private List<Category> categories;
        private SortedDictionary<int, double> catSums;
        private SortedDictionary<int, bool> catSyn;
        private SortedDictionary<string, DateTime> mounths;
        private SortedDictionary<int, double> walSums;
        public event PropertyChangedEventHandler PropertyChanged;
        public Chart ExpChart { get; private set; }
        public Chart RadChart { get; private set; }
        public List<string> Mounths { get; private set; }
        private DateTime date;
        public List<Entry> Entries { get; private set; }
        public List<Entry> WalletsSums { get; private set; }
        public double AllExpenses { get; private set; }
        public double AllIncome { get; private set; }
        public double Balance { get; private set; }
        public ChartsViewModel()
        {
            date = DateTime.Today;
            mounths = new SortedDictionary<string, DateTime>();
            catSums = new SortedDictionary<int, double>();
            catSyn = new SortedDictionary<int, bool>();
            walSums = new SortedDictionary<int, double>();
            WalletsSums = new List<Entry>();
            ExpChart = new BarChart()
            {
                LabelTextSize = 40f,

            };
            RadChart = new RadialGaugeChart()
            {
                LabelTextSize = 40f,
            };
            Entries = new List<Entry>();
            FillLists();
        }

        public void PickedDate(string key)
        {
            if (key == "")
            {
                return;
            }
            date = mounths[key];
        }

        private void FillLists()
        {
            notes?.Clear();
            wallets?.Clear();
            categories?.Clear();
            Mounths?.Clear();
            catSyn?.Clear();
            catSums?.Clear();
            mounths?.Clear();
            WalletsSums?.Clear();
            AllExpenses = 0;
            AllIncome = 0;
            Balance = 0;
            Entries?.Clear();
            Mounths = App.Notes.GetDateTimes(mounths);
            wallets = App.Wallets.GetWallets();
            categories = App.Categories.GetCategories();
            notes = App.Notes.GetExactNotes(date);
            foreach (var i in categories)
            {
                catSums.Add(i.ID, 0);
                catSyn.Add(i.ID, i.InCome);
            }
            SyncSums();
            FillEntries();
        }

        private void SyncSums()
        {
            foreach (var i in notes)
            {
                catSums[i.CatId] += i.Sum;
                if (catSyn[i.CatId])
                {
                    AllIncome += i.Sum;
                }
                else
                {
                    AllExpenses += i.Sum;
                }
            }
        }

        private void FillEntries()
        {
            Entries.Clear();
            WalletsSums.Clear();
            foreach (var i in categories)
            {
                if (i.InCome)
                {
                    continue;
                }
                Entries.Add(new Entry((float)catSums[i.ID]) {
                    Label = i.Name,
                    ValueLabel = ((float)catSums[i.ID]).ToString(),
                    Color = SKColor.Parse(i.Color) });
            }
            foreach (var i in wallets)
            {
                WalletsSums.Add(new Entry((float)i.Sum)
                {
                    Label = i.WalName,
                    ValueLabel = ((float)i.Sum).ToString(),
                    Color = SKColor.Parse(HexConverter())
                });
                Balance += i.Sum;
            }
            WalletsSums.Add(new Entry((float)Balance)
            {
                Label = "Total Balance",
                ValueLabel = ((float)Balance).ToString(),
                Color = SKColor.Parse(HexConverter())
            });
            ExpChart.Entries = Entries;
            RadChart.Entries = WalletsSums;
            OnPropertyChanged("ExpChart");
            OnPropertyChanged("RadChart");
        }

        public void UpdateCharts()
        {
            FillLists();
            OnPropertyChanged("AllIncome");
            OnPropertyChanged("AllExpenses");
            OnPropertyChanged("Mounths");
            OnPropertyChanged("Balance");
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private static string HexConverter()
        { 
            var rand = new Random();
            var c = Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }
}
