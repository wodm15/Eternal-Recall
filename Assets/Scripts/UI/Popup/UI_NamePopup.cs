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

        Player = GameObject.FindGameObjectWithTag("Player");
        if(Player != null)
        {
            AnimationManager animationManager = Player.GetComponent<AnimationManager>();

            animationManager.ani = Random.Range(0, animationManager.aniName.Length);
        }
        else
            Debug.LogError("NO PLAYER FOUND");

        
        
        return true;
    }

    void OnClickConfirmButton()
    {
        //TODO
        Debug.Log("onClickConfirmButton");
    }

}
