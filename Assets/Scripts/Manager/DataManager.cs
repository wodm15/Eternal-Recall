using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


public interface ILoader<Key, Item>
{
    Dictionary<Key, Item> MakeDic();
    bool Validate();
}
public class DataManager
{
    public StartData Start { get; private set; }
    public Dictionary<int, TextData> Texts { get; private set; }
    public Dictionary<int, ShopData> Shops { get; private set; }


    public void Init()
    {
        Start = LoadSingleXml<StartData>("StartData");
        Texts = LoadXml<TextDataLoader, int, TextData>("TextData").MakeDic();
        Shops = LoadXml<ShopDataLoader, int, ShopData>("ShopData").MakeDic();
    }

    
    private Item LoadSingleXml<Item>(string name)
	{
		XmlSerializer xs = new XmlSerializer(typeof(Item));
		TextAsset textAsset = Resources.Load<TextAsset>("Data/" + name);
		using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
			return (Item)xs.Deserialize(stream);
	}

    private Loader LoadXml<Loader, Key, Item>(string name) where Loader : ILoader<Key, Item>, new()
    {
        XmlSerializer xs = new XmlSerializer(typeof(Loader));
        TextAsset textAsset = Resources.Load<TextAsset>("Data/" + name);
        using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
            return (Loader)xs.Deserialize(stream);
    }
    
}