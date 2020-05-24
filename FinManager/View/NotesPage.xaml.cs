using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FinManager.Model;
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
            BindingContext = noteList;
            noteList.UserNotify += Notification;
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
            noteList.SyncInfo();
            list.IsRefreshing = false;
        }

        public void OnDelete(object sender, EventArgs e)
        {
            NoteViewModel note = ((MenuItem)sender).CommandParameter as NoteViewModel;
            noteList.DeleteNote(note);
        }

        private async void Notification(string msg)
        {
            await DisplayAlert("Message", msg, "OK");
        }

    }
}