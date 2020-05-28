using FinManager.Model;
using FinManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using System.Xml.Schema;
using Xamarin.Forms;

namespace FinManager.ViewModel
{
    public class CategoryListViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<CategoryViewModel> Categories { get; set; }
        public ICommand AddCategoryCommand { get; protected set; }
        public ICommand SaveCategoryCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        public INavigation Navigation { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void Notificat(string msg);
        public event Notificat UserNotify;
        private CategoryViewModel selectedCategory;
        public CategoryListViewModel()
        {
            Categories = new ObservableCollection<CategoryViewModel>();
            GetItems();
            AddCategoryCommand = new Command(AddCategory);
            SaveCategoryCommand = new Command(SaveCategory);
            BackCommand = new Command(Back);
        }
        private void AddCategory()
        {
            Navigation.PushAsync(new CategoryPage(new CategoryViewModel() { LstViewModel = this })) ;
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        protected void OnNotify(string msg)
        {
            UserNotify?.Invoke(msg);
        }

        private void SaveCategory(object catObj)
        {
            CategoryViewModel categoryView = catObj as CategoryViewModel;
            if (categoryView != null && categoryView.IsValid)
            {
                if (categoryView.Category.ID == 0)
                {
                    Categories.Add(categoryView);
                }
                else
                {
                    GetItems();
                    OnPropertyChanged("Categories");
                    App.Wallets.CalcNotes(categoryView.Category.ID, false, true);
                }
                App.Categories.SaveCategory(categoryView.Category);
                App.Wallets.CalcNotes(categoryView.Category.ID, true, false);
                App.Wallets.BalanceSync();
            }
            Back();

        }

        public void DelCategory(CategoryViewModel categoryView)
        {
            if (categoryView != null)
            {
                if (categoryView.Category.ID == 1)
                {
                    OnNotify("You can't delete this category!");
                    return;
                }
                Categories.Remove(categoryView);
                App.Notes.AdjustNotes(categoryView.Category.ID);
                App.Categories.DeleteCategory(categoryView.Category.ID);
                OnPropertyChanged("Categories");
            }
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        public CategoryViewModel SelectedCateg
        {
            get { return selectedCategory; }
            set
            {
                if (value != null)
                {
                    if (value.Category.ID == 1)
                    {
                        OnNotify("You can't change this category!");
                        selectedCategory = null;
                        OnPropertyChanged("SelectedCateg");
                        return;
                    }
                    CategoryViewModel tempCat = value;
                    tempCat.LstViewModel = this;
                    selectedCategory = null;
                    OnPropertyChanged("SelectedCateg");
                    Navigation.PushAsync(new CategoryPage(tempCat));
                }
            }
        }
        private void GetItems()
        {
            Categories.Clear();
            var table = new ObservableCollection<Category>(App.Categories.GetCategories());
            foreach (var i in table)
            {
                Categories.Add(new CategoryViewModel(i));
            }
        }
    }
}
