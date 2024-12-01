using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_PlayPopup : UI_Popup
{
    public int[] playerIndex = new int[6] {0,0,0,0,0,0};
    public GameObject UI_PlayeScene;
    public int _speed=5;
    GameObject Stranger;

    enum Buttons
    {
        TheWorldButton,
    }
    enum Texts
    {
        TheWorldText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        Stranger = GameObject.Find("Stranger");
        if(Stranger == null)
            Debug.Log("Check Connect with UI_QUESTIONPOPUP(Stranger는 여기에서 생성됨)");

        Stranger.transform.position = new Vector3(-10,2,0);

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        // GetButton((int)Buttons.TheWorldButton).image.sprite = Managers.Resource.Load<Sprite>($"{
        GetButton((int)Buttons.TheWorldButton).gameObject.BindEvent(TheWorldEvent);

        GetText((int)Texts.TheWorldText).text = "The World :" + Managers.Game.TheWorld.ToString();

        
        return true;
    }
    public void FixedUpdate()
    {
       if (Stranger != null && _speed > 0.01) 
        {
            Stranger.transform.position += new Vector3(_speed * Time.deltaTime, 0, 0);
            if(Stranger.transform.position.x >= 10)
            {
                Debug.Log("stranger Passed");
                Managers.Sound.Play(Sound.Effect, "Sound_Question");
                Managers.UI.ClosePopupUI(this);
                Managers.UI.ShowPopupUI<UI_GuessPopup>();

                // Managers.Resource.Destroy(Stranger);
        }
        }
    }

    void TheWorldEvent()
    {
        if(Managers.Game.TheWorld <= 0)
        {
            Debug.Log("NO Item");
        }
        else
        {
            _speed = 0;
            Managers.Game.TheWorld--;
            GetText((int)Texts.TheWorldText).text = "The World :" + Managers.Game.TheWorld.ToString();
            StartCoroutine(TheWorldEffect());
        }
    }

    IEnumerator TheWorldEffect()
    {
        yield return new WaitForSeconds(3);

        _speed = 5;
    }


}
