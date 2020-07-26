using FinManager.Model;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Color = Xamarin.Forms.Color;
using Entry = Microcharts.Entry;

namespace FinManager.ViewModel
{
    public class ChartsViewModel : INotifyPropertyChanged
    {
        private List<Note> notes;
        private SortedDictionary<int, double> catSums;
        private SortedDictionary<int, bool> catSyn;
        private SortedDictionary<string, DateTime> months;
        private readonly string basicColor;
        private int lastColor;
        public event PropertyChangedEventHandler PropertyChanged;
        public Chart ExpChart { get; private set; }
        public Chart RadChart { get; private set; }
        public List<string> Months { get; private set; }
        private DateTime date;
        public List<Entry> Entries { get; private set; }
        public List<Entry> WalletsSums { get; private set; }
        public double AllExpenses { get; private set; }
        public double AllIncome { get; private set; }
        public double Balance { get; private set; }
        public ChartsViewModel()
        {
            date = DateTime.Today;
            months = new SortedDictionary<string, DateTime>();
            catSums = new SortedDictionary<int, double>();
            catSyn = new SortedDictionary<int, bool>();
            WalletsSums = new List<Entry>();
            Entries = new List<Entry>();
            basicColor = "0064";
            var rand = new Random();
            lastColor = rand.Next(0, 255);
            FillLists();
        }

        public void PickedDate(string key)
        {
            if (key == "")
            {
                return;
            }
            date = months[key];
        }

        private void FillLists()
        {
            catSums.Clear();
            catSyn.Clear();
            months.Clear();
            AllExpenses = 0;
            AllIncome = 0;
            Balance = 0;
            Entries?.Clear();
            Months = App.Notes.GetDateTimes(months);
            notes = App.Notes.GetExactNotes(date);
            foreach (var i in App.CategoriesList)
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
            foreach (var i in App.CategoriesList)
            {
                if (i.InCome)
                {
                    continue;
                }
                Entries.Add(new Entry((float)catSums[i.ID]) {
                    Label = i.Name,
                    ValueLabel = ((float)catSums[i.ID]).ToString(),
                    Color = SKColor.Parse(i.Color) 
                });
            }
            foreach (var i in App.WalletsList)
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
            InitCharts();
        }

        public void UpdateCharts()
        {
            FillLists();
            OnPropertyChanged("AllIncome");
            OnPropertyChanged("AllExpenses");
            OnPropertyChanged("Balance");
        }

        private void InitCharts()
        {
            ExpChart = new BarChart()
            {
                LabelTextSize = 40f,
                BackgroundColor = SKColor.Parse(((Color)App.theme["backColor"]).ToHex()),
                Entries = Entries
            };
            RadChart = new RadialGaugeChart()
            {
                LabelTextSize = 40f,
                BackgroundColor = SKColor.Parse(((Color)App.theme["backColor"]).ToHex()),
                Entries = WalletsSums
            };
            OnPropertyChanged("ExpChart");
            OnPropertyChanged("RadChart");
        }

        public void UpdateMonths()
        {
            OnPropertyChanged("Month");
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private string HexConverter()
        {
            lastColor += 35;
            if (lastColor > 255) lastColor -= 255;
            return "#00" + lastColor.ToString("X2") + "64";
        }
    }
}
