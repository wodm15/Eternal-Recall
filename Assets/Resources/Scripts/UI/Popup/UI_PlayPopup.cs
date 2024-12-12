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
    private string _birdName;


    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        Stranger = GameObject.Find("Stranger");
        if(Stranger == null)
            Debug.Log("Check Connect with UI_QUESTIONPOPUP(Stranger는 여기에서 생성됨)");

        Stranger.transform.position = new Vector3(-10,2,0);

        BirdResponse();

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

    public void BirdResponse()
    {
        // 스테이지 10 미만이면 새가 등장하지 않음
        if (Managers.Game.Stage < 10)
            return;

        float spawnChance = 0f;

        // 난이도에 따른 기본 확률 설정 (노말 1, 하드 3, 언리미티드 5)
        if (Managers.Game.DifficultyLevel == "Normal")
        {
            spawnChance = 1f; 
        }
        else if (Managers.Game.DifficultyLevel == "Hard")
        {
            spawnChance = 3f;
        }
        else if (Managers.Game.DifficultyLevel == "UnLimited")
        {
            spawnChance = 5f; 
        }

        // 스테이지에 따라 확률 증가 (5단계마다 1%씩 추가)
        int stageIncrement = (Managers.Game.Stage - 10) / 5;

        // 각 난이도별 최대 확률 설정 (노말 10, 하드 30, 언리미티드 60)
        if (Managers.Game.DifficultyLevel == "Normal")
        {
            spawnChance = Mathf.Min(spawnChance + stageIncrement * 1f, 10f);
        }
        else if (Managers.Game.DifficultyLevel == "Hard")
        {
            spawnChance = Mathf.Min(spawnChance + stageIncrement * 3f, 30f); 
        }
        else if (Managers.Game.DifficultyLevel == "UnLimited")
        {
            spawnChance = Mathf.Min(spawnChance + stageIncrement * 6f, 60f); 
        }

        // 확률에 따라 Bird 생성 여부 결정
        float randomValue = Random.Range(0f, 100f);
        
        if (randomValue < spawnChance)
        {
            // 1, 2, 3 중 무작위로 선택
            int birdIndex = Random.Range(1, 4); // 1부터 3까지 무작위 숫자 생성

            _birdName = "Bird" + birdIndex; // "Bird1", "Bird2", "Bird3" 중 하나

            // 무작위로 선택된 새 생성
            Bird = Managers.Resource.Instantiate(_birdName);
            Bird.transform.position = new Vector3(12, 2.5f, 0);
        }
        else
        {
            Debug.Log($"새 생성안됨 {spawnChance}% 랜덤 밸류: {randomValue})");
        }
    }



    public void BirdComing()
    {
        Bird = GameObject.Find(_birdName);

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
