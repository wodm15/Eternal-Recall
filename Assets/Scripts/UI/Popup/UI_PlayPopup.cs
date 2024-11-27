using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_PlayPopup : UI_Popup
{

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

		Managers.Sound.Clear();
		Managers.Sound.Play(Sound.Effect, "Sound_MainPlayBGM");

        
     
        return true;
    }
}

