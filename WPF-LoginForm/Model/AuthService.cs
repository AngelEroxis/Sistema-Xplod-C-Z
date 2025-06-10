using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Configuration;


namespace WPF_LoginForm.Model
{
    public static class AuthService
    {
        public static Usuario Login(string username, string password)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM USUARIO WHERE nombre_usuario = @user AND contrasena = @pass";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password); // Ya no usamos SHA256

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Usuario
                    {
                        IdUsuario = (int)reader["id_usuario"],
                        NombreUsuario = reader["nombre_usuario"].ToString(),
                        Rol = reader["rol"].ToString()
                    };
                }

                return null;
            }
        }
    }
}
