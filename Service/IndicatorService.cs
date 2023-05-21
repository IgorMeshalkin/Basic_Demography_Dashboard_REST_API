using BasicDmgAPI.DAO;
using BasicDmgAPI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Service
{ 
    public class IndicatorService
    {
        private readonly IndicatorDAO indicatorDAO;
        public IndicatorService(IndicatorDAO indicatorDAO)
        {
            this.indicatorDAO = indicatorDAO;
        }

        public List<Indicator> GetAll()
        {
            return indicatorDAO.FindAll();
        }
    }
}