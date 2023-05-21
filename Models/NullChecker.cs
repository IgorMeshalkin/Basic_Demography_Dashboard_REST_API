using BasicDmgAPI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Models
{
    //Объект служит для проверки не являлись ли все значения null при объединении данных по периоду. Применяется в методе FindUnitedData() класса DataDAO
    public class NullChecker
    {
        public string Name { get; set; }
        public bool Value { get; set; }
        public NullChecker(string name, bool value)
        {
            Name = name;
            Value = value;
        }

        public static List<NullChecker> GetNullCheckers()
        {
            List<NullChecker> result = new List<NullChecker>();
            IndicatorsList.Get().ForEach(indName =>
            {
                result.Add(new NullChecker(indName, false));
            });
            return result;
        }
    }
}