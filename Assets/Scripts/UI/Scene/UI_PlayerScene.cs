using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerScene : UI_Scene
{
    enum Texts
    {
        Stage,
    }
    enum Images
    {
        HP,
        HPBG, //hp 배경바
    }
    public int _stage = 1;

    GameManagerEx _game;
    GameObject StaticPlayer;
    GameObject _customManager;
    public CustomManager customManager;    
    public AnimationManager animationManager;

    // public static event Action OnPlayAnimationEvent;

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        //static 캐릭터
        StaticPlayer = Managers.Resource.Instantiate("StaticPlayer");
        StaticPlayer.transform.position = new Vector3(-5,-3,0);
        StaticPlayer.transform.localScale = new Vector3(0.6f,0.6f, 1);

        _customManager = GameObject.FindGameObjectWithTag("StaticManager");
        customManager = _customManager.GetComponent<CustomManager>();
        animationManager = _customManager.GetComponent<AnimationManager>();

        customManager.clothes = 10;
        customManager.numberCheck(1);  
        animationManager.PlayAni(true);

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetText((int)Texts.Stage).text = $"Stage : {(int)_stage}";


        return true;
    }

    public void StageUp()
    {
        _stage++;
        GetText((int)Texts.Stage).text = $"Stage : {(int)_stage}";
    }

}
