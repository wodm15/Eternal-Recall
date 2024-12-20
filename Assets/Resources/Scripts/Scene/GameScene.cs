using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override bool Init()
    {
		if (base.Init() == false)
			return false;

        SceneType = Define.Scene.Game;
		Application.runInBackground = true;
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		
		Managers.UI.ShowPopupUI<UI_TitlePopup>();
		Debug.Log("Init");
		return true;
	}
}

