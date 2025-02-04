using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autopark.Model
{
    public interface ILoginService
    {
        bool ValidateUser(string userName, string password);
    }
    public class LoginService : ILoginService
    {
        public bool ValidateUser(string userName, string password)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM Пользователи WHERE Логин='{userName}' AND Пароль='{password}'", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                return reader.HasRows;
            }
        }
    }
    public interface IStatusCheck
    {
        int ValidateUser(string userName);
    }
    public class StatusCheck : IStatusCheck
    {
        public int ValidateUser(string userName)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT ID_Статуса FROM Пользователи WHERE Логин='{userName}' ", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetInt32(0);
                }
                return 2;
            }
        }
    }
}
