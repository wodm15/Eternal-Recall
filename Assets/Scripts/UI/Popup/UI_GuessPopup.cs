using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GuessPopup : UI_Popup
{
    enum Texts
    {
        ConfirmButtonText,
    }
    enum Buttons
    {
        ConfirmButton,
    }
    GameObject GuessPlayer;
    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        // GuessPlayer = Managers.Resource.Instantiate("Player");
        // GuessPlayer.transform.position = new Vector3(-2,0,0);
        // GuessPlayer.transform.localScale = new Vector3(1,1,1);

		BindText(typeof(Texts));
		BindButton(typeof(Buttons));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

        return true;
    }

    void OnClickConfirmButton()
    {
        Debug.Log("Guess ClickConfirmButton");

        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_GetItemPopup>(); //
    }
    
}
