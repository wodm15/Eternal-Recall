using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class UI_PickupPopup : UI_Popup
{
    private List<StatData> _statData = new List<StatData>();
    CustomManager s_customManager;
    AnimationManager s_animationManager;
    GameObject Stranger;
    RectTransform ExplainimageRect;
    Vector2 Explainsize;
    RectTransform PercentimageRect;
    Vector2 Percentsize;

    enum GameObjects
    {
        Pick1Item,
        Pick2Item,
    }
    enum Buttons
    {
        Exit,
        Pickup1,
        Pickup2,
        Pickup3,
        Pickup4,
        Pickup5,
        Pickup6,
        Pickup7,
        Pickup8,
        Pickup9,
        Pickup10,
        Pickup11,
        PickPick,
        Toggle1Button,
        Toggle2Button,
    }
    enum Texts
    {
        ExitText,
        PickupText1,
        PickupText2,
        PickupText3,
        PickupText4,
        PickupText5,
        PickupText6,
        PickupText7,
        PickupText8,
        PickupText9,
        PickupText10,
        GetPercent,
        PickPickText,
        NoMoneyText,
        ExplainPower,
        MoneyText,
        IsGet1,
        IsGet2,
        IsGet3,
        IsGet4,
        IsGet5,
        IsGet6,
        IsGet7,
        IsGet8,
        IsGet9,
        IsGet10,
        Toggle1Text,
        Toggle2Text,
    }
    enum Images
    {
        ExplainPowerImage,
    }

    public override bool Init()
	{
		if (base.Init() == false)
			return false;

        Managers.Game.InterstitialAd();
        
        //구독
        UI_PickPopup.OnRefreshUI -= RefreshUI;
        UI_PickPopup.OnRefreshUI += RefreshUI;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        foreach (StatData statData in Managers.Data.Stat.Values)
        {
            _statData.Add(statData);
        }

        GetText((int)Texts.ExitText).text = Managers.GetText(Define.GoToTitleText);
        GetButton((int)Buttons.Exit).gameObject.BindEvent(OnClickGoBack); 

        RefreshUI();

        Stranger = Managers.Resource.Instantiate("Player");
        Stranger.transform.position = new Vector3(0,0,0);
        Stranger.transform.localScale = new Vector3(1f,1f,1f);        

        GameObject customManager2 = GameObject.FindGameObjectWithTag("GuessManager");
        s_customManager = customManager2.GetComponent<CustomManager>();
        s_animationManager = customManager2.GetComponent<AnimationManager>();

        GetObject((int)GameObjects.Pick1Item).SetActive(true);
        GetObject((int)GameObjects.Pick2Item).SetActive(false);

        GetText((int)Texts.PickupText1).text = Managers.GetText(Define.Pickup1);
        GetText((int)Texts.PickupText2).text = Managers.GetText(Define.Pickup2);
        GetText((int)Texts.PickupText3).text = Managers.GetText(Define.Pickup3);
        GetText((int)Texts.PickupText4).text = Managers.GetText(Define.Pickup4);
        GetText((int)Texts.PickupText5).text = Managers.GetText(Define.Pickup5);
        GetText((int)Texts.PickupText6).text = Managers.GetText(Define.Pickup6);
        GetText((int)Texts.PickupText7).text = Managers.GetText(Define.Pickup7);
        GetText((int)Texts.PickupText8).text = Managers.GetText(Define.Pickup8);
        GetText((int)Texts.PickupText9).text = Managers.GetText(Define.Pickup9);
        GetText((int)Texts.PickupText10).text = Managers.GetText(Define.Pickup10);
        GetText((int)Texts.Toggle1Text).text = "1";
        GetText((int)Texts.Toggle2Text).text = "2";


        GetText((int)Texts.GetPercent).gameObject.SetActive(false);
        GetText((int)Texts.NoMoneyText).text = Managers.GetText(Define.NoMoneyText);
        GetText((int)Texts.NoMoneyText).gameObject.SetActive(false);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(false);
        GetText((int)Texts.PickPickText).text = Managers.GetText(Define.PickPickText);
        GetButton((int)Buttons.PickPick).gameObject.SetActive(false);
        GetText((int)Texts.MoneyText).text = $"X{Managers.Game.Money}";

        ExplainimageRect = GetImage((int)Images.ExplainPowerImage).GetComponent<RectTransform>();
        Explainsize = ExplainimageRect.sizeDelta;
        // PercentimageRect = GetImage((int)Images.GetPercentImage).GetComponent<RectTransform>();
        // Percentsize = PercentimageRect.sizeDelta;

        GetButton((int)Buttons.Toggle1Button).gameObject.BindEvent(() => 
        {
            GetObject((int)GameObjects.Pick1Item).SetActive(true);
            GetObject((int)GameObjects.Pick2Item).SetActive(false);
        });
        GetButton((int)Buttons.Toggle2Button).gameObject.BindEvent(() => 
        {
            GetObject((int)GameObjects.Pick1Item).SetActive(false);
            GetObject((int)GameObjects.Pick2Item).SetActive(true);
        });
        
        GetButton((int)Buttons.Pickup1).gameObject.BindEvent(() => OnClickPickup1());
        GetButton((int)Buttons.Pickup2).gameObject.BindEvent(() => OnClickPickup2());
        GetButton((int)Buttons.Pickup3).gameObject.BindEvent(() => OnClickPickup3());
        GetButton((int)Buttons.Pickup4).gameObject.BindEvent(() => OnClickPickup4());
        GetButton((int)Buttons.Pickup5).gameObject.BindEvent(() => OnClickPickup5());
        GetButton((int)Buttons.Pickup6).gameObject.BindEvent(() => OnClickPickup6());
        GetButton((int)Buttons.Pickup7).gameObject.BindEvent(() => OnClickPickup7());
        GetButton((int)Buttons.Pickup8).gameObject.BindEvent(() => OnClickPickup8());
        GetButton((int)Buttons.Pickup9).gameObject.BindEvent(() => OnClickPickup9());
        GetButton((int)Buttons.Pickup10).gameObject.BindEvent(() => OnClickPickup10());
        GetButton((int)Buttons.PickPick).gameObject.BindEvent(() => OnClickPickPick());

        // Player.gameObject.SetActive(false);

        // Managers.Game.Money = 59;
        // Managers.Game.SaveGame();

        return true;
    }

    public void OnClickPickup1()
    {
        pickupfunc(Define.Bikini); //비키니
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup1Percent);
        GetText((int)Texts.ExplainPower).text = _statData[Define.Bikini].Power;
    }
    public void OnClickPickup2()
    {
        pickupfunc(Define.Maid); //메이드
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup2Percent);
        GetText((int)Texts.ExplainPower).text = _statData[Define.Maid].Power;
    }
    public void OnClickPickup3()
    {
        pickupfunc(Define.sailer); //세일러복
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup3Percent);
        GetText((int)Texts.ExplainPower).text = _statData[Define.sailer].Power;
    }
    public void OnClickPickup4()
    {
        pickupfunc(Define.nightWear); //잠옷
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup4Percent);
        GetText((int)Texts.ExplainPower).text = _statData[Define.nightWear].Power;
    }
    public void OnClickPickup5()
    {
        pickupfunc(Define.magic); //마법소녀
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup5Percent);
        GetText((int)Texts.ExplainPower).text = _statData[Define.magic].Power;
    }
    public void OnClickPickup6()
    {
        pickupfunc(Define.Swimsuit2); //수영복2
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup5Percent);
        GetText((int)Texts.ExplainPower).text = _statData[Define.Swimsuit2].Power;
    }
    public void OnClickPickup7()
    {
        pickupfunc(Define.weddingDress); //웨딩드레스
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup5Percent);
        GetText((int)Texts.ExplainPower).text = _statData[Define.weddingDress].Power;
    }
    public void OnClickPickup8()
    {
        pickupfunc(Define.Hanbok); //한복
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup5Percent);
        GetText((int)Texts.ExplainPower).text = _statData[Define.Hanbok].Power;
    }
    public void OnClickPickup9()
    {
        pickupfunc(Define.Yukata); //유카타
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup5Percent);
        GetText((int)Texts.ExplainPower).text = _statData[Define.Yukata].Power;
    }
    public void OnClickPickup10()
    {
        pickupfunc(Define.Princess); //공주옷
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup5Percent);
        GetText((int)Texts.ExplainPower).text = _statData[Define.Princess].Power;
    }

    public void pickupfunc(int num)
    {
        Explainsize.x  = _statData[num].Power.Length * 19;
        ExplainimageRect.sizeDelta = Explainsize;

        // Percentsize.x = GetText((int)Texts.GetPercent).text.Length * 19;
        // PercentimageRect.sizeDelta = Percentsize;
        

        s_customManager.clothes = num;
        s_customManager.eyebrow = 3;
        s_customManager.mouth = 11;

        s_customManager.numberCheck(0);
        s_customManager.numberCheck(1);
        s_customManager.numberCheck(2);
        s_customManager.numberCheck(3);
        s_customManager.numberCheck(4);
        s_customManager.numberCheck(5);

        s_animationManager.ani = 1;
        s_animationManager.PlayAni(true);
    
        // s_customManager.emotion = n;
        // StartCoroutine(VibrateUI(GetButton((int)Buttons.PickPick).gameObject, 0.2f, 5f));

        GetText((int)Texts.GetPercent).gameObject.SetActive(true);
    }

    private void OnClickPickPick()
    {
        if(Managers.Game.Money < 50)
        {
            GetText((int)Texts.NoMoneyText).gameObject.SetActive(true);
            return;
        }
        else
        {
            Managers.UI.ShowPopupUI<UI_PickPopup>();
        }

    }


    public void RefreshUI()
    {
        GetText((int)Texts.MoneyText).text = $"X{Managers.Game.Money}";
    
        if(Managers.Game.Collections[Define.magic] == CollectionState.Done)
        {
            GetText((int)Texts.IsGet1).text= Managers.GetText(Define.YesGet);
            GetText((int)Texts.IsGet1).color = Color.green;
        }
        else
        {
            GetText((int)Texts.IsGet1).text= Managers.GetText(Define.NoGet);
            GetText((int)Texts.IsGet1).color = Color.red;
        }

        if(Managers.Game.Collections[Define.nightWear] == CollectionState.Done)
        {
            GetText((int)Texts.IsGet2).text= Managers.GetText(Define.YesGet);
            GetText((int)Texts.IsGet2).color = Color.green;
        }
        else
        {
            GetText((int)Texts.IsGet2).text= Managers.GetText(Define.NoGet);
            GetText((int)Texts.IsGet2).color = Color.red;
        }
        if(Managers.Game.Collections[Define.sailer] == CollectionState.Done)
        {
            GetText((int)Texts.IsGet3).text= Managers.GetText(Define.YesGet);
            GetText((int)Texts.IsGet3).color = Color.green;
        }
        else
        {
            GetText((int)Texts.IsGet3).text= Managers.GetText(Define.NoGet);
            GetText((int)Texts.IsGet3).color = Color.red;
        }
        if(Managers.Game.Collections[Define.Maid] == CollectionState.Done)
        {
            GetText((int)Texts.IsGet4).text= Managers.GetText(Define.YesGet);
            GetText((int)Texts.IsGet4).color = Color.green;
        }
        else
        {
            GetText((int)Texts.IsGet4).text= Managers.GetText(Define.NoGet);
            GetText((int)Texts.IsGet4).color = Color.red;
        }
        if(Managers.Game.Collections[Define.Bikini] == CollectionState.Done)
        {
            GetText((int)Texts.IsGet5).text= Managers.GetText(Define.YesGet);
            GetText((int)Texts.IsGet5).color = Color.green;
        }
        else
        {
            GetText((int)Texts.IsGet5).text= Managers.GetText(Define.NoGet);
            GetText((int)Texts.IsGet5).color = Color.red;
        }
        if(Managers.Game.Collections[Define.Princess] == CollectionState.Done)
        {
            GetText((int)Texts.IsGet6).text= Managers.GetText(Define.YesGet);
            GetText((int)Texts.IsGet6).color = Color.green;
        }
        else
        {
            GetText((int)Texts.IsGet6).text= Managers.GetText(Define.NoGet);
            GetText((int)Texts.IsGet6).color = Color.red;
        }
        if(Managers.Game.Collections[Define.Yukata] == CollectionState.Done)
        {
            GetText((int)Texts.IsGet7).text= Managers.GetText(Define.YesGet);
            GetText((int)Texts.IsGet7).color = Color.green;
        }
        else
        {
            GetText((int)Texts.IsGet7).text= Managers.GetText(Define.NoGet);
            GetText((int)Texts.IsGet7).color = Color.red;
        }
        if(Managers.Game.Collections[Define.Hanbok] == CollectionState.Done)
        {
            GetText((int)Texts.IsGet8).text= Managers.GetText(Define.YesGet);
            GetText((int)Texts.IsGet8).color = Color.green;
        }
        else
        {
            GetText((int)Texts.IsGet8).text= Managers.GetText(Define.NoGet);
            GetText((int)Texts.IsGet8).color = Color.red;
        }
            if(Managers.Game.Collections[Define.weddingDress] == CollectionState.Done)
        {
            GetText((int)Texts.IsGet9).text= Managers.GetText(Define.YesGet);
            GetText((int)Texts.IsGet9).color = Color.green;
        }
        else
        {
            GetText((int)Texts.IsGet9).text= Managers.GetText(Define.NoGet);
            GetText((int)Texts.IsGet9).color = Color.red;
        }
            if(Managers.Game.Collections[Define.Swimsuit2] == CollectionState.Done)
        {
            GetText((int)Texts.IsGet10).text= Managers.GetText(Define.YesGet);
            GetText((int)Texts.IsGet10).color = Color.green;
        }
        else
        {
            GetText((int)Texts.IsGet10).text= Managers.GetText(Define.NoGet);
            GetText((int)Texts.IsGet10).color = Color.red;
        }
    }


    public void OnClickGoBack()
    {
        Managers.UI.ClosePopupUI(this);

        GameObject Player = GameObject.Find("StaticPlayer");
        Player.transform.position = new Vector3(0,-1,0);
        Player.transform.localScale = new Vector3(0.5f,0.5f,0.5f);

        Managers.Resource.Destroy(Stranger);
    }

}