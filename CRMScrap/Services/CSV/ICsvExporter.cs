namespace CRMScrap.Services.CSV
{
    public interface ICsvExporter
    {
        void ExportToCsv(string filePath, List<string[]> data);
    }
}
