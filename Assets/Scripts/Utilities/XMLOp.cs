using System.IO;
using System.Xml.Serialization;

public class XMLOp  {

    public static void Serialize(object obj, string path)
    {
        XMLSerializer serializer = new XMLSerializer(obj.GetType());
        StreamWriter writer = new StreamWriter(path);
        serializer
    }
}
