using FPT_Vote.Models;

namespace FPT_Vote.IServices
{
    public interface IExcelService
    {
        Task ExportToExcel(string filePath, string title, List<ExcelData> datas);
        Task<List<ExcelData>> ImportAsync(Stream stream);

        Task<string> CreateSample(string filePath, string title);
    }
}