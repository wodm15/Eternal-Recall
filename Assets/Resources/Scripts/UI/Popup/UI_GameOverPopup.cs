using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_GameOverPopup : UI_Popup
{
    UI_PlayerScene playerScene;
    enum Texts
    {
        GameOverText,
        GoToTitleText,
        ReviveText,
        
    }
    enum Buttons
    {
        GoToTitleButton,
        ReviveButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
			return false;
        
		BindText(typeof(Texts));
		BindButton(typeof(Buttons));
        // BindImage(typeof(Images));

        playerScene = Managers.UI.GetSceneUI<UI_PlayerScene>();
        // GameObject button = playerScene.transform.Find("Button").gameObject;
        // if(button == null)
        //     Debug.Log("PlayerScnene Button not found");
        
        // button.SetActive(false);
        
        GameObject guessPlayer = GameObject.Find("Player");
        if(guessPlayer != null)
            Managers.Resource.Destroy(guessPlayer);
        GameObject Stranger = GameObject.Find("Stranger");
        if(guessPlayer != null)
            Managers.Resource.Destroy(Stranger);

        // Managers.UI.ClosePlayerSceneUI();
        Managers.Sound.Clear();
        Managers.Sound.Play(Sound.Effect, "Sound_Gameover");

        GetText((int)Texts.GameOverText).text = Managers.GetText(Define.GameOverText);
        GetText((int)Texts.GoToTitleText).text = Managers.GetText(Define.GameOverTitle);
        GetText((int)Texts.ReviveText).text = Managers.GetText(Define.ReviveText);
        GetButton((int)Buttons.ReviveButton).gameObject.SetActive(true);

        GetButton((int)Buttons.GoToTitleButton).gameObject.BindEvent(() => OnClickConfirmButton());
        GetButton((int)Buttons.ReviveButton).gameObject.BindEvent(() => OnClickReviveButton());
        if(Managers.Game.Revive <= 0)
        {
            GetButton((int)Buttons.ReviveButton).gameObject.SetActive(false);
        }

        playerScene = Managers.UI.GetSceneUI<UI_PlayerScene>();
        playerScene.StaticPlayerEx("GameOver");
        return true;
    }

    void OnClickConfirmButton()
    {
        Managers.UI.ClosePopupUI(this);

        Managers.Game.SaveGame();
        Managers.UI.ShowPopupUI<UI_TitlePopup>();  
        Managers.UI.ClosePlayerSceneUI();
    }

    void OnClickReviveButton()
    {
        Managers.Game.Revive--;
        Managers.Game.ReviveLife = true;
        
        Managers.UI.ClosePopupUI(this);

        Managers.Game.Hp= 1;
        Managers.Game.RewardedAd();
        Managers.Game.SaveGame();
        Managers.UI.ShowPopupUI<UI_TitlePopup>();  
        Managers.UI.ClosePlayerSceneUI();


    }

}
