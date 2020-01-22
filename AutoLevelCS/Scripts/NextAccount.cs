using AutoLevelCS.Server;
using System;
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
        public (string username, string password) GetNextAccount()
        {
            Console.Write("\rGetting Next Login Details...");

            AccountInfo ai = new AccountInfo();
            Database db = new Database();

            db.Query("SELECT * FROM `accounts` WHERE status=0 LIMIT 1");

            foreach (DataRow Item in db.Results.Rows)
            {
                ai.username = Item[1].ToString();
                ai.password = Item[2].ToString();
            }

            Console.Write("\rLogin Details Retrieved\n");

            // Return w/ Tuples
            return (ai.username, ai.password);
        }
    }
}
