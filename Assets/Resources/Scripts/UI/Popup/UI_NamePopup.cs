using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using System.Linq;
using JetBrains.Annotations;
using System.Collections.ObjectModel;
using Unity.VisualScripting;


public class UI_NamePopup : UI_Popup
{
    //플레이어 모습 저장 배열
    public int[] playerIndex = new int[6] {0,0,0,0,0,0};
    
    //스테틱플레이어 매니저
    GameObject Player;
    CustomManager customManager;
    AnimationManager animationManager;

    [SerializeField] Toggle Normal;
    [SerializeField] Toggle Hard;
    [SerializeField] Toggle UnLimited;
    [SerializeField] Toggle Nightmare;
    enum GameObjects
	{
		InputField
	}

    enum Texts
	{
		ConfirmButtonText,
		HintText,
		ValueText,
        ClothesText,
        ClothesEffectText,
        DifficultyExplainText,
        WarningInput,
        GotoTitleText,
        UnlockText,
	}

	enum Buttons
	{
		ConfirmButton,
        ClothesMinus,
        ClothesPlus,
        GotoTitleButton,
        HardLock,
        UnlimtedLock,
        NightmareLock,
	}



    TMP_InputField _inputField;
    [SerializeField] ToggleGroup toggleGroup; 


    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        Player = Managers.Resource.Instantiate("StaticPlayer");
        GameObject player = GameObject.FindGameObjectWithTag("StaticManager");
        customManager = player.GetComponent<CustomManager>();
        animationManager = player.GetComponent<AnimationManager>();

        Player.transform.position = Vector3.zero;
        Player.transform.localScale = new Vector3(1, 1, 1);
        
        BindObject(typeof(GameObjects));
		BindText(typeof(Texts));
		BindButton(typeof(Buttons));
        
        GetText((int)Texts.ConfirmButtonText).text = Managers.GetText(Define.NicknameConfirm);
		GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        GetButton((int)Buttons.GotoTitleButton).gameObject.BindEvent(GotoTitle);
        GetText((int)Texts.WarningInput).text = Managers.GetText(Define.WarningInputText);
        GetText((int)Texts.WarningInput).gameObject.SetActive(false);
        GetText((int)Texts.GotoTitleText).text = Managers.GetText(Define.GoToTitleText);

        GetButton((int)Buttons.HardLock).gameObject.BindEvent(() =>
        {
            GetText((int)Texts.UnlockText).text = Managers.GetText(Define.UnlockHardText);
            GetText((int)Texts.UnlockText).gameObject.SetActive(true);
        });
        GetButton((int)Buttons.UnlimtedLock).gameObject.BindEvent(() =>
        {
            GetText((int)Texts.UnlockText).text = Managers.GetText(Define.UnlockUnlimitedText);
            GetText((int)Texts.UnlockText).gameObject.SetActive(true);
        });

        Normal.onValueChanged.AddListener((isOn) =>
        {
            if(isOn)
            {
                GetText((int)Texts.UnlockText).gameObject.SetActive(false);
            }
        });
        
        GetButton((int)Buttons.HardLock).gameObject.SetActive(false);
        GetButton((int)Buttons.UnlimtedLock).gameObject.SetActive(false);
        GetButton((int)Buttons.NightmareLock).gameObject.SetActive(false);
        GetText((int)Texts.UnlockText).gameObject.SetActive(false);


        if (Managers.Game.Unlocked[0] == CollectionState.None) // Hard 잠금 상태
        {
            GetButton((int)Buttons.HardLock).gameObject.SetActive(true);
            Hard.interactable = false;
        }

        if (Managers.Game.Unlocked[1] == CollectionState.None) //언리미티드 잠금 상태
        {
             GetButton((int)Buttons.UnlimtedLock).gameObject.SetActive(true);
             UnLimited.interactable = false;
        }
        if (Managers.Game.Unlocked[2] == CollectionState.None) //언리미티드 잠금 상태
        {
             GetButton((int)Buttons.NightmareLock).gameObject.SetActive(true);
             Nightmare.interactable = false;
        }


        
        #region 코디 (현재는 옷만)

        RefreshClothesText();

        //가지고 있는 옷만 필터링해서 버튼 클릭시 이동
        int maxClothesIndex = customManager.clothesM.count.Length - 1;

        GetButton((int)Buttons.ClothesMinus).gameObject.BindEvent(() =>
        {
            // 현재 customManager.clothes - 1이 Done 상태라면 바로 이동
            if (customManager.clothes > 0 && Managers.Game.Collections[customManager.clothes - 1] == CollectionState.Done)
            {

                customManager.clothes--;  
            }
            else
            {
                // Done 상태의 가장 가까운 항목으로 이동
                for (int i = customManager.clothes - 1; i >= 0; i--)
                {
                    if (Managers.Game.Collections[i] == CollectionState.Done)
                    {
                        customManager.clothes = i;  
                        break;
                    }
                }
            }

            customManager.numberCheck(1);  
            RefreshClothesText();
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });


        GetButton((int)Buttons.ClothesPlus).gameObject.BindEvent(() =>
        {
            // 배열 범위 확인 추가
            if (customManager.clothes + 1 < customManager.clothesM.count.Length &&
                Managers.Game.Collections[customManager.clothes + 1] == CollectionState.Done)
            {
                customManager.clothes++;  // Done 상태의 다음 항목으로 이동
            }
            else
            {
            for (int i = customManager.clothes + 1; i < customManager.clothesM.count.Length; i++)
            {
                if (i >= Managers.Game.Collections.Length) // 배열 길이 검사
                    break;

                if (Managers.Game.Collections[i] == CollectionState.Done)
                {
                    customManager.clothes = i;  // Done 상태의 항목으로 이동
                    break;
                }
            }
            }

            customManager.numberCheck(1);
            RefreshClothesText();
            Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        });



        #endregion
        
        #region 토글 설정
        Normal.group = toggleGroup;
        Hard.group = toggleGroup;
        UnLimited.group = toggleGroup;
        Nightmare.group = toggleGroup;

        string clothesEffect = $"\n 특수능력 :{Managers.Data.Stat[Managers.Game.ClothesIndex].EffectType} +{Managers.Data.Stat[Managers.Game.ClothesIndex].EffectValue}";
        // 토글 설정
        Normal.isOn = true;
        if (Normal.isOn)
        {
            GetText((int)Texts.DifficultyExplainText).text = Managers.GetText(Define.DifficultyNormal);
        }
        // 토글 상태 변경 이벤트 등록
        Normal.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                GetText((int)Texts.DifficultyExplainText).text = Managers.GetText(Define.DifficultyNormal);
                Managers.Game.DifficultyLevel = "Normal";
                Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
            }
        });

        Hard.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                GetText((int)Texts.DifficultyExplainText).text = Managers.GetText(Define.DifficultyHard);
                Managers.Game.DifficultyLevel = "Hard";
                Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
            }
        });

        UnLimited.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                GetText((int)Texts.DifficultyExplainText).text = Managers.GetText(Define.DifficultyUnlimited);
                Managers.Game.DifficultyLevel = "UnLimited";
                Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
            }
        });

        Nightmare.onValueChanged.AddListener((isOn) => 
        {
            if (isOn)
            {
                GetText((int)Texts.DifficultyExplainText).text = Managers.GetText(Define.DifficultyNightmare);
                Managers.Game.DifficultyLevel = "Nightmare";
                Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
            }
        });

        #endregion

        RefreshUI();
    	_inputField = GetObject((int)GameObjects.InputField).gameObject.GetComponent<TMP_InputField>();
		_inputField.text = "";

        return true;
    }

    //옷 텍스트 가져오기
    public void RefreshClothesText()
    {
        string statDataName = Managers.Data.GetStatNameById(customManager.clothes); 
        GetText((int)Texts.ClothesText).text = $"{statDataName}";
        string statDataDes = Managers.Data.GetStatDesById(customManager.clothes); 
        GetText((int)Texts.ClothesEffectText).text = $"{statDataDes}";
    }
    public void RefreshClothes()
    {
        customManager.numberCheck(1);  
    }

    // private void SetToggleGroup()
    // {

    // }


	void RefreshUI()
	{
		GetText((int)Texts.HintText).text = Managers.GetText(Define.WriteNickname);
        GetText((int)Texts.ConfirmButtonText).text = Managers.GetText(Define.NicknameConfirm);
		GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

        
	}

    void OnClickConfirmButton()
    {
        //닉네임 유효성 검증
        if (string.IsNullOrEmpty(_inputField.text) || _inputField.text.Length >= 7)
        {
            GetText((int)Texts.WarningInput).gameObject.SetActive(true);
            Debug.Log("입력 오류: 유효한 값이 아닙니다.");
            return; 
        }

        Managers.Game.ClothesIndex = customManager.clothes;
        Managers.Data.Start.ClothesIndex = Managers.Game.ClothesIndex;

        //순서 중요 (난이도로 스탯 변경 후 -> 옷 능력치 추가)
        if(Normal.isOn) Managers.Game.DifficultyLevel = "Normal";
        else if(Hard.isOn) Managers.Game.DifficultyLevel = "Hard";
        else if(UnLimited.isOn) Managers.Game.DifficultyLevel = "UnLimited";
        else if(Nightmare.isOn) Managers.Game.DifficultyLevel = "Nightmare";

        //옷 능력치 추가
        ApplyInitialStats();

        Managers.Sound.Play(Sound.Effect, "Sound_Checkbutton");
        Debug.Log("onClickConfirmButton");
        Debug.Log($"Input ID {_inputField.text}");

        Managers.Game.Name = _inputField.text;
        // Managers.Resource.Destroy(Player);
		// Managers.UI.ShowPopupUI<UI_IntroPopup>();
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_CountPopup>();
        Managers.UI.ShowSceneUI<UI_PlayerScene>();
    }

    public void GotoTitle()
    {
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_TitlePopup>();
        GameObject Player = GameObject.Find("StaticPlayer");
        Player.transform.position = new Vector3(0, -1,0);
        Player.transform.localScale = new Vector3(0.5f, 0.5f,0.5f);
    }

    //처음 컬렉션 업적 만큼 스탯 더하기
    private void ApplyInitialStats()
    {
        foreach (int statID in Managers.Data.Stat.Keys)
        {
            if (Managers.Game.ClothesIndex == statID)
            {
                StatData statData = Managers.Data.Stat[statID];
                Debug.Log($"statData.EffectType: {statData.EffectType}");
                Debug.Log($"statData.EffectValue: {statData.EffectValue}");
                Debug.Log(Managers.Game.HintKey);
                if(statData.EffectType == "Stage")
                {
                    Managers.Game.Stage += statData.EffectValue;
                }
                else if(statData.EffectType == "Avoid")
                {
                    Managers.Game.Avoid += statData.EffectValue;
                }
                else if(statData.EffectType == "HintKey")
                {
                    Managers.Game.HintKey += statData.EffectValue;
                }
                else if(statData.EffectType == "Luck")
                {
                    Managers.Game.LuckPercent += statData.EffectValue;
                }
                else if(statData.EffectType == "GuessTimer")
                {
                    Managers.Game.GuessTimer += statData.EffectValue;
                }
                else if(statData.EffectType == "Defence")
                {
                    Managers.Game.Defence += statData.EffectValue;
                }
                else
                    Debug.Log("XML STATDATA EFFECTTYPE ERROR");
            }
        }
    }
}
