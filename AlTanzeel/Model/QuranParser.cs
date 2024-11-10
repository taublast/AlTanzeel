using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace QuranParser;

// Main parser class that handles reading and parsing the Quran XML
public class QuranXmlParser
{
    public async Task<Collection<Surah>> ParseQuranXmlAsync()
    {
        // Open the Quran XML file from Resources\Raw
        using var stream = await FileSystem.OpenAppPackageFileAsync("Quran-simple.xml");

        // Read the stream content
        using var reader = new StreamReader(stream);
        var xmlContent = await reader.ReadToEndAsync();

        // Load the XML content into an XDocument
        var xdoc = XDocument.Parse(xmlContent);

        // Initialize a list to hold all surahs
        Collection<Surah> suras = new();

        // Parse all Surah elements
        var suraElements = xdoc.Descendants("sura");
        foreach (var suraElement in suraElements)
        {
            // Create a Surah object
            var sura = new Surah
            {
                Index = int.Parse(suraElement.Attribute("index")?.Value),
                Name = suraElement.Attribute("name")?.Value
            };

            // Parse Ayas and add them to the Surah
            var ayas = suraElement.Descendants("aya");
            foreach (var ayaElement in ayas)
            {
                var aya = new Aya
                {
                    Index = int.Parse(ayaElement.Attribute("index")?.Value),
                    Text = ayaElement.Attribute("text")?.Value
                };
                sura.Ayas.Add(aya);
            }

            // Add the Surah to the list of Surahs
            suras.Add(sura);
        }

        return suras;
    }
}