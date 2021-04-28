using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Linq;
using Sheet = SheetToObjects.Lib.Sheet;

namespace SheetToObjects.Adapters.ProtectedGoogleSheets.Extensions
{
    public static class SheetExtensions
    {
        public static ValueRange SheetToValueRange(this Sheet sheet)
        {
            return new ValueRange()
            {
                Values = sheet.ToSheetData().Cast<IList<object>>().ToList()
            };
        }
    }
}
