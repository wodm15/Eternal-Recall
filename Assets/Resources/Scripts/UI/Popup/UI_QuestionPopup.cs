using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestionPopup : UI_Popup
{   
    GameObject Stranger;
    
    enum Texts
    {
        Question,
    }

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        BindText(typeof(Texts));

        Stranger = Managers.Resource.Instantiate("Stranger");
        RandomStranger();
        Stranger.transform.position = new Vector3(-11,2,0);

        Managers.Game.quizData = Managers.Game.LoadRandomQuiz();
        GetText((int)Texts.Question).text = $"{Managers.Game.quizData.kor}";
        StartCoroutine(CloseAndShowPopupAfterDelay(5f));
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
            animationManager.ani = Random.Range(0, animationManager.aniName.Length);

            Managers.Game.StrangerIndex[0] = Random.Range(0, 20);
            Managers.Game.StrangerIndex[1] = Random.Range(0, 20);
            Managers.Game.StrangerIndex[2] = Random.Range(0, 5);
            Managers.Game.StrangerIndex[3] = Random.Range(0, 20);
            Managers.Game.StrangerIndex[4] = Random.Range(0, 24);
            Managers.Game.StrangerIndex[5] = Random.Range(0, 6);
            Managers.Game.StrangerIndex[6] = animationManager.ani;

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
}