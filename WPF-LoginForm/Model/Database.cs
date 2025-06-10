using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;


namespace WPF_LoginForm.Model
{
    public static class Database
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDbContext"].ConnectionString;
            return new SqlConnection(connectionString);
        }

    }
}
