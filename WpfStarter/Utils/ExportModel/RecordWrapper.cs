using System.Xml.Serialization;

namespace WpfStarter.Utils.ExportModel;

public class RecordWrapper
{
    [XmlAttribute("id")]
    public int Id { get; set; }

    public string? Date         { get; set; }
    public string? FirstName    { get; set; }
    public string? LastName     { get; set; }
    public string? SurName      { get; set; }
    public string? City         { get; set; }
    public string? Country      { get; set; }

}