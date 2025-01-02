using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        // 화면이 자동으로 회전하도록 설정
        Screen.orientation = ScreenOrientation.AutoRotation;

        Managers.UI.ShowPopupUI<UI_TitlePopup>();
        Debug.Log("Init");

        return true;
    }
}


