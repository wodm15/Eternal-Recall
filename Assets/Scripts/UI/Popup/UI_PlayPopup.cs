using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayPopup : UI_Popup
{
    public int _speed = 5;
    GameObject Player;
    GameObject Stranger;

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        
        
        Player = Managers.Resource.Instantiate("StaticPlayer");
        Player.transform.position = new Vector3(-4,-4,0);
        Player.transform.localScale = new Vector3(1,1, 1);
        

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
                Debug.Log("ENDENDEND");
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
