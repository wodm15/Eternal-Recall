using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_ConfirmPopup : UI_Popup
{
	enum Texts
	{
		MessageText
	}

	enum Buttons
	{
		YesButton,
		NoButton
	}

	string _text;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindText(typeof(Texts));
		BindButton(typeof(Buttons));

		GetButton((int)Buttons.YesButton).gameObject.BindEvent(OnClickYesButton);
		GetButton((int)Buttons.NoButton).gameObject.BindEvent(OnClickNoButton);
		GetText((int)Texts.MessageText).text = Managers.GetText(Define.DataResetConfirm);

		RefreshUI();
		return true;
	}


	void RefreshUI()
	{
		if (_init == false)
			return;

	}

	void OnClickYesButton()
	{	
		Managers.Game.Init();
		Managers.Game.SaveGame();
		Managers.UI.ClosePopupUI(this);
		Managers.UI.ShowPopupUI<UI_NamePopup>();

		// Managers.Sound.Play(Sound.Effect, "Sound_CheckButton");
	}

	void OnClickNoButton()
	{
		Managers.UI.ClosePopupUI(this);
		Managers.Game.Init();
		Managers.Game.LoadGame();
		Managers.UI.ShowPopupUI<UI_CountPopup>();
		Managers.UI.ShowSceneUI<UI_PlayerScene>();
	}


	void OnComplete()
	{
		Managers.UI.ClosePopupUI(this);
	}
}
