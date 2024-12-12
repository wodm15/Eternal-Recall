using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using System.Linq;
using JetBrains.Annotations;
using System.Collections.ObjectModel;


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
	}

	enum Buttons
	{
		ConfirmButton,
        ClothesMinus,
        ClothesPlus,
        GotoTitleButton,
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
            }
        });

        Hard.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                GetText((int)Texts.DifficultyExplainText).text = Managers.GetText(Define.DifficultyHard);
                Managers.Game.DifficultyLevel = "Hard";
            }
        });

        UnLimited.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                GetText((int)Texts.DifficultyExplainText).text = Managers.GetText(Define.DifficultyUnlimited);
                Managers.Game.DifficultyLevel = "UnLimited";
            }
        });
        #endregion

        RefreshUI();
    	_inputField = GetObject((int)GameObjects.InputField).gameObject.GetComponent<TMP_InputField>();
		_inputField.text = "";

        if(Managers.Game.StatData.ID == 101)
            Debug.Log("101옷 있음");

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

    private void SetToggleGroup()
    {

    }


	void RefreshUI()
	{
		GetText((int)Texts.HintText).text = Managers.GetText(Define.WriteNickname);
        GetText((int)Texts.ConfirmButtonText).text = Managers.GetText(Define.NicknameConfirm);
		GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

        
	}

    //캐릭 랜덤 생성 (아직 안씀)
    //TODO
    public void CharacterResponse()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if(Player != null)
        {
            GameObject customManager = Utils.FindChild(Player, "CustomManager");
            //스크립트 접근
            CustomManager _customManager = customManager.GetComponent<CustomManager>();


            //애니메이션 랜덤 변경
            AnimationManager animationManager = customManager.GetComponent<AnimationManager>();
            animationManager.ani = Random.Range(0, animationManager.aniName.Length);
            

            GameObject hair = GameObject.FindGameObjectWithTag("Hair");
            GameObject clothes = GameObject.FindGameObjectWithTag("Clothes");
            GameObject eyebrow = GameObject.FindGameObjectWithTag("Eyebrow");
            GameObject eye = GameObject.FindGameObjectWithTag("Eye");
            GameObject mouth = GameObject.FindGameObjectWithTag("Mouth");
            GameObject emotion = GameObject.FindGameObjectWithTag("Emotion");
            
            int hairLength = hair.transform.childCount;
            // int clothesLength = clothes.transform.childCount;
            int clothesLength = 20;
            int eyebrowLength = eyebrow.transform.childCount;
            int eyeLength = eye.transform.childCount;
            int mouthLength = mouth.transform.childCount;
            int emotionLength = mouth.transform.childCount;

            playerIndex[0] = Random.Range(0, hairLength);
            playerIndex[1] = Random.Range(0,clothesLength );
            playerIndex[2] = Random.Range(0,eyebrowLength);
            playerIndex[3] = Random.Range(0,eyeLength);
            playerIndex[4] = Random.Range(0,mouthLength);
            playerIndex[5] = Random.Range(0,emotionLength);

            _customManager.hair = playerIndex[0];
            _customManager.clothes = playerIndex[1];
            _customManager.eyebrow = playerIndex[2];
            _customManager.eye = playerIndex[3];
            _customManager.mouth = playerIndex[4];
            _customManager.emotion = playerIndex[5];

        }
        else
            Debug.LogError("NO PLAYER FOUND");
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
        // Managers.Game.Init();

        //순서 중요 (난이도로 스탯 변경 후 -> 옷 능력치 추가)
        if(Normal.isOn) Managers.Game.DifficultyLevel = "Normal";
        else if(Hard.isOn) Managers.Game.DifficultyLevel = "Hard";
        else if(UnLimited.isOn) Managers.Game.DifficultyLevel = "UnLimited";

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
