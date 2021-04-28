using Google.Apis.Sheets.v4.Data;
using SheetToObjects.Adapters.ProtectedGoogleSheets.Extensions;
using SheetToObjects.Lib;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheet = SheetToObjects.Lib.Sheet;

namespace SheetToObjects.Adapters.ProtectedGoogleSheets
{
    public class ProtectedGoogleSheetAdapter : IProvideSheet
    {
        private readonly ICreateGoogleClientService _googleClientServiceCreator;
        private readonly IConvertDataToSheet<ValueRange> _protectedGoogleSheetAdapter;

        internal ProtectedGoogleSheetAdapter(
            ICreateGoogleClientService googleClientServiceCreator,
            IConvertDataToSheet<ValueRange> protectedGoogleSheetAdapter)
        {
            _googleClientServiceCreator = googleClientServiceCreator;
            _protectedGoogleSheetAdapter = protectedGoogleSheetAdapter;
        }

        public ProtectedGoogleSheetAdapter() : this(new GoogleClientServiceFactory(), new ProtectedGoogleSheetToSheetConverter()) { }

        public async Task<Sheet> GetAsync(string authenticationJsonFilePath, string documentName, string sheetId, string range)
        {
            var service = _googleClientServiceCreator.Create(authenticationJsonFilePath, documentName);

            var sheetDataResponse = await service.Get(sheetId, range);

            return _protectedGoogleSheetAdapter.Convert(sheetDataResponse);
        }

        public async Task AppendAsync(string authenticationJsonFilePath, string documentName, string sheetId, string range, Sheet sheet)
        {
            var service = _googleClientServiceCreator.Create(authenticationJsonFilePath, documentName);

            var values = sheet.SheetToValueRange();

            await service.Append(values, sheetId, range);
        }

        public async Task AppendAsync(string authenticationJsonFilePath, string documentName, string sheetId, string range, IList<IList<object>> values)
        {
            var service = _googleClientServiceCreator.Create(authenticationJsonFilePath, documentName);

            await service.Append(new ValueRange() { Values = values }, sheetId, range);
        }
    }
}