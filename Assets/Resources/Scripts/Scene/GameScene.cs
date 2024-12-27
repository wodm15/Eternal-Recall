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

        // 화면이 자동으로 회전하도록 설정
        Screen.orientation = ScreenOrientation.AutoRotation;

        // 가로모드일 때 180도 회전 적용
        Screen.autorotateToPortrait = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        // 화면이 Landscape 모드일 때 180도 회전
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            Camera.main.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        Managers.UI.ShowPopupUI<UI_TitlePopup>();
        Debug.Log("Init");

        return true;
    }
}


