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
	}

	enum Buttons
	{
		ConfirmButton,
        ClothesMinus,
        ClothesPlus,
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
        
        #region 코디 (현재는 옷만)

        RefreshClothesText();
        
        GetButton((int)Buttons.ClothesMinus).gameObject.BindEvent(() =>
        {            
            customManager.clothes--;
            customManager.numberCheck(1);
            RefreshClothesText();

            // RefreshClothes();
        });
        GetButton((int)Buttons.ClothesPlus).gameObject.BindEvent(() =>
        {            
            customManager.clothes++;
            customManager.numberCheck(1);  
            RefreshClothesText();
            // RefreshClothes();
        });

            

        // //가지고 있는 옷만 필터링해서 버튼 클릭시 이동
        // int maxClothesIndex = customManager.clothesM.count.Length - 1;

        // GetButton((int)Buttons.ClothesMinus).gameObject.BindEvent(() =>
        // {
        //     // 현재 customManager.clothes - 1이 Done 상태라면 바로 이동
        //     if (customManager.clothes > 0 && Managers.Game.Collections[customManager.clothes - 1] == CollectionState.Done)
        //     {

        //         customManager.clothes--;  
        //         customManager.numberCheck(1);  
        //     }
        //     else
        //     {
        //         // Done 상태의 가장 가까운 항목으로 이동
        //         for (int i = customManager.clothes - 1; i >= 0; i--)
        //         {
        //             if (Managers.Game.Collections[i] == CollectionState.Done)
        //             {
        //                 customManager.clothes = i;  
        //                 customManager.numberCheck(1);  
        //                 break;
        //             }
        //         }
        //     }

        //     RefreshClothesText();
        //     Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        // });



        // GetButton((int)Buttons.ClothesPlus).gameObject.BindEvent(() =>
        // {
        //     // Done 상태의 항목만 처리
        //     if (Managers.Game.Collections[customManager.clothes + 1] == CollectionState.Done)
        //     {
        //         if (customManager.clothesM.count.Length > customManager.clothes)
        //         {
        //             customManager.clothes++;  // customManager.clothes 값을 증가시킴
        //         }

        //         customManager.numberCheck(1);  // 추가된 로직 실행
        //         RefreshClothesText();
        //         Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        //     }
        //     else
        //     {
        //         // // Done 상태의 가장 가까운 항목으로 이동
        //         // int startIndex = customManager.clothes + 1 < customManager.clothesM.count.Length -1 ? customManager.clothes + 1 : customManager.clothes;

        //         for (int i = customManager.clothes + 1; i < customManager.clothesM.count.Length ; i++)
        //         {
        //             if (Managers.Game.Collections[i] == CollectionState.Done)
        //             {
        //                 customManager.clothes = i;  // Done 상태의 항목으로 이동
        //                 break;
        //             }
        //         }

        //         customManager.numberCheck(1); 
        //         RefreshClothesText();
        //         Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
        //     }
        // });


        #endregion
        
        #region 토글 설정
        Normal.group = toggleGroup;
        Hard.group = toggleGroup;
        UnLimited.group = toggleGroup;

        // 토글 설정
        Normal.isOn = true;
        if (Normal.isOn)
            GetText((int)Texts.DifficultyExplainText).text = Managers.GetText(Define.DifficultyNormal);
        
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
                Managers.Game.DifficultyLevel = "Unlimited";
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
        Managers.Game.ClothesIndex = customManager.clothes;
        Managers.Game.SaveGame();

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


}
