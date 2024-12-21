using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestionPopup : UI_Popup
{   
    GameObject Stranger;
    GameObject Bird;
    private string _birdName;
    
    enum Texts
    {
        Question,
    }

    enum Images
    {
        BG,
        birdSoundImage,
    }

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        Stranger = Managers.Resource.Instantiate("Stranger");
        RandomStranger();
        Stranger.transform.position = new Vector3(-12,2,0);

        Managers.Game.quizData = Managers.Game.LoadRandomQuiz();


        //배경 변경
        if(Managers.Game.Stage == 1 )
            Managers.Game.changeBG();

        GetImage((int)Images.BG).sprite = Managers.Resource.Load<Sprite>($"Sprites/Background/{Managers.Game.BG}");
        
        if(Managers.Game.DifficultyLevel == "Nightmare")
        {
            GetText((int)Texts.Question).text = $"{Managers.Game.quizData.eng}";
        }
        else
            GetText((int)Texts.Question).text = $"{Managers.Game.quizData.kor}";
        GetImage((int)Images.birdSoundImage).gameObject.SetActive(false);

        StartCoroutine(CloseAndShowPopupAfterDelay(5f));

        if(BirdResponse())
        {
            GetImage((int)Images.birdSoundImage).gameObject.SetActive(true);
            Managers.Sound.Play(Define.Sound.Effect , "Sound_Bird");
        }

    return true;

}

    private IEnumerator CloseAndShowPopupAfterDelay(float delay)
    {
        // 5초 대기
        yield return new WaitForSeconds(delay);

        // 팝업을 닫고 새로운 팝업을 열기
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_PlayPopup>();
    }

    public void RandomStranger()
    {
        if(Stranger != null)
        {
            GameObject customManager = GameObject.FindGameObjectWithTag("StrangerManager");
            //스크립트 접근
            CustomManager _customManager = customManager.GetComponent<CustomManager>();


            //애니메이션 랜덤 변경
            AnimationManager animationManager = customManager.GetComponent<AnimationManager>();
            // animationManager.ani = Random.Range(0, animationManager.aniName.Length); 10개 있음

            if(Managers.Game.DifficultyLevel == "Normal")
            {
            Managers.Game.StrangerIndex[0] = Random.Range(0, Define.NormalIndex); //헤어
            Managers.Game.StrangerIndex[1] = Random.Range(0, Define.NormalIndex); //옷
            Managers.Game.StrangerIndex[2] = Random.Range(0, Define.NormalIndex);  //눈썹
            Managers.Game.StrangerIndex[3] = Random.Range(0, Define.NormalIndex); //눈
            Managers.Game.StrangerIndex[4] = Random.Range(0, Define.NormalIndex); //입
            Managers.Game.StrangerIndex[5] = Random.Range(0, Define.NormalIndex);  //감정

            // 옷이 1, 7 , 9 , 13 인덱스일 경우 다리꼬는게 안보이기 때문에 0과 1은 무조건 패스하도록 추가
            int outfitIndex = Managers.Game.StrangerIndex[1];
            do
            {
                animationManager.ani = Random.Range(0, Define.NormalIndex);
            }
            while ((outfitIndex == 1 || outfitIndex == 7 || outfitIndex == 9 || outfitIndex == 13) 
                && (animationManager.ani == 0 || animationManager.ani == 1));

            Managers.Game.StrangerIndex[6] = animationManager.ani; // 애니메이션
            }

            
            else if (Managers.Game.DifficultyLevel == "Hard")
            {
            
            Managers.Game.StrangerIndex[0] = Random.Range(0, Define.HardIndex); //헤어
            Managers.Game.StrangerIndex[1] = Random.Range(0, Define.HardIndex); //옷
            Managers.Game.StrangerIndex[2] = Random.Range(0, Define.MaxIndexEyebrow);  //눈썹 5개밖에 없음 
            Managers.Game.StrangerIndex[3] = Random.Range(0, Define.HardIndex); //눈
            Managers.Game.StrangerIndex[4] = Random.Range(0, Define.HardIndex); //입
            Managers.Game.StrangerIndex[5] = Random.Range(0, Define.MaxIndexEmotion);  //감정 6개밖에 없음

            // 옷이 1, 7 , 9 , 13 인덱스일 경우 다리꼬는게 안보이기 때문에 0과 1은 무조건 패스하도록 추가
            int outfitIndex = Managers.Game.StrangerIndex[1];
            do
            {
                animationManager.ani = Random.Range(0, Define.MaxAnimationIndex); 
            }
            while ((outfitIndex == 1 || outfitIndex == 7 || outfitIndex == 9 || outfitIndex == 13) 
                && (animationManager.ani == 0 || animationManager.ani == 1));

            Managers.Game.StrangerIndex[6] = animationManager.ani; // 애니메이션
            }


            else if (Managers.Game.DifficultyLevel == "UnLimited" || Managers.Game.DifficultyLevel == "Nightmare")
            {
            Managers.Game.StrangerIndex[0] = Random.Range(0, Define.UnlimitedIndex); //헤어
            Managers.Game.StrangerIndex[1] = Random.Range(0, Define.UnlimitedIndex); //옷
            Managers.Game.StrangerIndex[2] = Random.Range(0, Define.MaxIndexEyebrow);  //눈썹
            Managers.Game.StrangerIndex[3] = Random.Range(0, Define.UnlimitedIndex); //눈
            Managers.Game.StrangerIndex[4] = Random.Range(0, Define.UnlimitedIndex); //입 24 -> 20개로
            Managers.Game.StrangerIndex[5] = Random.Range(0, Define.MaxIndexEmotion);  //감정
            Managers.Game.StrangerIndex[6] = animationManager.ani; //애니메이션

            // 옷이 1, 7 , 9 , 13 인덱스일 경우 다리꼬는게 안보이기 때문에 0과 1은 무조건 패스하도록 추가
            int outfitIndex = Managers.Game.StrangerIndex[1];
            do
            {
                animationManager.ani = Random.Range(0, Define.MaxAnimationIndex); 
            }
            while ((outfitIndex == 1 || outfitIndex == 7 || outfitIndex == 9 || outfitIndex == 13) 
                && (animationManager.ani == 0 || animationManager.ani == 1));

            Managers.Game.StrangerIndex[6] = animationManager.ani; // 애니메이션
            }

            _customManager.hair = Managers.Game.StrangerIndex[0];
            _customManager.clothes = Managers.Game.StrangerIndex[1];
            _customManager.eyebrow = Managers.Game.StrangerIndex[2];
            _customManager.eye = Managers.Game.StrangerIndex[3];
            _customManager.mouth = Managers.Game.StrangerIndex[4];
            _customManager.emotion = Managers.Game.StrangerIndex[5];

        }
        else
            Debug.LogError("NO PLAYER FOUND");
    }

    public bool BirdResponse()
    {
        // 난이도에 따른 기본 확률 설정 (노말 1, 하드 3, 언리미티드 5)
        if (Managers.Game.DifficultyLevel == "Normal" && Managers.Game.Stage >= 10 && Managers.Game.Stage % 5 == 0)
        {
            Managers.Game.BirdPercent +=1;
        }
        else if (Managers.Game.DifficultyLevel == "Hard" && Managers.Game.Stage >= 1 && Managers.Game.Stage % 5 == 0)
        {
            Managers.Game.BirdPercent +=5;
        }
        else if (Managers.Game.DifficultyLevel == "UnLimited" && Managers.Game.Stage >= 1 && Managers.Game.Stage % 5 == 0)
        {
            Managers.Game.BirdPercent +=10;
        }
        else if (Managers.Game.DifficultyLevel == "Nightmare" && Managers.Game.Stage >= 1 && Managers.Game.Stage % 5 == 0)
        {
            Managers.Game.BirdPercent +=50;
        }

        //70%를 최대로
        if(Managers.Game.BirdPercent >= 80)
                Managers.Game.BirdPercent =80;


        // 확률에 따라 Bird 생성 여부 결정
        float randomValue = Random.Range(0f, 100f);
        
        if (randomValue < Managers.Game.BirdPercent)
        {
            // 1, 2, 3 중 무작위로 선택
            int birdIndex = Random.Range(1, 4); // 1부터 3까지 무작위 숫자 생성

            _birdName = "Bird" + birdIndex; // "Bird1", "Bird2", "Bird3" 중 하나

            // 무작위로 선택된 새 생성
            Bird = Managers.Resource.Instantiate(_birdName);
            Bird.transform.position = new Vector3(12, 2.5f, 0);
            return true;
        }
        else
        {
            Debug.Log($"새 생성안됨 {Managers.Game.BirdPercent} 랜덤 밸류: {randomValue})");
            return false;
        }
    }
}
