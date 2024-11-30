using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerScene : UI_Scene
{
    enum Texts
    {
        Stage,
        HPText,
        PlayerInfo,
    }
    enum Images
    {
        HP,
        HPBG, //hp 배경바
    }
    
    GameManagerEx InitStat;
    UI_PlayPopup uI_PlayPopup;
    GameObject StaticPlayer;
    GameObject Character;
    GameObject _customManager;
    public CustomManager customManager;    
    public AnimationManager animationManager;

    // public static event Action OnPlayAnimationEvent;

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        //캐릭터 초기 스탯 가져오기
        Managers.Game.Init();
        InitStat = Managers.Game; 
        //TODO

        //static 캐릭터
        StaticPlayer = Managers.Resource.Instantiate("StaticPlayer");
        StaticPlayer.transform.position = new Vector3(-5,-3,0);
        StaticPlayer.transform.localScale = new Vector3(0.6f,0.6f, 1);

        _customManager = GameObject.FindGameObjectWithTag("StaticManager");
        customManager = _customManager.GetComponent<CustomManager>();
        animationManager = _customManager.GetComponent<AnimationManager>();

        customManager.clothes = 10;
        customManager.numberCheck(1);  

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetText((int)Texts.Stage).text = $"Stage : {Managers.Game.Stage}";
        GetText((int)Texts.HPText).text = $"{Managers.Game.Hp}";
        GetText((int)Texts.PlayerInfo).text = $"Luck : {Managers.Game.LuckPercent}% \nDefence : {Managers.Game.Defence} \nExpendTime: {Managers.Game.ExpendTime} \nGuessTimer: {Managers.Game.GuessTimer}";
        
        
        return true;
    }

    public void StageUp()
    {
        Managers.Game.Stage++;
        GetText((int)Texts.Stage).text = $"Stage : {Managers.Game.Stage}";
        GetText((int)Texts.PlayerInfo).text = $"Luck : {Managers.Game.LuckPercent}% \nDefence : {Managers.Game.Defence} \nExpendTime: {Managers.Game.ExpendTime} \nGuessTimer: {Managers.Game.GuessTimer}";
    }

    public void HPUp()
    {
        animationManager.ani = 6;
        if(Managers.Game.Hp < 0)
            Managers.Game.Hp = 0;
        GetText((int)Texts.HPText).text = $"{Managers.Game.Hp}";
    }

    //캐릭터 감정 표현 TODO
    public void StaticPlayerEx(string express)
    {
        // Character = Utils.FindChild(gameObject, "Character");
        // Animator _anim = Character.GetComponent<Animator>();

        if(express == "Wrong")
        {
            customManager.hair++; 
            customManager.numberCheck(0);  
        }
        else if(express == "Correct")
        {
             customManager.emotion++; 
            customManager.numberCheck(5);  
        }
        else
        {
            Debug.LogError("StaticPlayerEX Worng");
        }


    }
}
