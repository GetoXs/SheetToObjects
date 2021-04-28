using System.Collections.Generic;
using System.Linq;

namespace SheetToObjects.Lib
{
    public class Sheet
    {
        public List<Row> Rows { get; }

        public Sheet(List<Row> rows)
        {
            Rows = rows;
        }

        public List<List<object>> ToSheetData()
        {
            return Rows.Select(r => r.ToRowData()).ToList();
        }
    }
}

