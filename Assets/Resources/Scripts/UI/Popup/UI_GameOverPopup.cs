using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_GameOverPopup : UI_Popup
{
    enum Texts
    {
        GameOverText,
        GoToTitleText,
    }
    enum Buttons
    {
        GoToTitleButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

		BindText(typeof(Texts));
		BindButton(typeof(Buttons));
        // BindImage(typeof(Images));
        
        GameObject guessPlayer = GameObject.Find("Player");
        if(guessPlayer != null)
            Managers.Resource.Destroy(guessPlayer);
        GameObject Stranger = GameObject.Find("Stranger");
        if(guessPlayer != null)
            Managers.Resource.Destroy(Stranger);

        // Managers.UI.ClosePlayerSceneUI();
        Managers.Sound.Clear();
        Managers.Sound.Play(Sound.Effect, "Sound_Gameover");

        GetText((int)Texts.GameOverText).text = "Game Over";
        GetText((int)Texts.GoToTitleText).text = "타이틀로 돌아가기";
        GetButton((int)Buttons.GoToTitleButton).gameObject.BindEvent(() => OnClickConfirmButton());

        return true;
    }

    void OnClickConfirmButton()
    {
        Managers.Game.SaveGame();
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_TitlePopup>();  
        Managers.UI.ClosePlayerSceneUI();
        
    }

}
