namespace FPT_Vote.Models
{
    public class ExcelData
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public int Point { get; set; } = 0;
    }

    public class ExcelDataShow : ExcelData
    {
        public string Rank { get; set; } = string.Empty;
        public string RankImage { get; set; } = string.Empty;
        public string Ordinal_Suffix { get; set; } = string.Empty;

        public ExcelDataShow(ExcelData excelData)
        {
            Id = excelData.Id;
            Name = excelData.Name;
            Group = excelData.Group;
            Point = excelData.Point;

            // Gọi hàm WhatIsYourRank từ câu hỏi trước
            Rank = WhatIsYourRank(Point);
            RankImage = Rank + ".png";
            // Gọi hàm để lấy hậu tố thứ tự
            // Ordinal_Suffix = GetOrdinalSuffix(Id);
        }

        private string WhatIsYourRank(int point)
        {
            var ranks = new[]
            {
(5, "Iron"),
(10, "Bronze"),
(15, "Silver"),
(20, "Gold"),
(25, "Platinum"),
(30, "Diamond"),
(40, "Master"),
(50, "GrandMaster"),
(int.MaxValue, "Challenger")
};

            foreach (var (threshold, rank) in ranks)
            {
                if (point <= threshold)
                {
                    return rank;
                }
            }

            return "Iron";
        }


    }
}