using FinManager.Model;
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
    public partial class CategoryPage : ContentPage
    {
        public CategoryViewModel CategoryView { get; private set; }
        private Button last;
        public CategoryPage(CategoryViewModel viewModel)
        {
            CategoryView = viewModel;
            this.BindingContext = CategoryView;
            InitializeComponent();
            switcher.IsToggled = CategoryView.Category.InCome;
        }

        private void Switchh(object sender, ToggledEventArgs e)
        {
            CategoryView.Category.InCome = e.Value;
        }

        private void ChooseColor(object sender, EventArgs e)
        {
            if (last != null)
            {
                last.BorderColor = Color.FromHex(App.theme);
            }
            last = (Button)sender;
            last.BorderColor = Color.FromHex("#1E90FF");
            CategoryView.Color = last.BackgroundColor.ToHex();
        }
    }
}