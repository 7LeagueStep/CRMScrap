namespace CRMScrap.Services
{
    public interface ICsvExporter
    {
        void ExportToCsv(string filePath, List<string[]> data);
    }
}
