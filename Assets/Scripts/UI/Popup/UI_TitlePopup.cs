using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_TitlePopup : UI_Popup
{
	enum Texts
	{
		TouchToStartText,
		StartButtonText,
		ContinueButtonText,
		CollectionButtonText,
		//DataResetConfirmText
	}

    enum Buttons
	{
		StartButton,
		ContinueButton,
		CollectionButton
	}

    public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindText(typeof(Texts));
		BindButton(typeof(Buttons)); 

		GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
		GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
		GetButton((int)Buttons.CollectionButton).gameObject.BindEvent(OnClickCollectionButton);

        GetText((int)Texts.StartButtonText).text = "시작하기";
        GetText((int)Texts.ContinueButtonText).text = "이어하기";
        GetText((int)Texts.CollectionButtonText).text = "컬렉션";

		// GetText((int)Texts.StartButtonText).text = Managers.GetText(Define.StartButtonText);
		// GetText((int)Texts.ContinueButtonText).text = Managers.GetText(Define.ContinueButtonText);
		// GetText((int)Texts.CollectionButtonText).text = Managers.GetText(Define.CollectionButtonText);

		Managers.Sound.Clear();
		Managers.Sound.Play(Sound.Effect, "Sound_MainTitle");
		return true;
	}

void OnClickStartButton()
	{
		Debug.Log("OnClickStartButton");
        //ui 추가

	}

	void OnClickContinueButton()
	{
		Debug.Log("OnClickContinueButton");
        //ui 추가

	}

	void OnClickCollectionButton()
	{
		Debug.Log("OnClickCollectionButton");
        //ui 추가
	}
}
