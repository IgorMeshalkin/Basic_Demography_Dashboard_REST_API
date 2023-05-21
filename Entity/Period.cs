using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Entity
{
    public class Period
    { 
        public int Id { get; set; }
        public int SequenceNumber { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }

        public Period ()
        {

        }

        public Period(int year, int quarter)
        {
            Year = year;
            Quarter = quarter;
        }
        public Period(int id, int year, int quarter)
        {
            Id = id;
            Year = year;
            Quarter = quarter;
        }
    }
}