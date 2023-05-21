using BasicDmgAPI.DAO;
using BasicDmgAPI.Entity;
using BasicDmgAPI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Service
{
    public class PeriodService
    {
        private readonly PeriodDAO periodDAO;
        private readonly DistrictDAO districtDAO;
        private readonly DataDAO dataDAO;

        public PeriodService(PeriodDAO periodDAO, DistrictDAO districtDAO, DataDAO dataDAO)
        {
            this.periodDAO = periodDAO;
            this.districtDAO = districtDAO;
            this.dataDAO = dataDAO;
        }

        //Возвращает все периоды. 
        public List<Period> GetAll()
        {
            List<Period> result = periodDAO.FindAll();
            return result;
        }

        //Возвращает все периоды, при этом объединяет периоды в один если календарный год уже закончился. 
        public List<Period> GetAllUnited()
        {
            List<Period> periodsFromDAO = periodDAO.FindAll();
            List<Period> result = new List<Period>();
            List<int> unitedYears = new List<int>();

            periodsFromDAO.ForEach(period =>
            {
                if (DateTime.Now.Year == period.Year)
                {
                    //Присваиваю код состоящий из года и номера квартала в качестве поряднового номера периода.
                    period.SequenceNumber = int.Parse(period.Year.ToString() + period.Quarter.ToString());
                    result.Add(period);
                } 
                else
                {
                    if (!unitedYears.Contains(period.Year))
                    {
                        //Объединяю периоды прошедшего года в один, при этом Id объединённого периода назначаю Id первого периода этого года, что соответствует требованию уникальности Id при дальнейшей работе с Frontend.
                        Period unitedPeriod = new Period(period.Id, period.Year, 0);

                        //Присваиваю год в качестве порядкового номера объединённого периода.
                        unitedPeriod.SequenceNumber = period.Year;
                        result.Add(unitedPeriod);
                        unitedYears.Add(period.Year);
                    }
                }
            });

            return result;
        }

        public Period Create(Period period)
        {
            Period result = periodDAO.Create(period);
            if (result != null && result.Id > 0)
            {
                districtDAO.FindAll().ForEach(district =>
                {
                    dataDAO.Create(new Data(
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get(),
                        ConditionalNumber.Get()
                        ), district.Id, result.Id);
                });
            }

            return result;
        }

    }
}