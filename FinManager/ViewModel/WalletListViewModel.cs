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
            WalletViewModel walletView = catObj as WalletViewModel;
            if (walletView != null && walletView.IsValid)
            {
                if (walletView.Wallet.ID == 0)
                {
                    Wallets.Add(walletView);
                }
                App.Wallets.SaveWallet(walletView.Wallet);
            }
            Back();

        }

        public void DelWallet(WalletViewModel walletView)
        {
            if (walletView != null)
            {
                if (walletView.Wallet.ID == 1)
                {
                    //dumb?
                    return;
                }
                Wallets.Remove(walletView);
                App.Notes.AdjustNotes(walletView.Wallet.ID);
                App.Wallets.DeleteWallet(walletView.Wallet.ID);
                OnPropertyChanged("Categories");
            }
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        public WalletViewModel SelecetedWallet
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
            var table = new ObservableCollection<Wallet>(App.Wallets.GetWallets());
            foreach (var i in table)
            {
                Wallets.Add(new WalletViewModel(i));
            }
        }
    }
}
