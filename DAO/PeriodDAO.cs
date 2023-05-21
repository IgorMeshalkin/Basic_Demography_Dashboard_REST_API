using BasicDmgAPI.Entity;
using BasicDmgAPI.Util;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.DAO
{
    public class PeriodDAO
    {
        private readonly string CONNECTION_STRING = ConnectionStringManager.GetConnectionString();

        //Возвращает все периоды из БД
        public List<Period> FindAll()
        {
            List<Period> result = new List<Period>();

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                string commandText = $"SELECT * FROM \"Periods\"";
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
                {
                    try
                    {
                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                result.Add(GetPeriodFromReader(rdr));
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

        //Возвращает период по ID
        public Period FindById(int id)
        {
            Period result = null;

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                string commandText = $"SELECT * FROM \"Periods\" WHERE \"ID\" = @id";
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    try
                    {
                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            result = GetPeriodFromReader(rdr);

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

        //Сохраняет в БД новый объект Period и возвращает его с присвоенным ID
        public Period Create(Period period)
        {
            Period result = period;

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                if (FindByYearAndQuarter(period.Year, period.Quarter, conn) == null) //проверяю что периода с таким годом и кварталом нет в БД
                {
                    string commandText = $"INSERT INTO \"Periods\" (\"Year\", \"Quarter\") " +
                    $"VALUES (@Year, @Quarter)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
                    {
                        cmd.Parameters.AddWithValue("Year", period.Year);
                        cmd.Parameters.AddWithValue("Quarter", period.Quarter);

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch
                        {
                            return null;
                        }
                    }
                    result = FindByYearAndQuarter(period.Year, period.Quarter, conn);
                }
            }
            return result; 
        }

        //Возвращает объект Period из объекта NpgsqlDataReader
        private Period GetPeriodFromReader(NpgsqlDataReader rdr)
        {
            Period result = new Period(
            (int)rdr["ID"],
            (int)rdr["Year"],
            (int)rdr["Quarter"]);

            return result;
        }

        //Возвращает период по году и кварталу, используется в методе Create()
        private Period FindByYearAndQuarter(int year, int quarter, NpgsqlConnection conn)
        {
            Period result = null;

            string commandText = $"SELECT * FROM \"Periods\" WHERE \"Year\" = @Year AND \"Quarter\" = @Quarter";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("Year", year);
                cmd.Parameters.AddWithValue("Quarter", quarter);
                try
                {
                    using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        result = GetPeriodFromReader(rdr);

                    }
                }
                catch
                {
                    return null;
                }
            }

            return result;
        }
    }
}