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
    public bool isAvoid;
    public string WrongRegion;
    public bool CheckSomethingWrong;
    public int totalDamage;

    private float lastSoundTime = 0f; 

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
        WhenWrongText,
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
        BG,
        Correct,
        Wrong,
        RemindImage,
        WhenWrongImage,
    }
    
    GameObject GuessPlayer;
    GameObject _customManager;
    CustomManager customManager;
    AnimationManager animationManager;


    public override bool Init()
    {
        if (base.Init() == false)
			return false;

		BindText(typeof(Texts));
		BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        //배경 변경
        // Managers.Game.changeBG();
        GetImage((int)Images.BG).sprite = Managers.Resource.Load<Sprite>($"Sprites/Background/{Managers.Game.BG}");
        
            

        Stranger = GameObject.Find("Stranger");
        playerScene = Managers.UI.GetSceneUI<UI_PlayerScene>();

        DeleteBirds();
        
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

        // Text에 설정
        GetText((int)Texts.Timer).text = $"{Managers.Game.GuessTimer}";
        GetText((int)Texts.TheWorldText).text = Managers.GetText(Define.RemindButtonText);
        GetText((int)Texts.TheWorldText).text += $" *{Managers.Game.TheWorld}";
        GetButton((int)Buttons.TheWorldButton).gameObject.SetActive(Managers.Game.TheWorld > 0);
        GetText((int)Texts.AvoidButtonText).text = Managers.GetText(Define.AvoidButtonText);
        GetButton((int)Buttons.AvoidButton).gameObject.SetActive(false);
        GetText((int)Texts.Avoid).gameObject.SetActive(false);
        GetImage((int)Images.RemindImage).gameObject.SetActive(false);

        GetImage((int)Images.WhenWrongImage).gameObject.SetActive(false);
        GetText((int)Texts.WhenWrongText).gameObject.SetActive(false);

        GetText((int)Texts.ConfirmButtonText).text = Managers.GetText(Define.ConfirmButtonText);


        //초반 Guess 어딘지 확인
        if(Managers.Game.DifficultyLevel == "Normal")
        {
            GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {Define.NormalIndex}"; 
            GetText((int)Texts.ClothesText).text = $"Clothes {customManager.clothes+1} / {Define.NormalIndex}"; 
            GetText((int)Texts.EyebrowText).text = $"Eyebrow {customManager.eyebrow+1} / {Define.NormalIndex}"; 
            GetText((int)Texts.EyeText).text = $"Eye {customManager.eye+1} / {Define.NormalIndex}"; 
            GetText((int)Texts.MouthText).text = $"Mouth {customManager.mouth+1} / {Define.NormalIndex}"; 
            GetText((int)Texts.EmotionText).text = $"Emotion {customManager.emotion+1} / {Define.NormalIndex}"; 
            GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {Define.NormalIndex}"; 
            GetText((int)Texts.AnimationText).text = $"Animation {animationManager.ani+1} / 5"; 
        }
        if(Managers.Game.DifficultyLevel == "Hard")
        {
            GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {Define.HardIndex}"; 
            GetText((int)Texts.ClothesText).text = $"Clothes {customManager.clothes+1} / {Define.HardIndex}"; 
            GetText((int)Texts.EyebrowText).text = $"Eyebrow {customManager.eyebrow+1} / {Define.MaxIndexEyebrow}"; 
            GetText((int)Texts.EyeText).text = $"Eye {customManager.eye+1} / {Define.HardIndex}"; 
            GetText((int)Texts.MouthText).text = $"Mouth {customManager.mouth+1} / {Define.HardIndex}"; 
            GetText((int)Texts.EmotionText).text = $"Emotion {customManager.emotion+1} / {Define.MaxIndexEmotion}"; 
            GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {Define.HardIndex}"; 
            GetText((int)Texts.AnimationText).text = $"Animation {animationManager.ani+1} / 10"; 
        }
        if(Managers.Game.DifficultyLevel == "UnLimted")
        {
            GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {customManager.hairM.count.Length}"; 
            GetText((int)Texts.ClothesText).text = $"Clothes {customManager.clothes+1} / {customManager.clothesM.count.Length}"; 
            GetText((int)Texts.EyebrowText).text = $"Eyebrow {customManager.eyebrow+1} / {customManager.eyebrowM.count.Length}"; 
            GetText((int)Texts.EyeText).text = $"Eye {customManager.eye+1} / {customManager.eyeM.count.Length}"; 
            GetText((int)Texts.MouthText).text = $"Mouth {customManager.mouth+1} / {customManager.mouthM.count.Length}"; 
            GetText((int)Texts.EmotionText).text = $"Emotion {customManager.emotion+1} / {customManager.emotionM.count.Length}"; 
            GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {customManager.hairM.count.Length}"; 
            GetText((int)Texts.AnimationText).text = $"Animation {animationManager.ani+1} / 11"; 
        }
        #region 질문 텍스트

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
        string[] hintKeys = { "HairHint", "ClothesHint", "EyebrowHint", "EyeHint", "MouthHint", "EmotionHint", "AnimationHint" };
        foreach (var hintKey in hintKeys)
        {
            Buttons button = (Buttons)System.Enum.Parse(typeof(Buttons), hintKey);
            GetButton((int)button).gameObject.BindEvent(() =>
            {
                Managers.Sound.Play(Sound.Effect, "Sound_Hint");
                UseHintKey(hintKey);

            });
        }
        #endregion

        
        //추측하는 버튼 바인딩
        #region 추측플레이어 바인딩
        if(Managers.Game.DifficultyLevel == "Normal")
            NormalBinding();
        else if(Managers.Game.DifficultyLevel == "Hard")
            HardBinding();
        else if (Managers.Game.DifficultyLevel == "UnLimited")
            UnLimitedBinding();
        //인덱스는 언리미티드와 같음
        else if (Managers.Game.DifficultyLevel == "Nightmare")
            UnLimitedBinding();
        
        #endregion



        return true;

    }

    void Update()
    {
        if(!IsButtonClick)
            {
            //시간이 다 지나면 자동으로 false
            RemainTime -= Time.deltaTime;
            if (RemainTime < 0)
                RemainTime = 0;
                
            GetText((int)Texts.Timer).text = $"{(int)RemainTime}";
            
            //3초 이내는 빨간색
            if (RemainTime <= 4 )
            {
                GetText((int)Texts.Timer).color = Color.red;
                // 1초마다 한 번만 사운드 재생
                if (Time.time - lastSoundTime >= 1f && RemainTime > 1)
                {
                    Managers.Sound.Play(Sound.Effect, "Sound_RemainTime");
                    lastSoundTime = Time.time;
                }
                // Managers.Sound.Play(Sound.Effect, "Sound_RemainTime");
                // Managers.Sound.Stop(Sound.Effect);
            }
            else
            {
                GetText((int)Texts.Timer).color = Color.black;  
            }

            if (RemainTime <= 0)
            {
                GetButton((int)Buttons.ConfirmButton).gameObject.SetActive(false);
                GetImage((int)Images.Wrong).gameObject.SetActive(false);
                
                OnClickConfirmButton(isCorrect);
            }
        }
    }

    //stranger와 추측 캐릭터 비교
    string CompareCharacter(string WrongRegion)
    {
        int[] indicesToCompare = { 0, 1, 2, 3, 4, 5, 6 };
        object[] propertiesToCompare = {
            customManager.hair,
            customManager.clothes,
            customManager.eyebrow,
            customManager.eye,
            customManager.mouth,
            customManager.emotion,
            animationManager.ani
        };

        for (int i = 0; i < indicesToCompare.Length; i++)
        {
            if (!propertiesToCompare[i].Equals(Managers.Game.StrangerIndex[indicesToCompare[i]]))
            {
                WrongRegion += "1";
                IncorrectCount++;
            }
            else
            {
                WrongRegion += "0";
            }
        }

        Debug.Log($"틀린 개수: {IncorrectCount}");
        return WrongRegion;
    }




    void OnClickConfirmButton(bool isCorrect)
    {
        CloseAllButton();

        IsButtonClick = true;
        Managers.Sound.Play(Sound.Effect, "Sound_CheckButton");
        string _wrong = "";
        //캐릭 비교
        WrongRegion = CompareCharacter(_wrong);

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
                bool isAvoidClick = false;

                playerScene.StaticPlayerEx("AvoidPlease"); 

                GetButton((int)Buttons.AvoidButton).gameObject.SetActive(true);
                GetButton((int)Buttons.TheWorldButton).gameObject.SetActive(false);

                GetButton((int)Buttons.AvoidButton).gameObject.BindEvent(() =>
                {
                    isAvoidClick = true;
                    GetButton((int)Buttons.AvoidButton).gameObject.SetActive(false);

                    if (Random.Range(1, 3) == 1)
                    {
                        Debug.Log("회피 성공!");
                        isAvoid = true;
                        playerScene.StaticPlayerEx("AvoidSucess"); 
                        // 회피 성공 처리 (TODO 회피 애니메이션 등)
                        Managers.Sound.Play(Sound.Effect, "Sound_Avoid"); 
                        if(isAvoidClick)
                            GetButton((int)Buttons.AvoidButton).gameObject.SetActive(false);

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
        totalDamage = Mathf.Max(damage - Managers.Game.Defence, 0);  // 방어력을 고려한 데미지 계산
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
        else if(Managers.Game.Stage >= Define.NormalGameEnd -1 && Managers.Game.DifficultyLevel == "Normal")
        {
            Managers.Game.Stage++;
            GameEnd();
        }
        else if(Managers.Game.Stage >= Define.HardGameEnd -1 && Managers.Game.DifficultyLevel == "Hard")
        {
            Managers.Game.Stage++;
            GameEnd();
        }
        else if(Managers.Game.Stage >= Define.UnLimitedGameEnd -1 && Managers.Game.DifficultyLevel == "UnLimited")
        {
            Managers.Game.Stage++;
            GameEnd();
        }
        else if(Managers.Game.Stage >= Define.NightmareGameEnd && Managers.Game.DifficultyLevel == "Nightmare")
        {
            Managers.Game.Stage++;
            GameEnd();
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
        Managers.Game.Hp =0;
        Managers.Game.SaveGame();
        Managers.Game.CharacterDelete();
        Managers.Game.StaticCharacterDelete();
        Managers.Sound.Stop(Sound.Bgm);
        Managers.UI.ClosePopupUI(this);
        UI_GameOverPopup gameOverPopup = Managers.UI.ShowPopupUI<UI_GameOverPopup>();
        gameOverPopup.transform.SetParent(null);

        Managers.UI.ClosePlayerSceneUI();

    }
    //게임 스테이지 모두 클리어일 경우
    public void GameEnd()
    {
        Managers.Game.Hp =0;
        Managers.Game.CharacterDelete();
        Managers.Game.StaticCharacterDelete();
        Managers.Game.SaveGame();
        Managers.Sound.Stop(Sound.Bgm);

        if(Managers.Game.DifficultyLevel == "Normal")
        {
            Managers.Game.Unlocked[0] = CollectionState.Done;
            Managers.Game.Money += 1000;
        }
        else if( Managers.Game.DifficultyLevel == "Hard")
        {
            Managers.Game.Unlocked[1] = CollectionState.Done;
            Managers.Game.Money += 3000;
        }
        else if( Managers.Game.DifficultyLevel == "UnLimited")
        {
            Managers.Game.Unlocked[2] = CollectionState.Done;
            Managers.Game.Money += 5000;
        }
        else if( Managers.Game.DifficultyLevel == "Nightmare")
        {
            Managers.Game.Money += 10000;
        }

        if(Managers.Game.Money > 1000000)
        {
            Managers.Game.Money = 999999;
        } 

        Managers.UI.ClosePopupUI(this);
        UI_GameEndPopup gameEndPopup = Managers.UI.ShowPopupUI<UI_GameEndPopup>();
        gameEndPopup.transform.SetParent(null);

        Managers.UI.ClosePlayerSceneUI();
        
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
        // GetText((int)Texts.Question).text = $"{randomQuiz.kor}";
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
                animationManager.ani = Managers.Game.StrangerIndex[6] -1;
                animationManager.PlayAni(true);
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
        playerScene.StaticPlayerEx("Reminding");
        GetImage((int)Images.RemindImage).gameObject.SetActive(true);
        // GetText((int)Texts.TheWorldText).text = $"{Managers.Game.TheWorld}";
    }

    //5초 이내 틀리는거 방지 힌트용
    void CompareForHint()
    {
        int[] indicesToCompare = { 0, 1, 2, 3, 4, 5, 6 };
        object[] propertiesToCompare = {
            customManager.hair,
            customManager.clothes,
            customManager.eyebrow,
            customManager.eye,
            customManager.mouth,
            customManager.emotion,
            animationManager.ani ,
        };

         CheckSomethingWrong = false;

        for (int i = 0; i < indicesToCompare.Length; i++)
        {
            if (!propertiesToCompare[i].Equals(Managers.Game.StrangerIndex[indicesToCompare[i]]))
        {
            CheckSomethingWrong = true; // 잘못된 점 발견 시 true 설정
            break; // 하나라도 발견되면 반복 종료
        }
        }

        
    }
    private void FixedUpdate()
    {

        GiveHint();

        if (_isMoving)
        {
            Stranger.transform.position = new Vector3(-7.5f, 1.5f, 0); // 특정 위치로 이동
            _isMoving = false; // 위치 설정 후 바로 이동 멈춤
            StartCoroutine(HideStrangerAfterDelay(2f)); // 2초 후 사라지게 하기
        }
    }


    private IEnumerator HideStrangerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간 동안 대기
        Stranger.transform.position = new Vector3(-15, 2.2f, 0);
        GetImage((int)Images.RemindImage).gameObject.SetActive(false);
        playerScene.StaticPlayerEx("Initial");
    }
    
    void NormalBinding()
    {
    GetButton((int)Buttons.HairMinus).gameObject.BindEvent(() => 
        {
            if(customManager.hair == 0)
            {
                customManager.hair = Define.NormalIndex -1;
            }
            else if ( 0 < customManager.hair )
                {
                    customManager.hair--; 
                }
            
            customManager.numberCheck(0);  
            GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {Define.NormalIndex}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        
        });
        GetButton((int)Buttons.HairPlus).gameObject.BindEvent(() => 
        {
            if(Define.NormalIndex  <= customManager.hair+1)
            {
                customManager.hair = 0;
            }
            else if (Define.NormalIndex  > customManager.hair)
                {
                    customManager.hair++; 
                }
            customManager.numberCheck(0);  
            GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {Define.NormalIndex }";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.ClothesMinus).gameObject.BindEvent(() => 
        {
            if(customManager.clothes == 0)
            {
                customManager.clothes = Define.NormalIndex  -1;
            }
            else if ( 0 < customManager.clothes )
                {
                    customManager.clothes--; 
                }
            customManager.numberCheck(1);  
            GetText((int)Texts.ClothesText).text = $"Clothes {customManager.clothes+1} / {Define.NormalIndex }";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.ClothesPlus).gameObject.BindEvent(() => 
        {
            if(Define.NormalIndex  <= customManager.clothes+1)
            {
                customManager.clothes = 0;
            }
            else if (Define.NormalIndex  > customManager.clothes)
                {
                    customManager.clothes++; 
                }
            GetText((int)Texts.ClothesText).text = $"Clothes {customManager.clothes+1} / {Define.NormalIndex }";
            customManager.numberCheck(1);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.EyebrowMinus).gameObject.BindEvent(() => 
        {
            if(customManager.eyebrow == 0)
            {
                customManager.eyebrow = Define.NormalIndex  -1;
            }
            else if ( 0 < customManager.eyebrow )
                {
                    customManager.eyebrow--; 
                }
                
            customManager.numberCheck(2); 
            GetText((int)Texts.EyebrowText).text = $"Eyebrow {customManager.eyebrow+1} / {Define.NormalIndex }";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton"); 
        });
        GetButton((int)Buttons.EyebrowPlus).gameObject.BindEvent(() => 
        {
            if(Define.NormalIndex  <= customManager.eyebrow+1)
            {
                customManager.eyebrow = 0;
            }
            else if (Define.NormalIndex  > customManager.eyebrow)
                {
                    customManager.eyebrow++; 
                }
            customManager.numberCheck(2);  
            GetText((int)Texts.EyebrowText).text = $"Eyebrow {customManager.eyebrow+1} / {Define.NormalIndex }";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.EyeMinus).gameObject.BindEvent(() => 
        {
            if(customManager.eye == 0)
            {
                customManager.eye = Define.NormalIndex  -1;
            }
            else if ( 0 < customManager.eye )
                {
                    customManager.eye--; 
                }
            customManager.numberCheck(3);  
            GetText((int)Texts.EyeText).text = $"Eye {customManager.eye+1} / {Define.NormalIndex }";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.EyePlus).gameObject.BindEvent(() => 
        {
            if(Define.NormalIndex  <= customManager.eye+1)
            {
                customManager.eye = 0;
            }
            else if (Define.NormalIndex  > customManager.eye)
                {
                    customManager.eye++; 
                }

            customManager.numberCheck(3);  
            GetText((int)Texts.EyeText).text = $"Eye {customManager.eye+1} / {Define.NormalIndex }";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.MouthMinus).gameObject.BindEvent(() => 
        {
            if(customManager.mouth == 0)
            {
                customManager.mouth = Define.NormalIndex  -1;
            }
            else if ( 0 < customManager.mouth )
                {
                    customManager.mouth--; 
                }
            customManager.numberCheck(4);  
            GetText((int)Texts.MouthText).text = $"Mouth {customManager.mouth+1} / {Define.NormalIndex }";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.MouthPlus).gameObject.BindEvent(() => 
        {
            if(Define.NormalIndex  <= customManager.mouth+1)
            {
                customManager.mouth = 0;
            }
            else if (Define.NormalIndex  > customManager.mouth)
                {
                    customManager.mouth++; 
                }
            customManager.numberCheck(4); 
            GetText((int)Texts.MouthText).text = $"Mouth {customManager.mouth+1} / {Define.NormalIndex }";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton"); 
        });

        GetButton((int)Buttons.EmotionMinus).gameObject.BindEvent(() => 
        {
            if(customManager.emotion == 0)
            {
                customManager.emotion = Define.NormalIndex  -1;
            }
            else if ( 0 < customManager.emotion )
                {
                    customManager.emotion--; 
                }

            customManager.numberCheck(5); 
            GetText((int)Texts.EmotionText).text = $"Emotion {customManager.emotion+1} / {Define.NormalIndex }"; 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.EmotionPlus).gameObject.BindEvent(() => 
        {
            if(Define.NormalIndex  <= customManager.emotion+1)
            {
                customManager.emotion = 0;
            }
            else if (Define.NormalIndex  > customManager.emotion)
                {
                    customManager.emotion++; 
                }
            customManager.numberCheck(5);  
            GetText((int)Texts.EmotionText).text = $"Emotion {customManager.emotion+1} / {Define.NormalIndex }"; 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.AnimationMinus).gameObject.BindEvent(() => 
        {
            if(animationManager.ani == 0)
            {
                animationManager.ani = 4;
                animationManager.PlayAni(false);
            }
            else
                animationManager.PlayAni(false);

            GetText((int)Texts.AnimationText).text = $"Animation {animationManager.ani+1} / 5"; 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.AnimationPlus).gameObject.BindEvent(() => 
        {
            if(animationManager.ani == 4)
            {
                animationManager.ani = -1;
                animationManager.PlayAni(true);
            }
            else
                animationManager.PlayAni(true);

            GetText((int)Texts.AnimationText).text = $"Animation {animationManager.ani+1} / 5"; 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        
        
    }


    void HardBinding()
    {
        GetButton((int)Buttons.HairMinus).gameObject.BindEvent(() => 
        {
            if(customManager.hair == 0)
            {
                customManager.hair = Define.HardIndex -1;
            }
            else if ( 0 < customManager.hair )
                {
                    customManager.hair--; 
                }
            
            customManager.numberCheck(0);  
            GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {Define.HardIndex}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        
        });
        GetButton((int)Buttons.HairPlus).gameObject.BindEvent(() => 
        {
            if(Define.HardIndex <= customManager.hair+1)
            {
                customManager.hair = 0;
            }
            else if (Define.HardIndex > customManager.hair)
                {
                    customManager.hair++; 
                }
            customManager.numberCheck(0);  
            GetText((int)Texts.HairText).text = $"Hair {customManager.hair+1} / {Define.HardIndex}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.ClothesMinus).gameObject.BindEvent(() => 
        {
            if(customManager.clothes == 0)
            {
                customManager.clothes = Define.HardIndex -1;
            }
            else if ( 0 < customManager.clothes )
                {
                    customManager.clothes--; 
                }
            customManager.numberCheck(1);  
            GetText((int)Texts.ClothesText).text = $"Clothes {customManager.clothes+1} / {Define.HardIndex}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.ClothesPlus).gameObject.BindEvent(() => 
        {
            if(Define.HardIndex <= customManager.clothes+1)
            {
                customManager.clothes = 0;
            }
            else if (customManager.clothesM.count.Length > customManager.clothes)
                {
                    customManager.clothes++; 
                }
            GetText((int)Texts.ClothesText).text = $"Clothes {customManager.clothes+1} / {Define.HardIndex}";
            customManager.numberCheck(1);  
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        //Define.HardIndex - 5
        GetButton((int)Buttons.EyebrowMinus).gameObject.BindEvent(() => 
        {
            if(customManager.eyebrow == 0)
            {
                customManager.eyebrow = (Define.MaxIndexEyebrow) -1;
            }
            else if ( 0 < customManager.eyebrow )
                {
                    customManager.eyebrow--; 
                }
                
            customManager.numberCheck(2); 
            GetText((int)Texts.EyebrowText).text = $"Eyebrow {customManager.eyebrow+1} / {Define.MaxIndexEyebrow}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton"); 
        });
        GetButton((int)Buttons.EyebrowPlus).gameObject.BindEvent(() => 
        {
            if(Define.HardIndex - 5 <= customManager.eyebrow+1)
            {
                customManager.eyebrow = 0;
            }
            else if (Define.HardIndex - 5 > customManager.eyebrow)
                {
                    customManager.eyebrow++; 
                }
            customManager.numberCheck(2);  
            GetText((int)Texts.EyebrowText).text = $"Eyebrow {customManager.eyebrow+1} / {Define.MaxIndexEyebrow}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.EyeMinus).gameObject.BindEvent(() => 
        {
            if(customManager.eye == 0)
            {
                customManager.eye = Define.HardIndex -1;
            }
            else if ( 0 < customManager.eye )
                {
                    customManager.eye--; 
                }
            customManager.numberCheck(3);  
            GetText((int)Texts.EyeText).text = $"Eye {customManager.eye+1} / {Define.HardIndex}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.EyePlus).gameObject.BindEvent(() => 
        {
            if(Define.HardIndex <= customManager.eye+1)
            {
                customManager.eye = 0;
            }
            else if (Define.HardIndex > customManager.eye)
                {
                    customManager.eye++; 
                }

            customManager.numberCheck(3);  
            GetText((int)Texts.EyeText).text = $"Eye {customManager.eye+1} / {Define.HardIndex}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.MouthMinus).gameObject.BindEvent(() => 
        {
            if(customManager.mouth == 0)
            {
                customManager.mouth = Define.HardIndex -1;
            }
            else if ( 0 < customManager.mouth )
                {
                    customManager.mouth--; 
                }
            customManager.numberCheck(4);  
            GetText((int)Texts.MouthText).text = $"Mouth {customManager.mouth+1} / {Define.HardIndex}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.MouthPlus).gameObject.BindEvent(() => 
        {
            if(Define.HardIndex <= customManager.mouth+1)
            {
                customManager.mouth = 0;
            }
            else if (Define.HardIndex > customManager.mouth)
                {
                    customManager.mouth++; 
                }
            customManager.numberCheck(4); 
            GetText((int)Texts.MouthText).text = $"Mouth {customManager.mouth+1} / {Define.HardIndex}";
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton"); 
        });

        GetButton((int)Buttons.EmotionMinus).gameObject.BindEvent(() => 
        {
            if(customManager.emotion == 0)
            {
                customManager.emotion = Define.MaxIndexEmotion -1;
            }
            else if ( 0 < customManager.emotion )
                {
                    customManager.emotion--; 
                }

            customManager.numberCheck(5); 
            GetText((int)Texts.EmotionText).text = $"Emotion {customManager.emotion+1} / {Define.MaxIndexEmotion}"; 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.EmotionPlus).gameObject.BindEvent(() => 
        {
            if(Define.MaxIndexEmotion <= customManager.emotion+1)
            {
                customManager.emotion = 0;
            }
            else if (Define.MaxIndexEmotion > customManager.emotion)
                {
                    customManager.emotion++; 
                }
            customManager.numberCheck(5);  
            GetText((int)Texts.EmotionText).text = $"Emotion {customManager.emotion+1} / {Define.MaxIndexEmotion}"; 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });

        GetButton((int)Buttons.AnimationMinus).gameObject.BindEvent(() => 
        {
            if(animationManager.ani == 0)
            {
                animationManager.ani = 10;
                animationManager.PlayAni(false);
            }
            else
                animationManager.PlayAni(false);

            GetText((int)Texts.AnimationText).text = $"Animation {animationManager.ani+1} / 10"; 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
        GetButton((int)Buttons.AnimationPlus).gameObject.BindEvent(() => 
        {
            if(animationManager.ani == 9)
            {
                animationManager.ani = -1;
                animationManager.PlayAni(true);
            }
            else
                animationManager.PlayAni(true);

            GetText((int)Texts.AnimationText).text = $"Animation {animationManager.ani+1} / 10"; 
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });
    }

    void UnLimitedBinding()
    {
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
    }

    void DeleteBirds()
    {
        // 배열에 찾으려는 오브젝트 이름들을 저장
        string[] birdNames = { "Bird1", "Bird2", "Bird3" };

        foreach (string birdName in birdNames)
        {
            // GameObject.Find로 해당 이름의 오브젝트를 검색
            GameObject bird = GameObject.Find(birdName);
            
            // 검색된 오브젝트가 있다면 삭제
            if (bird != null)
            {
                Destroy(bird);
            }
        }
    }



    public void GiveHint()
    {
        
        if(Managers.Game.DifficultyLevel != "Nightmare")
        {
            int chanceTime = 0;
            if(Managers.Game.DifficultyLevel == "Unlimited")
                chanceTime = Define.GiveHintUnLimted;
            if(Managers.Game.DifficultyLevel == "Hard")
                chanceTime = Define.GiveHintHard;
            else if(Managers.Game.DifficultyLevel == "Normal")
                chanceTime = Define.GiveHintNormal;

            if(RemainTime > chanceTime)
                CompareForHint();

            if(CheckSomethingWrong && RemainTime < chanceTime  && RemainTime > chanceTime -1)
            {
                GetImage((int)Images.WhenWrongImage).gameObject.SetActive(true);
                GetText((int)Texts.WhenWrongText).text = Managers.GetText(Define.WhenWrongText);
                GetText((int)Texts.WhenWrongText).gameObject.SetActive(true);
                playerScene.StaticPlayerEx("SomethingWrong");
            }
            else if( RemainTime < Define.GiveHintNormal -1 && RemainTime > 1)
            {
                GetImage((int)Images.WhenWrongImage).gameObject.SetActive(false);
                GetText((int)Texts.WhenWrongText).gameObject.SetActive(false);
                // playerScene.StaticPlayerEx("Initial");
            }
        }

    }

}
