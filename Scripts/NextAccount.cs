using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoLevelCS.Server;
using System.Data;

namespace AutoLevelCS.Scripts
{
    public class AccountInfo
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    class NextAccount
    {
        public static void GetNextAccount()
        {
            AccountInfo ai = new AccountInfo();
            Database db = new Database();

            db.Query("SELECT * FROM `accounts` WHERE status=0 LIMIT 1");

            foreach(DataRow Item in db.Results.Rows)
            {
                ai.username = Item[1].ToString();
                ai.password = Item[2].ToString();
            }

            Console.WriteLine($"{ai.username}, {ai.password}");
        }
    }
}
