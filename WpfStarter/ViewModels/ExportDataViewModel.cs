using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Input;
using WpfStarter.Data;
using WpfStarter.Models;
using WpfStarter.utils.Export;
using WpfStarter.Utils;
using WpfStarter.Utils.Export;
using WpfStarter.Utils.Validator;

namespace WpfStarter.ViewModels;

public class ExportDataViewModel : INotifyPropertyChanged
{
    public Record ExpRecord { get; }

    private string  _filePath;
    private bool    _excelCBChecked;
    private bool    _xmlCBChecked;

    private string? _excelFilePath;
    private string? _xmlFilePath;

    public ICommand OpenSaveFileDialogCommand   { get; }
    public ICommand ExportDataCommand           { get; }
    public ICommand CancelCommand               { get; }

    public event Action? CloseRequested;

    public string FilePath
    {
        get { return _filePath; }
        set { _filePath = value; OnPropertyChanged(nameof(FilePath)); }
    }
    public bool ExcelCBChecked
    {
        get { return _excelCBChecked; }
        set { _excelCBChecked = value; OnPropertyChanged(nameof(ExcelCBChecked)); }
    }
    public bool XmlCBChecked
    {
        get { return _xmlCBChecked; }
        set { _xmlCBChecked = value; OnPropertyChanged(nameof(XmlCBChecked)); }
    }

    public ExportDataViewModel()
    {
        ExpRecord = new();
        _filePath = string.Empty;

        OpenSaveFileDialogCommand   = new RelayCommand(OnOpenDialog);
        ExportDataCommand           = new RelayCommand(async () => await OnExport());
        CancelCommand               = new RelayCommand(OnCancel);
    }

    private void OnOpenDialog()
    {
        var saveDialog = new SaveFileDialog()
        {
            Filter = "All files (*.*)|*.*",
            DefaultExt = ""
        };

        if (saveDialog.ShowDialog() == true)
            FilePath = saveDialog.FileName;
    }

    private async Task OnExport()
    {
        ExpRecord.FormatRecord();

        if (!ExportFormValidator.IsFormValid(_excelCBChecked, _xmlCBChecked, FilePath, ExpRecord))
            return;

        var records = DbManager.QueryRecordsBySample(ExpRecord);

        (_excelFilePath, _xmlFilePath) =
            ExportDataVMUtils.CreateExportPaths(FilePath, ExcelCBChecked, XmlCBChecked);

        if (ExcelCBChecked)
            await ExcelExporter.ExportAsync(_excelFilePath, records);
        if (XmlCBChecked)
            await XmlExporter.ExportAsync(_xmlFilePath, records);

        ExportDataVMUtils.ShowExportResultMessage(ExpRecord, ExcelCBChecked, XmlCBChecked, _excelFilePath, _xmlFilePath);

        FilePath = ExportDataVMUtils.ResetForm(ExpRecord);
        ExcelCBChecked = false;
        XmlCBChecked = false;
    }

    private void OnCancel() =>
        CloseRequested?.Invoke();

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}