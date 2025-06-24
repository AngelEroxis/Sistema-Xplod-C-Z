using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WPF_LoginForm.Model
{
    public static class Database
    {
        public static SQLiteConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDbContext"].ConnectionString;
            return new SQLiteConnection(connectionString);
        }

    }
}
