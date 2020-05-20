using FinManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FinManager.ViewModel
{
    public class CategoryViewModel: INotifyPropertyChanged
    {
        CategoryListViewModel lvm;
        public event PropertyChangedEventHandler PropertyChanged;
        public Category Category { get; private set; }

        public CategoryViewModel()
        {
            Category = new Category();
        }

        public CategoryViewModel(Category category)
        {
            Category = new Category
            {
                ID = category.ID,
                Name = category.Name,
                Color = category.Color,
                InCome = category.InCome

            };
        }

        protected void OnPropertyChanged(string note)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(note));
        }

        public CategoryListViewModel LstViewModel
        {
            get { return lvm; }
            set
            {
                if (lvm != value)
                {
                    lvm = value;
                    OnPropertyChanged("LstViewModel");
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(Category.Name);
            }
        }
        public string Color
        {
            get { return Category.Color; }
            set
            {
                Category.Color = value;
            }
        }

        public string Name
        {
            get { return Category.Name; }
            set
            {
                Category.Name = value;
                OnPropertyChanged("Name");
            }
        }

    }
}
