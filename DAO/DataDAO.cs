using BasicDmgAPI.Entity;
using BasicDmgAPI.Models;
using BasicDmgAPI.Util;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.DAO
{
    public class DataDAO
    {
        private readonly string CONNECTION_STRING = ConnectionStringManager.GetConnectionString();

        #region Основные методы

        //Возвращает объект Data из БД
        public Data FindData(int districtID, Period period)
        {
            Data result = null;

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                string commandText = $"SELECT * FROM \"Data\" WHERE \"DistrictID\" = @districtID AND \"PeriodID\" = @periodID";
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
                {
                    cmd.Parameters.AddWithValue("districtID", districtID);
                    cmd.Parameters.AddWithValue("periodID", period.Id);

                    try
                    {
                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            result = GetDataFromReader(rdr);
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            //Присваиваю код состоящий из года и номера квартала в качестве поряднового номера периода.
            result.PeriodSequenceNumber = int.Parse(period.Year.ToString() + period.Quarter.ToString());
            return result;
        }

        //Возвращает объект Data из БД, при этом объединяет в один объект значения по всем кварталам указанного года.
        public Data FindUnitedData(int districtID, int year)
        {
            Data result = null;

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                List<int> periodsId = new List<int>();

                string periodsCommandText = $"SELECT \"ID\" FROM \"Periods\" WHERE \"Year\" = @year";

                using (NpgsqlCommand cmd = new NpgsqlCommand(periodsCommandText, conn))
                {
                    cmd.Parameters.AddWithValue("year", year);

                    try
                    {
                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                periodsId.Add((int)rdr["ID"]);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }

                string commandText = $"SELECT * FROM \"Data\" WHERE \"DistrictID\" = @districtID AND " +
                    GetPeriodsCommandString(periodsId);

                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
                {
                    cmd.Parameters.AddWithValue("districtID", districtID);

                    periodsId.ForEach(id =>
                    {
                        cmd.Parameters.AddWithValue("periodID" + (periodsId.IndexOf(id) + 1), id);
                    });

                    try
                    {
                        //получаю список объектов для контроля того не являются ли значения всех периодов по каждому показателю пустыми
                        List<NullChecker> checkers = NullChecker.GetNullCheckers();

                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            //создаю переменную которая сохранит информацию о том сколько было не пустых значений по колонке ОПЖ(для расчёта среднего значения)
                            int OPZHValuesCount = 0;

                            rdr.Read();
                            UpdateNullCheckers(checkers, rdr);
                            if (rdr["OPZH"].GetType() != typeof(DBNull))
                            {
                                if ((decimal)rdr["OPZH"] != ConditionalNumber.Get())
                                {
                                    OPZHValuesCount++;
                                }
                            }
                            result = GetDataFromReader(rdr);
                            PrepareDataToMerge(result);

                            while (rdr.Read())
                            {
                                UpdateNullCheckers(checkers, rdr);
                                if (rdr["OPZH"].GetType() != typeof(DBNull))
                                {
                                    if ((decimal)rdr["OPZH"] != ConditionalNumber.Get())
                                    {
                                        OPZHValuesCount++;
                                    }
                                }

                                result.OPZH += IsEmpty(rdr, "OPZH") || (decimal)rdr["OPZH"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["OPZH"];
                                result.Birthrate += IsEmpty(rdr, "Birthrate") || (decimal)rdr["Birthrate"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["Birthrate"];
                                result.Mortality += IsEmpty(rdr, "Mortality") || (decimal)rdr["Mortality"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["Mortality"];
                                result.IncreaseDecrease += IsEmpty(rdr, "IncreaseDecrease") || (decimal)rdr["IncreaseDecrease"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["IncreaseDecrease"];
                                result.InfantMortality += IsEmpty(rdr, "InfantMortality") || (decimal)rdr["InfantMortality"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["InfantMortality"];
                                result.MaternalMortality += IsEmpty(rdr, "MaternalMortality") || (decimal)rdr["MaternalMortality"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["MaternalMortality"];
                                result.CirculatorySystem += IsEmpty(rdr, "CirculatorySystem") || (decimal)rdr["CirculatorySystem"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["CirculatorySystem"];
                                result.COVID += IsEmpty(rdr, "COVID") || (decimal)rdr["COVID"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["COVID"];
                                result.Neoplasms += IsEmpty(rdr, "Neoplasms") || (decimal)rdr["Neoplasms"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["Neoplasms"];
                                result.Respiratory += IsEmpty(rdr, "Respiratory") || (decimal)rdr["Respiratory"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["Respiratory"];
                                result.Tuberculosis += IsEmpty(rdr, "Tuberculosis") || (decimal)rdr["Tuberculosis"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["Tuberculosis"];
                                result.NervousSystem += IsEmpty(rdr, "NervousSystem") || (decimal)rdr["NervousSystem"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["NervousSystem"];
                                result.EndocrineSystem += IsEmpty(rdr, "EndocrineSystem") || (decimal)rdr["EndocrineSystem"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["EndocrineSystem"];
                                result.DigestiveOrgans += IsEmpty(rdr, "DigestiveOrgans") || (decimal)rdr["DigestiveOrgans"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["DigestiveOrgans"];
                                result.ExternalCauses += IsEmpty(rdr, "ExternalCauses") || (decimal)rdr["ExternalCauses"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["ExternalCauses"];
                                result.UncertainIntentions += IsEmpty(rdr, "UncertainIntentions") || (decimal)rdr["UncertainIntentions"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["UncertainIntentions"];
                                result.TransportAccidents += IsEmpty(rdr, "TransportAccidents") || (decimal)rdr["TransportAccidents"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["TransportAccidents"];
                                result.DTP += IsEmpty(rdr, "DTP") || (decimal)rdr["DTP"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["DTP"];
                                result.Suicides += IsEmpty(rdr, "Suicides") || (decimal)rdr["Suicides"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["Suicides"];
                                result.Murders += IsEmpty(rdr, "Murders") || (decimal)rdr["Murders"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["Murders"];
                                result.MortalityInWorkingAge += IsEmpty(rdr, "MortalityInWorkingAge") || (decimal)rdr["MortalityInWorkingAge"] == ConditionalNumber.Get() ? 0 : (decimal)rdr["MortalityInWorkingAge"];
                            }
                            //Присваиваю полю OPZH среднее значение или ConditionalNumber (вместо null) если все значения по году были пустые
                            result.OPZH = (result.OPZH > 0 && OPZHValuesCount > 0) ? result.OPZH / OPZHValuesCount : ConditionalNumber.Get();
                        }

                        UpdateResultByCheckers(checkers, result);
                    }
                    catch
                    {
                        return null;
                    }
                }
                //Присваиваю год в качестве порядкового номера объединённого периода.
                result.PeriodSequenceNumber = year;
            }
            return result;
        }

        //Возвращает объект Data последнего квартала указанного года (при работе с накопительным итогом).
        public Data FindLastQuarterData(int districtID, int year)
        {
            Data result = null;

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                int periodID;

                string periodsCommandText = $"SELECT \"ID\" FROM \"Periods\" WHERE \"Year\" = @year AND \"Quarter\" = 4";

                using (NpgsqlCommand cmd = new NpgsqlCommand(periodsCommandText, conn))
                {
                    cmd.Parameters.AddWithValue("year", year);

                    try
                    {
                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            periodID = (int)rdr["ID"];
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }

                string commandText = $"SELECT * FROM \"Data\" WHERE \"DistrictID\" = @DistrictID AND \"PeriodID\" = @PeriodID";

                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
                {
                    cmd.Parameters.AddWithValue("DistrictID", districtID);
                    cmd.Parameters.AddWithValue("PeriodID", periodID);

                    try
                    {
                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            result = GetDataFromReader(rdr);
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }

                //Присваиваю год в качестве порядкового номера объединённого периода.
                result.PeriodSequenceNumber = year;
            }
            return result;
        }

        //Сохраняет в БД новый объект Data и возвращает его с присвоенным ID
        public Data Create(Data data, int districtID, int periodID)
        {
            Data result = data;

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                string commandText = $"INSERT INTO \"Data\" (\"DistrictID\", \"PeriodID\", \"OPZH\", \"Birthrate\", \"Mortality\", \"IncreaseDecrease\", \"InfantMortality\", \"MaternalMortality\", \"CirculatorySystem\", \"COVID\", \"Neoplasms\", \"Respiratory\", \"Tuberculosis\", \"NervousSystem\", \"EndocrineSystem\", \"DigestiveOrgans\", \"ExternalCauses\", \"UncertainIntentions\", \"TransportAccidents\", \"DTP\", \"Suicides\", \"Murders\", \"MortalityInWorkingAge\") " +
                $"VALUES (@DistrictID, @PeriodID, @OPZH, @Birthrate, @Mortality, @IncreaseDecrease, @InfantMortality, @MaternalMortality, @CirculatorySystem, @COVID, @Neoplasms, @Respiratory, @Tuberculosis, @NervousSystem, @EndocrineSystem, @DigestiveOrgans, @ExternalCauses, @UncertainIntentions, @TransportAccidents, @DTP, @Suicides, @Murders, @MortalityInWorkingAge)";

                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
                {
                    cmd.Parameters.AddWithValue("DistrictID", districtID);
                    cmd.Parameters.AddWithValue("PeriodID", periodID);
                    cmd.Parameters.AddWithValue("OPZH", data.OPZH);
                    cmd.Parameters.AddWithValue("Birthrate", data.Birthrate);
                    cmd.Parameters.AddWithValue("Mortality", data.Mortality);
                    cmd.Parameters.AddWithValue("IncreaseDecrease", data.IncreaseDecrease);
                    cmd.Parameters.AddWithValue("InfantMortality", data.InfantMortality);
                    cmd.Parameters.AddWithValue("MaternalMortality", data.MaternalMortality);
                    cmd.Parameters.AddWithValue("CirculatorySystem", data.CirculatorySystem);
                    cmd.Parameters.AddWithValue("COVID", data.COVID);
                    cmd.Parameters.AddWithValue("Neoplasms", data.Neoplasms);
                    cmd.Parameters.AddWithValue("Respiratory", data.Respiratory);
                    cmd.Parameters.AddWithValue("Tuberculosis", data.Tuberculosis);
                    cmd.Parameters.AddWithValue("NervousSystem", data.NervousSystem);
                    cmd.Parameters.AddWithValue("EndocrineSystem", data.EndocrineSystem);
                    cmd.Parameters.AddWithValue("DigestiveOrgans", data.DigestiveOrgans);
                    cmd.Parameters.AddWithValue("ExternalCauses", data.ExternalCauses);
                    cmd.Parameters.AddWithValue("UncertainIntentions", data.UncertainIntentions);
                    cmd.Parameters.AddWithValue("TransportAccidents", data.TransportAccidents);
                    cmd.Parameters.AddWithValue("DTP", data.DTP);
                    cmd.Parameters.AddWithValue("Suicides", data.Suicides);
                    cmd.Parameters.AddWithValue("Murders", data.Murders);
                    cmd.Parameters.AddWithValue("MortalityInWorkingAge", data.MortalityInWorkingAge);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        return null;
                    }
                }

                result = GetDataByDistrictAndPeriodId(districtID, periodID, conn);
            }
            return result;
        }

        //Обновляет в БД объект Data и возвращает обновлённый объект из БД
        public Data Update(Data data)
        {
            Data result = null;

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                string updateCommandText = $"UPDATE \"Data\" SET \"OPZH\" = @OPZH, \"Birthrate\" = @Birthrate, \"Mortality\" = @Mortality, \"IncreaseDecrease\" = @IncreaseDecrease, \"InfantMortality\" = @InfantMortality, \"MaternalMortality\" = @MaternalMortality, \"CirculatorySystem\" = @CirculatorySystem, \"COVID\" = @COVID, \"Neoplasms\" = @Neoplasms, \"Respiratory\" = @Respiratory, \"Tuberculosis\" = @Tuberculosis, \"NervousSystem\" = @NervousSystem, \"EndocrineSystem\" = @EndocrineSystem, \"DigestiveOrgans\" = @DigestiveOrgans, \"ExternalCauses\" = @ExternalCauses, \"UncertainIntentions\" = @UncertainIntentions, \"TransportAccidents\" = @TransportAccidents, \"DTP\" = @DTP, \"Suicides\" = @Suicides, \"Murders\" = @Murders, \"MortalityInWorkingAge\" = @MortalityInWorkingAge " +
                $"WHERE \"ID\" = @Id";

                using (NpgsqlCommand cmd = new NpgsqlCommand(updateCommandText, conn))
                {
                    cmd.Parameters.AddWithValue("OPZH", data.OPZH);
                    cmd.Parameters.AddWithValue("Birthrate", data.Birthrate);
                    cmd.Parameters.AddWithValue("Mortality", data.Mortality);
                    cmd.Parameters.AddWithValue("IncreaseDecrease", data.IncreaseDecrease);
                    cmd.Parameters.AddWithValue("InfantMortality", data.InfantMortality);
                    cmd.Parameters.AddWithValue("MaternalMortality", data.MaternalMortality);
                    cmd.Parameters.AddWithValue("CirculatorySystem", data.CirculatorySystem);
                    cmd.Parameters.AddWithValue("COVID", data.COVID);
                    cmd.Parameters.AddWithValue("Neoplasms", data.Neoplasms);
                    cmd.Parameters.AddWithValue("Respiratory", data.Respiratory);
                    cmd.Parameters.AddWithValue("Tuberculosis", data.Tuberculosis);
                    cmd.Parameters.AddWithValue("NervousSystem", data.NervousSystem);
                    cmd.Parameters.AddWithValue("EndocrineSystem", data.EndocrineSystem);
                    cmd.Parameters.AddWithValue("DigestiveOrgans", data.DigestiveOrgans);
                    cmd.Parameters.AddWithValue("ExternalCauses", data.ExternalCauses);
                    cmd.Parameters.AddWithValue("UncertainIntentions", data.UncertainIntentions);
                    cmd.Parameters.AddWithValue("TransportAccidents", data.TransportAccidents);
                    cmd.Parameters.AddWithValue("DTP", data.DTP);
                    cmd.Parameters.AddWithValue("Suicides", data.Suicides);
                    cmd.Parameters.AddWithValue("Murders", data.Murders);
                    cmd.Parameters.AddWithValue("MortalityInWorkingAge", data.MortalityInWorkingAge);
                    cmd.Parameters.AddWithValue("Id", data.Id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        return null;
                    }
                }

                string getCommandText = $"SELECT * FROM \"Data\" WHERE \"ID\" = @Id";
                using (NpgsqlCommand cmd = new NpgsqlCommand(getCommandText, conn))
                {
                    cmd.Parameters.AddWithValue("Id", data.Id);

                    try
                    {
                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            result = GetDataFromReader(rdr);
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
        #endregion

        #region Вспомогательные методы

        //Возвращает объект Data из объекта NpgsqlDataReader
        private Data GetDataFromReader(NpgsqlDataReader rdr)
        {
            Data result = new Data((int)rdr["ID"]
                , rdr["OPZH"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["OPZH"]
                , rdr["Birthrate"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["Birthrate"]
                , rdr["Mortality"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["Mortality"]
                , rdr["IncreaseDecrease"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["IncreaseDecrease"]
                , rdr["InfantMortality"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["InfantMortality"]
                , rdr["MaternalMortality"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["MaternalMortality"]
                , rdr["CirculatorySystem"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["CirculatorySystem"]
                , rdr["COVID"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["COVID"]
                , rdr["Neoplasms"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["Neoplasms"]
                , rdr["Respiratory"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["Respiratory"]
                , rdr["Tuberculosis"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["Tuberculosis"]
                , rdr["NervousSystem"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["NervousSystem"]
                , rdr["EndocrineSystem"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["EndocrineSystem"]
                , rdr["DigestiveOrgans"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["DigestiveOrgans"]
                , rdr["ExternalCauses"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["ExternalCauses"]
                , rdr["UncertainIntentions"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["UncertainIntentions"]
                , rdr["TransportAccidents"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["TransportAccidents"]
                , rdr["DTP"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["DTP"]
                , rdr["Suicides"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["Suicides"]
                , rdr["Murders"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["Murders"]
                , rdr["MortalityInWorkingAge"].GetType() == typeof(DBNull) ? ConditionalNumber.Get() : (decimal)rdr["MortalityInWorkingAge"]);

            return result;
        }

        //Возвращает объект Data по ID района и периода. Используется в методах Create() и Update() что бы не открывать лишние соединения. 
        private Data GetDataByDistrictAndPeriodId(int districtID, int periodID, NpgsqlConnection conn)
        {
            Data result = null;

            string commandText = $"SELECT * FROM \"Data\" WHERE \"DistrictID\" = @districtID AND \"PeriodID\" = @periodID";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("districtID", districtID);
                cmd.Parameters.AddWithValue("periodID", periodID);

                try
                {
                    using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        result = GetDataFromReader(rdr);
                    }
                }
                catch
                {
                    return null;
                }
            }

            return result;
        }

        //Возвращает строку на основании списка ID периодов для вставки в поисковый запрос
        private string GetPeriodsCommandString(List<int> periodsId)
        {
            string result = " (";
            periodsId.ForEach(id =>
            {
                result = result + "\"PeriodID\" = @periodID" + (periodsId.IndexOf(id) + 1) + " ";

                if (periodsId.IndexOf(id) != periodsId.Count - 1)
                {
                    result = result + "OR ";
                }
                else
                {
                    result = result + ")";
                }
            });

            return result;
        }

        //Фиксирует и сохраняет в списке checkers если по каждому полю из БД приходит не null
        private void UpdateNullCheckers(List<NullChecker> checkers, NpgsqlDataReader rdr)
        {
            checkers.ForEach(checker =>
            {
                if (rdr[checker.Name].GetType() != typeof(DBNull))
                {
                    if ((decimal)rdr[checker.Name] != ConditionalNumber.Get())
                    {
                        checker.Value = true;
                    }
                }
            });
        }

        //Присваивает некоторым полям ConditionalNumber(вместо null) согласно списку checkers
        private void UpdateResultByCheckers(List<NullChecker> checkers, Data result)
        {
            checkers.ForEach(checker =>
            {
                if (!checker.Value)
                {
                    switch (checker.Name)
                    {
                        case "OPZH":
                            result.OPZH = ConditionalNumber.Get();
                            break;
                        case "Birthrate":
                            result.Birthrate = ConditionalNumber.Get();
                            break;
                        case "Mortality":
                            result.Mortality = ConditionalNumber.Get();
                            break;
                        case "IncreaseDecrease":
                            result.IncreaseDecrease = ConditionalNumber.Get();
                            break;
                        case "InfantMortality":
                            result.InfantMortality = ConditionalNumber.Get();
                            break;
                        case "MaternalMortality":
                            result.MaternalMortality = ConditionalNumber.Get();
                            break;
                        case "CirculatorySystem":
                            result.CirculatorySystem = ConditionalNumber.Get();
                            break;
                        case "COVID":
                            result.COVID = ConditionalNumber.Get();
                            break;
                        case "Neoplasms":
                            result.Neoplasms = ConditionalNumber.Get();
                            break;
                        case "Respiratory":
                            result.Respiratory = ConditionalNumber.Get();
                            break;
                        case "Tuberculosis":
                            result.Tuberculosis = ConditionalNumber.Get();
                            break;
                        case "NervousSystem":
                            result.NervousSystem = ConditionalNumber.Get();
                            break;
                        case "EndocrineSystem":
                            result.EndocrineSystem = ConditionalNumber.Get();
                            break;
                        case "DigestiveOrgans":
                            result.DigestiveOrgans = ConditionalNumber.Get();
                            break;
                        case "ExternalCauses":
                            result.ExternalCauses = ConditionalNumber.Get();
                            break;
                        case "UncertainIntentions":
                            result.UncertainIntentions = ConditionalNumber.Get();
                            break;
                        case "TransportAccidents":
                            result.TransportAccidents = ConditionalNumber.Get();
                            break;
                        case "DTP":
                            result.DTP = ConditionalNumber.Get();
                            break;
                        case "Suicides":
                            result.Suicides = ConditionalNumber.Get();
                            break;
                        case "Murders":
                            result.Murders = ConditionalNumber.Get();
                            break;
                        case "MortalityInWorkingAge":
                            result.MortalityInWorkingAge = ConditionalNumber.Get();
                            break;
                    }
                }
            });
        }

        //Проверяет возвращается ли пустое значение из БД (null или -1)
        private bool IsEmpty(NpgsqlDataReader rdr, string indicator)
        {
            bool result = false;

            if (rdr[indicator].GetType() == typeof(DBNull) || (decimal)rdr[indicator] == ConditionalNumber.Get())
            {
                result = true;
            }
            return result;
        }

        //Заменяет значения равные ConditionalNumber на 0, для объединения;
        private Data PrepareDataToMerge(Data data)
        {
            data.OPZH = data.OPZH == ConditionalNumber.Get() ? 0 : data.OPZH;
            data.Birthrate = data.Birthrate == ConditionalNumber.Get() ? 0 : data.Birthrate;
            data.Mortality = data.Mortality == ConditionalNumber.Get() ? 0 : data.Mortality;
            data.IncreaseDecrease = data.IncreaseDecrease == ConditionalNumber.Get() ? 0 : data.IncreaseDecrease;
            data.InfantMortality = data.InfantMortality == ConditionalNumber.Get() ? 0 : data.InfantMortality;
            data.MaternalMortality = data.MaternalMortality == ConditionalNumber.Get() ? 0 : data.MaternalMortality;
            data.CirculatorySystem = data.CirculatorySystem == ConditionalNumber.Get() ? 0 : data.CirculatorySystem;
            data.COVID = data.COVID == ConditionalNumber.Get() ? 0 : data.COVID;
            data.Neoplasms = data.Neoplasms == ConditionalNumber.Get() ? 0 : data.Neoplasms;
            data.Respiratory = data.Respiratory == ConditionalNumber.Get() ? 0 : data.Respiratory;
            data.Tuberculosis = data.Tuberculosis == ConditionalNumber.Get() ? 0 : data.Tuberculosis;
            data.NervousSystem = data.NervousSystem == ConditionalNumber.Get() ? 0 : data.NervousSystem;
            data.EndocrineSystem = data.EndocrineSystem == ConditionalNumber.Get() ? 0 : data.EndocrineSystem;
            data.DigestiveOrgans = data.DigestiveOrgans == ConditionalNumber.Get() ? 0 : data.DigestiveOrgans;
            data.ExternalCauses = data.ExternalCauses == ConditionalNumber.Get() ? 0 : data.ExternalCauses;
            data.UncertainIntentions = data.UncertainIntentions == ConditionalNumber.Get() ? 0 : data.UncertainIntentions;
            data.TransportAccidents = data.TransportAccidents == ConditionalNumber.Get() ? 0 : data.TransportAccidents;
            data.DTP = data.DTP == ConditionalNumber.Get() ? 0 : data.DTP;
            data.Suicides = data.Suicides == ConditionalNumber.Get() ? 0 : data.Suicides;
            data.Murders = data.Murders == ConditionalNumber.Get() ? 0 : data.Murders;
            data.MortalityInWorkingAge = data.MortalityInWorkingAge == ConditionalNumber.Get() ? 0 : data.MortalityInWorkingAge;
            return data;
        }
        #endregion
    }
}