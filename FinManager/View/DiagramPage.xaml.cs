using FinManager.ViewModel;
using Microcharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;


namespace FinManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiagramPage : ContentPage
    {
        public ChartsViewModel ChartsView { get; private set; }
        private object selected;
        public DiagramPage()
        {
            InitializeComponent();
            ChartsView = new ChartsViewModel();
            this.BindingContext = ChartsView;
            picker.SelectedItem = ChartsView.Mounths.Count != 0 ? ChartsView.Mounths[0] : null;
            selected = picker.SelectedItem;
            
        }

        private void SelectedMounth(object sender, EventArgs e)
        {
            if (picker.SelectedIndex < 0)
            { 
                return;
            }
            selected = picker.SelectedItem;
            ChartsView.PickedDate((string)selected ?? "");
        }

        public void OnRefresh(object sender, EventArgs e)
        {
            var list = (RefreshView)sender;
            ChartsView.UpdateCharts();
            list.IsRefreshing = false;
            picker.SelectedItem = selected;
            if (selected == null)
            {
                picker.SelectedItem = ChartsView.Mounths.Count != 0 ? ChartsView.Mounths[0] : null;
                selected = picker.SelectedItem;
            }
            
        }
    }
}