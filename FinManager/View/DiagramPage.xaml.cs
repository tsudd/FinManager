using FinManager.ViewModel;
using Microcharts;
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
    public partial class DiagramPage : ContentPage
    {
        public ChartsViewModel ChartsView { get; private set; }
        private object selected;
        public DiagramPage()
        {
            InitializeComponent();
            ChartsView = new ChartsViewModel();
            this.BindingContext = ChartsView;
            picker.SelectedItem = ChartsView.Months.Count != 0 ? ChartsView.Months[0] : null;
            selected = picker.SelectedItem;
            
        }

        private void SelectedMounth(object sender, EventArgs e)
        {
            if (picker.SelectedIndex < 0)
            { 
                return;
            }
            scroll.IsRefreshing = true;
            selected = picker.SelectedItem;
            ChartsView.PickedDate((string)selected ?? "");
            ChartsView.UpdateCharts();
            scroll.IsRefreshing = false;
        }

        protected override void OnAppearing()
        {
            scroll.IsRefreshing = true;
            ChartsView.UpdateCharts();
            picker.SelectedItem = selected;
            scroll.IsRefreshing = false;
            base.OnAppearing();
        }

        public void OnRefresh(object sender, EventArgs e)
        {
            ChartsView.UpdateMonths();
            var list = (RefreshView)sender;
            list.IsRefreshing = false;
        }
    }
}