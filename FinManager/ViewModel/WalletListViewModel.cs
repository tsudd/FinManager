using FinManager.Model;
using FinManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FinManager.ViewModel
{
    public class WalletListViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<WalletViewModel> Wallets { get; set; }
        public ICommand AddWalletCommand { get; protected set; }
        public ICommand SaveWalletCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public INavigation Navigation { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private WalletViewModel selectedWallet;
        public WalletListViewModel()
        {
            Wallets = new ObservableCollection<WalletViewModel>();
            GetItems();
            AddWalletCommand = new Command(AddWallet);
            SaveWalletCommand = new Command(SaveWallet);
            BackCommand = new Command(Back);
        }
        private void AddWallet()
        {
            Navigation.PushAsync(new WalletPage(new WalletViewModel() { ListViewModel = this }));
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void SaveWallet(object catObj)
        {
            if (catObj is WalletViewModel walletView && walletView.IsValid)
            {
                if (CheckIdent(walletView.WalName))
                {
                    App.OnNotify("Same wallet already exists! Can't make another.");
                    return;
                }
                if (walletView.Wallet.ID == 0)
                {
                    Wallets.Add(walletView);
                }
                else
                {
                    GetItems();
                    OnPropertyChanged("Wallets");
                }
                App.Wallets.SaveWallet(walletView.Wallet);
                App.SyncWallets();
            }
            Back();

        }

        public void DelWallet(WalletViewModel walletView)
        {
            if (walletView != null)
            {
                if (walletView.Wallet.ID == 1)
                {
                    App.OnNotify("You should have at least one wallet!");
                    return;
                }
                Wallets.Remove(walletView);
                App.Notes.AdjustNotesByWal(walletView.Wallet.ID);
                App.Wallets.DeleteWallet(walletView.Wallet.ID);
                App.SyncWallets();
                OnPropertyChanged("Wallets");
            }
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        public WalletViewModel SelectedWallet
        {
            get { return selectedWallet; }
            set
            {
                if (value != null && selectedWallet != value)
                {
                    WalletViewModel tempCat = value;
                    tempCat.ListViewModel = this;
                    selectedWallet = null;
                    OnPropertyChanged("SelectedWallet");
                    Navigation.PushAsync(new WalletPage(tempCat));
                }
            }
        }
        private void GetItems()
        {
            Wallets.Clear();
            foreach (var i in App.WalletsList)
            {
                Wallets.Add(new WalletViewModel(i));
            }
        }

        private static bool CheckIdent(string name)
        {
            foreach (var i in App.WalletsList)
            {
                if (i.WalName == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
