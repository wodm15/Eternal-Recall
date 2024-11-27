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

        Player = GameObject.FindGameObjectWithTag("Player");
        
        if(Player != null)
        {
            GameObject customManager = Utils.FindChild(Player, "CustomManager");
            //스크립트 접근
            CustomManager _customManager = customManager.GetComponent<CustomManager>();

            _customManager.hair = 6;
            Debug.Log("111");
            _customManager.clothes = UI_NamePopup.playerIndex[1]; 
            _customManager.eyebrow = UI_NamePopup.playerIndex[2]; 
            _customManager.eye = UI_NamePopup.playerIndex[3]; 
            _customManager.mouth = UI_NamePopup.playerIndex[4]; 
            _customManager.emotion = UI_NamePopup.playerIndex[5]; 

        }

        Stranger = Managers.Resource.Instantiate("Stranger");
        Stranger.transform.position = new Vector3(-10,0,0);

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
                // Managers.UI.ClosePopupUI(this);
                // Managers.UI.ShowPopupUI<UI_TitlePopup>();

                
                Managers.Resource.Destroy(Stranger);
        }
        }
    }

    public void MakeRandomStranger()
    {
        Stranger = Managers.Resource.Instantiate("Stranger");
    }
}
