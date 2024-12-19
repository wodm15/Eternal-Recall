using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_PlayPopup : UI_Popup
{
    public int[] playerIndex = new int[6] {0,0,0,0,0,0};
    // GameObject UI_PlayeScene;
    private float _speed =Define.GuessPlayerSpeed;
    
    private bool isPaused = false;
    private bool hasPausedOnce = false; // 멈춤 한번만 실행
    GameObject Stranger;
    GameObject Bird;

    enum Images
    {
        BG,
    }


    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        BindImage(typeof(Images));

        //배경 변경
        Managers.Game.changeBG();
        GetImage((int)Images.BG).sprite = Managers.Resource.Load<Sprite>($"Sprites/Background/{Managers.Game.BG}");

        Stranger = GameObject.Find("Stranger");
        if(Stranger == null)
            Debug.Log("Check Connect with UI_QUESTIONPOPUP(Stranger는 여기에서 생성됨)");

        Stranger.transform.position = new Vector3(-12,2,0);


        BirdComing();

        return true;
    }
    
    public void FixedUpdate()
    {
        BirdComing();

       if (Stranger != null && _speed > 0.01) 
        {
            Stranger.transform.position += new Vector3(_speed * Time.deltaTime, 0, 0);

            //중간에 잠시 멈추기
        if (Stranger.transform.position.x >= 0 && !hasPausedOnce)
        {
            //난이도 별
            if(Managers.Game.DifficultyLevel == "Normal")
                StartCoroutine(PauseMovement(3f));
            else if(Managers.Game.DifficultyLevel == "Hard")
                StartCoroutine(PauseMovement(2f));
            else if(Managers.Game.DifficultyLevel == "UnLimited")
                StartCoroutine(PauseMovement(1f));

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


    public void BirdComing()
    {
        
        if (GameObject.Find("Bird1") != null)
        {
            Bird = GameObject.Find("Bird1");
        }
        else if (GameObject.Find("Bird2") != null)
        {
            Bird = GameObject.Find("Bird2");
        }
        else if (GameObject.Find("Bird3") != null)
        {
            Bird = GameObject.Find("Bird3");
        }


        if (Bird != null)
        {        
            if (Managers.Game.DifficultyLevel == "UnLimited")
                Bird.transform.localScale = new Vector3(1, 1, 1);
            // Bird의 위치를 이동
            Bird.transform.position -= new Vector3(Define.BirdSpeed * Time.deltaTime, 0, 0);
            if (Bird.transform.position.x <= -11f)
            {
                Debug.Log("새가 제거됨");
                Destroy(Bird);  
            }
        }
    }
}
