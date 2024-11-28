using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GuessPopup : UI_Popup
{
    enum Texts
    {
        ConfirmButtonText,
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
    GameObject GuessPlayer;
    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        GuessPlayer = Managers.Resource.Instantiate("Player");
        GuessPlayer.transform.position = new Vector3(-2,0,0);
        GuessPlayer.transform.localScale = new Vector3(1,1,1);

        GameObject _customManager = GameObject.FindGameObjectWithTag("CustomManager");
        CustomManager customManager =_customManager.GetComponent<CustomManager>();
        AnimationManager animationManager = _customManager.GetComponent<AnimationManager>();

		BindText(typeof(Texts));
		BindButton(typeof(Buttons));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

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

    void OnClickConfirmButton()
    {
        Debug.Log("Guess ClickConfirmButton");

        Managers.UI.ClosePopupUI(this);
        Managers.Destroy(GuessPlayer);
        Managers.UI.ShowPopupUI<UI_GetItemPopup>(); //
    }

    void OnClickHairButton()
    {
        GameObject hairObject = GameObject.FindGameObjectWithTag("Player");
        hairObject.GetComponent<CustomManager>().hair = Random.Range(0, 20);
    }
    
}
