using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_GuessPopup : UI_Popup
{
    UI_PlayerScene playerScene;
    [SerializeField] UI_QuestionPopup QuestionPopup;
    //timer
    public float RemainTime;

    public int IncorrectCount;

    // 타이머와 클릭 연동
    bool IsButtonClick;


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
        IsButtonClick = false;
  
        //GuessPlayer 생성
        GuessPlayer = Managers.Resource.Instantiate("Player");
        GuessPlayer.transform.position = new Vector3(-2,0,0);
        GuessPlayer.transform.localScale = new Vector3(1,1,1);

        _customManager = GameObject.FindGameObjectWithTag("GuessManager");
        customManager = _customManager.GetComponent<CustomManager>();
        animationManager = _customManager.GetComponent<AnimationManager>();

		BindText(typeof(Texts));
		BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        // Text에 설정
        GetText((int)Texts.Timer).text = $"{Managers.Game.GuessTimer}";

        #region 질문 설정 알고리즘
        //Text에 질문 설정

        LoadRandomQuiz();

        // foreach (QuizData Quiz in Managers.Data.Quiz.Values)
        // {
        //     if( (Managers.Game.Stage-1) / 10 + 1 == Quiz.Difficulty)
        //         Quiz += 
        //     // for(int i=1 ; i<= Quiz.quizType.Length ; i++)
        //     // {
        //     //     string isQuiz = Quiz.quizType.Substring(0, 1); // 맨 앞 문자 추출
        //     //     Quiz.quizType = Quiz.quizType.Substring(1);    // 맨 앞 문자 제거

        //     //     if(isQuiz == "1")
        //     // }

            
        //     if(Quiz.ID == 600)
        //         GetText((int)Texts.Question).text = $"{Quiz.kor}";
        // }

        #endregion
        
        //정답일 때
        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(() => OnClickConfirmButton(isCorrect));

        GetImage((int)Images.Correct).gameObject.SetActive(false);
        GetImage((int)Images.Wrong).gameObject.SetActive(false);

        //추측하는 버튼 바인딩
        #region 추측플레이어 바인딩
        GetButton((int)Buttons.HairMinus).gameObject.BindEvent(() => 
        {
            if ( 0 < customManager.hair )
                {
                    customManager.hair--; 
                }
            
            customManager.numberCheck(0);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.HairPlus).gameObject.BindEvent(() => 
        {
            if (customManager.hairM.count.Length > customManager.hair)
                    {
                        customManager.hair++; 
                    }
            customManager.numberCheck(0);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.ClothesMinus).gameObject.BindEvent(() => 
        {
            if ( 0 < customManager.clothes )
                {
                    customManager.clothes--; 
                }
            customManager.numberCheck(1);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.ClothesPlus).gameObject.BindEvent(() => 
        {
            if (customManager.clothesM.count.Length > customManager.clothes)
                    {
                        customManager.clothes++;
                    }

            customManager.numberCheck(1);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.EyebrowMinus).gameObject.BindEvent(() => 
        {
            if ( 0 < customManager.eyebrow )
                {
                    customManager.eyebrow--; 
                }
            
            customManager.numberCheck(2); 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton"); 
        });
        GetButton((int)Buttons.EyebrowPlus).gameObject.BindEvent(() => 
        {
            if (customManager.eyebrowM.count.Length > customManager.eyebrow )
                    {
                        customManager.eyebrow++; 
                    }
            customManager.numberCheck(2);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.EyeMinus).gameObject.BindEvent(() => 
        {
            if ( 0 < customManager.eye )
                {
                    customManager.eye--; 
                }
            customManager.numberCheck(3);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.EyePlus).gameObject.BindEvent(() => 
        {
            if (customManager.eyeM.count.Length > customManager.eye )
                    {
                        customManager.eye++; 
                    }

            customManager.numberCheck(3);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.MouthMinus).gameObject.BindEvent(() => 
        {
            if ( 0 < customManager.mouth )
                {
                    customManager.mouth--; 
                }
            customManager.numberCheck(4);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.MouthPlus).gameObject.BindEvent(() => 
        {
            if (customManager.mouthM.count.Length > customManager.mouth )
                    {
                        customManager.mouth++;
                    } 
            customManager.numberCheck(4); 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton"); 
        });

        GetButton((int)Buttons.EmotionMinus).gameObject.BindEvent(() => 
        {
            if ( 0 < customManager.emotion )
                {
                    customManager.emotion--; 
                }

            customManager.numberCheck(5);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.EmotionPlus).gameObject.BindEvent(() => 
        {
            if (customManager.emotionM.count.Length > customManager.emotion )
                    {
                        customManager.emotion++; 
                    } 
            customManager.numberCheck(5);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.AnimationMinus).gameObject.BindEvent(() => 
        {
            animationManager.PlayAni(false);
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.AnimtionPlus).gameObject.BindEvent(() => 
        {
            animationManager.PlayAni(true);
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        
        
        
        #endregion




        return true;

    }

    void Update()
    {
        //시간이 다 지나면 자동으로 false
        RemainTime -= Time.deltaTime;
        if (RemainTime < 0)
            RemainTime = 0;
            
        GetText((int)Texts.Timer).text = $"{(int)RemainTime}";
        
        if (RemainTime <= 0 && IsButtonClick == false)
        {
            GetButton((int)Buttons.ConfirmButton).gameObject.SetActive(false);
            GetImage((int)Images.Wrong).gameObject.SetActive(false);
            
            OnClickConfirmButton(isCorrect);
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
        IsButtonClick = true;
        Managers.Sound.Play(Sound.Effect, "Sound_CheckButton");
        //캐릭 비교
        CompareCharacter();

        if (IncorrectCount == 0) //정답일 경우
        {
            Managers.Sound.Play(Sound.Effect, "Sound_Correct");

            GetButton((int)Buttons.ConfirmButton).gameObject.SetActive(false);
            GetImage((int)Images.Correct).gameObject.SetActive(true);

            playerScene.StaticPlayerEx("Correct"); //TODO

        }
        else //오답일 경우
        {
            Debug.Log("오답: Wrong");
            Managers.Sound.Play(Sound.Effect, "Sound_Wrong");

            //데미지 계산
            Managers.Game.Hp = Managers.Game.Hp - (IncorrectCount * Define.Damage) + Managers.Game.Defence;

            GetButton((int)Buttons.ConfirmButton).gameObject.SetActive(false);
            GetImage((int)Images.Wrong).gameObject.SetActive(true);
            
            // if(tmp)
            //     tmp.transform.position = new Vector3(0, 0, 0);
            //     tmp.transform.localScale = new Vector3(0.005f, 0.005f, 0);
            
            playerScene.StaticPlayerEx("Wrong"); //TODO
        }
        
        playerScene.HPUp();

        Invoke("HideResultAndProceed", 2f);
    }

    void HideResultAndProceed()
    {
        // 이미지 비활성화
        GetImage((int)Images.Correct).gameObject.SetActive(false);
        GetImage((int)Images.Wrong).gameObject.SetActive(false);

        // if (GuessPlayer != null)
        // {
        //     Managers.Resource.Destroy(GuessPlayer);
        // }
        if(Managers.Game.Hp <= 0)
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
        Managers.Sound.Stop(Sound.Bgm);
        Managers.Sound.Play(Sound.Effect, "Sound_GameOver");
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_GameOverPopup>();
    }
    //게임오버가 아닌 경우 
    void GameContinue()
    {
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_GetItemPopup>();
    }

    void LoadRandomQuiz()
    {
        QuizData randomQuiz = Managers.Game.quizData;
        if(randomQuiz == null)
            Debug.Log("null임");
        Debug.Log(randomQuiz.kor);
        QuizFilterButton(randomQuiz.quizType);

        // 선택된 퀴즈의 질문 텍스트 출력
        GetText((int)Texts.Question).text = $"{randomQuiz.kor}";
    }

    public void QuizFilterButton(string QuizType)
    {
        string[] buttonNames = {
            "HairButton",
            "ClothesButton",
            "EyebrowButton",
            "EyeButton",
            "MouthButton",
            "EmotionButton",
            "AnimationButton"
        };

        List<GameObject> activeButtons = new List<GameObject>();
        RectTransform parentRect = GetComponent<RectTransform>(); // 부모 컨테이너의 RectTransform 가져오기

        // 모든 버튼에 대해 QuizType을 확인하고 활성화 또는 비활성화
        for (int i = 0; i < QuizType.Length && i < buttonNames.Length; i++)
        {
            string currentButton = buttonNames[i];
            GameObject button = Utils.FindChild(gameObject, currentButton, true);

            if (QuizType[i] == '1') // QuizType의 값이 '1'이면 버튼을 활성화
            {
                button.SetActive(true);
                activeButtons.Add(button); // 활성화된 버튼 리스트에 추가
            }
            else // QuizType의 값이 '0'이면 버튼 비활성화
            {
                button.SetActive(false);

                // i 값에 따라 customManager의 속성 설정
                if (i == 0)
                {
                    customManager.hair = Managers.Game.StrangerIndex[0];
                }
                else if (i == 1)
                {
                    customManager.clothes = Managers.Game.StrangerIndex[1];
                }
                else if (i == 2)
                {
                    customManager.eyebrow = Managers.Game.StrangerIndex[2];
                }
                else if (i == 3)
                {
                    customManager.eye = Managers.Game.StrangerIndex[3];
                }
                else if (i == 4)
                {
                    customManager.mouth = Managers.Game.StrangerIndex[4];
                }
                else if (i == 5)
                {
                    customManager.emotion = Managers.Game.StrangerIndex[5];
                }
                else if (i == 6)
                {
                    animationManager.ani = Managers.Game.StrangerIndex[6];
                }
            }
        }

        // 활성화된 버튼들을 위로 정렬
        float spacing = 50f; // 버튼 간 간격 (조정 가능)
        float yOffset = 0f;

        foreach (GameObject button in activeButtons)
        {
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(buttonRect.anchoredPosition.x, -yOffset);
            yOffset += spacing; // 다음 버튼의 Y 위치 증가
        }
    }



}
