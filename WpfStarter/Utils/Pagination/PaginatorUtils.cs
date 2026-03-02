namespace WpfStarter.Utils.Pagination;

public static class PaginatorUtils
{
    public static int CalculateAllPages(int recordsCount, int pageSize) =>
        (recordsCount + pageSize - 1) / pageSize;

    public static string MakePageString(Paginator paginator) =>
        $"Страница {paginator.Page}/{paginator.AllPages}";
}
