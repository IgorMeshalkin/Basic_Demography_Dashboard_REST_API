using BasicDmgAPI.Entity;
using BasicDmgAPI.Util;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.DAO
{
    public class IndicatorDAO
    {
        private readonly string CONNECTION_STRING = ConnectionStringManager.GetConnectionString();

        //Возвращает все индикаторы из БД
        public List<Indicator> FindAll()
        {
            List<Indicator> result = new List<Indicator>();

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                string commandText = $"SELECT * FROM \"Indicators\"";
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

        //Возвращает объект Indicator из объекта NpgsqlDataReader
        private Indicator GetPeriodFromReader(NpgsqlDataReader rdr)
        {
            Indicator result = new Indicator(
            (int)rdr["ID"],
            (string)rdr["RussianName"],
            (string)rdr["Title"],
            (string)rdr["EnglishName"]);

            return result;
        }
    }
}