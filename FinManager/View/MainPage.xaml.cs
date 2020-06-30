using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinManager;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();

            var notes = new NotesPage();
            var diagrams = new DiagramPage();
            var settings = new SettingsPage();

            Children.Add(notes);
            Children.Add(diagrams);
            Children.Add(settings);
            App.UserNotify += Notification;
        }

        private async void Notification(string msg)
        {
            await DisplayAlert("Message", msg, "OK");
        }
    }
}