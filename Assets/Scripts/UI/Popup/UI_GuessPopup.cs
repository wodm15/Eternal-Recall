using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GuessPopup : UI_Popup
{
    public bool isCorrect = true;
    enum Texts
    {
        ConfirmButtonText,
        Question,
    }
    enum Buttons
    {
        ConfirmButton,
        HairMinus,
        HairPlus,
        ClothesMinus,
        ClothesPlus,
        EyebrowMinus,
        EyebrowPlus,
        EyeMinus,
        EyePlus,
        MouthMinus,
        MouthPlus,
        EmotionMinus,
        EmotionPlus,
        AnimationMinus,
        AnimtionPlus,
    }

    enum Images
    {
        Correct,
        Wrong,
    }
    
    GameObject GuessPlayer;
    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        GuessPlayer = Managers.Resource.Instantiate("Player");
        GuessPlayer.transform.position = new Vector3(-2,0,0);
        GuessPlayer.transform.localScale = new Vector3(1,1,1);

        GameObject _customManager = GameObject.FindGameObjectWithTag("GuessManager");
        CustomManager customManager = _customManager.GetComponent<CustomManager>();
        AnimationManager animationManager = _customManager.GetComponent<AnimationManager>();
        if(_customManager == null || animationManager == null || customManager == null)
            Debug.LogError("custommanger null");

		BindText(typeof(Texts));
		BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetText((int)Texts.Question).text = Managers.GetText(Define.Question1);

        //정답일 때
        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(() => OnClickConfirmButton(true));

        GetImage((int)Images.Correct).gameObject.SetActive(false);
        GetImage((int)Images.Wrong).gameObject.SetActive(false);

        //추측하는 버튼 바인딩
        #region 추측플레이어 바인딩
        GetButton((int)Buttons.HairMinus).gameObject.BindEvent(() => 
        {
            customManager.hair--; 
            customManager.numberCheck(0);  
        });
        GetButton((int)Buttons.HairPlus).gameObject.BindEvent(() => 
        {
            customManager.hair++; 
            customManager.numberCheck(0);  
        });

        GetButton((int)Buttons.ClothesMinus).gameObject.BindEvent(() => 
        {
            customManager.clothes--; 
            customManager.numberCheck(1);  
        });
        GetButton((int)Buttons.ClothesPlus).gameObject.BindEvent(() => 
        {
            customManager.clothes++; 
            customManager.numberCheck(1);  
        });

        GetButton((int)Buttons.EyebrowMinus).gameObject.BindEvent(() => 
        {
            customManager.eyebrow--; 
            customManager.numberCheck(2);  
        });
        GetButton((int)Buttons.EyebrowPlus).gameObject.BindEvent(() => 
        {
            customManager.eyebrow++; 
            customManager.numberCheck(2);  
        });

        GetButton((int)Buttons.EyeMinus).gameObject.BindEvent(() => 
        {
            customManager.eye--; 
            customManager.numberCheck(3);  
        });
        GetButton((int)Buttons.EyePlus).gameObject.BindEvent(() => 
        {
            customManager.eye++; 
            customManager.numberCheck(3);  
        });

        GetButton((int)Buttons.MouthMinus).gameObject.BindEvent(() => 
        {
            customManager.mouth--; 
            customManager.numberCheck(4);  
        });
        GetButton((int)Buttons.MouthPlus).gameObject.BindEvent(() => 
        {
            customManager.mouth++; 
            customManager.numberCheck(4);  
        });

        GetButton((int)Buttons.EmotionMinus).gameObject.BindEvent(() => 
        {
            customManager.emotion--; 
            customManager.numberCheck(5);  
        });
        GetButton((int)Buttons.EmotionPlus).gameObject.BindEvent(() => 
        {
            customManager.emotion++; 
            customManager.numberCheck(5);  
        });

        GetButton((int)Buttons.AnimationMinus).gameObject.BindEvent(() => 
        {
            animationManager.PlayAni(false);
        });
        GetButton((int)Buttons.AnimtionPlus).gameObject.BindEvent(() => 
        {
            animationManager.PlayAni(true);
        });
        
        
        
        #endregion


        return true;
    }

    void OnClickConfirmButton(bool isCorrect)
    {
        if (isCorrect)
        {
            Debug.Log("정답: Correct");
            GetImage((int)Images.Correct).gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("오답: Wrong");
            GetImage((int)Images.Wrong).gameObject.SetActive(true);
        }
        
        Invoke("HideResultAndProceed", 2f);
    }

    void HideResultAndProceed()
    {
        // 이미지 비활성화
        GetImage((int)Images.Correct).gameObject.SetActive(false);
        GetImage((int)Images.Wrong).gameObject.SetActive(false);

        // 후속 작업 실행
        Managers.UI.ClosePopupUI(this);
        if (GuessPlayer != null)
        {
            Managers.Resource.Destroy(GuessPlayer);
        }
        Managers.UI.ShowPopupUI<UI_GetItemPopup>();
    }

}
