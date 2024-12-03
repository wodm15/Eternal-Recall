using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class StatData
{
    [XmlAttribute]
    public int ID;

    [XmlAttribute]
    public int name;

    [XmlAttribute]
    public string productID;

    [XmlAttribute]
    public string icon;

    [XmlAttribute]
    public string description;

    [XmlAttribute]
    public string effectType;  // Health, Skill, Passive

    [XmlAttribute("effectValue")]
    public string effectValueString; // Effect value를 string으로 받아옴
    public List<float> effectValues = new List<float>(); //나중에 소수점 올 수도 있으니 float 저장

    // XML 파싱 후 effectValueString을 List로 변환하는 함수
    public void ParseEffectValue()
    {
		// 여러 값이 있을 때
        if (effectValueString.StartsWith("{") && effectValueString.EndsWith("}"))
        {
            string[] values = effectValueString.Substring(1, effectValueString.Length - 2).Split(',');
            foreach (var value in values)
            {
                effectValues.Add(float.Parse(value.Trim()));
            }
        }
        // 하나의 값만 있을 때
        else
        {
            effectValues.Add(float.Parse(effectValueString));
        }
    }
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
            data.ParseEffectValue(); 
            dic.Add(data.ID, data);
        }

        return dic;
    }

    public bool Validate()
    {
        return true;
    }
}
