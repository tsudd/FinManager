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
            CategoryViewModel categoryView = catObj as CategoryViewModel;
            if (categoryView != null && categoryView.IsValid)
            {
                if (categoryView.Category.ID == 0)
                {
                    Categories.Add(categoryView);
                }
                App.Categories.SaveCategory(categoryView.Category);
            }
            Back();

        }

        public void DelCategory(CategoryViewModel categoryView)
        {
            if (categoryView != null)
            {
                if (categoryView.Category.ID == 1)
                {
                    //dumb?
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

        public CategoryViewModel SelecetedCateg
        {
            get { return selectedCategory; }
            set
            {
                if (value != null)
                {
                    if (value.Category.ID == 0)
                    {
                        //dump??
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
            var table = new ObservableCollection<Category>(App.Categories.GetCategories());
            foreach (var i in table)
            {
                Categories.Add(new CategoryViewModel(i));
            }
        }
    }
}
