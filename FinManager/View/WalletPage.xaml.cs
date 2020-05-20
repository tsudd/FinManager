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
    public partial class WalletPage : ContentPage
    {
        public WalletViewModel WalletView { get; private set; }
        public WalletPage(WalletViewModel wvm)
        {
            WalletView = wvm;
            this.BindingContext = WalletView;
            InitializeComponent();
        }


    }
}