using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class UI_NamePopup : UI_Popup
{
    //플레이어 모습 저장 배열
    int[] playerIndex = new int[6] {0,0,0,0,0,0};
    GameObject Player;
    enum GameObjects
	{
		InputField
	}

    enum Texts
	{
		ConfirmButtonText,
		HintText,
		ValueText
	}

	enum Buttons
	{
		ConfirmButton
	}

    TMP_InputField _inputField;


    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        BindObject(typeof(GameObjects));
		BindText(typeof(Texts));
		BindButton(typeof(Buttons));

		GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

    	_inputField = GetObject((int)GameObjects.InputField).gameObject.GetComponent<TMP_InputField>();
		_inputField.text = "";

        RefreshUI();
        // CharacterResponse();

     
        return true;
    }

	void RefreshUI()
	{
		GetText((int)Texts.HintText).text = "Write the NickName";
        // GetText((int)Texts.HintText).text = Managers.GetText(Define.WriteNickname);
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
        Managers.Sound.Play(Sound.Effect, "Sound_Checkbutton");
        Debug.Log("onClickConfirmButton");
        Debug.Log($"Input ID {_inputField.text}");

        Managers.Game.Name = _inputField.text;

		// Managers.UI.ShowPopupUI<UI_IntroPopup>();
        Managers.UI.ClosePopupUI(this);
        //Temp
        Managers.UI.ShowPopupUI<UI_CountPopup>();
        Managers.UI.ShowSceneUI<UI_PlayerScene>();
    }

}
