using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
public class XmlDialogsReader : MonoBehaviour
{
    //public string dialogToLoad;
    public static dialogs LoadXMLData(string dialogToLoad)
    {
        string path = $"{Application.dataPath}/StreamingAssets/Data/{dialogToLoad}.xml";
        XmlSerializer serializer = new XmlSerializer(typeof(dialogs));
        StreamReader reader = new StreamReader(path);
        dialogs deserialized = (dialogs)serializer.Deserialize(reader.BaseStream);
        reader.Close(); // использовать using
        ChoiseSelect.DebugShowChange("Reader работает");
        return deserialized;
    }
}

//[XmlRoot("dialogs")]
//public class Dialogs
//{
//    [XmlArray("dialogs")]
//    [XmlArrayItem("dialog")]
//    public Dialog[] dialog;
//}
//public class Dialog
//{
//    [XmlAttribute("id")]
//    public int id;
//    [XmlAttribute("name")]
//    public string name;
//    [XmlElement("textNPC")]
//    public string textNPC;
//    [XmlArray("dialog")]
//    [XmlArrayItem("choise")]
//    public Choises[] choises;
//}
//public class Choises
//{
//    [XmlAttribute("gotoID")]
//    public int gotoID;
//    [XmlAttribute("theEnd")]
//    public bool theEnd;
//    [XmlElement("choise")]
//    public string textChoise;
//}


