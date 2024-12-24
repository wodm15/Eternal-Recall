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
		PickupText,
		SayingText,
		ShopButtonText,
	}

    enum Buttons
	{
		StartButton,
		ContinueButton,
		CollectionButton,
		PickupButton,
		QuitButton,
		ShopButton,
	}

    public override bool Init()
	{
		if (base.Init() == false)
			return false;
		if(!Managers.Game.LoadGame())
			Managers.Game.Init();

		//일단 확인 후 false로 초기화
		bool isRevive = Managers.Game.ReviveLife;
		Managers.Game.ReviveLife = false;

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
		
		initialPlayer();
		
		animationManager.ani = Random.Range(0,10);
		animationManager.PlayAni(true);
		
		if(isRevive)
		{
			GetText((int)Texts.SayingText).text = Managers.GetText(Define.ReviveSayText);
		}
		else
		{
			int randSay = Random.Range(0, Define.CharacterSaying.Length);
			GetText((int)Texts.SayingText).text = Managers.GetText(Define.CharacterSaying[randSay]);
		}

		GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
		GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
		GetButton((int)Buttons.CollectionButton).gameObject.BindEvent(() =>
		{
			//TODO 광고
			// int a = Random.Range(0,50);
			// if(a < 50)
			// Managers.Game.InterstitialAd();

			OnClickCollectionButton();
		});
		GetButton((int)Buttons.PickupButton).gameObject.BindEvent(() =>
        {
            
			OnClickPickupButton();

        });
		GetButton((int)Buttons.QuitButton).gameObject.BindEvent(() =>
        {
            
			Application.Quit();

        });
		GetButton((int)Buttons.ShopButton).gameObject.BindEvent(() =>
		{
			OnClickShopButton();
		});

		
		// GetButton((int)Buttons.CollectionButton).gameObject.BindEvent(OnClickCollectionButton);
		GetText((int)Texts.StartButtonText).text = Managers.GetText(Define.StartButtonText);
		GetText((int)Texts.ContinueButtonText).text = Managers.GetText(Define.ContinueButtonText);
		GetText((int)Texts.CollectionButtonText).text = Managers.GetText(Define.CollectionButtonText);
		GetText((int)Texts.QuitButtonText).text = Managers.GetText(Define.QuitButtonText);
		GetText((int)Texts.PickupText).text = Managers.GetText(Define.PickupText);
		GetText((int)Texts.ShopButtonText).text = Managers.GetText(Define.ShopButtonText);

		//노래 설정
		Managers.Sound.Clear();
		List<string> bgmTracks = new List<string>
		{
			"Sound_MainTitle"
		};

		int randomIndex = Random.Range(0, bgmTracks.Count);
		Managers.Sound.Play(Sound.Bgm, bgmTracks[randomIndex]);

		//잘못된 값이 있어 삭제
		if(Managers.CheckJson.CheckJson() == -1)
			GetText((int)Texts.SayingText).text = Managers.GetText(Define.DeleteFile);

		
		return true;
	}

void OnClickStartButton()
	{
		Debug.Log("OnClickStartButton");

		if (Managers.Game.HasSavedData())
		{
			Managers.Game.Init();
			// CheckFirstVisit();
			Managers.Game.LoadGame();

			Managers.UI.ClosePopupUI(this);
			Managers.UI.ShowPopupUI<UI_ConfirmPopup>();
			Managers.Resource.Destroy(Player);
		}
		else
		{
			Managers.Game.CharacterDelete();
			Managers.Game.Init();

			Managers.UI.ClosePopupUI(this);
			Managers.UI.ShowPopupUI<UI_NamePopup>();
			Managers.Resource.Destroy(Player);
		}	

	}

	void OnClickContinueButton()
	{
		Debug.Log("OnClickContinueButton");
		// Managers.Game.CharacterDelete();


		bool isGameLoaded = Managers.Game.LoadGame();

		if(!isGameLoaded)
			{
				GetText((int)Texts.SayingText).text = Managers.GetText(Define.SaveNothing);
			}
		else if(isGameLoaded && Managers.Game.Hp <=0)
		{
			GetText((int)Texts.SayingText).text = Managers.GetText(Define.SaveButEnd);
		}
		
		else if(isGameLoaded && Managers.Game.Hp > 0)
		{
			Managers.Sound.Clear();
			Managers.Sound.Play(Sound.Bgm , Managers.Game.BGM);
			Managers.UI.ClosePopupUI(this);
			Managers.UI.ShowPopupUI<UI_CountPopup>();
			Managers.UI.ShowSceneUI<UI_PlayerScene>();
		}
	}
	void initialPlayer()
	{
		customManager.hair = 2;
		customManager.clothes = 2;
		customManager.eyebrow = 0;
		customManager.eye = 2;
		customManager.mouth = 0;
		customManager.emotion = 0;

	    customManager.numberCheck(0);
        customManager.numberCheck(1);
        customManager.numberCheck(2);
        customManager.numberCheck(3);
        customManager.numberCheck(4);
        customManager.numberCheck(5);
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

	void OnClickPickupButton()
	{
		Managers.Game.LoadGame();
		Player.transform.localPosition = new Vector3(12,0,0);

		Debug.Log("OnClickPickupButton");
		Managers.UI.ShowPopupUI<UI_PickupPopup>();
	}
	void OnClickShopButton()
	{
		Managers.Game.LoadGame();
		Player.transform.localPosition = new Vector3(0,0,0);
		Player.transform.localScale = new Vector3(1,1,1);

		Debug.Log("OnClickShopButton");
		Managers.UI.ShowPopupUI<UI_ShopPopup>();
	}

	void CheckFirstVisit()
	{
		//처음 접속일 경우 병아리 옷 획득 후 저장
		if (Managers.Game.Collections[2] == CollectionState.None)
		{
        	Managers.Game.Collections[2] = CollectionState.Done;
		}
	}
}
