using WpfStarter.Models;

namespace WpfStarter.Utils.Validator;

public static class ExportFormValidator
{
    public static bool IsFormValid(bool excelCBChecked, bool xmlCBChecked, string path, Record record)
    {
        if (!IsExportModeValid(excelCBChecked, xmlCBChecked))
            return false;

        if (!IsPathValid(path))
            return false;

        if (!IsStringValid(record.FirstName, "Имя введено неправильно!"))
            return false;

        if (!IsStringValid(record.LastName, "Фамилия введена неправильно!"))
            return false;

        if (!IsStringValid(record.SurName, "Отчество введено неправильно!"))
            return false;

        if (!IsStringValid(record.City, "Город введен неправильно!"))
            return false;

        if (!IsStringValid(record.Country, "Страна введена неправильно!"))
            return false;

        return true;
    }

    private static bool IsPathValid(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            MessageBoxHelper.ShowWarning("Выберите путь для сохранения!");
            return false;
        }
        return true;
    }

    private static bool IsExportModeValid(bool isExcelChecked, bool isXmlChecked)
    {
        if (!isExcelChecked && !isXmlChecked)
        {
            MessageBoxHelper.ShowWarning("Выберите формат для экспорта!");
            return false;
        }
        return true;
    }

    private static bool IsStringValid(string value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
            return true;

        if (value.Trim().Contains(' ') || !value.All(char.IsLetter))
        {
            MessageBoxHelper.ShowWarning(message);
            return false;
        }

        return true;
    }
}