using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



namespace WPF_LoginForm.Model
{
    public static class AuthService
    {
        public static Usuario Login(string username, string password)
        {
            using (SQLiteConnection conn = Database.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM USUARIO WHERE nombre_usuario = @user AND contrasena = @pass";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                NombreUsuario = reader["nombre_usuario"].ToString(),
                                Rol = reader["rol"].ToString(),
                                IdVendedor = reader["id_vendedor"] != DBNull.Value ? Convert.ToInt32(reader["id_vendedor"]) : 0
                            };
                        }
                    }
                }
            }

            return null;
        }
    }

}
