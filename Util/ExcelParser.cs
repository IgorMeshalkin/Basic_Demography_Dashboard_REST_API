using BasicDmgAPI.DAO;
using BasicDmgAPI.Entity;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Util
{
    public class ExcelParser
    {

        public static void Parse()
        {
            DataDAO dataDAO = new DataDAO();
            PeriodDAO periodDAO = new PeriodDAO();

            List<Data> result = new List<Data>();

            Application excelApp = new Application();

            if (excelApp == null)
            {
                Console.WriteLine("Excel is not installed!!");
                return;
            }

            Workbook excelBook = excelApp.Workbooks.Open(@"C:\Users\MeshalkinIE\Desktop\Данные для дашборда МЗ ХК 19_22.xlsx");
            _Worksheet excelSheet = excelBook.Sheets[4];
            Range excelRange = excelSheet.UsedRange;

            for (int i = 3; i <= 24; i++)
            {
                Period period = periodDAO.FindById(13);
                int DistrictID = Convert.ToInt32(excelRange.Cells[i, 1].Value);
                Data data = dataDAO.FindData(DistrictID, period);

                data.OPZH = Convert.ToString(excelRange.Cells[i, 3].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 3].Value) : ConditionalNumber.Get();
                data.Birthrate = Convert.ToString(excelRange.Cells[i, 4].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 4].Value) : ConditionalNumber.Get();
                data.Mortality = Convert.ToString(excelRange.Cells[i, 5].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 5].Value) : ConditionalNumber.Get();
                data.IncreaseDecrease = Convert.ToString(excelRange.Cells[i, 6].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 6].Value) : ConditionalNumber.Get();
                data.InfantMortality = Convert.ToString(excelRange.Cells[i, 7].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 7].Value) : ConditionalNumber.Get();
                data.MaternalMortality = Convert.ToString(excelRange.Cells[i, 8].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 8].Value) : ConditionalNumber.Get();
                data.CirculatorySystem = Convert.ToString(excelRange.Cells[i, 9].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 9].Value) : ConditionalNumber.Get();
                data.COVID = Convert.ToString(excelRange.Cells[i, 10].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 10].Value) : ConditionalNumber.Get();
                data.Neoplasms = Convert.ToString(excelRange.Cells[i, 11].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 11].Value) : ConditionalNumber.Get();
                data.Respiratory = Convert.ToString(excelRange.Cells[i, 12].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 12].Value) : ConditionalNumber.Get();
                data.Tuberculosis = Convert.ToString(excelRange.Cells[i, 13].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 13].Value) : ConditionalNumber.Get();
                data.NervousSystem = Convert.ToString(excelRange.Cells[i, 14].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 14].Value) : ConditionalNumber.Get();
                data.EndocrineSystem = Convert.ToString(excelRange.Cells[i, 15].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 15].Value) : ConditionalNumber.Get();
                data.DigestiveOrgans = Convert.ToString(excelRange.Cells[i, 16].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 16].Value) : ConditionalNumber.Get();
                data.ExternalCauses = Convert.ToString(excelRange.Cells[i, 17].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 17].Value) : ConditionalNumber.Get();
                data.UncertainIntentions = Convert.ToString(excelRange.Cells[i, 18].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 18].Value) : ConditionalNumber.Get();
                data.TransportAccidents = Convert.ToString(excelRange.Cells[i, 19].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 19].Value) : ConditionalNumber.Get();
                data.DTP = Convert.ToString(excelRange.Cells[i, 20].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 20].Value) : ConditionalNumber.Get();
                data.Suicides = Convert.ToString(excelRange.Cells[i, 21].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 21].Value) : ConditionalNumber.Get();
                data.Murders = Convert.ToString(excelRange.Cells[i, 22].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 22].Value) : ConditionalNumber.Get();
                data.MortalityInWorkingAge = Convert.ToString(excelRange.Cells[i, 23].Value) != "S" ? Convert.ToDecimal(excelRange.Cells[i, 23].Value) : ConditionalNumber.Get(); ;

                dataDAO.Update(data);
            }
            excelBook.Save();
            excelApp.Quit();
        }
        private decimal GetDecimalValue(string value)
        {
            try
            {
                return Decimal.Parse(value);
            }
            catch
            {
                return ConditionalNumber.Get();
            }
;
        }
    }
}