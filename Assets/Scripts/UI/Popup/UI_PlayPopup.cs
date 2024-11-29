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


        Stranger = Managers.Resource.Instantiate("Stranger");
        RandomStranger();
        Stranger.transform.position = new Vector3(-10,2,0);

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

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
            GetText((int)Texts.TheWorldText).text = "The World :" + Managers.Game.TheWorld.ToString();
            StartCoroutine(TheWorldEffect());
        }
    }

    IEnumerator TheWorldEffect()
    {
        yield return new WaitForSeconds(3);

        _speed = 5;
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
            

            GameObject hair = GameObject.FindGameObjectWithTag("Hair");
            GameObject clothes = GameObject.FindGameObjectWithTag("Clothes");
            GameObject eyebrow = GameObject.FindGameObjectWithTag("Eyebrow");
            GameObject eye = GameObject.FindGameObjectWithTag("Eye");
            GameObject mouth = GameObject.FindGameObjectWithTag("Mouth");
            GameObject emotion = GameObject.FindGameObjectWithTag("Emotion");
            
            int hairLength = hair.transform.childCount;
            // int clothesLength = clothes.transform.childCount;
            int clothesLength = 19;
            int eyebrowLength = eyebrow.transform.childCount;
            int eyeLength = eye.transform.childCount;
            int mouthLength = mouth.transform.childCount;
            int emotionLength = 5;

            Managers.Game.StrangerIndex[0] = Random.Range(0, hairLength);
            Managers.Game.StrangerIndex[1] = Random.Range(0, clothesLength);
            Managers.Game.StrangerIndex[2] = Random.Range(0, eyebrowLength);
            Managers.Game.StrangerIndex[3] = Random.Range(0, eyeLength);
            Managers.Game.StrangerIndex[4] = Random.Range(0, mouthLength);
            Managers.Game.StrangerIndex[5] = Random.Range(0, emotionLength);
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
