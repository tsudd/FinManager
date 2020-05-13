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
        public static NoteAsyncRep notes;
        public static WalletRep wallets;
        public static CategRep categories;

        public static NoteAsyncRep Notes
        {
            get
            {
                if (notes == null)
                {
                    notes = new NoteAsyncRep(Path.Combine(
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
                        wal = new Wallet { Sum = 130, WalName = "Card" };
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

            MainPage = new NavigationPage(new MainPage());
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
