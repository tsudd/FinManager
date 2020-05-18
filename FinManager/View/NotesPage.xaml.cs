using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FinManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesPage : ContentPage
    {
        public NoteListViewModel noteList;
        public NotesPage()
        {
            InitializeComponent();
            noteList = new NoteListViewModel() { Navigation = this.Navigation };
            noteList.Wallets = App.Wallets.GetWallets();
            noteList.Categories = App.Categories.GetCategories();
            BindingContext = noteList;
        }

        protected override void OnAppearing()
        {
            noteList.MakeGrouping();
            base.OnAppearing();
        }

        public void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            noteList.MakeGrouping();
            list.IsRefreshing = false;
        }

        public void OnDelete(object sender, EventArgs e)
        {
            NoteViewModel note = ((MenuItem)sender).CommandParameter as NoteViewModel;
            noteList.DeleteNote(note);
        }

    }
}