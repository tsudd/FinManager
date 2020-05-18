using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinManager.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotePage : ContentPage
    {
        public NoteViewModel ViewModel { get; private set; }
        public NotePage(NoteViewModel noteViewModel)
        {
            ViewModel = noteViewModel;
            this.BindingContext = ViewModel;
            InitializeComponent();
            
        }

        private void DatePicker(object sender, DateChangedEventArgs e)
        {
            ViewModel.DateTime = e.NewDate;
        }
    }
}