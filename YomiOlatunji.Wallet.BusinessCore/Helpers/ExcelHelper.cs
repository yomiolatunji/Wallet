using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YomiOlatunji.Wallet.BusinessCore.Helpers
{
    public class ExcelHelper
    {
        public static byte[] ConvertListToExcelByte(DataTable data)
        {
            byte[] content;
            using (var workbook = new XLWorkbook())
            {
                workbook.Worksheets.Add(data, "Report");

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    content = stream.ToArray();
                }
            }

            return content;
        }

        public static byte[] ConvertListToExcelByte<T>(List<T> data)
        {
            var dataTable = data.ToDataTable();
            return ConvertListToExcelByte(dataTable);
        }
    }
}
