using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_CountPopup : UI_Popup
{
    UI_PlayerScene playerScene;
    private string _description;
    private string _productId;
    private float _effectValue1;

    enum Images
    {
        CountDown1,
        CountDown2,
        // CountDown3,
    }

    enum Texts
    {
        PreviousStage,
        ShowStage,
        AmountText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetText((int)Texts.PreviousStage).gameObject.SetActive(false);
        GetText((int)Texts.ShowStage).gameObject.SetActive(false);
        // GetImage((int)Images.CountDown3).gameObject.SetActive(false);
        GetImage((int)Images.CountDown2).gameObject.SetActive(false);
        GetImage((int)Images.CountDown1).gameObject.SetActive(false);

        int _Stage = Managers.Game.Stage;

        playerScene = Managers.UI.GetSceneUI<UI_PlayerScene>();

        GetText((int)Texts.PreviousStage).text = $" Stage : {_Stage -1}";
        GetText((int)Texts.ShowStage).text = $" Stage : {_Stage}";
        if(_description !=null)
        {
            GetText((int)Texts.AmountText).gameObject.SetActive(true);
            string amountText = $"{_description}";

            if (_effectValue1 != -1)
                GetText((int)Texts.AmountText).text = $"{_description} \n 힐링:{_effectValue1}";
            else
                GetText((int)Texts.AmountText).text = $"{_description}";
        }
        

        StartCoroutine(StartCountDown());
       
        return true;
    }
        // AmountText 값을 설정하는 메서드
public void SetAmountText(ShopData _selectedItem)
{
    _description = _selectedItem.description;
    _productId = _selectedItem.productID;
    _effectValue1 = _selectedItem.effectValues[0];

    //gambleHealing 일 때만 양도 가져가기
    if (_selectedItem.productID == "gambleHealing")
    {
        _effectValue1 = _selectedItem.effectValues[0];
    }
    else
    {
        _effectValue1 = -1;
    }

    // if (_selectedItem.effectValues.Count > 2 )
    // {
    //     _effectValue3 = _selectedItem.effectValues[2];
    // }

}


 private IEnumerator StartCountDown()
    {
        if(Managers.Game.Stage != 0)
        {
            yield return new WaitForSeconds(2);
            GetText((int)Texts.AmountText).gameObject.SetActive(false);
            yield return ShowText((int)Texts.PreviousStage);
        }
        else
        {
            yield return ShowText((int)Texts.PreviousStage);
        }
            

        GetText((int)Texts.AmountText).gameObject.SetActive(false);

        yield return ShowText((int)Texts.ShowStage);

        // yield return ShowImage((int)Images.CountDown3);
        yield return ShowImage((int)Images.CountDown2);
        yield return ShowImage((int)Images.CountDown1);

        yield return new WaitForSeconds(0.5f); 
        Managers.UI.ClosePopupUI(this); 
        Managers.UI.ShowPopupUI<UI_QuestionPopup>(); 
    }

    private IEnumerator ShowText(int textIndex)
    {
        GetText(textIndex).gameObject.SetActive(true);
        if(textIndex == 1)
            Managers.Sound.Play(Sound.Effect, "Sound_StageUp");
        yield return new WaitForSeconds(1f);
        GetText(textIndex).gameObject.SetActive(false);
    }

    private IEnumerator ShowImage(int imageIndex)
    {
        GetImage(imageIndex).gameObject.SetActive(true);
        Managers.Sound.Play(Sound.Effect, "Sound_Checkbutton");
        yield return new WaitForSeconds(1f); // 1초 동안 이미지 표시
        GetImage(imageIndex).gameObject.SetActive(false);
    }
}
