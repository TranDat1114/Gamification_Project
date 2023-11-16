using FPT_Vote.Models;

namespace FPT_Vote.IServices
{
    public interface IExcelService
    {
        void ExportToExcel<T>(List<T> data, string sheetName, string title, string fileName);
        Task<List<ExcelData>> ImportAsync(Stream stream);

        void CreateSample(string filePath);
    }
}