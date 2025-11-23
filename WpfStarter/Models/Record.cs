using System.ComponentModel;


namespace WpfStarter.Models;

public class Record : INotifyPropertyChanged
{
    public int          Id              { get; set; }

	private DateOnly?   _date           = null;
	private string      _firstName      = string.Empty;
    private string      _secondName     = string.Empty;
    private string      _surName        = string.Empty;
    private string      _city           = string.Empty;
    private string      _country        = string.Empty;

	public DateOnly? Date
	{
		get { return _date; }
		set { _date = value;  OnPropertyChanged(nameof(Date)); }
	}
	public string FirstName
	{
		get { return _firstName; }
		set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
	}
    public string SecondName
    {
        get { return _secondName; }
        set { _secondName = value; OnPropertyChanged(nameof(SecondName)); }
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

    public void CleanRecord()
    {
        Date        = null;
        FirstName   = string.Empty;
        SecondName  = string.Empty;
        SurName     = string.Empty;
        City        = string.Empty;
        Country     = string.Empty;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}