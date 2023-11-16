namespace FPT_Vote.Services;
using System.Collections.Generic;
using System.IO;
using FPT_Vote.IServices;
using FPT_Vote.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; // Sử dụng XSSF cho định dạng .xlsx

public class ExcelService : IExcelService
{
    public async Task<List<ExcelData>> ImportAsync(Stream stream)
    {
        var dataList = new List<ExcelData>();

        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using (var workbook = new XSSFWorkbook(memoryStream))
            {
                var sheet = workbook.GetSheetAt(0);

                for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++)
                {
                    var row = sheet.GetRow(rowIdx);

                    if (row == null)
                        continue;

                    var data = new ExcelData
                    {
                        Name = GetCellValue(row.GetCell(0)),
                        Group = GetCellValue(row.GetCell(1)),
                        Point = Convert.ToInt32(GetCellValue(row.GetCell(2)))
                    };

                    dataList.Add(data);
                }
            }
        }

        return dataList;
    }

    private string GetCellValue(ICell cell)
    {
        if (cell == null)
            return string.Empty;

        switch (cell.CellType)
        {
            case CellType.String:
                return cell.StringCellValue;

            case CellType.Numeric:
                if (DateUtil.IsCellDateFormatted(cell))
                {
                    return cell.DateCellValue.ToString("MM/dd/yyyy");
                }
                else
                {
                    return cell.NumericCellValue.ToString();
                }

            case CellType.Boolean:
                return cell.BooleanCellValue.ToString();

            default:
                return string.Empty;
        }
    }

    public void CreateSample(string filePath)
    {
        var dataModels = new List<ExcelData>
        {
            new ExcelData { Name = "John", Group = "A", Point = 100 },
            new ExcelData { Name = "Jane", Group = "B", Point = 150 },
            new ExcelData { Name = "Bob", Group = "A", Point = 120 },
            // Thêm các đối tượng DataModel khác nếu cần
        };

        using (var workbook = new XSSFWorkbook())
        {
            // Tạo một worksheet mới
            var worksheet = workbook.CreateSheet("SampleSheet");

            // Tạo một dòng mới cho tiêu đề cột
            var headerRow = worksheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Name");
            headerRow.CreateCell(1).SetCellValue("Group");
            headerRow.CreateCell(2).SetCellValue("Point");

            // Ghi dữ liệu từ danh sách DataModel vào worksheet
            for (int i = 0; i < dataModels.Count; i++)
            {
                var dataModel = dataModels[i];
                var dataRow = worksheet.CreateRow(i + 1);
                dataRow.CreateCell(0).SetCellValue(dataModel.Name);
                dataRow.CreateCell(1).SetCellValue(dataModel.Group);
                dataRow.CreateCell(2).SetCellValue(dataModel.Point);
            }

            // Lưu workbook xuống đĩa
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }
        }

    }

    public void ExportToExcel<T>(List<T> data, string sheetName, string title, string fileName)
    {
        throw new NotImplementedException();
    }

}
