using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GetItemPopup : UI_Popup
{
    private List<ShopData> _shopData = new List<ShopData>();
    public string spritePath = "Sprites/ItemIcon"; 
    enum Buttons
    {
        Item1,
        Item2,
        Item3
    }

    enum Texts
    {
        Text1,
        Text2,
        Text3,
    }
    
    public override bool Init()
    {
        if (base.Init() == false)
			return false;

		foreach (ShopData shopData in Managers.Data.Shops.Values)
		{
			_shopData.Add(shopData);
		}

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        
        GetButton((int)Buttons.Item1).gameObject.BindEvent(OnClickItem1);
        GetButton((int)Buttons.Item2).gameObject.BindEvent(OnClickItem2);
        GetButton((int)Buttons.Item3).gameObject.BindEvent(OnClickItem3);
       
        for (int i = 0; i < 3; i++) // 0부터 2까지
        {
            int index = GetRandomIndex(); // 랜덤 인덱스

            Buttons buttonEnum = (Buttons)i; // i를 Buttons enum으로 변환

            switch (buttonEnum)
            {
                case Buttons.Item1:
                    GetButton((int)Buttons.Item1).image.sprite = Managers.Resource.Load<Sprite>($"Sprites/{_shopData[index].icon}");
                    GetText((int)Texts.Text1).text = _shopData[index].description; // 설명을 텍스트로 설정
                    break;
                case Buttons.Item2:
                    GetButton((int)Buttons.Item2).image.sprite = Managers.Resource.Load<Sprite>($"Sprites/{_shopData[index].icon}");
                    GetText((int)Texts.Text2).text = _shopData[index].description; // 설명을 텍스트로 설정
                    break;
                case Buttons.Item3:
                    GetButton((int)Buttons.Item3).image.sprite = Managers.Resource.Load<Sprite>($"Sprites/{_shopData[index].icon}");
                    GetText((int)Texts.Text3).text = _shopData[index].description; // 설명을 텍스트로 설정
                    break;
            }
        }
        

        GetButton(1).image.sprite = Managers.Resource.Load<Sprite>("Sprites/ItemIcon/Healing");
        


    // Player = Managers.Resource.Instantiate("StaticPlayer");
    // Player.transform.position = new Vector3(-4,-4,0);
    // Player.transform.localScale = new Vector3(1,1, 1);

        return true;
    }

    void OnClickItem1()
    {
        Debug.Log("getItem1");
        Managers.UI.ClosePopupUI(this);
    }
    void OnClickItem2()
    {
        Debug.Log("getItem2");
        Managers.UI.ClosePopupUI(this);
    }
    void OnClickItem3()
    {
        Debug.Log("getItem3");
        Managers.UI.ClosePopupUI(this);
    }

    private int GetRandomIndex()
    {
        return Random.Range(0, _shopData.Count);
    }

}
