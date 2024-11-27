using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerScene : UI_Scene
{
    GameObject Player;

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        for(int i=0; i<5; i++)
        {
            Debug.Log(UI_NamePopup.playerIndex[i]);
        }

        Player = GameObject.FindGameObjectWithTag("Player");
        
        if(Player != null)
        {
            GameObject customManager = Utils.FindChild(Player, "CustomManager");
            //스크립트 접근
            CustomManager _customManager = customManager.GetComponent<CustomManager>();
            _customManager.hair = 6;
            _customManager.clothes = UI_NamePopup.playerIndex[1]; 
            _customManager.eyebrow = UI_NamePopup.playerIndex[2]; 
            _customManager.eye = UI_NamePopup.playerIndex[3]; 
            _customManager.mouth = UI_NamePopup.playerIndex[4]; 
            _customManager.emotion = UI_NamePopup.playerIndex[5]; 
        }

        return true;
    }
}
