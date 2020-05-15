using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FinManager.Model
{
    [Table("Categories")]
    public class Category
    {
        public enum Colors
        {
            Aqua,
            Cadetblue
        }
        

        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }

        public string Color { get; set; }
        public string Name { get; set; }
        public byte InCome { get; set; }

        public Category()
        {
            Color = "#FF0000";
            Name = "SOMETHING";
        }

        public Category(string nm, string clr, byte inCom = 0)
        {
            Color = clr;
            Name = nm.Trim().ToUpper();
            InCome = inCom;
        }

        public static string GetColorHex(Colors color)
        {
            switch (color)
            {
                case Colors.Aqua: return "#00FFFF";
                case Colors.Cadetblue: return "#5F9EA0";
                default: return "#FF0000";
            }
        }
    }
}
