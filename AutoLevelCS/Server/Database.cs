using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

namespace AutoLevelCS.Server
{
    class Database
    {
        MySqlDataAdapter adapter;
        public DataTable tbl = new DataTable();
        private MySqlConnection conn;
        private string server;
        private string port;
        private string database;
        private string dbUsername;
        private string dbPassword;
        private DataTable _results;


        public DataTable Results
        {
            get => _results;
            set { _results = value;}
        }

        public void Query(string query)
        {
            Connect();
            ExecuteQuery(query);
        }

        private void Connect()
        {
            server = "192.168.0.10";
            port = "3306";
            database = "auto_level";
            dbUsername = "auto_level_user";
            dbPassword = "F6he2oGiD1VaQ1xulAW148rA5eFEC7";

            conn = new MySqlConnection($"SERVER={server};PORT={port};USERNAME={dbUsername};PASSWORD={dbPassword};DATABASE={database}");
        }

        private bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine($"{ex} Cannot connect to server.");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void ExecuteQuery(string query)
        {
            if (this.OpenConnection() != true)
            {
                // Exit if connection failed
                Environment.Exit(2);
            }

            try
            {
                adapter = new MySqlDataAdapter(query, conn);
                adapter.Fill(tbl);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                //close connection
                this.CloseConnection();

                // Return results
                _results = tbl;
            }
        }
    }
}
