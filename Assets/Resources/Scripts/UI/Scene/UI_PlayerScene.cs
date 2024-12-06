using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_PlayerScene : UI_Scene
{
    private List<CollectionData> _collectionData = new List<CollectionData>();
    enum Texts
    {
        Stage,
        HPText,
        PlayerInfo,
        CollectionSuccessText,
        GoTitleText
    }
    enum Images
    {
        HP,
        HPBG, //hp 배경바
        CollectionSuccessImage,
    }
    
    enum Buttons
    {
        GoTitle
    }
    UI_PlayPopup uI_PlayPopup;
    GameObject StaticPlayer;
    GameObject Character;
    GameObject _customManager;
    CustomManager customManager;    
    protected AnimationManager animationManager;

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        Managers.Game.OnNewCollection = OnNewCollection;
        //TODO

        foreach (CollectionData collectionData in Managers.Data.Collections.Values)
        {
            _collectionData.Add(collectionData);
        }

        //static 캐릭터
        StaticPlayer = Managers.Resource.Instantiate("StaticPlayer");
        StaticPlayer.transform.position = new Vector3(-5,-3,0);
        StaticPlayer.transform.localScale = new Vector3(0.6f,0.6f, 1);

        _customManager = GameObject.FindGameObjectWithTag("StaticManager");
        customManager = _customManager.GetComponent<CustomManager>();
        animationManager = _customManager.GetComponent<AnimationManager>();

        //헤어 다시 설정
        StaticPlayerEx();

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        GetText((int)Texts.Stage).text = $"{Managers.Game.Stage} 단계";
        GetText((int)Texts.HPText).text = $"{Managers.Game.Hp}";
        GetText((int)Texts.GoTitleText).text = Managers.GetText(Define.GOBACKText);
        //상태창
        GetText((int)Texts.PlayerInfo).text = $"이름 : {Managers.Game.Name} \n운: {Managers.Game.LuckPercent}% \n회피력: {Managers.Game.Avoid}% \n추측 시간: {Managers.Game.GuessTimer}초 \n힌트키: {Managers.Game.HintKey}개";
        GetImage((int)Images.CollectionSuccessImage).gameObject.SetActive(false);

        GetButton((int)Buttons.GoTitle).gameObject.BindEvent(ClearGame);

        return true;
    }


    public void StageUp()
    {
        Managers.Game.Stage++;
        
        //스테이지 70층이면 clear 검증
        if(Managers.Game.Stage >= 70)
        {
            Managers.UI.CloseAllPopupUI();
            Managers.Sound.Clear();
            Managers.UI.ShowPopupUI<UI_GameEndPopup>();
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        GetText((int)Texts.Stage).text = $"{Managers.Game.Stage}단계";
        GetText((int)Texts.PlayerInfo).text = $"이름 : {Managers.Game.Name} \n운: {Managers.Game.LuckPercent}% \n회피력: {Managers.Game.Avoid}% \n추측 시간: {Managers.Game.GuessTimer}초 \n힌트키: {Managers.Game.HintKey}개";
    }

    public void HPUp()
    {
        if(Managers.Game.Hp < 0)
            Managers.Game.Hp = 0;
        GetText((int)Texts.HPText).text = $"{Managers.Game.Hp}";
    }

    //캐릭터 감정 표현 TODO
    public void StaticPlayerEx(string express = null)
    {
        GameObject t = GameObject.FindGameObjectWithTag("StaticManager");
        customManager = t.GetComponent<CustomManager>();

        if(express == null)
        {
            customManager.clothes = Managers.Game.ClothesIndex;
            customManager.numberCheck(1);
        }

        if(express == "Initial")
        {
            customManager.eyebrow = 0;
            customManager.eye = 2;
            customManager.emotion = 0;
            customManager.mouth = 0;
            customManager.numberCheck(2);
            customManager.numberCheck(3);  
            customManager.numberCheck(4); 
            customManager.numberCheck(5); 
            Debug.Log("StaticPlayerExInitial");
        }

        else if(express == "Correct")
        {
            customManager.emotion = 0;
            customManager.mouth = 15;
            customManager.numberCheck(4); 
            customManager.numberCheck(5); 
            Debug.Log("StaticPlayerExCorrect");
        }
        else if(express == "Wrong")
        {
            Debug.Log("StaticPlayerExWrong");
            customManager.emotion =4;
            customManager.mouth = 20;
            customManager.numberCheck(4); 
            customManager.numberCheck(5); 
        }
        else if(express == "AvoidFail")
        {
            customManager.mouth = 19;
            customManager.emotion =1;
            customManager.numberCheck(4);
            customManager.numberCheck(5); 
        }
        else if(express == "AvoidSucess")
        {
            customManager.eye = 14;
            customManager.mouth =16;
            customManager.numberCheck(3);
            customManager.numberCheck(4); 
        }

        else if(express == "GameOver")
        {
            customManager.eye = 17;
            customManager.mouth = 19;
            customManager.numberCheck(3);
            customManager.numberCheck(4);
        }

        else if(express == "ChangeClothes")
        {
            customManager.clothes = 0;
            customManager.numberCheck(0); 
        }

            // customManager.hair = Managers.Game.StrangerIndex[0];
            // customManager.hairM.changeItem(Managers.Game.StrangerIndex[0]);


    }

    //업적 달성 시 collection
	void OnNewCollection(CollectionData data)
	{
        Debug.Log("onNewCollection go");
        GetText((int)Texts.CollectionSuccessText).gameObject.SetActive(true);
        GetImage((int)Images.CollectionSuccessImage).gameObject.SetActive(true);
        Managers.Sound.Play(Sound.Effect, "Sound_Archive");

		GetText((int)Texts.CollectionSuccessText).text = $"업적 달성! {data.description}";
        GetImage((int)Images.CollectionSuccessImage).sprite = Managers.Resource.Load<Sprite>(data.iconPath);
    
        Managers.Game.SaveGame();

		if (_coHideCollection != null)
			StopCoroutine(_coHideCollection);
		_coHideCollection = StartCoroutine(CoHideCollection(3.0f));
	}

	Coroutine _coHideCollection;
	IEnumerator CoHideCollection(float seconds)
	{
		yield return new WaitForSeconds(seconds);
        GetText((int)Texts.CollectionSuccessText).gameObject.SetActive(false);
        GetImage((int)Images.CollectionSuccessImage).gameObject.SetActive(false);
		// GetImage((int)Images.CollectionSuccess).gameObject.SetActive(false);
	}

    public void ClearGame()
    {
        Managers.Game.SaveGame();
        Managers.Game.CharacterDelete();
        Managers.UI.CloseAllPopupUI();
        Managers.UI.ShowPopupUI<UI_TitlePopup>();  
        Managers.UI.ClosePlayerSceneUI();
    }

}
