using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Input;
using WpfStarter.Data;
using WpfStarter.Models;
using WpfStarter.utils;
using WpfStarter.utils.Export;


namespace WpfStarter.ViewModels;

public class ExportDataViewModel : INotifyPropertyChanged
{
    public	Record	ExpRecord { get; }

	private string	_filePath;
	private bool	_excelCBChecked;
	private bool	_xmlCBChecked;

	private string? _excelFilePath;
	private string? _xmlFilePath;

	public ICommand OpenSaveFileDialogCommand	{ get; }
	public ICommand ExportDataCommand			{ get; }
	public ICommand CancelCommand				{ get; }
	
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

		OpenSaveFileDialogCommand	= new RelayCommand(OnOpenDialog);
		ExportDataCommand			= new RelayCommand(OnExport);
		CancelCommand				= new RelayCommand(OnCancel);
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

    private void OnExport()
    {
		if (!ExportDataVMUtils.IsFormValid(_excelCBChecked, _xmlCBChecked, FilePath))
			return;

		var records = DbManager.QueryRecordsBySample(ExpRecord);

		(_excelFilePath, _xmlFilePath) = 
			ExportDataVMUtils.CreateExportPaths(FilePath, ExcelCBChecked, XmlCBChecked);

		if (ExcelCBChecked)
            ExcelExporter.Export(_excelFilePath, records);
		if (XmlCBChecked)
            XmlExporter.Export(_xmlFilePath, records);

		ExportDataVMUtils.ShowExportResultMessage(ExpRecord, ExcelCBChecked, XmlCBChecked, _excelFilePath, _xmlFilePath);


		FilePath = ExportDataVMUtils.ResetForm(ExpRecord);
    }

    private void OnCancel() =>
		CloseRequested?.Invoke();

    public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}