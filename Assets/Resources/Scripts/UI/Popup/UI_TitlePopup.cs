using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_TitlePopup : UI_Popup
{
	GameObject Player;
	enum Texts
	{
		// TouchToStartText,
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
		GetButton((int)Buttons.CollectionButton).gameObject.BindEvent(() =>
		{
			int a = Random.Range(0,50);
			if(a < 50)
				Managers.Game.ADSHOW();
			OnClickCollectionButton();
		});
		// GetButton((int)Buttons.CollectionButton).gameObject.BindEvent(OnClickCollectionButton);
		GetText((int)Texts.StartButtonText).text = Managers.GetText(Define.StartButtonText);
		GetText((int)Texts.ContinueButtonText).text = Managers.GetText(Define.ContinueButtonText);
		GetText((int)Texts.CollectionButtonText).text = Managers.GetText(Define.CollectionButtonText);

		Managers.Sound.Clear();

		Player = Managers.Resource.Instantiate("StaticPlayer");
        Player.transform.position = Vector3.zero;
        Player.transform.localScale = new Vector3(1, 1, 1);

		int music = Random.Range(0,100);
		if( music < 50)
			Managers.Sound.Play(Sound.Bgm, "Sound_MainTitle");
		else
			Managers.Sound.Play(Sound.Bgm, "Sound_MainPlayBGM");
		return true;
	}

void OnClickStartButton()
	{
		Debug.Log("OnClickStartButton");

		if (Managers.Game.HasSavedData())
		{
			Managers.Game.Init();
			Managers.Game.LoadGame();

			Managers.UI.ClosePopupUI(this);
			Managers.UI.ShowPopupUI<UI_ConfirmPopup>();
		}
		else
		{
			Managers.Game.CharacterDelete();
			Managers.Game.Init();

			Managers.UI.ClosePopupUI(this);
			Managers.UI.ShowPopupUI<UI_NamePopup>();
		}	

	}

	void OnClickContinueButton()
	{
		Debug.Log("OnClickContinueButton");
		Managers.Game.CharacterDelete();
		Managers.Game.Init();
		if(!Managers.Game.LoadGame())
			{

			}
		else if(Managers.Game.LoadGame() && Managers.Game.Hp > 0)
		{
			Managers.UI.ClosePopupUI(this);
			Managers.UI.ShowSceneUI<UI_PlayerScene>();
			Managers.UI.ShowPopupUI<UI_CountPopup>();
		}
	}

	void OnClickCollectionButton()
	{
		Managers.Sound.Play(Sound.Effect, ("Sound_"));
		Managers.Game.Init();
		Managers.Game.LoadGame();

		Debug.Log("OnClickCollectionButton");
		Managers.UI.ShowPopupUI<UI_CollectionPopup>();
	}
}
