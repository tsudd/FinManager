using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SQLite;

namespace FinManager.Model
{
    [Table("Notes")]
    public class Note
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public double Sum { get; set; }
        public int CatId { get; set; }
        public int WalId{ get; set; }
        public DateTime Date { get; set; }
        [Ignore]
        public string Cat { get; set; }
        public static string GetCategory(int id)
        {
            return App.Categories.GetCategory(id).Name;
        }

    }

}
