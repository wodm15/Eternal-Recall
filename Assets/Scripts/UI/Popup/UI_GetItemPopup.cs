using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GetItemPopup : UI_Popup
{
    UI_PlayerScene playerScene;
    
    //임시로 저장할 조건부 shopData 전체 가져오기
    private List<ShopData> _shopData = new List<ShopData>();

    // 선택된 인덱스를 저장할 리스트
    private List<int> _selectedIndexes = new List<int>(); 
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

        playerScene = Managers.UI.GetSceneUI<UI_PlayerScene>();
        

		foreach (ShopData shopData in Managers.Data.Shops.Values)
		{
			 if (Managers.Game.Stage < 10 && shopData.ID >= 1 && shopData.ID <= 100)
                _shopData.Add(shopData);
            
            else if (Managers.Game.Stage >= 10 && Managers.Game.Stage < 20 && shopData.ID > 100 && shopData.ID <= 200)
                _shopData.Add(shopData);
		}

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        
        GetButton((int)Buttons.Item1).gameObject.BindEvent(OnClickItem1);
        GetButton((int)Buttons.Item2).gameObject.BindEvent(OnClickItem2);
        GetButton((int)Buttons.Item3).gameObject.BindEvent(OnClickItem3);
       
       //랜덤 아이템 3개 생성(이미지 + 글자)
        for (int i = 0; i < 3; i++) 
        {
            int index;
            //중복 안되게 진열
            do
            {
                index = Random.Range(0, _shopData.Count);
            } while (_selectedIndexes.Contains(index));

            _selectedIndexes.Add(index); 

            Buttons buttonEnum = (Buttons)i; 
            switch (buttonEnum)
            {
                case Buttons.Item1:
                    GetButton((int)Buttons.Item1).image.sprite = Managers.Resource.Load<Sprite>($"{spritePath}/{_shopData[index].icon}");
                    GetText((int)Texts.Text1).text = _shopData[index].description; 
                    break;
                case Buttons.Item2:
                    GetButton((int)Buttons.Item2).image.sprite = Managers.Resource.Load<Sprite>($"{spritePath}/{_shopData[index].icon}");
                    GetText((int)Texts.Text2).text = _shopData[index].description;
                    break;
                case Buttons.Item3:
                    GetButton((int)Buttons.Item3).image.sprite = Managers.Resource.Load<Sprite>($"{spritePath}/{_shopData[index].icon}");
                    GetText((int)Texts.Text3).text = _shopData[index].description; 
                    break;
            }
        }

        return true;
    }

    void OnClickItem1()
    {
        int index = _selectedIndexes[0];
        ShopData selectedItem = _shopData[index];

        ApplyItemEffect(selectedItem);  // 아이템 효과 적용
        ClearShopData(); //랜덤 선택한 리스트 , 배열 초기화
        onClickEnd(); // 팝업창 이동
    }
    void OnClickItem2()
    {
        int index = _selectedIndexes[1];
        ShopData selectedItem = _shopData[index];

        ApplyItemEffect(selectedItem);

        ClearShopData();
        onClickEnd();
    }
    void OnClickItem3()
    {
        int index = _selectedIndexes[2];
        ShopData selectedItem = _shopData[index];

        ApplyItemEffect(selectedItem);

        ClearShopData();
        onClickEnd();
    }
    
    //획득아이템 적용하기
    void ApplyItemEffect(ShopData selectedItem)
    {
        switch (selectedItem.effectType)
        {
            case "Health":
                Managers.Game.Hp += (int)selectedItem.effectValue;
                break;
            case "Skill":
                UpdateSkill(selectedItem);
                break;
            case "Passive":
                updatePassive(selectedItem);
                break;
        }
    }

    //리스트와 배열 청소하기
    public void ClearShopData()
    {
        _shopData.Clear();
        _selectedIndexes.Clear();
    }

    //마지막 팝업용
    void onClickEnd()
    {
        Managers.UI.ClosePopupUI(this);
        playerScene.StageUp();
        playerScene.animationManager.ani = Managers.Game.StrangerIndex[6];
        Managers.UI.ShowPopupUI<UI_CountPopup>();
    }

    //스킬 업데이트용
    void UpdateSkill(ShopData selectedItem)
    {
        if(selectedItem.productID == "GetExpendTime")
            Managers.Game.GuessTimer += (int)selectedItem.effectValue;
        else if(selectedItem.productID == "GetTheWorld")
            {
                Managers.Game.TheWorld += (int)selectedItem.effectValue;
                Debug.Log($"TheWorld 개수 : {Managers.Game.TheWorld}");

            }
        else
            Debug.Log("NO SKILL FOUND");
    }

    //패시브 업데이트용
    void updatePassive(ShopData selectedItem)
    {
        if(selectedItem.productID == "upLuck")
            Managers.Game.LuckPercent += (int)selectedItem.effectValue;
    }


}
