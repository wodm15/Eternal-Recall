using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_PlayPopup : UI_Popup
{
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


        Stranger = Managers.Resource.Instantiate("Stranger");
        Stranger.transform.position = new Vector3(-10,2,0);

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.TheWorldButton).gameObject.BindEvent(TheWorldEvent);

        
        return true;
    }
    public void Update()
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

                Managers.Resource.Destroy(Stranger);
        }
        }
    }

    public void MakeRandomStranger()
    {
        Stranger = Managers.Resource.Instantiate("Stranger");
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
            Debug.Log(Managers.Game.TheWorld);
            StartCoroutine(TheWorldEffect());
        }
    }

    IEnumerator TheWorldEffect()
    {
        // _speed를 0으로 설정한 후 3초 대기
        Debug.Log("The World activated!");
        yield return new WaitForSeconds(3);

        // 3초 뒤에 _speed 값을 원래대로 복원
        _speed = 5;
        Debug.Log("The World deactivated, speed restored.");
    }
}
