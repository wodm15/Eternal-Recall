using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_PlayPopup : UI_Popup
{
    public int[] playerIndex = new int[6] {0,0,0,0,0,0};
    // GameObject UI_PlayeScene;
    private float _speed =5;
    private bool isPaused = false;
    private bool hasPausedOnce = false; // 멈춤 한번만 실행
    GameObject Stranger;

    // enum Buttons
    // {

    // }
    // enum Texts
    // {

    // }

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        Stranger = GameObject.Find("Stranger");
        if(Stranger == null)
            Debug.Log("Check Connect with UI_QUESTIONPOPUP(Stranger는 여기에서 생성됨)");

        Stranger.transform.position = new Vector3(-10,2,0);

        // BindButton(typeof(Buttons));
        // BindText(typeof(Texts));

        // GetButton((int)Buttons.TheWorldButton).gameObject.BindEvent(TheWorldEvent);

        // GetText((int)Texts.TheWorldText).text = "The World :" + Managers.Game.TheWorld.ToString();

        
        return true;
    }
    public void FixedUpdate()
    {
       if (Stranger != null && _speed > 0.01) 
        {
            Stranger.transform.position += new Vector3(_speed * Time.deltaTime, 0, 0);

            //중간에 잠시 멈추기
        if (Stranger.transform.position.x >= 0 && !hasPausedOnce)
        {
            StartCoroutine(PauseMovement(3f));
            hasPausedOnce = true;
        }

            
            if(Stranger.transform.position.x >= 13)
            {
                Debug.Log("stranger Passed");
                Managers.Sound.Play(Sound.Effect, "Sound_Question");
                Managers.UI.ClosePopupUI(this);
                Managers.UI.ShowPopupUI<UI_GuessPopup>();

                // Managers.Resource.Destroy(Stranger);
        }
        }
    }

    // void TheWorldEvent()
    // {
    //     if(Managers.Game.TheWorld <= 0)
    //     {
    //         Debug.Log("NO Item");
    //     }
    //     else
    //     {
    //         _speed = 0;
    //         Managers.Game.TheWorld--;
    //         GetText((int)Texts.TheWorldText).text = "The World :" + Managers.Game.TheWorld.ToString();
    //         StartCoroutine(TheWorldEffect());
    //     }
    // }

    // IEnumerator TheWorldEffect()
    // {
    //     yield return new WaitForSeconds(3);

    //     _speed = 5;
    // }
    private IEnumerator PauseMovement(float duration)
    {
        if (isPaused) yield break;  // 이미 실행 중이면 종료

        isPaused = true;
        float originalSpeed = _speed;
        _speed = 0;  // 정지
        yield return new WaitForSeconds(duration);
        _speed = originalSpeed;  // 다시 움직이기
        isPaused = false;
    }
}
