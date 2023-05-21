using BasicDmgAPI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Models
{
    public class DataWithCredentials
    {
        public string Password { get; set; }
        public int Id { get; set; }
        public int PeriodSequenceNumber { get; set; }
        public decimal OPZH { get; set; }
        public decimal Birthrate { get; set; }
        public decimal Mortality { get; set; }
        public decimal IncreaseDecrease { get; set; }
        public decimal InfantMortality { get; set; }
        public decimal MaternalMortality { get; set; }
        public decimal CirculatorySystem { get; set; }
        public decimal COVID { get; set; }
        public decimal Neoplasms { get; set; }
        public decimal Respiratory { get; set; }
        public decimal Tuberculosis { get; set; }
        public decimal NervousSystem { get; set; }
        public decimal EndocrineSystem { get; set; }
        public decimal DigestiveOrgans { get; set; }
        public decimal ExternalCauses { get; set; }
        public decimal UncertainIntentions { get; set; }
        public decimal TransportAccidents { get; set; }
        public decimal DTP { get; set; }
        public decimal Suicides { get; set; }
        public decimal Murders { get; set; }
        public decimal MortalityInWorkingAge { get; set; }

        public Data GetData()
        {
            Data result = new Data(
                            this.Id,
                            this.OPZH,
                            this.Birthrate,
                            this.Mortality,
                            this.IncreaseDecrease,
                            this.InfantMortality,
                            this.MaternalMortality,
                            this.CirculatorySystem,
                            this.COVID,
                            this.Neoplasms,
                            this.Respiratory,
                            this.Tuberculosis,
                            this.NervousSystem,
                            this.EndocrineSystem,
                            this.DigestiveOrgans,
                            this.ExternalCauses,
                            this.UncertainIntentions,
                            this.TransportAccidents,
                            this.DTP,
                            this.Suicides,
                            this.Murders,
                            this.MortalityInWorkingAge);

            return result;
        }
    }
}