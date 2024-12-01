using System.Collections;
using System.Collections.Generic;
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
        Item3,
    }
    enum Images
    {
    }

    enum Texts
    {
        Text1,
        Text2,
        Text3,
        ExplainText,
    }
    
    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        //추측 플레이어 아직 남아있을 경우 검증
        GameObject PassingPlayer = GameObject.Find("Stranger");
        PassingPlayer.transform.position = new Vector3(4, -3, 0);

        GameObject GuessPlayer = GameObject.Find("Player");
        GuessPlayer.transform.position = new Vector3(0,-3, 0);
        GuessPlayer.transform.localScale = new Vector3(0.7f,0.7f, 1);

        playerScene = Managers.UI.GetSceneUI<UI_PlayerScene>();
        
        // shopData 스테이지별 픽업
        int stageRangeStart = (Managers.Game.Stage / 10) * 100;  //Stage 10이면 100, Stage 20이면 200, ...
        int stageRangeEnd = stageRangeStart + 100;

        foreach (ShopData shopData in Managers.Data.Shops.Values)
        {
            if (shopData.ID >= stageRangeStart && shopData.ID < stageRangeEnd)
            {
                _shopData.Add(shopData);
            }
        }

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetText((int)Texts.ExplainText).text = Managers.GetText(Define.SkillChoseText);
        
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
        onClickEnd(selectedItem); // 팝업창 이동
    }
    void OnClickItem2()
    {
        int index = _selectedIndexes[1];
        ShopData selectedItem = _shopData[index];

        ApplyItemEffect(selectedItem);

        ClearShopData();
        onClickEnd(selectedItem);
    }
    void OnClickItem3()
    {
        int index = _selectedIndexes[2];
        ShopData selectedItem = _shopData[index];

        ApplyItemEffect(selectedItem);

        ClearShopData();
        onClickEnd(selectedItem);
    }
    
    //획득아이템 적용하기
    void ApplyItemEffect(ShopData selectedItem)
    {
        switch (selectedItem.effectType)
        {
            case "Health":
                UpdateHealth(selectedItem);

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
    void onClickEnd(ShopData selectedItem = null)
    {
        Managers.Game.SaveGame();
        //stranger 캐릭터와 Player 삭제
        GameObject PassingPlayer = GameObject.Find("Stranger");
        if (PassingPlayer != null)
        {
            Managers.Resource.Destroy(PassingPlayer);
        }
        GameObject guessPlayer = GameObject.Find("Player");
        if (guessPlayer != null)
        {
            Managers.Resource.Destroy(guessPlayer);
        }


        //만약 행운으로 인해 스킬을 한번 더 획득
        if(Random.Range(1, 101) <= Managers.Game.LuckPercent)
        {
            Managers.UI.ClosePopupUI(this);
            playerScene.StageUp();
            Debug.Log("행운 효과로 인해 2배 상승");
            ApplyItemEffect(selectedItem);
            Managers.UI.ShowPopupUI<UI_CountPopup>();
        }

        //행운이 있었어도 확률에 안걸리면
        else
        {
            Managers.UI.ClosePopupUI(this);
            playerScene.StageUp();
            Managers.UI.ShowPopupUI<UI_CountPopup>();
        }
    }

    //체력 업데이트
    ShopData UpdateHealth(ShopData selectedItem)
    {
        if(selectedItem.productID == "Healing")
        {
            Managers.Game.Hp += (int)selectedItem.effectValues[0];
            Managers.Game.Hp = Mathf.Clamp(Managers.Game.Hp, 0, 100); //회복 100까지만 제한

        }
        else if(selectedItem.productID == "gambleHealing")
        {
            int gameble = Random.Range(0, (int)selectedItem.effectValues[0]);
            Managers.Game.Hp += gameble;
            Managers.Game.Hp = Mathf.Clamp(Managers.Game.Hp, 0, 100); //회복 100까지만 제한
        }

        playerScene.HPUp();
        return selectedItem;
    }

    //스킬 업데이트용
    ShopData UpdateSkill(ShopData selectedItem)
    {
        if(selectedItem.productID == "GetExpendTime")
            Managers.Game.GuessTimer += (int)selectedItem.effectValues[0];
        else if(selectedItem.productID == "GetTheWorld")
            {
                Managers.Game.TheWorld += (int)selectedItem.effectValues[0];
                Debug.Log($"TheWorld 개수 : {Managers.Game.TheWorld}");
            }
        else if(selectedItem.productID == "FreePass")
            {
                Debug.Log($"TheWorld 개수 : {(int)selectedItem.effectValues[0]} {(int)selectedItem.effectValues[1]} {(int)selectedItem.effectValues[2]}");
                //HP, 운, 스테이지 순서
                Managers.Game.Hp += (int)selectedItem.effectValues[0];
                Managers.Game.Hp = Mathf.Clamp(Managers.Game.Hp, 0, 100); //회복 100까지만 제한
                Managers.Game.LuckPercent += (int)selectedItem.effectValues[1];
                Managers.Game.Stage += 1;
            }
        else
            Debug.Log("NO SKILL FOUND");
        
        return selectedItem;
    }

    //패시브 업데이트용
    ShopData updatePassive(ShopData selectedItem)
    {
        if(selectedItem.productID == "upLuck")
            Managers.Game.LuckPercent += (int)selectedItem.effectValues[0];
        else if(selectedItem.productID == "upDefence")
            Managers.Game.Defence += (int)selectedItem.effectValues[0];

        return selectedItem;
    }

}
