using System.IO;
using WpfStarter.Models;
using WpfStarter.utils.Validator;

namespace WpfStarter.utils;

public static class ExportDataVMUtils
{
    public static void ShowExportResultMessage(Record expRecord, bool excelCBChecked, bool xmlCBChecked, 
        string? excelFilePath, string? xmlFilePath)
    {
        MessageBoxHelper.ShowInfo($"Созданы файлы:\n" +
            $"Excel:\t {(excelCBChecked ? excelFilePath : "")}\n" +
            $"Xml:\t {(xmlCBChecked ? xmlFilePath : "")}\n\n" +
            $"По шаблону:\n" +
            $"Дата:\t\t {(expRecord.Date != null ? expRecord.Date.Value.ToString("dd.MM.yyyy") : "")}\n" +
            $"Имя:\t\t {expRecord.FirstName}\n" +
            $"Фамилия:\t\t {expRecord.SecondName}\n" +
            $"Отчество:\t\t {expRecord.SurName}\n" +
            $"Город:\t\t {expRecord.City}\n" +
            $"Страна:\t\t {expRecord.Country}\n",
            "Готово!");
    }

    public static bool IsFormValid(bool excelCBChecked, bool xmlCBChecked, string path)
    {
        if (!ExportFormValidator.IsExportModeValid(excelCBChecked, xmlCBChecked))
            return false;

        if (!ExportFormValidator.IsPathValid(path))
            return false;

        return true;
    }

    public static (string?, string?) CreateExportPaths(string filePath, bool excel, bool xml) =>
        (excel ? Path.ChangeExtension(filePath, ".xlsx") : null,
           xml ? Path.ChangeExtension(filePath, ".xml")  : null);

    public static string ResetForm(Record record)
    {
        record.CleanRecord();
        return string.Empty;
    }
}