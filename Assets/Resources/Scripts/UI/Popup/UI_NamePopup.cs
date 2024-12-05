using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using System.Linq;  


public class UI_NamePopup : UI_Popup
{
    //플레이어 모습 저장 배열
    int[] playerIndex = new int[6] {0,0,0,0,0,0};
    
    //스테틱플레이어 매니저
    GameObject Player;
    GameObject _customManager;
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

        // Player = Managers.Resource.Instantiate("StaticPlayer");

        GameObject player = GameObject.FindGameObjectWithTag("StaticManager");
        customManager = player.GetComponent<CustomManager>();
        animationManager = player.GetComponent<AnimationManager>();
        // Player.transform.position = Vector3.zero;
        // Player.transform.localScale = new Vector3(1, 1, 1);


        BindObject(typeof(GameObjects));
		BindText(typeof(Texts));
		BindButton(typeof(Buttons));

        GetText((int)Texts.ConfirmButtonText).text = Managers.GetText(Define.NicknameConfirm);
		GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        
        #region 코디 (현재는 옷만)

        GetText((int)Texts.ClothesEffectText).text = "옷 효과입니다.";

GetButton((int)Buttons.ClothesMinus).gameObject.BindEvent(() =>
{   
    // 현재 customManager.clothes - 1이 Done 상태라면 바로 이동
    if (Managers.Game.StatDataState[customManager.clothes - 1] == StatDataState.Done)
    {
        // Done 상태라면 customManager.clothes 값을 감소시켜 진행
        if (0 < customManager.clothes)
        {
            customManager.clothes--;  // 현재 값을 감소시킴
        }

        customManager.numberCheck(1);  // 추가된 로직 실행
    
        string statDataName = Managers.Data.GetStatNameById(customManager.clothes); 
        GetText((int)Texts.ClothesText).text = $"{statDataName}";
        // }
        Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
    }
    else
    {
        // Done 상태의 가장 가까운 항목으로 이동
        for (int i = customManager.clothes - 1; i >= 0; i--)
        {
            if (Managers.Game.StatDataState[i] == StatDataState.Done)
            {
                customManager.clothes = i;  // Done 상태의 항목으로 이동
                break;
            }
        }

        customManager.numberCheck(1);  
        string statDataName = Managers.Data.GetStatNameById(customManager.clothes); 
        GetText((int)Texts.ClothesText).text = $"{statDataName}";

        Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
    }
});


GetButton((int)Buttons.ClothesPlus).gameObject.BindEvent(() =>
{
    // Done 상태의 항목만 처리
    if (Managers.Game.StatDataState[customManager.clothes + 1] == StatDataState.Done)
    {
        if (customManager.clothesM.count.Length > customManager.clothes)
        {
            customManager.clothes++;  // customManager.clothes 값을 증가시킴
        }

        customManager.numberCheck(1);  // 추가된 로직 실행
        string statDataName = Managers.Data.GetStatNameById(customManager.clothes); 
        GetText((int)Texts.ClothesText).text = $"{statDataName}";
        Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
    }
    else
    {
        // Done 상태의 가장 가까운 항목으로 이동
        for (int i = customManager.clothes + 1; i < customManager.clothesM.count.Length -1 ; i++)
        {
            if (Managers.Game.StatDataState[i] == StatDataState.Done)
            {
                customManager.clothes = i;  // Done 상태의 항목으로 이동
                break;
            }
        }

        customManager.numberCheck(1); 
        string statDataName = Managers.Data.GetStatNameById(customManager.clothes); 
        GetText((int)Texts.ClothesText).text = $"{statDataName}";
        Managers.Sound.Play(Sound.Effect, "Sound_GuessButton");
    }
});



        #endregion
        
        Normal.group = toggleGroup;
        Hard.group = toggleGroup;
        UnLimited.group = toggleGroup;

        Normal.isOn = true;

        RefreshUI();
    	_inputField = GetObject((int)GameObjects.InputField).gameObject.GetComponent<TMP_InputField>();
		_inputField.text = "";

        if(Managers.Game.StatData.ID == 101)
            Debug.Log("101옷 있음");

        return true;
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
        Managers.Game.CharacterDelete();

        Managers.Sound.Play(Sound.Effect, "Sound_Checkbutton");
        Debug.Log("onClickConfirmButton");
        Debug.Log($"Input ID {_inputField.text}");

        Managers.Game.Name = _inputField.text;
        Managers.Resource.Destroy(Player);
		// Managers.UI.ShowPopupUI<UI_IntroPopup>();
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_CountPopup>();
        Managers.UI.ShowSceneUI<UI_PlayerScene>();
    }


}
