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
    private static DataManager s_dataManager = new DataManager();
    private static GameManagerEx s_gameManager = new GameManagerEx();
    private static AdsManager s_adsManager = new AdsManager();
    private static ValidFileManager s_validFileManager = new ValidFileManager();


    // 프로퍼티 생성
    public static ResourceManager Resource { get { Init(); return s_resourceManager; } }
    public static UIManager UI{ get { Init(); return s_uiManager; }  }
    public static SoundManager Sound { get { Init(); return s_soundManager; }  }
    public static SceneManagerEx Scene { get { Init(); return s_sceneManager; } }
    public static DataManager Data {get { Init(); return s_dataManager; } }
    public static GameManagerEx Game { get { Init(); return s_gameManager; } }
    public static AdsManager Ads { get { Init(); return s_adsManager; } }
    public static ValidFileManager CheckJson { get { Init(); return s_validFileManager;}}

    public static string GetText(int id)
	{
        if (Managers.Data.Texts.TryGetValue(id, out TextData value) == false)
            return "";

        return value.kor.Replace("{userName}", Managers.Game.Name);
	}
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

            // s_validFileManager.Init();
            s_resourceManager.Init();
            s_sceneManager.Init();
            s_soundManager.Init();
            s_dataManager.Init();

            Application.targetFrameRate = 60;
        }
    }
}
