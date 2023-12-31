namespace FPT_Vote.Services;
using System.Collections.Generic;
using System.IO;
using FPT_Vote.IServices;
using FPT_Vote.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; // Sử dụng XSSF cho định dạng .xlsx



public class ExcelService : IExcelService
{
    public async Task<IndividuallyNGroup> ImportAsync(Stream stream)
    {

        var dataModel = new IndividuallyNGroup()
        {
            Group = [],
            Individually = []
 
       };

        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using var workbook = new XSSFWorkbook(memoryStream);
            var sheet = workbook.GetSheetAt(0);

            for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++)
            {
                var row = sheet.GetRow(rowIdx);

                if (row == null)
                    continue;

                var data = new ExcelData
                {
                    Id = Convert.ToInt32(GetCellValue(row.GetCell(0))),
                    Name = GetCellValue(row.GetCell(1)),
                    Group = GetCellValue(row.GetCell(2)),
                    Point = Convert.ToInt32(GetCellValue(row.GetCell(3)))
                };

                dataModel.Individually.Add(data);
            }

            var sheet2 = workbook.GetSheetAt(1);

            for (int rowIdx = 1; rowIdx <= sheet2.LastRowNum; rowIdx++)
            {
                var row = sheet2.GetRow(rowIdx);

                if (row == null)
                    continue;

                var data = new GroupData
                {
                    GroupId = Convert.ToInt32(GetCellValue(row.GetCell(0))),
                    GroupName = GetCellValue(row.GetCell(1)),
                    Point = Convert.ToInt32(GetCellValue(row.GetCell(2)))
                };

                dataModel.Group.Add(data);
            }

        }

        return dataModel;
    }

    private static string GetCellValue(ICell cell)
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

    public async Task<string> CreateSample(string filePath, string title)
    {
        var dataModels = new List<ExcelData>
        {
            new() { Id=1, Name = "Trần Hoàng", Group = "A", Point = 0 },
            new() { Id=2,Name = "Jay Andy", Group = "B", Point = 0 },
            new() { Id=3,Name = "Thân Hoàng Lộc", Group = "A", Point = 0 },
            new() { Id=4,Name = "Phan Viết Thế", Group = "A", Point = 0 },
            new() { Id=5,Name = "Nguyễn Tăng Thanh Phương", Group = "A", Point = 0 },
            new() { Id=6,Name = "Bob", Group = "B", Point = 0 },
            new() { Id=7,Name = "Rock", Group = "B", Point = 0 },
            new() { Id=8,Name = "Stone", Group = "B", Point = 0 },
            new() { Id=9,Name = "Kick", Group = "B", Point = 0 },
            new() { Id=10,Name = "Ice", Group = "A", Point = 0 },
            // Thêm các đối tượng DataModel khác nếu cần
        };

        var groupDatas = new List<GroupData>(){
            new() {GroupId=1, GroupName = "A", Point = 0 },
            new() {GroupId=2, GroupName = "B", Point = 0 },
        };

        string folderPath = Path.GetDirectoryName(filePath);
        string filePathAndName = Path.Combine(folderPath, title + ".xlsx");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        using var workbook = new XSSFWorkbook();
        // Tạo một worksheet mới
        var worksheet1 = workbook.CreateSheet("SampleSheet_1");

        // Tạo một dòng mới cho tiêu đề cột
        var headerRow = worksheet1.CreateRow(0);
        headerRow.CreateCell(0).SetCellValue("Id");
        headerRow.CreateCell(1).SetCellValue("Name");
        headerRow.CreateCell(2).SetCellValue("Group");
        headerRow.CreateCell(3).SetCellValue("Point");

        // Ghi dữ liệu từ danh sách DataModel vào worksheet
        for (int i = 0; i < dataModels.Count; i++)
        {
            var dataModel = dataModels[i];
            var dataRow = worksheet1.CreateRow(i + 1);
            dataRow.CreateCell(0).SetCellValue(dataModel.Id);
            dataRow.CreateCell(1).SetCellValue(dataModel.Name);
            dataRow.CreateCell(2).SetCellValue(dataModel.Group);
            dataRow.CreateCell(3).SetCellValue(dataModel.Point);
        }

        var worksheet2 = workbook.CreateSheet("SampleSheet_2");

        var headerRow2 = worksheet2.CreateRow(0);
        headerRow2.CreateCell(0).SetCellValue("Id");
        headerRow2.CreateCell(1).SetCellValue("Group Name");
        headerRow2.CreateCell(2).SetCellValue("Point");

        for (int i = 0; i < groupDatas.Count; i++)
        {
            var dataModel = groupDatas[i];
            var dataRow = worksheet2.CreateRow(i + 1);
            dataRow.CreateCell(0).SetCellValue(dataModel.GroupId);
            dataRow.CreateCell(1).SetCellValue(dataModel.GroupName);
            dataRow.CreateCell(2).SetCellValue(dataModel.Point);
        }

        // Lưu workbook xuống đĩa
        using var fileStream = new FileStream(filePathAndName, FileMode.Create, FileAccess.Write);
        workbook.Write(fileStream);
        return filePathAndName;
    }

    public async Task ExportToExcel(string filePath, string title, List<ExcelData> datas, List<GroupData>? groupDatas)
    {
        string folderPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePathAndName = Path.Combine(folderPath, title + ".xlsx");
        using var workbook = new XSSFWorkbook();
        // Tạo một worksheet mới
        var worksheet1 = workbook.CreateSheet("Sheet 1");

        // Tạo một dòng mới cho tiêu đề cột
        var headerRow = worksheet1.CreateRow(0);
        headerRow.CreateCell(0).SetCellValue("Id");
        headerRow.CreateCell(1).SetCellValue("Name");
        headerRow.CreateCell(2).SetCellValue("Group");
        headerRow.CreateCell(3).SetCellValue("Point");

        // Ghi dữ liệu từ danh sách DataModel vào worksheet
        for (int i = 0; i < datas.Count; i++)
        {
            var dataModel = datas[i];
            var dataRow = worksheet1.CreateRow(i + 1);
            dataRow.CreateCell(0).SetCellValue(dataModel.Id);
            dataRow.CreateCell(1).SetCellValue(dataModel.Name);
            dataRow.CreateCell(2).SetCellValue(dataModel.Group);
            dataRow.CreateCell(3).SetCellValue(dataModel.Point);
        }

        if (groupDatas != null)
        {
            var worksheet2 = workbook.CreateSheet("Sheet 2");

            var headerRow2 = worksheet2.CreateRow(0);
            headerRow2.CreateCell(0).SetCellValue("Id");
            headerRow2.CreateCell(1).SetCellValue("Group Name");
            headerRow2.CreateCell(2).SetCellValue("Point");

            for (int i = 0; i < groupDatas.Count; i++)
            {
                var dataModel = groupDatas[i];
                var dataRow = worksheet2.CreateRow(i + 1);
                dataRow.CreateCell(0).SetCellValue(dataModel.GroupId);
                dataRow.CreateCell(1).SetCellValue(dataModel.GroupName);
                dataRow.CreateCell(2).SetCellValue(dataModel.Point);
            }
        }

        // Lưu workbook xuống đĩa
        using var fileStream = new FileStream(filePathAndName, FileMode.Create, FileAccess.Write);
        workbook.Write(fileStream);
    }

}
