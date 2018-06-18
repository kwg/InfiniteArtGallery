using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class XMLSerializer : MonoBehaviour
{
    private void Start()
    {
        TWEANNGenotype geno = new TWEANNGenotype();


        XmlSerializer serializer = new XmlSerializer(typeof(TWEANNGenotype));
        StreamWriter writer = new StreamWriter("geno.xml");
        serializer.Serialize(writer.BaseStream, geno);
        writer.Close();
    }
}
