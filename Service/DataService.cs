using BasicDmgAPI.DAO;
using BasicDmgAPI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Service
{
    public class DataService
    {
        private readonly DataDAO dataDAO;
        private readonly PeriodDAO periodDAO;
        
        public DataService(DataDAO dataDAO, PeriodDAO periodDAO)
        {
            this.dataDAO = dataDAO;
            this.periodDAO = periodDAO;
        }

        //Возвращает данные из БД.
        public Data GetData(int districtID, int periodID)
        {
            Period period = periodDAO.FindById(periodID);
            return dataDAO.FindData(districtID, period);
        }

        //Обновляет данные в БД по ID и возвращает обновлённые данные.
        public Data Update(Data data)
        {
            return dataDAO.Update(data);
        }
    }
}