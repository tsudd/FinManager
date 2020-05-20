using FinManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FinManager.ViewModel
{
    public class WalletViewModel: INotifyPropertyChanged
    {
        WalletListViewModel lvm;
        public event PropertyChangedEventHandler PropertyChanged;
        public Wallet Wallet { get; private set; }

        public WalletViewModel()
        {
            Wallet = new Wallet();
        }

        public WalletViewModel(Wallet wallet)
        {
            Wallet = new Wallet
            {
                ID = wallet.ID,
                WalName = wallet.WalName,
                Sum = wallet.Sum

            };
        }

        protected void OnPropertyChanged(string note)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(note));
        }

        public WalletListViewModel ListViewModel
        {
            get { return lvm; }
            set
            {
                if (lvm != value)
                {
                    lvm = value;
                    OnPropertyChanged("ListViewModel");
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(Wallet.WalName);
            }
        }

        public string WalName
        {
            get { return Wallet.WalName; }
            set
            {
                
                Wallet.WalName = value;
                OnPropertyChanged("Name");
            }
        }
    }
}
