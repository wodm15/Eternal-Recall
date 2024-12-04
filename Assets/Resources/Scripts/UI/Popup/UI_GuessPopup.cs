using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_GuessPopup : UI_Popup
{
    UI_PlayerScene playerScene;
    [SerializeField] UI_QuestionPopup QuestionPopup;
    //timer
    public float RemainTime;
    public bool isAvoid;

    private int _incorrectCount = 0;
    public int IncorrectCount
    {
        get { return _incorrectCount; }
        set { _incorrectCount = value; }
    }

    // 타이머와 클릭 연동
    bool IsButtonClick;
    
    GameObject Stranger;
    GameObject Button;
    private bool _isMoving = false; // 이동 상태 플래그


    public bool isCorrect = true;
    enum Texts
    {
        ConfirmButtonText,
        Question,
        Timer,
        TheWorldText,
        AvoidButtonText,
        Avoid,
        HairText,
        ClothesText,
        EyebrowText,
        EyeText,
        MouthText,
        EmotionText,
        AnimationText,
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
        AnimationPlus,
        HairHint,
        ClothesHint,
        EyebrowHint,
        EyeHint,
        MouthHint,
        EmotionHint,
        AnimationHint,
        TheWorldButton,
        AvoidButton,
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

        Stranger = GameObject.Find("Stranger");
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
        GetText((int)Texts.TheWorldText).text = $"{Managers.Game.TheWorld}";
        GetButton((int)Buttons.TheWorldButton).gameObject.SetActive(Managers.Game.TheWorld > 0);
        GetText((int)Texts.AvoidButtonText).text = Managers.GetText(Define.AvoidButtonText);
        GetButton((int)Buttons.AvoidButton).gameObject.SetActive(false);
        GetText((int)Texts.Avoid).gameObject.SetActive(false);

        //초반 Guess 어딘지 확인
        GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {customManager.hairM.count.Length}"; 
        GetText((int)Texts.ClothesText).text = $"Clothes {customManager.clothes+1} / {customManager.clothesM.count.Length}"; 
        GetText((int)Texts.EyebrowText).text = $"Eyebrow {customManager.eyebrow+1} / {customManager.eyebrowM.count.Length}"; 
        GetText((int)Texts.EyeText).text = $"Eye {customManager.eye+1} / {customManager.eyeM.count.Length}"; 
        GetText((int)Texts.MouthText).text = $"Mouth {customManager.mouth+1} / {customManager.mouthM.count.Length}"; 
        GetText((int)Texts.EmotionText).text = $"Emotion {customManager.emotion+1} / {customManager.emotionM.count.Length}"; 
        GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {customManager.hairM.count.Length}"; 
        GetText((int)Texts.AnimationText).text = $"Animation {animationManager.ani+1} / 11"; 
        #region 질문 설정 알고리즘
        //Text에 질문 설정
        LoadRandomQuiz();

        #endregion
        
        GetButton((int)Buttons.TheWorldButton).gameObject.BindEvent(() => 
        {
            OnClickTheWorld(); 
            GetButton((int)Buttons.TheWorldButton).gameObject.SetActive(false);
        });

        //정답 확인
        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(() => OnClickConfirmButton(isCorrect));

        GetImage((int)Images.Correct).gameObject.SetActive(false);
        GetImage((int)Images.Wrong).gameObject.SetActive(false);

        
        CheckHintZero();
        

        #region 힌트키 바인드
        GetButton((int)Buttons.HairHint).gameObject.BindEvent(() => 
        {
            Managers.Sound.Play(Sound.Effect, "Sound_Hint");
            UseHintKey("HairHint");
        });
        GetButton((int)Buttons.ClothesHint).gameObject.BindEvent(() => 
        {
            Managers.Sound.Play(Sound.Effect, "Sound_Hint");
            UseHintKey("ClothesHint");
        });
        GetButton((int)Buttons.EyebrowHint).gameObject.BindEvent(() => 
        {
            Managers.Sound.Play(Sound.Effect, "Sound_Hint");
            UseHintKey("EyebrowHint");
        });
        GetButton((int)Buttons.EyeHint).gameObject.BindEvent(() => 
        {
            Managers.Sound.Play(Sound.Effect, "Sound_Hint");
            UseHintKey("EyeHint");
        });
        GetButton((int)Buttons.EyeHint).gameObject.BindEvent(() => 
        {
            Managers.Sound.Play(Sound.Effect, "Sound_Hint");
            UseHintKey("EyeHint");
        });
        GetButton((int)Buttons.MouthHint).gameObject.BindEvent(() => 
        {
            Managers.Sound.Play(Sound.Effect, "Sound_Hint");
            UseHintKey("MouthHint");
        });
        GetButton((int)Buttons.EmotionHint).gameObject.BindEvent(() => 
        {
            Managers.Sound.Play(Sound.Effect, "Sound_Hint");
            UseHintKey("EmotionHint");
        });
        GetButton((int)Buttons.AnimationHint).gameObject.BindEvent(() => 
        {
            Managers.Sound.Play(Sound.Effect, "Sound_Hint");
            UseHintKey("AnimationHint");
        });
        
        #endregion
        //추측하는 버튼 바인딩
        #region 추측플레이어 바인딩
        GetButton((int)Buttons.HairMinus).gameObject.BindEvent(() => 
        {
            if(customManager.hair == 0)
            {
                customManager.hair = customManager.hairM.count.Length -1;
            }
            else if ( 0 < customManager.hair )
                {
                    customManager.hair--; 
                }
            
            customManager.numberCheck(0);  
            GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {customManager.hairM.count.Length}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        
        });
        GetButton((int)Buttons.HairPlus).gameObject.BindEvent(() => 
        {
            if(customManager.hairM.count.Length <= customManager.hair+1)
            {
                customManager.hair = 0;
            }
            else if (customManager.hairM.count.Length > customManager.hair)
                {
                    customManager.hair++; 
                }
            customManager.numberCheck(0);  
            GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {customManager.hairM.count.Length}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.ClothesMinus).gameObject.BindEvent(() => 
        {
            if(customManager.clothes == 0)
            {
                customManager.clothes = customManager.clothesM.count.Length -1;
            }
            else if ( 0 < customManager.clothes )
                {
                    customManager.clothes--; 
                }
            customManager.numberCheck(1);  
            GetText((int)Texts.ClothesText).text = $"Clothes {customManager.clothes+1} / {customManager.clothesM.count.Length}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.ClothesPlus).gameObject.BindEvent(() => 
        {
            if(customManager.clothesM.count.Length <= customManager.clothes+1)
            {
                customManager.clothes = 0;
            }
            else if (customManager.clothesM.count.Length > customManager.clothes)
                {
                    customManager.clothes++; 
                }
            GetText((int)Texts.ClothesText).text = $"Clothes {customManager.clothes+1} / {customManager.clothesM.count.Length}";
            customManager.numberCheck(1);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.EyebrowMinus).gameObject.BindEvent(() => 
        {
            if(customManager.eyebrow == 0)
            {
                customManager.eyebrow = customManager.eyebrowM.count.Length -1;
            }
            else if ( 0 < customManager.eyebrow )
                {
                    customManager.eyebrow--; 
                }
                
            customManager.numberCheck(2); 
            GetText((int)Texts.EyebrowText).text = $"Eyebrow {customManager.eyebrow+1} / {customManager.eyebrowM.count.Length}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton"); 
        });
        GetButton((int)Buttons.EyebrowPlus).gameObject.BindEvent(() => 
        {
            if(customManager.eyebrowM.count.Length <= customManager.eyebrow+1)
            {
                customManager.eyebrow = 0;
            }
            else if (customManager.eyebrowM.count.Length > customManager.eyebrow)
                {
                    customManager.eyebrow++; 
                }
            customManager.numberCheck(2);  
            GetText((int)Texts.EyebrowText).text = $"Eyebrow {customManager.eyebrow+1} / {customManager.eyebrowM.count.Length}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.EyeMinus).gameObject.BindEvent(() => 
        {
            if(customManager.eye == 0)
            {
                customManager.eye = customManager.eyeM.count.Length -1;
            }
            else if ( 0 < customManager.eye )
                {
                    customManager.eye--; 
                }
            customManager.numberCheck(3);  
            GetText((int)Texts.EyeText).text = $"Eye {customManager.eye+1} / {customManager.eyeM.count.Length}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.EyePlus).gameObject.BindEvent(() => 
        {
            if(customManager.eyeM.count.Length <= customManager.eye+1)
            {
                customManager.eye = 0;
            }
            else if (customManager.eyeM.count.Length > customManager.eye)
                {
                    customManager.eye++; 
                }

            customManager.numberCheck(3);  
            GetText((int)Texts.EyeText).text = $"Eye {customManager.eye+1} / {customManager.eyeM.count.Length}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.MouthMinus).gameObject.BindEvent(() => 
        {
            if(customManager.mouth == 0)
            {
                customManager.mouth = customManager.mouthM.count.Length -1;
            }
            else if ( 0 < customManager.mouth )
                {
                    customManager.mouth--; 
                }
            customManager.numberCheck(4);  
            GetText((int)Texts.MouthText).text = $"Mouth {customManager.mouth+1} / {customManager.mouthM.count.Length}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.MouthPlus).gameObject.BindEvent(() => 
        {
            if(customManager.mouthM.count.Length <= customManager.mouth+1)
            {
                customManager.mouth = 0;
            }
            else if (customManager.mouthM.count.Length > customManager.mouth)
                {
                    customManager.mouth++; 
                }
            customManager.numberCheck(4); 
            GetText((int)Texts.MouthText).text = $"Mouth {customManager.mouth+1} / {customManager.mouthM.count.Length}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton"); 
        });

        GetButton((int)Buttons.EmotionMinus).gameObject.BindEvent(() => 
        {
            if(customManager.emotion == 0)
            {
                customManager.emotion = customManager.emotionM.count.Length -1;
            }
            else if ( 0 < customManager.emotion )
                {
                    customManager.emotion--; 
                }

            customManager.numberCheck(5); 
            GetText((int)Texts.EmotionText).text = $"Emotion {customManager.emotion+1} / {customManager.emotionM.count.Length}"; 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.EmotionPlus).gameObject.BindEvent(() => 
        {
            if(customManager.emotionM.count.Length <= customManager.emotion+1)
            {
                customManager.emotion = 0;
            }
            else if (customManager.emotionM.count.Length > customManager.emotion)
                {
                    customManager.emotion++; 
                }
            customManager.numberCheck(5);  
            GetText((int)Texts.EmotionText).text = $"Emotion {customManager.emotion+1} / {customManager.emotionM.count.Length}"; 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.AnimationMinus).gameObject.BindEvent(() => 
        {
            if(animationManager.ani == 0)
            {
                animationManager.ani = 11;
                animationManager.PlayAni(false);
            }
            else
                animationManager.PlayAni(false);

            GetText((int)Texts.AnimationText).text = $"Animation {animationManager.ani+1} / 11"; 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.AnimationPlus).gameObject.BindEvent(() => 
        {
            if(animationManager.ani == 10)
            {
                animationManager.ani = -1;
                animationManager.PlayAni(true);
            }
            else
                animationManager.PlayAni(true);

            GetText((int)Texts.AnimationText).text = $"Animation {animationManager.ani+1} / 11"; 
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
        
        //3초 이내는 빨간색
        if (RemainTime <= 4)
        {
            GetText((int)Texts.Timer).color = Color.red;  
        }
        else
        {
            GetText((int)Texts.Timer).color = Color.black;  
        }

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
        CloseAllButton();

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

            Invoke("HideResultAndProceed", 2f);
        }
        else // 오답일 경우
        {
            Debug.Log("오답: Wrong");
            Managers.Sound.Play(Sound.Effect, "Sound_Wrong");
            isAvoid = false;
            // 회피 되는지 확인 
            if (Random.Range(0, 100) < Managers.Game.Avoid)
            {

                GetButton((int)Buttons.AvoidButton).gameObject.SetActive(true);
                GetButton((int)Buttons.TheWorldButton).gameObject.SetActive(false);

                GetButton((int)Buttons.AvoidButton).gameObject.BindEvent(() =>
                {
                    if (Random.Range(1, 3) == 1)
                    {
                        Debug.Log("회피 성공!");
                        isAvoid = true;
                        playerScene.StaticPlayerEx("AvoidSucess"); 
                        // 회피 성공 처리 (TODO 회피 애니메이션 등)
                        Managers.Sound.Play(Sound.Effect, "Sound_Avoid"); 

                        GetText((int)Texts.Avoid).text = Managers.GetText(Define.AvoidSucessText);
                        GetText((int)Texts.Avoid).gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("회피 실패!");
                        playerScene.StaticPlayerEx("AvoidFail"); 
                        isAvoid = false;
                        // 데미지 계산

                        DamageCalculate();
                        GetText((int)Texts.Avoid).text = Managers.GetText(Define.AvoidFailText);
                        GetText((int)Texts.Avoid).gameObject.SetActive(true);


                        
                        // GetImage((int)Images.FailAvoid).gameObject.SetActive(true); // 실패 이미지 추가
                    }

                Invoke("HideResultAndProceed", 2f);
            });
        }
        else
        {
            DamageCalculate();
            playerScene.StaticPlayerEx("Wrong"); 
            Invoke("HideResultAndProceed", 2f);
        }
        

        GetButton((int)Buttons.ConfirmButton).gameObject.SetActive(false);
        GetImage((int)Images.Wrong).gameObject.SetActive(true);

    }
}
    void DamageCalculate()
    {
        int damage = IncorrectCount * Define.Damage;
        int totalDamage = Mathf.Max(damage - Managers.Game.Defence, 0);  // 방어력을 고려한 데미지 계산
        Managers.Game.Hp -= totalDamage; // 데미지 적용
        Debug.Log($"HP 감소: {totalDamage}, 남은 HP: {Managers.Game.Hp}");
    }

    void HideResultAndProceed()
    {
        playerScene.HPUp();
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
    //confirm 버튼 클릭시 모든 버튼 비활성화
    void CloseAllButton()
    {
        GameObject Button= GameObject.Find("Button");
        GameObject ConfirmButton= GameObject.Find("ConfirmButton");
        GameObject TheWorldButton= GameObject.Find("TheWorldButton");
        if(Button != null)
            Button.gameObject.SetActive(false);
        if(ConfirmButton != null)
            ConfirmButton.gameObject.SetActive(false);
        if(TheWorldButton != null)
            TheWorldButton.gameObject.SetActive(false);
    }

    //게임오버일 경우
    void GameOver()
    {
        Managers.Game.SaveGame();
        Managers.Sound.Stop(Sound.Bgm);
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_GameOverPopup>();
        
    }
    //게임오버가 아닌 경우 
    void GameContinue()
    {
        Managers.Game.SaveGame();
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
        // RectTransform parentRect = GetComponent<RectTransform>(); // 부모 컨테이너의 RectTransform 가져오기

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

    //힌트 한개 제거까지 
    public void UseHintKey(string hintKeyName)
    {
        Managers.Game.HintKey = Managers.Game.HintKey - 1;
        playerScene.RefreshUI();

        switch (hintKeyName)
        {
            case "HairHint":
                customManager.hair = Managers.Game.StrangerIndex[0];
                customManager.hairM.changeItem(Managers.Game.StrangerIndex[0]);
                GetButton((int)Buttons.HairHint).gameObject.SetActive(false);
                GetButton((int)Buttons.HairMinus).gameObject.SetActive(false);
                GetButton((int)Buttons.HairPlus).gameObject.SetActive(false);
                CheckHintZero();
                break;

            case "ClothesHint":
                customManager.clothes = Managers.Game.StrangerIndex[1];
                customManager.clothesM.changeItem(Managers.Game.StrangerIndex[1]);
                GetButton((int)Buttons.ClothesHint).gameObject.SetActive(false);
                GetButton((int)Buttons.ClothesMinus).gameObject.SetActive(false);
                GetButton((int)Buttons.ClothesPlus).gameObject.SetActive(false);
                CheckHintZero();
                break;
            case "EyebrowHint":
                customManager.eyebrow= Managers.Game.StrangerIndex[2];
                customManager.eyebrowM.changeItem(Managers.Game.StrangerIndex[2]);
                GetButton((int)Buttons.EyebrowHint).gameObject.SetActive(false);
                GetButton((int)Buttons.EyebrowMinus).gameObject.SetActive(false);
                GetButton((int)Buttons.EyebrowPlus).gameObject.SetActive(false);
                CheckHintZero();
                break;
            case "EyeHint":
                customManager.eye = Managers.Game.StrangerIndex[3];
                customManager.eyeM.changeItem(Managers.Game.StrangerIndex[3]);
                GetButton((int)Buttons.EyeHint).gameObject.SetActive(false);
                GetButton((int)Buttons.EyeMinus).gameObject.SetActive(false);
                GetButton((int)Buttons.EyePlus).gameObject.SetActive(false);
                CheckHintZero();
                break;
            case "MouthHint":
                customManager.mouth = Managers.Game.StrangerIndex[4];
                customManager.mouthM.changeItem(Managers.Game.StrangerIndex[4]); 
                GetButton((int)Buttons.MouthHint).gameObject.SetActive(false);
                GetButton((int)Buttons.MouthMinus).gameObject.SetActive(false);
                GetButton((int)Buttons.MouthPlus).gameObject.SetActive(false);
                CheckHintZero();
                break;
            case "EmotionHint":
                customManager.emotion = Managers.Game.StrangerIndex[5];
                customManager.emotionM.changeItem(Managers.Game.StrangerIndex[5]); 
                GetButton((int)Buttons.EmotionHint).gameObject.SetActive(false);
                GetButton((int)Buttons.EmotionMinus).gameObject.SetActive(false);
                GetButton((int)Buttons.EmotionPlus).gameObject.SetActive(false);
                CheckHintZero();
                break;
            case "AnimationHint":
                animationManager.ani = Managers.Game.StrangerIndex[6];
                // animationManager.animator.Play(Managers.Game.StrangerIndex[6]);
                GetButton((int)Buttons.AnimationHint).gameObject.SetActive(false);
                GetButton((int)Buttons.AnimationMinus).gameObject.SetActive(false);
                GetButton((int)Buttons.AnimationPlus).gameObject.SetActive(false);
                CheckHintZero();
                break;

            default:
                Debug.LogWarning($"Unknown hint key: {hintKeyName}");
                break;
        }
    }

    //힌트가 0개면 힌트버튼 없애기
    public void CheckHintZero()
    {
        if (Managers.Game.HintKey <= 0)
        {
            Buttons[] hintButtons = 
            {
                Buttons.HairHint,
                Buttons.ClothesHint,
                Buttons.EyebrowHint,
                Buttons.EyeHint,
                Buttons.MouthHint,
                Buttons.EmotionHint,
                Buttons.AnimationHint
            };

            foreach (var button in hintButtons)
            {
                GetButton((int)button).gameObject.SetActive(false);
            }
        }
    }

    //리마인드 스킬
    public void OnClickTheWorld()
    {
        _isMoving = true;
        Managers.Game.TheWorld--;
        GetText((int)Texts.TheWorldText).text = $"{Managers.Game.TheWorld}";
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            Stranger.transform.position = new Vector3(-7, 2.2f, 0); // 특정 위치로 이동
            _isMoving = false; // 위치 설정 후 바로 이동 멈춤
            StartCoroutine(HideStrangerAfterDelay(2f)); // 2초 후 사라지게 하기
        }
    }

    private IEnumerator HideStrangerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간 동안 대기
        Stranger.transform.position = new Vector3(-12, 2.2f, 0);
    }

}
