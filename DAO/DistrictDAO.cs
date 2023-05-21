using BasicDmgAPI.Entity;
using BasicDmgAPI.Util;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.DAO
{
    public class DistrictDAO
    {
        private readonly string CONNECTION_STRING = ConnectionStringManager.GetConnectionString();

        //Возвращает все районы из БД
        public List<District> FindAll()
        {
            List<District> result = new List<District>();

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                string commandText = $"SELECT * FROM \"Districs\"";
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
                {
                    try
                    {
                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                result.Add(GetDistrictFromReader(rdr));
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

        //Возвращает объект District из объекта NpgsqlDataReader
        private District GetDistrictFromReader(NpgsqlDataReader rdr)
        {
            District result = new District();
            result.Id = (int)rdr["ID"];
            result.ShortName = (string)rdr["ShortName"];
            result.FullName = (string)rdr["FullName"];

            return result;
        }
    }
}