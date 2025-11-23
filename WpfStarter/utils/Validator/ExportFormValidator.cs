using WpfStarter.Models;


namespace WpfStarter.utils.Validator;

public static class ExportFormValidator
{
    public static bool IsPathValid(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            MessageBoxHelper.ShowError("Выберите путь для сохранения!");
            return false;
        }
        return true;
    }

    public static bool IsExportModeValid(bool isExcelChecked, bool isXmlChecked)
    {
        if (!isExcelChecked && !isXmlChecked)
        {
            MessageBoxHelper.ShowError("Выберите формат для экспорта!");
            return false;
        }
        return true;
    }
}
