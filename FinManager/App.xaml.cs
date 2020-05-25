using System;
using Xamarin.Forms;
using FinManager.View;
using FinManager.ViewModel;
using FinManager.Data;
using System.IO;
using System.Reflection;
using FinManager.Model;

namespace FinManager
{
    public partial class App : Application
    {
        public const string DATABASE_NOTES = "notess.db";
        public const string DATABASE_CATEGORIES = "categories.db";
        public const string DATABASE_WALLETS = "walltes.db";
        public static NoteRep notes;
        public static WalletRep wallets;
        public static CategRep categories;
        public static string theme;

        public static NoteRep Notes
        {
            get
            {
                if (notes == null)
                {
                    notes = new NoteRep(Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData), DATABASE_NOTES));
                }

                return notes;
            }
        }

        public static WalletRep Wallets
        {
            get
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_WALLETS);
                if (wallets == null)
                {
                    Wallet wal = null;
                    if (!File.Exists(dbPath))
                    {
                        wal = new Wallet { WalName = "Cash" };
                    }
                    wallets = new WalletRep(dbPath);
                    if (wal != null)
                    {
                        wallets.SaveWallet(wal);
                    }

                }

                return wallets;
            }
        }

        public static CategRep Categories
        {
            get
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_CATEGORIES);
                if (categories == null)
                {
                    Category cat = null;
                    if (!File.Exists(dbPath))
                    {
                        cat = new Category();
                    }
                    categories = new CategRep(dbPath);
                    if (cat != null)
                    {
                        categories.SaveCategory(cat);
                    }

                }

                return categories;
            }
        }


        public App()
        {
            InitializeComponent();
            theme = ((Color)themee["backColor"]).ToHex();
            MainPage = new NavigationPage(new MainPage());
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = ((Color)themee["menuColor"]);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
