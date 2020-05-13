using System;
using System.Collections.Generic;
using System.Linq;
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
            BindingContext = noteList;
        }

        protected override void OnAppearing()
        {
            noteList.MakeGrouping();

            base.OnAppearing();
        }

    }
}