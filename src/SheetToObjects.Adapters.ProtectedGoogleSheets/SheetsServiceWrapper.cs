using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Threading.Tasks;

namespace SheetToObjects.Adapters.ProtectedGoogleSheets
{
    public class SheetsServiceWrapper : ISheetsServiceWrapper
    {
        private readonly SheetsService _sheetsService;

        public SheetsServiceWrapper(SheetsService sheetsService)
        {
            _sheetsService = sheetsService;
        }

        public async Task<ValueRange> Get(string spreadsheetId, string range)
        {
            return await _sheetsService.Spreadsheets.Values.Get(spreadsheetId, range).ExecuteAsync();
        }

        public async Task<AppendValuesResponse> Append(ValueRange body, string spreadsheetId, string range)
        {
            var req = _sheetsService.Spreadsheets.Values.Append(body, spreadsheetId, range);
            req.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            return await req.ExecuteAsync();
        }
    }
}