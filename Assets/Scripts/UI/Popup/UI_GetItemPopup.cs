using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GetItemPopup : UI_Popup
{
    enum Images
    {
        ItemImage1,
        ItemImage2,
        ItemImage3,
    }
    enum Texts 
    {
        ItemText1,
        ItemText2,
        ItemText3,
    }
    enum Buttons
    {
        ItemButton1,
        ItemButton2,
        ItemButton3,
    }
    GameObject Player;
    public override bool Init()
    {
        if (base.Init() == false)
			return false;

    BindObject(typeof(Images));
	BindText(typeof(Texts));
	BindButton(typeof(Buttons));
    
    GetButton((int)Buttons.ItemButton1).gameObject.BindEvent(OnClickItem1);
    GetButton((int)Buttons.ItemButton2).gameObject.BindEvent(OnClickItem2);
    GetButton((int)Buttons.ItemButton3).gameObject.BindEvent(OnClickItem3);

    // Player = Managers.Resource.Instantiate("StaticPlayer");
    // Player.transform.position = new Vector3(-4,-4,0);
    // Player.transform.localScale = new Vector3(1,1, 1);

        return true;
    }

    void OnClickItem1()
    {
        Debug.Log("getItem1");
        Managers.UI.ClosePopupUI(this);
    }
    void OnClickItem2()
    {
        Debug.Log("getItem2");
        Managers.UI.ClosePopupUI(this);
    }
    void OnClickItem3()
    {
        Debug.Log("getItem3");
        Managers.UI.ClosePopupUI(this);
    }
}
