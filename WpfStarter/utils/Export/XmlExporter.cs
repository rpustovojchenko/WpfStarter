using System.IO;
using System.Xml.Serialization;
using WpfStarter.Models;

namespace WpfStarter.utils.Export;

public static class XmlExporter
{
    public static void Export(string? path, List<Record> records)
    {
        if (path == null) return;

        var serializer = new XmlSerializer(typeof(List<Record>));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, records);
        }
    }
}
