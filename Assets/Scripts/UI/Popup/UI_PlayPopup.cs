using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_PlayPopup : UI_Popup
{
    UI_PlayerScene playerScene;
    public int _speed = 5;
    GameObject Stranger;

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        Stranger = Managers.Resource.Instantiate("Stranger");
        Stranger.transform.position = new Vector3(-10,2,0);


        return true;
    }
    public void Update()
    {
        if(Stranger != null)
        {
            Stranger.transform.position += new Vector3(_speed * Time.deltaTime, 0, 0);
            if(Stranger.transform.position.x >= 10)
            {
                Debug.Log("stranger Passed");
                Managers.Sound.Play(Sound.Effect, "Sound_Question");
                Managers.UI.ClosePopupUI(this);
                Managers.UI.ShowPopupUI<UI_GuessPopup>();

                Managers.Resource.Destroy(Stranger);
        }
        }
    }

    public void MakeRandomStranger()
    {
        Stranger = Managers.Resource.Instantiate("Stranger");
    }
}
