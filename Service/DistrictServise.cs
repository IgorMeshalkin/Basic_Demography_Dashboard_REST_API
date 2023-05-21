using BasicDmgAPI.DAO;
using BasicDmgAPI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Service
{
    public class DistrictServise
    {
        private readonly DistrictDAO districtDAO;
        private readonly PeriodDAO periodDAO;
        private readonly DataDAO dataDAO;

        public DistrictServise(DistrictDAO districtDAO, PeriodDAO periodDAO, DataDAO dataDAO)
        {
            this.districtDAO = districtDAO;
            this.periodDAO = periodDAO;
            this.dataDAO = dataDAO;
        }

        //Возвращает список районов с данными сгруппированными по периодам
        public List<District> GetAll()
        {
            List<District> districts = districtDAO.FindAll();
            List<Period> periods = periodDAO.FindAll();

            districts.ForEach(d =>
            {
                d.Data = new List<Data>();
                List<int> alreadyEndedYears = new List<int>();

                periods.ForEach(p =>
                {
                    if (p.Year == DateTime.Now.Year)
                    {
                        d.Data.Add(dataDAO.FindData(d.Id, p));
                    }
                    else
                    {
                        if (!alreadyEndedYears.Exists(i => i.Equals(p.Year)))
                        {
                            d.Data.Add(dataDAO.FindLastQuarterData(d.Id, p.Year));
                            alreadyEndedYears.Add(p.Year);
                        }
                    }
                });
            });
            return districts;
        }

        //Возвращает список районов без данных
        public List<District> GetAllWithoutData()
        {
            return districtDAO.FindAll();
        }
    }
}