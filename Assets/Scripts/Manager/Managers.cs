using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    // 인스턴스 생성
    private static ResourceManager s_resourceManager = new ResourceManager();
    private static UIManager s_uiManager = new UIManager();
    private static SoundManager s_soundManager = new SoundManager();
    private static SceneManagerEx s_sceneManager = new SceneManagerEx();


    // 프로퍼티 생성
    public static ResourceManager Resource { get { Init(); return s_resourceManager; } }
    public static UIManager UI{ get { Init(); return s_uiManager; }  }
    public static SoundManager Sound { get { Init(); return s_soundManager; }  }
    public static SceneManagerEx Scene { get { Init(); return s_sceneManager; } }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
                go = new GameObject { name = "@Managers" };

            s_instance = Utils.GetOrAddComponent<Managers>(go);
            DontDestroyOnLoad(go);

            s_resourceManager.Init();
            s_sceneManager.Init();
            s_soundManager.Init();
        }
    }
}
