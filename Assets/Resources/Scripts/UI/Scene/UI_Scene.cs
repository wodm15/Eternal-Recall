using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        // Camera.main을 전달하여 Canvas에 카메라 설정
        Managers.UI.SetCanvas(gameObject, Camera.main, false);
        return true;
    }
}
