using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_NamePopup : UI_Popup
{
    GameObject Player;
    enum GameObjects
	{
		InputField
	}

    enum Texts
	{
		ConfirmButtonText,
		NameText,
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
        // BindObject(typeof(GameObjects));
		// BindText(typeof(Texts));
		// BindButton(typeof(Buttons));

		// GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

    	// _inputField = GetObject((int)GameObjects.InputField).gameObject.GetComponent<TMP_InputField>();
		// _inputField.text = "";

        CharacterResponse();

     
        return true;
    }

    //캐릭 랜덤 생성
    void CharacterResponse()
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


            _customManager.hair = Random.Range(0, hairLength);
            _customManager.clothes = Random.Range(0,clothesLength );
            _customManager.eyebrow = Random.Range(0,eyebrowLength);
            _customManager.eye = Random.Range(0,eyeLength);
            _customManager.mouth = Random.Range(0,mouthLength);
            _customManager.emotion = Random.Range(0,emotionLength);

        }
        else
            Debug.LogError("NO PLAYER FOUND");
    }

    void OnClickConfirmButton()
    {
        //TODO
        Debug.Log("onClickConfirmButton");
    }

}
