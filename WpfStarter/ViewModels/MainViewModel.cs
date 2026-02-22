using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Input;
using WpfStarter.Data;
using WpfStarter.Models;
using WpfStarter.Utils;
using WpfStarter.Views;

namespace WpfStarter.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private List<Record> _records = new();
    public List<Record> Records
    {
        get => _records;
        set
        {
            if (_records != value)
            {
                _records = value;
                OnPropertyChanged(nameof(Records));
            }
        }
    }

    public ICommand ImportDataCommand       { get; }
    public ICommand OpenExportWindowCommand { get; }

    public MainViewModel()
    {
        using (var db = new AppDbContext())
        {
            db.Database.Migrate();
        }

        Records = DbManager.GetRecords();

        ImportDataCommand =
            new RelayCommand(ImportData);

        OpenExportWindowCommand =
            new RelayCommand(OpenExportWindow);
    }

    private async void ImportData()
    {
        var saveDialog = new OpenFileDialog()
        {
            Filter = "CSV файлы (*.csv)|*.csv"
        };

        string filePath = string.Empty;
        if (saveDialog.ShowDialog() == true)
            filePath = saveDialog.FileName;

        if (!DbManager.IsDbEmpty()) 
            DbManager.ClearAllRecords();

        await CsvUtils.ImportCsvToDbAsync(filePath);
        Records = DbManager.GetRecords();
    }

    private void OpenExportWindow()
    {
        if (DbManager.IsDbEmpty())
        {
            MessageBoxHelper.ShowWarning("Сначала нужно импортировать данные");
            return;
        }

        var vm = new ExportDataViewModel();
        var window = new ExportDataWindow() { DataContext = vm };

        vm.CloseRequested += () => window.Close();

        window.Show();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}