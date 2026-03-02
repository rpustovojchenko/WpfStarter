using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Windows.Input;
using WpfStarter.Data;
using WpfStarter.Models;
using WpfStarter.Utils;
using WpfStarter.Utils.Commands;
using WpfStarter.Utils.Pagination;
using WpfStarter.ViewModels.Base;
using WpfStarter.Views;

namespace WpfStarter.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly Paginator  _paginator      = new();
    private string              _pageString     = string.Empty;
    private List<Record>        _recordsByPage  = new();
    private bool                _isBusy;

    public ICommand ImportDataCommand { get; }
    public ICommand OpenExportWindowCommand { get; }
    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }

    public string PageString
    {
        get { return _pageString; }
        set { _pageString = value; OnPropertyChanged(nameof(PageString)); }
    }
    public List<Record> RecordsByPage
    {
        get => _recordsByPage;
        set
        {
            if (_recordsByPage != value)
            {
                _recordsByPage = value;
                OnPropertyChanged(nameof(RecordsByPage));
            }
        }
    }
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy =  value; OnPropertyChanged(nameof(IsBusy)); }
    }

    public MainViewModel()
    {
        using (var db = new AppDbContext())
        {
            db.Database.Migrate();
        }

        _ = LoadPageAsync(1);

        ImportDataCommand           = new AsyncRelayCommand(ImportDataAsync,        () => !IsBusy);
        OpenExportWindowCommand     = new AsyncRelayCommand(OpenExportWindowAsync,  () => !IsBusy);
        NextPageCommand             = new AsyncRelayCommand(NextPageAsync,          () => !IsBusy);
        PreviousPageCommand         = new AsyncRelayCommand(PreviousPageAsync,      () => !IsBusy);
    }

    private async Task ImportDataAsync() =>
        await RunSafeAsync(ImportDataInternalAsync, "Ошибка при импорте данных");

    private async Task OpenExportWindowAsync() =>
        await RunSafeAsync(OpenExportWindowInternalAsync, "Ошибка при открытии окна экспорта");

    private async Task NextPageAsync() =>
        await LoadPageAsync(_paginator.Page + 1);

    private async Task PreviousPageAsync() =>
        await LoadPageAsync(_paginator.Page - 1);

    private async Task RunSafeAsync(Func<Task> action, string message)
    {
        if (IsBusy) return;
        IsBusy = true;

        try { await action(); }
        catch (Exception ex)
        {
            MessageBoxHelper.ShowError($"{message}:\n{ex.Message}");
        }
        finally { IsBusy = false; }
    }

    private async Task LoadPageAsync(int page)
    {
        if (_paginator.AllPages == 0)
            _paginator.AllPages = 
                PaginatorUtils.CalculateAllPages(await DbManager.GetRecordsCountAsync(), _paginator.PageSize);

        if (_paginator.AllPages == 0)
        {
            _paginator.Page = 0;
            RecordsByPage = new();
            PageString = "Страница 0/0";
            return;
        }

        _paginator.Page = page;
        RecordsByPage = await DbManager.GetRecordsPageAsync(_paginator.Page, _paginator.PageSize);
        PageString = PaginatorUtils.MakePageString(_paginator);
    }

    private async Task ImportDataInternalAsync()
    {
        var saveDialog = new OpenFileDialog()
        {
            Filter = "CSV файлы (*.csv)|*.csv"
        };

        string filePath = string.Empty;

        if (saveDialog.ShowDialog() == true)
            filePath = saveDialog.FileName;
        else return;

        if (!await DbManager.IsDbEmptyAsync())
            DbManager.ClearAllRecords();

        await CsvUtils.ImportCsvToDbAsync(filePath);

        _paginator.AllPages = 0;
        await LoadPageAsync(1);
    }

    private async Task OpenExportWindowInternalAsync()
    {
        if (await DbManager.IsDbEmptyAsync())
        {
            MessageBoxHelper.ShowWarning("Сначала нужно импортировать данные");
            return;
        }

        var vm = new ExportDataViewModel();
        var window = new ExportDataWindow() { DataContext = vm };

        vm.CloseRequested += () => window.Close();

        window.Show();
    }
}