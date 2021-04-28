using SheetToObjects.Lib;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SheetToObjects.Adapters.ProtectedGoogleSheets
{
    public interface IProvideSheet
    {
        Task<Sheet> GetAsync(string authenticationJsonFilePath, string documentName, string sheetId, string range);

        Task AppendAsync(string authenticationJsonFilePath, string documentName, string sheetId, string range, Sheet sheet);

        Task AppendAsync(string authenticationJsonFilePath, string documentName, string sheetId, string range, IList<IList<object>> values);
    }
}