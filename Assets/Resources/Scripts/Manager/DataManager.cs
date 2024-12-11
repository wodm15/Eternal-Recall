using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public Dictionary<int, QuizData> Quiz { get; private set; }
    public Dictionary<int, StatData> Stat { get; private set; }
    public Dictionary<int, EndingData> Endings { get; private set; }

    //컬렉션
    public Dictionary<int, CollectionData> Collections { get; private set; }

    public List<CollectionData> CodyCollections { get; private set; }

    public void Init()
    {
        Start = LoadSingleXml<StartData>("StartData");
        Texts = LoadXml<TextDataLoader, int, TextData>("TextData").MakeDic();
        Shops = LoadXml<ShopDataLoader, int, ShopData>("ShopData").MakeDic();
        Quiz = LoadXml<QuizDataLoader, int, QuizData>("QuizData").MakeDic();
        Stat = LoadXml<StatDataLoader, int, StatData>("StatData").MakeDic();
        Endings = LoadXml<EndingDataLoader, int, EndingData>("EndingData").MakeDic();

		// Collection
		var collectionLoader = LoadXml<CollectionDataLoader, int, CollectionData>("CollectionData");
		CodyCollections = collectionLoader._collectionData.Where(c => c.type == CollectionType.Cody).ToList();


		Collections = collectionLoader.MakeDic();

        
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
    
    //코디 아이템들 가져오기
    public string GetStatNameById(int id)
    {
        foreach (KeyValuePair<int, StatData> entry in Managers.Data.Stat)
        {
            int key = entry.Key; // key는 StatData의 ID
            StatData data = entry.Value; // value는 StatData 객체

            // Debug.Log($"ID: {data.ID}, Name: {data.Name}, Effect: {data.EffectValue}");
            if(data.ID == id)
                return data.Name;

        }
        return "null";
    }
    //옷 효과 가져오기
    public string GetStatDesById(int id)
    {
        foreach (KeyValuePair<int, StatData> entry in Managers.Data.Stat)
        {
            int key = entry.Key; 
            StatData data = entry.Value; 
            if(data.ID == id)
            {
                return $"{data.Description} \n \n {data.Power}";
            }
        }
        return "null";
    }
    
}
