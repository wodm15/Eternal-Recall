using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.UI;

public class UI_PickupPopup : UI_Popup
{
    private List<StatData> _statData = new List<StatData>();
    CustomManager s_customManager;
    AnimationManager s_animationManager;
    GameObject Stranger;
    
    enum Buttons
    {
        Exit,
        Pickup1,
        Pickup2,
        Pickup3,
        Pickup4,
        Pickup5,
        PickPick,
    }
    enum Texts
    {
        ExitText,
        PickupText1,
        PickupText2,
        PickupText3,
        PickupText4,
        PickupText5,
        GetPercent,
        PickPickText,
        NoMoneyText,
        ExplainPower,
        MoneyText,
    }

    public override bool Init()
	{
		if (base.Init() == false)
			return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        foreach (StatData statData in Managers.Data.Stat.Values)
        {
            _statData.Add(statData);
        }

        GetText((int)Texts.ExitText).text = Managers.GetText(Define.GoToTitleText);
        GetButton((int)Buttons.Exit).gameObject.BindEvent(OnClickGoBack); 


        Stranger = Managers.Resource.Instantiate("Player");
        Stranger.transform.position = new Vector3(0,0,0);
        Stranger.transform.localScale = new Vector3(1f,1f,1f);        

        GameObject customManager2 = GameObject.FindGameObjectWithTag("GuessManager");
        s_customManager = customManager2.GetComponent<CustomManager>();
        s_animationManager = customManager2.GetComponent<AnimationManager>();

        GetText((int)Texts.PickupText1).text = Managers.GetText(Define.Pickup1);
        GetText((int)Texts.PickupText2).text = Managers.GetText(Define.Pickup2);
        GetText((int)Texts.PickupText3).text = Managers.GetText(Define.Pickup3);
        GetText((int)Texts.PickupText4).text = Managers.GetText(Define.Pickup4);
        GetText((int)Texts.PickupText5).text = Managers.GetText(Define.Pickup5);
        GetText((int)Texts.GetPercent).gameObject.SetActive(false);
        GetText((int)Texts.NoMoneyText).text = Managers.GetText(Define.NoMoneyText);
        GetText((int)Texts.NoMoneyText).gameObject.SetActive(false);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(false);
        GetText((int)Texts.PickPickText).text = Managers.GetText(Define.PickPickText);
        GetButton((int)Buttons.PickPick).gameObject.SetActive(false);
        GetText((int)Texts.MoneyText).text = $"X{Managers.Game.Money}";
        
        GetButton((int)Buttons.Pickup1).gameObject.BindEvent(() => OnClickPickup1());
        GetButton((int)Buttons.Pickup2).gameObject.BindEvent(() => OnClickPickup2());
        GetButton((int)Buttons.Pickup3).gameObject.BindEvent(() => OnClickPickup3());
        GetButton((int)Buttons.Pickup4).gameObject.BindEvent(() => OnClickPickup4());
        GetButton((int)Buttons.Pickup5).gameObject.BindEvent(() => OnClickPickup5());
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
        GetText((int)Texts.ExplainPower).text = _statData[3].Power;
    }
    public void OnClickPickup2()
    {
        pickupfunc(Define.Maid); //메이드
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup2Percent);
        GetText((int)Texts.ExplainPower).text = _statData[4].Power;
    }
    public void OnClickPickup3()
    {
        pickupfunc(Define.sailer); //세일러복
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup3Percent);
        GetText((int)Texts.ExplainPower).text = _statData[0].Power;
    }
    public void OnClickPickup4()
    {
        pickupfunc(Define.sailer); //잠옷
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup4Percent);
        GetText((int)Texts.ExplainPower).text = _statData[10].Power;
    }
    public void OnClickPickup5()
    {
        pickupfunc(Define.magic); //마법소녀
        GetButton((int)Buttons.PickPick).gameObject.SetActive(true);
        GetText((int)Texts.ExplainPower).gameObject.SetActive(true);
        GetText((int)Texts.GetPercent).text = Managers.GetText(Define.Pickup5Percent);
        GetText((int)Texts.ExplainPower).text = _statData[17].Power;
    }

    public void pickupfunc(int num)
    {
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
        StartCoroutine(VibrateUI(GetButton((int)Buttons.PickPick).gameObject, 0.2f, 5f));

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

    protected IEnumerator VibrateUI(GameObject uiObject, float duration, float magnitude)
    {
        RectTransform rectTransform = uiObject.GetComponent<RectTransform>();
        Vector3 originalPos = rectTransform.anchoredPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-magnitude, magnitude);
            float offsetY = Random.Range(-magnitude, magnitude);
            rectTransform.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPos;
    }

    public void RefreshUI()
    {
        GetText((int)Texts.MoneyText).text = $"X{Managers.Game.Money}";
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