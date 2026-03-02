using System.ComponentModel;
using WpfStarter.ViewModels.Base;

namespace WpfStarter.Utils.Pagination;

public class Paginator : ViewModelBase
{
    private readonly int _pageSize = 100;

    private int _page = 1;
    private int _allPages;

    public int Page
    {
        get => _page;
        set
        {
            if (value < 1) _page = 1;
            else if (value > AllPages) _page = AllPages;
            else _page = value;
            OnPropertyChanged(nameof(Page));
        }
    }
    public int AllPages
    {
        get { return _allPages; }
        set { _allPages = value; OnPropertyChanged(nameof(AllPages)); }
    }
    public int PageSize => _pageSize;
}
