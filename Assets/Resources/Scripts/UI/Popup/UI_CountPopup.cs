using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_CountPopup : UI_Popup
{
    UI_PlayerScene playerScene;
    private string _description;
    private string _productId;
    private float _effectValue1;
    private float _effectValue2;
    bool _isLuck;

    enum Images
    {
        CountDown1,
        CountDown2,
        BG,
        // CountDown3,
    }

    enum Texts
    {
        PreviousStage,
        ShowStage,
        AmountText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        
        int _Stage = Managers.Game.Stage;
        //배경 변경
        Managers.Game.changeBG();
        GetImage((int)Images.BG).sprite = Managers.Resource.Load<Sprite>($"Sprites/Background/{Managers.Game.BG}");

        //스테이지 검증
        if(Managers.Game.Stage == Define.NormalGameEnd)
            Managers.Game.PreviousStage = Define.NormalGameEnd;
        if(Managers.Game.Stage == Define.HardGameEnd)
            Managers.Game.PreviousStage = Define.HardGameEnd;
        if(Managers.Game.Stage == Define.UnLimitedGameEnd)
            Managers.Game.PreviousStage = Define.UnLimitedGameEnd;
        if(Managers.Game.Stage == Define.NightmareGameEnd)
            Managers.Game.PreviousStage = Define.NightmareGameEnd;


        Managers.Game.ReviveLife = false;

        GetText((int)Texts.PreviousStage).gameObject.SetActive(false);
        GetText((int)Texts.ShowStage).gameObject.SetActive(false);
        GetImage((int)Images.CountDown2).gameObject.SetActive(false);
        GetImage((int)Images.CountDown1).gameObject.SetActive(false);


        playerScene = Managers.UI.GetSceneUI<UI_PlayerScene>();
    
    // 10단계씩마다 음악 변경 로직
    if (_Stage >= 1 && _Stage <= 101)
    {
        string[] soundFiles = new string[]
        {
            "Sound_MainPlay1",
            "Sound_MainPlay10", // 10단계
            "Sound_MainPlay20", // 20단계
            "Sound_MainPlay30", // 30단계
            "Sound_MainPlay40", // 40단계
            "Sound_MainPlay50", // 50단계
            "Sound_MainPlay60", // 60단계
            "Sound_MainPlay70",  // 70단계
            "Sound_MainPlay80",  // 80단계
            "Sound_MainPlay90",  // 90단계
            "Sound_MainPlay100",  // 100단계
        };


        int index = (_Stage / 10); // 배열은 0부터 시작 (Stage 10 → index 0)
        string nextBgm = soundFiles[index];

        if (Managers.Game.BGM != nextBgm) // 동일한 음악이면 변경하지 않음
        {
            Managers.Game.BGM = nextBgm;
            Managers.Sound.Play(Sound.Bgm, Managers.Game.BGM);
        }
    }
        if(Managers.Game.BGM == null)
            Managers.Sound.Play(Sound.Bgm,"Sound_MainPlay10");

        GetText((int)Texts.PreviousStage).text = $" Stage : {Managers.Game.PreviousStage}";
        if(_Stage == Define.NormalGameEnd -1 && Managers.Game.DifficultyLevel == "Normal")
            GetText((int)Texts.ShowStage).text = $" Final Normal Stage : {_Stage}";
        if(_Stage == Define.HardGameEnd -1 && Managers.Game.DifficultyLevel == "Hard")
            GetText((int)Texts.ShowStage).text = $" Final Hard Stage : {_Stage}";
        if(_Stage == Define.UnLimitedGameEnd -1 && Managers.Game.DifficultyLevel == "UnLimited")
            GetText((int)Texts.ShowStage).text = $" Final Unlimited Stage : {_Stage}";
        if(_Stage == Define.NightmareGameEnd && Managers.Game.DifficultyLevel == "Nightmare")
            GetText((int)Texts.ShowStage).text = $" Final Nightmare Stage : {_Stage}";
        
        else
            GetText((int)Texts.ShowStage).text = $" Stage : {_Stage}";

        if(_description !=null)
        {
            GetText((int)Texts.AmountText).gameObject.SetActive(true);

            if(_effectValue1 != -1 && _isLuck)
            {
                Managers.Sound.Play(Sound.Effect , "Sound_Luck");
                GetText((int)Texts.AmountText).text = $"{_description} \n 두배 획득 성공! \n 힐링:{_effectValue1} -> {2*_effectValue1}";
            }
            if (_effectValue1 != -1 && !_isLuck)
            {
                Managers.Sound.Play(Sound.Effect , "Sound_GetItem");
                GetText((int)Texts.AmountText).text = $"{_description} \n 힐링:{_effectValue1}";
            }
            else if(_effectValue1 == -1 && _isLuck)
            {
                Managers.Sound.Play(Sound.Effect , "Sound_Luck");
                GetText((int)Texts.AmountText).text = $"두배 획득 성공! \n {_description}\n  {_effectValue2} -> {2 * _effectValue2}";
            }
            else if(_effectValue1 == -1 && !_isLuck)
            {
                Managers.Sound.Play(Sound.Effect , "Sound_GetItem");
                GetText((int)Texts.AmountText).text = $"{_description}";
            }
        }
        

        StartCoroutine(StartCountDown());
       
        return true;
    }
        // AmountText 값을 설정하는 메서드
public void SetAmountText(ShopData _selectedItem , bool isLuck = false)
{
    _description = _selectedItem.description;
    _productId = _selectedItem.productID;
    _effectValue1 = _selectedItem.effectValues[0];
    if(isLuck)
        _isLuck = true;

    //gambleHealing 일 때만 양도 가져가기
    else if (_selectedItem.productID == "gambleHealing")
    {
        _effectValue1 = _selectedItem.effectValues[0];
    }
    else
    {
        _effectValue1 = -1;
        _effectValue2 = _selectedItem.effectValues[0];
    }

    // if (_selectedItem.effectValues.Count > 2 )
    // {
    //     _effectValue3 = _selectedItem.effectValues[2];
    // }

}


 private IEnumerator StartCountDown()
    {
        if(Managers.Game.Stage != 0)
        {
            yield return new WaitForSeconds(2);
            GetText((int)Texts.AmountText).gameObject.SetActive(false);
            yield return ShowText((int)Texts.PreviousStage);
        }
        else
        {
            yield return ShowText((int)Texts.PreviousStage);
        }
            

        GetText((int)Texts.AmountText).gameObject.SetActive(false);

        yield return ShowText((int)Texts.ShowStage);

        // yield return ShowImage((int)Images.CountDown3);
        yield return ShowImage((int)Images.CountDown2);
        yield return ShowImage((int)Images.CountDown1);

        yield return new WaitForSeconds(0.5f); 
        Managers.UI.ClosePopupUI(this); 
        Managers.UI.ShowPopupUI<UI_QuestionPopup>(); 
    }

    private IEnumerator ShowText(int textIndex)
    {
        GetText(textIndex).gameObject.SetActive(true);
        if(textIndex == 1)
            Managers.Sound.Play(Sound.Effect, "Sound_StageUp");
        yield return new WaitForSeconds(1f);
        GetText(textIndex).gameObject.SetActive(false);
    }

    private IEnumerator ShowImage(int imageIndex)
    {
        GetImage(imageIndex).gameObject.SetActive(true);
        Managers.Sound.Play(Sound.Effect, "Sound_Checkbutton");
        yield return new WaitForSeconds(1f); // 1초 동안 이미지 표시
        GetImage(imageIndex).gameObject.SetActive(false);
    }
    // public void changeBG()
    // {
    //     int galaxyIndex = Managers.Game.Stage / 10 + 1;
    //     galaxyIndex = Math.Min(galaxyIndex, 8); // Limit to Galaxy8
    //     Managers.Game.BG = $"Galaxy{galaxyIndex}";
    // }



}
