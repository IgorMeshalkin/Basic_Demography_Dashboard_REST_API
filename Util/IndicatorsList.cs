using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Util
{
    public class IndicatorsList
    {
        public static List<string> Get()
        {
            List<string> result = new List<string>();
            result.Add("OPZH");
            result.Add("Birthrate");
            result.Add("Mortality");
            result.Add("IncreaseDecrease");
            result.Add("InfantMortality");
            result.Add("MaternalMortality");
            result.Add("CirculatorySystem");
            result.Add("COVID");
            result.Add("Neoplasms");
            result.Add("Respiratory");
            result.Add("Tuberculosis");
            result.Add("NervousSystem");
            result.Add("EndocrineSystem");
            result.Add("DigestiveOrgans");
            result.Add("ExternalCauses");
            result.Add("UncertainIntentions");
            result.Add("TransportAccidents");
            result.Add("DTP");
            result.Add("Suicides");
            result.Add("Murders");
            result.Add("MortalityInWorkingAge");
            return result;
        }
    }
}