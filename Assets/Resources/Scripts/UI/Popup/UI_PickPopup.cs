using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.XR;

public class UI_PickPopup : UI_Popup
{
    int index;
    GameObject Player;
    GameObject Stranger;
    CustomManager customManager;
    AnimationManager animationManager;
    CustomManager s_customManager;
    AnimationManager s_animationManager;

    enum Images
    {
        WhenGetImage,
    }
    enum Buttons
    {
        Exit,
        PickupButton,
        GetSkinButton,
    }
    enum Texts
    {
        ExitText,
        MoneyText,
        PickupText,
        GetSkinText,
        WhenGetText,
    }
    public override bool Init()
	{
		if (base.Init() == false)
			return false;
        
        Player = GameObject.Find("StaticPlayer");
        Player.transform.position = new Vector3(-3,-1,0);
        Player.transform.localScale = new Vector3(1,1,1);

        Stranger = GameObject.Find("Player");
        Stranger.transform.position = new Vector3(3,-1,0);
        Stranger.transform.localScale = new Vector3(1f,1f,1f);

        GameObject _customManager = GameObject.FindGameObjectWithTag("StaticManager");
        customManager = _customManager.GetComponent<CustomManager>();
        animationManager = _customManager.GetComponent<AnimationManager>();
        

        GameObject customManager2 = GameObject.FindGameObjectWithTag("GuessManager");
        s_customManager = customManager2.GetComponent<CustomManager>();
        s_animationManager = customManager2.GetComponent<AnimationManager>();

        matching();

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.Exit).gameObject.BindEvent(()=> OnClickGoBack());
        GetButton((int)Buttons.PickupButton).gameObject.BindEvent(()=> OnClickPickup());
        GetButton((int)Buttons.GetSkinButton).gameObject.BindEvent(()=> OnClickGetSkin(index));

        

        GetText((int)Texts.ExitText).text = Managers.GetText(Define.GoToTitleText);
        GetText((int)Texts.MoneyText).text = $"X{Managers.Game.Money}";
        GetText((int)Texts.PickupText).text = "픽업";
        GetText((int)Texts.GetSkinText).text = "스킨 얻기";
        GetText((int)Texts.WhenGetText).text = "스킨을 얻었습니다!";


        GetButton((int)Buttons.GetSkinButton).gameObject.SetActive(false);
        GetImage((int)Images.WhenGetImage).gameObject.SetActive(false);
        GetText((int)Texts.WhenGetText).gameObject.SetActive(false);

        return true;
    }

    private void matching()
    {
        customManager.hair = s_customManager.hair;
        customManager.clothes = s_customManager.clothes;
        customManager.eyebrow = s_customManager.eyebrow;
        customManager.eye = s_customManager.eye;
        customManager.mouth = s_customManager.mouth;
        customManager.emotion = s_customManager.emotion;
        animationManager.ani = s_animationManager.ani;

        numberCheck();
    }

    //0부터 헤어 20까지, 옷 19까지, 눈썹 4까지, 눈 19까지, 입 23까지 , 감정 5까지, 
    private void OnClickPickup()
    {
        index = 0;
        if(Managers.Game.Money <= 500)
        {
            StartCoroutine(VibrateUI(GetButton((int)Buttons.PickupButton).gameObject, 0.2f, 5f));
            StartCoroutine(VibrateUI(GetText((int)Texts.MoneyText).gameObject, 0.2f, 5f));
            return;
        }
        
        Managers.Game.Money -= 500;
        RefreshUI();
        // s_customManager.eyebrow = 3;
        // s_customManager.mouth = 11;
        if(s_customManager.clothes == Define.Bikini) //비키니
        {
            customManager.hair = Random.Range(0,20 +1 );
            customManager.clothes = Random.Range(0, 19+1) ; 
            customManager.eyebrow = Random.Range(0, 4+1);
            customManager.eye = Random.Range(0, 19+1);
            customManager.mouth = Random.Range(0, 23+1);
            customManager.emotion = Random.Range(0, 5+1);
            index = 3;
        }

        else if(s_customManager.clothes == Define.Maid) //메이드 0.2%
        {
            customManager.clothes = Random.Range(0,20);
            customManager.hair = Random.Range(0,5);
            customManager.eyebrow = Random.Range(0, 5);
            index = 4;
        }

        else if(s_customManager.clothes == Define.sailer) //세일러복 1%
        {
            customManager.clothes = Random.Range(0,20);
            customManager.eye = Random.Range(0,5);
            index = 0;
        }

        else if(s_customManager.clothes == Define.nightWear) //잠옷 2.5%
        {
            customManager.clothes = Random.Range(0,20); 
            customManager.mouth = Random.Range(10,12);
            index = 10;
        }

        else if(s_customManager.clothes == Define.magic) //마법소녀 5%
        {
            customManager.clothes = Random.Range(0,20); 
            index = 17;
        }

        numberCheck();
        Managers.Game.SaveGame();

        //실행 결과가 맞으면
        if (customManager.clothes == s_customManager.clothes 
            && customManager.hair == s_customManager.hair
            && customManager.eyebrow == s_customManager.eyebrow
            && customManager.eye == s_customManager.eye
            && customManager.emotion == s_customManager.emotion)
        {
            GetButton((int)Buttons.GetSkinButton).gameObject.SetActive(true);
            GetButton((int)Buttons.PickupButton).gameObject.SetActive(false);

            Managers.Game.Collections[index] = CollectionState.Done;
            Managers.Game.SaveGame();
        
        }
            
        
    }

    public void numberCheck()
    {
        customManager.numberCheck(0);
        customManager.numberCheck(1);
        customManager.numberCheck(2);
        customManager.numberCheck(3);
        customManager.numberCheck(4);
        customManager.numberCheck(5);

        animationManager.ani--;
        animationManager.PlayAni(true);
    }

    public void RefreshUI()
    {
        GetText((int)Texts.MoneyText).text = $"X{Managers.Game.Money}";
    }
    private void OnClickGetSkin(int index)
    {
        GetImage((int)Images.WhenGetImage).gameObject.SetActive(true);
        GetText((int)Texts.WhenGetText).gameObject.SetActive(true);
        GetButton((int)Buttons.GetSkinButton).gameObject.SetActive(false);
        Managers.Game.Collections[index] = CollectionState.Done;
        StartCoroutine(DelayedGoBack());
    }
    private IEnumerator DelayedGoBack()
    {
        yield return new WaitForSeconds(3f); // 3초 대기
        OnClickGoBack(); // OnClickGoBack() 실행
    }


    public void InitialPlayer()
    {
	{
		customManager.hair = 2;
		customManager.clothes = 2;
		customManager.eyebrow = 0;
		customManager.eye = 2;
		customManager.mouth = 0;
		customManager.emotion = 0;

	    customManager.numberCheck(0);
        customManager.numberCheck(1);
        customManager.numberCheck(2);
        customManager.numberCheck(3);
        customManager.numberCheck(4);
        customManager.numberCheck(5);
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

    public void OnClickGoBack()
    {
        InitialPlayer();
        Managers.UI.ClosePopupUI(this);

        GameObject Player = GameObject.Find("StaticPlayer");
        Player.transform.position = new Vector3(0,50,0);
        Player.transform.localScale = new Vector3(0.5f,0.5f,0.5f);

        Stranger.transform.position = new Vector3(0,0,0);
        Stranger.transform.localScale = new Vector3(1f,1f,1f);

    }
}