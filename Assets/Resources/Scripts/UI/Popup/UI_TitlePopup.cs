using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_TitlePopup : UI_Popup
{
	GameObject Player;
	CustomManager customManager;
    AnimationManager animationManager;
	enum Texts
	{
		// TouchToStartText,
		StartButtonText,
		ContinueButtonText,
		CollectionButtonText,
		QuitButtonText,
		SayingText,
	}

    enum Buttons
	{
		StartButton,
		ContinueButton,
		CollectionButton,
		QuitButton,
	}

    public override bool Init()
	{
		if (base.Init() == false)
			return false;
		
		BindText(typeof(Texts));
		BindButton(typeof(Buttons)); 

		Player = GameObject.Find("StaticPlayer");
		if(Player == null)
			Player = Managers.Resource.Instantiate("StaticPlayer");
			
		Player.transform.position = new Vector3(0,-1,-0);
        Player.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        GameObject _customManager = GameObject.FindGameObjectWithTag("StaticManager");
        customManager = _customManager.GetComponent<CustomManager>();
        animationManager = _customManager.GetComponent<AnimationManager>();
		animationManager.ani = Random.Range(0,10);
		animationManager.PlayAni(true);
		
		int randSay = Random.Range(0, Define.CharacterSaying.Length);
		GetText((int)Texts.SayingText).text = Managers.GetText(Define.CharacterSaying[randSay]);

		GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
		GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
		GetButton((int)Buttons.CollectionButton).gameObject.BindEvent(() =>
		{
			// int a = Random.Range(0,50);
			// if(a < 50)
			// 	Managers.Game.ADSHOW();
			OnClickCollectionButton();
		});
		GetButton((int)Buttons.QuitButton).gameObject.BindEvent(() =>
        {
            #if UNITY_EDITOR
					UnityEditor.EditorApplication.isPlaying = false;
			#else
					// 빌드된 게임에서 종료
					Application.Quit();
			#endif
        });
		// GetButton((int)Buttons.CollectionButton).gameObject.BindEvent(OnClickCollectionButton);
		GetText((int)Texts.StartButtonText).text = Managers.GetText(Define.StartButtonText);
		GetText((int)Texts.ContinueButtonText).text = Managers.GetText(Define.ContinueButtonText);
		GetText((int)Texts.CollectionButtonText).text = Managers.GetText(Define.CollectionButtonText);
		GetText((int)Texts.QuitButtonText).text = Managers.GetText(Define.QuitButtonText);

		Managers.Game.Init();


		//노래 설정
		Managers.Sound.Clear();
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
			// Managers.Game.Init();
			Managers.Game.LoadGame();

			Managers.UI.ClosePopupUI(this);
			Managers.UI.ShowPopupUI<UI_ConfirmPopup>();
			Managers.Resource.Destroy(Player);
		}
		else
		{
			Managers.Game.CharacterDelete();
			// Managers.Game.Init();

			Managers.UI.ClosePopupUI(this);
			Managers.UI.ShowPopupUI<UI_NamePopup>();
			Managers.Resource.Destroy(Player);
		}	

	}

	void OnClickContinueButton()
	{
		Debug.Log("OnClickContinueButton");
		// Managers.Game.CharacterDelete();
		// Managers.Game.Init();
		if(!Managers.Game.LoadGame())
			{

			}
		else if(Managers.Game.LoadGame() && Managers.Game.Hp > 0)
		{
			Managers.UI.ClosePopupUI(this);
			Managers.UI.ShowPopupUI<UI_CountPopup>();
			Managers.UI.ShowSceneUI<UI_PlayerScene>();
		}
	}

	void OnClickCollectionButton()
	{
		// Managers.Sound.Play(Sound.Effect, ("Sound_"));
		// Managers.Resource.Destroy(Player);
		// Managers.Game.Init();
		Managers.Game.LoadGame();
		Player.transform.localPosition = new Vector3(12,0,0);

		Debug.Log("OnClickCollectionButton");
		Managers.UI.ShowPopupUI<UI_CollectionPopup>();
	}
}
