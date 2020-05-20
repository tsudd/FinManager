using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;

namespace FinManager.Model
{
    [Table("Categories")]
    public class Category
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }

        public string Color { get; set; }
        private string name;
        public bool InCome { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value.Trim().ToUpper();
            }
        }

        public Category()
        {
            Color = "#DC143C";
            Name = "SOMETHING";
        }

        public Category(string nm, string clr, bool inCom = false)
        {
            Color = clr;
            Name = nm.Trim().ToUpper();
            InCome = inCom;
        }
    }
}
