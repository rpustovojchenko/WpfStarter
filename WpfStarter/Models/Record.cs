using WpfStarter.Utils;
using WpfStarter.ViewModels.Base;


namespace WpfStarter.Models;

public class Record : ViewModelBase
{
    public int          Id              { get; set; }

    private DateTime?   _date           = null;
    private string      _firstName      = string.Empty;
    private string      _lastName       = string.Empty;
    private string      _surName        = string.Empty;
    private string      _city           = string.Empty;
    private string      _country        = string.Empty;

    public DateTime? Date
    {
        get { return _date; }
        set { _date = value; OnPropertyChanged(nameof(Date)); }
    }
    public string FirstName
    {
        get { return _firstName; }
        set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
    }
    public string LastName
    {
        get { return _lastName; }
        set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
    }
    public string SurName
    {
        get { return _surName; }
        set { _surName = value; OnPropertyChanged(nameof(SurName)); }
    }
    public string City
    {
        get { return _city; }
        set { _city = value; OnPropertyChanged(nameof(City)); }
    }
    public string Country
    {
        get { return _country; }
        set { _country = value; OnPropertyChanged(nameof(Country)); }
    }

    public void FormatRecord()
    {
        FirstName   = StringUtils.Capitalize(FirstName);
        LastName    = StringUtils.Capitalize(LastName);
        SurName     = StringUtils.Capitalize(SurName);
        City        = StringUtils.Capitalize(City);
        Country     = StringUtils.Capitalize(Country);
    }

    public void CleanRecord()
    {
        Date        = null;
        FirstName   = string.Empty;
        LastName    = string.Empty;
        SurName     = string.Empty;
        City        = string.Empty;
        Country     = string.Empty;
    }
}