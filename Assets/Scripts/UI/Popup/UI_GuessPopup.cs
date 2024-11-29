using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_GuessPopup : UI_Popup
{
    UI_PlayerScene playerScene;
    [SerializeField] UI_PlayPopup playPopup;
    //timer
    public float RemainTime;

    public int IncorrectCount;


    public bool isCorrect = true;
    enum Texts
    {
        ConfirmButtonText,
        Question,
        Timer,
    }
    enum Buttons
    {
        ConfirmButton,
        HairMinus,
        HairPlus,
        ClothesMinus,
        ClothesPlus,
        EyebrowMinus,
        EyebrowPlus,
        EyeMinus,
        EyePlus,
        MouthMinus,
        MouthPlus,
        EmotionMinus,
        EmotionPlus,
        AnimationMinus,
        AnimtionPlus,
    }

    enum Images
    {
        Correct,
        Wrong,
    }
    
    GameObject GuessPlayer;
    GameObject _customManager;
    CustomManager customManager;
    AnimationManager animationManager;


    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        playerScene = Managers.UI.GetSceneUI<UI_PlayerScene>();

        //타이머 관리
        RemainTime = Managers.Game.GuessTimer;
        //추측 틀린 개수 (hp 깎기용)
        IncorrectCount = 0;
  
        //GuessPlayer 생성
        GuessPlayer = Managers.Resource.Instantiate("Player");
        GuessPlayer.transform.position = new Vector3(-2,0,0);
        GuessPlayer.transform.localScale = new Vector3(1,1,1);

        _customManager = GameObject.FindGameObjectWithTag("GuessManager");
        customManager = _customManager.GetComponent<CustomManager>();
        animationManager = _customManager.GetComponent<AnimationManager>();

        //Guessplayer 질문 빼고 stranger와 같이 꾸미기
        customManager.hair = Managers.Game.StrangerIndex[0];
        customManager.clothes = Managers.Game.StrangerIndex[1];
        customManager.eyebrow = Managers.Game.StrangerIndex[2];
        customManager.eye = Managers.Game.StrangerIndex[3];
        customManager.mouth = Managers.Game.StrangerIndex[4];
        customManager.emotion = Managers.Game.StrangerIndex[5];
        animationManager.ani = Managers.Game.StrangerIndex[6];

		BindText(typeof(Texts));
		BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        // Text에 설정
        GetText((int)Texts.Question).text = Managers.GetText(Define.HairQuestion);
        GetText((int)Texts.Timer).text = $"{Managers.Game.GuessTimer}";

        //정답일 때
        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(() => OnClickConfirmButton(isCorrect));

        GetImage((int)Images.Correct).gameObject.SetActive(false);
        GetImage((int)Images.Wrong).gameObject.SetActive(false);

        //추측하는 버튼 바인딩
        #region 추측플레이어 바인딩
        GetButton((int)Buttons.HairMinus).gameObject.BindEvent(() => 
        {
            customManager.hair--; 
            customManager.numberCheck(0);  
        });
        GetButton((int)Buttons.HairPlus).gameObject.BindEvent(() => 
        {
            customManager.hair++; 
            customManager.numberCheck(0);  
        });

        GetButton((int)Buttons.ClothesMinus).gameObject.BindEvent(() => 
        {
            customManager.clothes--; 
            customManager.numberCheck(1);  
        });
        GetButton((int)Buttons.ClothesPlus).gameObject.BindEvent(() => 
        {
            customManager.clothes++; 
            customManager.numberCheck(1);  
        });

        GetButton((int)Buttons.EyebrowMinus).gameObject.BindEvent(() => 
        {
            customManager.eyebrow--; 
            customManager.numberCheck(2);  
        });
        GetButton((int)Buttons.EyebrowPlus).gameObject.BindEvent(() => 
        {
            customManager.eyebrow++; 
            customManager.numberCheck(2);  
        });

        GetButton((int)Buttons.EyeMinus).gameObject.BindEvent(() => 
        {
            customManager.eye--; 
            customManager.numberCheck(3);  
        });
        GetButton((int)Buttons.EyePlus).gameObject.BindEvent(() => 
        {
            customManager.eye++; 
            customManager.numberCheck(3);  
        });

        GetButton((int)Buttons.MouthMinus).gameObject.BindEvent(() => 
        {
            customManager.mouth--; 
            customManager.numberCheck(4);  
        });
        GetButton((int)Buttons.MouthPlus).gameObject.BindEvent(() => 
        {
            customManager.mouth++; 
            customManager.numberCheck(4);  
        });

        GetButton((int)Buttons.EmotionMinus).gameObject.BindEvent(() => 
        {
            customManager.emotion--; 
            customManager.numberCheck(5);  
        });
        GetButton((int)Buttons.EmotionPlus).gameObject.BindEvent(() => 
        {
            customManager.emotion++; 
            customManager.numberCheck(5);  
        });

        GetButton((int)Buttons.AnimationMinus).gameObject.BindEvent(() => 
        {
            animationManager.PlayAni(false);
        });
        GetButton((int)Buttons.AnimtionPlus).gameObject.BindEvent(() => 
        {
            animationManager.PlayAni(true);
        });
        
        
        
        #endregion



        //추측 플레이어 퀴즈빼고 매칭시켜두기 TODO

        return true;

    }

    void Update()
    {
        //시간이 다 지나면 자동으로 false
        RemainTime -= Time.deltaTime;
        GetText((int)Texts.Timer).text = $"{(int)RemainTime}";

        if (RemainTime <= 0)
        {
            GetButton((int)Buttons.ConfirmButton).gameObject.SetActive(false);
            GetImage((int)Images.Wrong).gameObject.SetActive(true);
            
            Invoke("HideResultAndProceed", 2f);
        }
    }

    //stranger와 추측 캐릭터 비교
    void CompareCharacter()
    {
        if (customManager.hair != Managers.Game.StrangerIndex[0])
            IncorrectCount++;
        if (customManager.clothes != Managers.Game.StrangerIndex[1])
            IncorrectCount++;
        if (customManager.eyebrow != Managers.Game.StrangerIndex[2])
            IncorrectCount++;
        if (customManager.eye != Managers.Game.StrangerIndex[3])
            IncorrectCount++;
        if (customManager.mouth != Managers.Game.StrangerIndex[4])
            IncorrectCount++;
        if (customManager.emotion != Managers.Game.StrangerIndex[5])
            IncorrectCount++;
        if (animationManager.ani != Managers.Game.StrangerIndex[6])
            IncorrectCount++;

            Debug.Log($"틀린 개수: {IncorrectCount}");
    }

    void OnClickConfirmButton(bool isCorrect)
    {
        //캐릭 비교
        CompareCharacter();

        if (IncorrectCount == 0)
        {
            Debug.Log("정답: Correct");
            GetButton((int)Buttons.ConfirmButton).gameObject.SetActive(false);
            GetImage((int)Images.Correct).gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("오답: Wrong");
            Managers.Game.Hp = Managers.Game.Hp - (IncorrectCount * Define.Damage) + Managers.Game.Defence;
            GetButton((int)Buttons.ConfirmButton).gameObject.SetActive(false);
            GetImage((int)Images.Wrong).gameObject.SetActive(true);
        }
        
        playerScene.HPUp();

        Invoke("HideResultAndProceed", 2f);
    }

    void HideResultAndProceed()
    {
        // 이미지 비활성화
        GetImage((int)Images.Correct).gameObject.SetActive(false);
        GetImage((int)Images.Wrong).gameObject.SetActive(false);

        if (GuessPlayer != null)
        {
            Managers.Resource.Destroy(GuessPlayer);
        }
        if(Managers.Game.Hp <0)
        {
            GameOver();
        }
        else
        {
            GameContinue();
        }
    }

    //게임오버일 경우
    void GameOver()
    {
        Managers.Sound.Play(Sound.Effect, "Sound_GameOver");
        Managers.UI.ClosePopupUI(this);
        Managers.UI.CloseSceneUI();
        // Managers.UI.ShowPopupUI<UI_GameOverPopup>();
        Managers.UI.ShowPopupUI<UI_TitlePopup>();
    }
    //게임오버가 아닌 경우 
    void GameContinue()
    {
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_GetItemPopup>();
    }

}
