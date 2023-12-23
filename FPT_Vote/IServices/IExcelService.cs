using FPT_Vote.Models;
using FPT_Vote.Services;

namespace FPT_Vote.IServices
{
    public interface IExcelService
    {
        Task ExportToExcel(string filePath, string title, List<ExcelData> datas, List<GroupData> groupDatas);
        Task<IndividuallyNGroup> ImportAsync(Stream stream);

        Task<string> CreateSample(string filePath, string title);
    }
}