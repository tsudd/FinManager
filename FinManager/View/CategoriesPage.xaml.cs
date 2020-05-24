using FinManager.Model;
using FinManager.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;

namespace FinManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesPage : ContentPage
    {
        CategoryListViewModel categoryList;
        public CategoriesPage()
        {
            InitializeComponent();
            categoryList = new CategoryListViewModel() { Navigation = this.Navigation };
            BindingContext = categoryList;
            categoryList.UserNotify += Notification;
        }

        public void OnDelete(object sender, EventArgs e)
        {
            CategoryViewModel cat = ((MenuItem)sender).CommandParameter as CategoryViewModel;
            categoryList.DelCategory(cat);
        }

        private async void Notification(string msg)
        {
            await DisplayAlert("Message", msg, "OK");
        }

    }
}