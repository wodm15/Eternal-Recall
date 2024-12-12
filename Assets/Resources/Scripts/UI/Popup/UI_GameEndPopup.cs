using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_GameEndPopup : UI_Popup
{
    enum Texts
    {
        GameEndText,
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

        Managers.Sound.Clear();
        Managers.Sound.Play(Sound.Effect, "Sound_GameEnd");

        GetText((int)Texts.GameEndText).text =  Managers.GetText(Define.GameEndText);
        GetText((int)Texts.GoToTitleText).text =  Managers.GetText(Define.GoToTitleText);

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
