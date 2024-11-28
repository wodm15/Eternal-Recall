using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerScene : UI_Scene
{
    public GameObject StaticPlayer;

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        StaticPlayer = Managers.Resource.Instantiate("StaticPlayer");
        StaticPlayer.transform.position = new Vector3(-4,-4,0);
        StaticPlayer.transform.localScale = new Vector3(1,1, 1);

        return true;
    }
}
