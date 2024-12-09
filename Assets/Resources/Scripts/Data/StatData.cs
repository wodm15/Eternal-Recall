using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class StatData
{
    [XmlAttribute]
    public int ID;

    [XmlAttribute]
    public string type; // 문자열로 변경

    [XmlAttribute]
    public int nameID;

    [XmlAttribute]
    public string icon;

    [XmlAttribute("name")]
    public string Name; // 속성명과 XML 일치

    [XmlAttribute("description")]
    public string Description; // 속성명과 XML 일치

    [XmlAttribute("effectType")]
    public string EffectType; // 속성명과 XML 일치

    [XmlAttribute("effectValue")]
    public int EffectValue; // 이름 일치 및 정수형
}

[Serializable, XmlRoot("ArrayOfStatData")]
public class StatDataLoader : ILoader<int, StatData>
{
    [XmlElement("StatData")]
    public List<StatData> _statDatas = new List<StatData>();

    public Dictionary<int, StatData> MakeDic()
    {
        Dictionary<int, StatData> dic = new Dictionary<int, StatData>();

        foreach (StatData data in _statDatas)
        {
            dic.Add(data.ID, data);
        }

        return dic;
    }

    public bool Validate()
    {
        return true;
    }
}
