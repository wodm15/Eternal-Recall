using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerEx
{
    public void Init()
    {

    }
    private Define.Scene _curSceneType = Define.Scene.Unknown;

    public Define.Scene CurrentSceneType
    {
        get
        {
            if (_curSceneType != Define.Scene.Unknown)
                return _curSceneType;
            return CurrentScene.SceneType;
        }
        set {  _curSceneType = value; }
    }

    public BaseScene CurrentScene { get { return GameObject.Find("Scene").GetComponent<BaseScene>(); } }

}
