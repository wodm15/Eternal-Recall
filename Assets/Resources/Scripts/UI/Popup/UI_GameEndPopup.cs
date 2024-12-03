using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        GetText((int)Texts.GameEndText).text =  Managers.GetText(Define.GameEndText);
        GetText((int)Texts.GoToTitleText).text =  Managers.GetText(Define.GoToTitleText);

        GetButton((int)Buttons.GoToTitleButton).gameObject.BindEvent(() => OnClickConfirmButton());

        return true;
    }

    void OnClickConfirmButton()
    {
        //TODO 게임데이터 저장
        Managers.Sound.Clear();
        Managers.UI.ClosePopupUI(this);  
        Managers.UI.ClosePlayerSceneUI();
        Managers.UI.ShowPopupUI<UI_TitlePopup>();  
    }
}