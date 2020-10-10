using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;


namespace ManagerMap
{
    class Connection
    {
        
        private static MySqlConnection get_connection() // Return a connection to the database
        {
            while (true)
            {
                try
                {
                    MySqlConnection con = new MySqlConnection($"Server={Constants.HOST};Database={Constants.DBS};Uid={Constants.USER};Pwd={Constants.PASS};Port={Constants.PORT};");
                    con.Open();
                    return con;
                }
                catch
                {
                    Console.Write("Error creating the connection!");
                }
            }
        }

        public static List<DataRow> execute_procedure(string querry)
        {
            List<DataRow> res = new List<DataRow>();
            MySqlConnection con = get_connection();
            MySqlDataAdapter adapter = new MySqlDataAdapter(querry, con);
            con.Close();
            DataTable data = new DataTable();
            adapter.Fill(data);
            foreach(DataRow row in data.Rows)
            {
                res.Add(row);
            }    
            return res;
        }

        
    }
}
