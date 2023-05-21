using BasicDmgAPI.Entity;
using BasicDmgAPI.Util;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.DAO
{
    public class UserDAO
    {
        private readonly string CONNECTION_STRING = ConnectionStringManager.GetConnectionString();

        #region Основные методы

        //Возвращает всех юзеров из БД
        public List<User> FindAll()
        {
            List<User> result = new List<User>();

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                string commandText = $"SELECT * FROM \"Users\"";
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
                {
                    try
                    {
                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                result.Add(GetUserFromReader(rdr));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return result;
        }

        //Создаёт нового User в БД и возвращает его с присвоенным Id
        public User Create(User user)
        {
            User result = null;

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                string commandText = $"INSERT INTO \"Users\" (\"Username\", \"Password\") VALUES (@Username, @Password)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
                {
                    cmd.Parameters.AddWithValue("Username", user.Username);
                    cmd.Parameters.AddWithValue("Password", user.Password);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        return null;
                    }
                }
                result = GetByUsername(user, conn);
            }
            return result;
        }
        #endregion

        #region Вспомогательные методы

        //Возвращает объект User из объекта NpgsqlDataReader
        private User GetUserFromReader(NpgsqlDataReader rdr)
        {
            User result = new User(
                (int)rdr["ID"],
                (string)rdr["Username"],
                (string)rdr["Password"]);
            return result;
        }

        //Возвращает объект User по Username, используется в методе Create что бы не открывать лишние соединения.
        private User GetByUsername(User user, NpgsqlConnection conn)
        {
            string commandText = $"SELECT * FROM \"Users\" WHERE \"Username\" = @Username";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("Username", user.Username);

                try
                {
                    using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        return GetUserFromReader(rdr);
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion
    }
}