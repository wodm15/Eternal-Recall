using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.XR;

public class UI_PickPopup : UI_Popup
{
    GameObject Player;
    GameObject Stranger;
    CustomManager customManager;
    AnimationManager animationManager;
    CustomManager s_customManager;
    AnimationManager s_animationManager;

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

        GetButton((int)Buttons.Exit).gameObject.BindEvent(()=> OnClickGoBack());
        GetButton((int)Buttons.PickupButton).gameObject.BindEvent(()=> OnClickPickup());
        GetButton((int)Buttons.GetSkinButton).gameObject.BindEvent(()=> OnClickGetSkin());

        GetText((int)Texts.ExitText).text = Managers.GetText(Define.GoToTitleText);
        GetText((int)Texts.MoneyText).text = $"X{Managers.Game.Money}";
        GetText((int)Texts.PickupText).text = "픽업";
        GetText((int)Texts.GetSkinText).text = "스킨 얻기";

        GetButton((int)Buttons.GetSkinButton).gameObject.SetActive(false);

        return true;
    }

    public void OnClickGoBack()
    {
        Managers.UI.ClosePopupUI(this);

        GameObject Player = GameObject.Find("StaticPlayer");
        Player.transform.position = new Vector3(0,50,0);
        Player.transform.localScale = new Vector3(0.5f,0.5f,0.5f);

        Stranger.transform.position = new Vector3(0,0,0);
        Stranger.transform.localScale = new Vector3(1f,1f,1f);
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
        if(Managers.Game.Money <= 500)
            return;
        
        Managers.Game.Money -= 500;
        RefreshUI();

        if(s_customManager.clothes == Define.Bikini) //비키니
        {
            customManager.hair = Random.Range(0,20 +1 );
            customManager.clothes = Random.Range(0, 19+1) ; 
            customManager.eyebrow = Random.Range(0, 4+1);
            customManager.eye = Random.Range(0, 19+1);
            customManager.mouth = Random.Range(0, 23+1);
            customManager.emotion = Random.Range(0, 5+1);
        }

        else if(s_customManager.clothes == Define.Maid) //메이드
        {
            customManager.clothes = Random.Range(0,20); // 19까지
        }

        else if(s_customManager.clothes == Define.sailer) //세일러복
        {
            customManager.clothes = Random.Range(0,20); // 19까지
        }

        else if(s_customManager.clothes == Define.nightWear) //잠옷
        {
            customManager.clothes = Random.Range(0,20); // 19까지
        }

        else if(s_customManager.clothes == Define.magic) //마법소녀
        {
            customManager.clothes = Random.Range(0,20); // 19까지
        }

        numberCheck();


        //실행 결과가 맞으면
        if (customManager.clothes == s_customManager.clothes 
            && customManager.hair == s_customManager.hair
            && customManager.eyebrow == s_customManager.eyebrow
            && customManager.eye == s_customManager.eye
            && customManager.emotion == s_customManager.emotion)
        {
            GetButton((int)Buttons.GetSkinButton).gameObject.SetActive(true);
            GetButton((int)Buttons.PickupButton).gameObject.SetActive(false);
        }

            
        
        
        Managers.Game.SaveGame();
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
    private void OnClickGetSkin()
    {

    }
}
