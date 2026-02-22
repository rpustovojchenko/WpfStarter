using System.IO;
using System.Xml;
using System.Xml.Serialization;
using WpfStarter.Models;
using WpfStarter.Utils.ExportModel;

namespace WpfStarter.Utils.Export;

public static class XmlExporter
{
    public static async Task ExportAsync(string? path, List<Record> records, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(path))
            return;

        await Task.Run(() =>
        {
            var root = new TestProgramExport
            {
                Records = records.Select(r => new RecordWrapper
                {
                    Id = r.Id,
                    Date = r.Date?.ToString("dd.MM.yyyy"),
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    SurName = r.SurName,
                    City = r.City,
                    Country = r.Country
                }).ToList()
            };

            var rootAttr = new XmlRootAttribute("TestProgram");
            var serializer = new XmlSerializer(typeof(TestProgramExport), rootAttr);

            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            using var stream = new FileStream(path, FileMode.Create);
            using var writer = XmlWriter.Create(stream, settings);
            serializer.Serialize(writer, root, ns);
        }, ct);
    }
}