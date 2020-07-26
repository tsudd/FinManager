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

        private void SaveCategory(object catObj)
        {
            if (catObj is CategoryViewModel categoryView && categoryView.IsValid)
            {
                if (CheckIdent(categoryView.Name))
                {
                    App.OnNotify("Same category already exists! Can't make another.");
                    return;
                }
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
                App.SyncCategories();
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
                    App.OnNotify("You can't delete this category!");
                    return;
                }
                Categories.Remove(categoryView);
                App.Notes.AdjustNotesByCat(categoryView.Category.ID);
                App.Categories.DeleteCategory(categoryView.Category.ID);
                App.SyncCategories();
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
                        App.OnNotify("You can't edit this category!");
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
            foreach (var i in App.CategoriesList)
            {
                Categories.Add(new CategoryViewModel(i));
            }
        }

        private static bool CheckIdent(string name)
        {
            foreach (var i in App.CategoriesList)
            {
                if (i.Name == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
