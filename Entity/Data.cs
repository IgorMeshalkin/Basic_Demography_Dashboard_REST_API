using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Entity
{
    public class Data
    {
        public int Id { get; set; }
        public int PeriodSequenceNumber { get; set; }
        public decimal? OPZH { get; set; }
        public decimal? Birthrate { get; set; }
        public decimal? Mortality { get; set; }
        public decimal? IncreaseDecrease { get; set; }
        public decimal? InfantMortality { get; set; }
        public decimal? MaternalMortality { get; set; }
        public decimal? CirculatorySystem { get; set; }
        public decimal? COVID { get; set; }
        public decimal? Neoplasms { get; set; }
        public decimal? Respiratory { get; set; }
        public decimal? Tuberculosis { get; set; }
        public decimal? NervousSystem { get; set; }
        public decimal? EndocrineSystem { get; set; }
        public decimal? DigestiveOrgans { get; set; }
        public decimal? ExternalCauses { get; set; }
        public decimal? UncertainIntentions { get; set; }
        public decimal? TransportAccidents { get; set; }
        public decimal? DTP { get; set; }
        public decimal? Suicides { get; set; }
        public decimal? Murders { get; set; }
        public decimal? MortalityInWorkingAge { get; set; }

        public Data()
        {

        }

        public Data(decimal oPZH, decimal birthrate, decimal mortality, decimal increaseDecrease, decimal infantMortality, decimal maternalMortality, decimal circulatorySystem, decimal cOVID, decimal neoplasms, decimal respiratory, decimal tuberculosis, decimal nervousSystem, decimal endocrineSystem, decimal digestiveOrgans, decimal externalCauses, decimal uncertainIntentions, decimal transportAccidents, decimal dTP, decimal suicides, decimal murders, decimal mortalityInWorkingAge)
        {
            OPZH = oPZH;
            Birthrate = birthrate;
            Mortality = mortality;
            IncreaseDecrease = increaseDecrease;
            InfantMortality = infantMortality;
            MaternalMortality = maternalMortality;
            CirculatorySystem = circulatorySystem;
            COVID = cOVID;
            Neoplasms = neoplasms;
            Respiratory = respiratory;
            Tuberculosis = tuberculosis;
            NervousSystem = nervousSystem;
            EndocrineSystem = endocrineSystem;
            DigestiveOrgans = digestiveOrgans;
            ExternalCauses = externalCauses;
            UncertainIntentions = uncertainIntentions;
            TransportAccidents = transportAccidents;
            DTP = dTP;
            Suicides = suicides;
            Murders = murders;
            MortalityInWorkingAge = mortalityInWorkingAge;
        }

        public Data(int id, decimal oPZH, decimal birthrate, decimal mortality, decimal increaseDecrease, decimal infantMortality, decimal maternalMortality, decimal circulatorySystem, decimal cOVID, decimal neoplasms, decimal respiratory, decimal tuberculosis, decimal nervousSystem, decimal endocrineSystem, decimal digestiveOrgans, decimal externalCauses, decimal uncertainIntentions, decimal transportAccidents, decimal dTP, decimal suicides, decimal murders, decimal mortalityInWorkingAge)
        {
            Id = id;
            OPZH = oPZH;
            Birthrate = birthrate;
            Mortality = mortality;
            IncreaseDecrease = increaseDecrease;
            InfantMortality = infantMortality;
            MaternalMortality = maternalMortality;
            CirculatorySystem = circulatorySystem;
            COVID = cOVID;
            Neoplasms = neoplasms;
            Respiratory = respiratory;
            Tuberculosis = tuberculosis;
            NervousSystem = nervousSystem;
            EndocrineSystem = endocrineSystem;
            DigestiveOrgans = digestiveOrgans;
            ExternalCauses = externalCauses;
            UncertainIntentions = uncertainIntentions;
            TransportAccidents = transportAccidents;
            DTP = dTP;
            Suicides = suicides;
            Murders = murders;
            MortalityInWorkingAge = mortalityInWorkingAge;
        }

        public override bool Equals(object obj)
        {
            return obj is Data data &&
                   Id == data.Id &&
                   OPZH == data.OPZH &&
                   Birthrate == data.Birthrate &&
                   Mortality == data.Mortality &&
                   IncreaseDecrease == data.IncreaseDecrease &&
                   InfantMortality == data.InfantMortality &&
                   MaternalMortality == data.MaternalMortality &&
                   CirculatorySystem == data.CirculatorySystem &&
                   COVID == data.COVID &&
                   Neoplasms == data.Neoplasms &&
                   Respiratory == data.Respiratory &&
                   Tuberculosis == data.Tuberculosis &&
                   NervousSystem == data.NervousSystem &&
                   EndocrineSystem == data.EndocrineSystem &&
                   DigestiveOrgans == data.DigestiveOrgans &&
                   ExternalCauses == data.ExternalCauses &&
                   UncertainIntentions == data.UncertainIntentions &&
                   TransportAccidents == data.TransportAccidents &&
                   DTP == data.DTP &&
                   Suicides == data.Suicides &&
                   Murders == data.Murders &&
                   MortalityInWorkingAge == data.MortalityInWorkingAge;
        }
    }
}