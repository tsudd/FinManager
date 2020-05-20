using FinManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalletsPage : ContentPage
    {
        WalletListViewModel walletList;
        public WalletsPage()
        {
            InitializeComponent();
            walletList = new WalletListViewModel() { Navigation = this.Navigation };
            this.BindingContext = walletList;
        }

        public void OnDelete(object sender, EventArgs e)
        {
            WalletViewModel cat = ((MenuItem)sender).CommandParameter as WalletViewModel;
            walletList.DelWallet(cat);
        }
    }
}