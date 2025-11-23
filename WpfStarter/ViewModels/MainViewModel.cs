using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WpfStarter.Data;
using WpfStarter.Models;
using WpfStarter.utils;
using WpfStarter.Views;


namespace WpfStarter.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Record> Records { get; }
    public ICommand OpenExportWindowCommand     { get; }

    public MainViewModel()
    {
        using (var db = new AppDbContext())
        {
            db.Database.Migrate();
        }

        CsvUtils.ImportCsvToDb();

        Records = new(DbManager.GetRecords());

        OpenExportWindowCommand =
            new RelayCommand(OpenExportWindow);
    }

    private void OpenExportWindow()
    {
        var vm = new ExportDataViewModel();
        var window = new ExportDataWindow() { DataContext = vm };

        vm.CloseRequested += () => window.Close();

        window.ShowDialog();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
