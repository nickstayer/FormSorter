using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace FormSorter;

public class Xml
{
    public void CutTag(string file, string newFile, string tag)
    {
        XDocument doc = XDocument.Load(file);
        XElement nodeToRemove = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == tag);
        nodeToRemove?.Remove();
        using (var writer = new StreamWriter(newFile.ToUpper(), false, new UTF8Encoding(false)))
        {
            doc.Save(writer);
        }
    }

    public string GetUmmsId(string file)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(file);
        var result = string.Empty;

        var node = xmlDoc.GetElementsByTagName("ummsId");

        if (node?.Count > 0)
        {
            result = node[0].InnerText;
        }

        return result;
    }

    public static string GetHotelId(string xmlFile)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlNodeList node;
        xmlDoc.Load(xmlFile);

        node = xmlDoc.GetElementsByTagName("supplierInfo");
        if (node.Count == 0)
        {
            node = xmlDoc.GetElementsByTagName("ns1:supplierInfo");
        }

        string hotelId = null;

        if (node?.Count > 0)
        {
            hotelId = node[0].InnerText;
        }

        return hotelId;
    }
}
