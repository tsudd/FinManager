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
    public partial class NotePage : ContentPage
    {
        public NoteViewModel ViewModel { get; private set; }
        public NotePage(NoteViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            this.BindingContext = ViewModel;
        } 
    }
}