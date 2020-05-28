using FinManager.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FinManager.ViewModel
{
    class SettingsViewModel
    {
        public ICommand ChangeThemeCommand { protected set; get; }
        public ICommand ManageWalletsCommand { protected set; get; }
        public ICommand ManageCategoriesCommand { protected set; get; }
        public ICommand ChangeLanguageCommand { protected set; get; }
        public INavigation Navigation { get; set; }

        public SettingsViewModel()
        {
            ChangeThemeCommand = new Command(ChangeTheme);
            ManageWalletsCommand = new Command(ManageWallets);
            ManageCategoriesCommand = new Command(ManageCategories);
            ChangeLanguageCommand = new Command(ChangeLanguage);
        }

        private void ChangeTheme()
        {
            
        }

        private void ManageWallets()
        {
            Navigation.PushAsync(new WalletsPage());
        }

        private void ManageCategories()
        {
            Navigation.PushAsync(new CategoriesPage());
        }

        private void ChangeLanguage()
        {

        }
    }
}
