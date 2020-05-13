using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FinManager.Model
{
    [Table("Wallets")]
    public class Wallet
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public string WalName { get; set; }
        public double Sum { get; set; }

    }
}
