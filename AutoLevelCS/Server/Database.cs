using System;
using System.Data;
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
                Console.WriteLine($"Ex No:{ex.Number} - Error opening connection to database.");
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
                // Error executing query
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
