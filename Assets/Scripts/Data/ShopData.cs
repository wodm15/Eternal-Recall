using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ShopData
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
	public string effectType;  

    [XmlAttribute]
    public float effectValue; 
}

[Serializable, XmlRoot("ArrayOfShopData")]
public class ShopDataLoader : ILoader<int, ShopData>
{
	[XmlElement("ShopData")]
	public List<ShopData> _shopDatas = new List<ShopData>();

	public Dictionary<int, ShopData> MakeDic()
	{
		Dictionary<int, ShopData> dic = new Dictionary<int, ShopData>();

		foreach (ShopData data in _shopDatas)
			dic.Add(data.ID, data);

		return dic;
	}

	public bool Validate()
	{
		return true;
	}
}