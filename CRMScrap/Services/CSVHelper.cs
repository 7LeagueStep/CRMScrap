using CsvHelper;
using System.Globalization;

namespace CRMScrap.Services
{
    public class CSVHelper : ICsvExporter
    {
        public void ExportToCsv(string filePath, List<string[]> data)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                foreach (var row in data)
                {
                    foreach (var field in row)
                    {
                        csv.WriteField(field);
                    }
                    csv.NextRecord();
                }
            }
        }
    }
}
