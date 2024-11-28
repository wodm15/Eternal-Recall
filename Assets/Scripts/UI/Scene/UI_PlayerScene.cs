using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerScene : UI_Scene
{
    GameObject StaticPlayer;

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        StaticPlayer = Managers.Resource.Instantiate("StaticPlayer");
        StaticPlayer.transform.position = new Vector3(-4,-3,0);
        StaticPlayer.transform.localScale = new Vector3(0.6f,0.6f, 1);

        GameObject _customManager = GameObject.FindGameObjectWithTag("StaticManager");
        CustomManager customManager = _customManager.GetComponent<CustomManager>();
        AnimationManager animationManager = _customManager.GetComponent<AnimationManager>();

        customManager.clothes = 10;
        customManager.numberCheck(1);  
        animationManager.PlayAni(true);
        
        return true;
    }
}
