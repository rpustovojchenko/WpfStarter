using OfficeOpenXml;
using WpfStarter.Models;

namespace WpfStarter.utils.Export;

public static class ExcelExporter
{
    public static void Export(string? path, List<Record> records)
    {
        if (path == null) return;

        OfficeOpenXml.ExcelPackage.LicenseContext =
            OfficeOpenXml.LicenseContext.NonCommercial;


        using (var package = new ExcelPackage())
        {
            var ws = package.Workbook.Worksheets.Add("Records");

            ws.Cells[1, 1].Value = "Дата";
            ws.Cells[1, 2].Value = "Имя";
            ws.Cells[1, 3].Value = "Фамилия";
            ws.Cells[1, 4].Value = "Отчество";
            ws.Cells[1, 5].Value = "Город";
            ws.Cells[1, 6].Value = "Страна";

            for (int i = 0; i < records.Count; i++)
            {
                var rec = records[i];
                ws.Cells[i + 2, 1].Value = rec.Date?.ToString("dd.MM.yyyy") ?? "-";
                ws.Cells[i + 2, 2].Value = rec.FirstName;
                ws.Cells[i + 2, 3].Value = rec.SecondName;
                ws.Cells[i + 2, 4].Value = rec.SurName;
                ws.Cells[i + 2, 5].Value = rec.City;
                ws.Cells[i + 2, 6].Value = rec.Country;
            }
            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            package.SaveAs(new System.IO.FileInfo(path));
        }
    }
}
