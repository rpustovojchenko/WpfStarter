using System.Xml.Serialization;

namespace WpfStarter.Utils.ExportModel;

[XmlRoot("TestProgram")]
public class TestProgramExport
{
    [XmlElement("Record")]
    public List<RecordWrapper> Records { get; set; } = new();
}
