using System.Data;
using System.Reflection;

namespace YomiOlatunji.Wallet.BusinessCore.Helpers
{
    public static class ListExtensions
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static DataTable ToDataTable<T>(this List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            var Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in Props)
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            foreach (var item in items)
            {
                var values = new object[Props.Length];
                for (var i = 0; i < Props.Length; i++)
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                dataTable.Rows.Add(values);
            }

            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
