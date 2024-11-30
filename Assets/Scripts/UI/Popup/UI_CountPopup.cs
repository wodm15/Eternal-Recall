using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_CountPopup : UI_Popup
{
    enum Images
    {
        CountDown1,
        CountDown2,
        CountDown3,
    }
    public override bool Init()
    {
        if (base.Init() == false)
			return false;

        BindImage(typeof(Images));

        GetImage((int)Images.CountDown3).gameObject.SetActive(false);
        GetImage((int)Images.CountDown2).gameObject.SetActive(false);
        GetImage((int)Images.CountDown1).gameObject.SetActive(false);

        StartCoroutine(StartCountDown());
       
        return true;
    }

 private IEnumerator StartCountDown()
    {
        yield return ShowImage((int)Images.CountDown3);
        yield return ShowImage((int)Images.CountDown2);
        yield return ShowImage((int)Images.CountDown1);

        yield return new WaitForSeconds(0.5f); 
        Managers.UI.ClosePopupUI(this); 
        Managers.UI.ShowPopupUI<UI_QuestionPopup>(); 
        // Managers.UI.ShowSceneUI<UI_PlayerScene>(); //
    }

    private IEnumerator ShowImage(int imageIndex)
    {
        GetImage(imageIndex).gameObject.SetActive(true);
        Managers.Sound.Play(Sound.Effect, "Sound_Checkbutton");
        yield return new WaitForSeconds(1f); // 1초 동안 이미지 표시
        GetImage(imageIndex).gameObject.SetActive(false);
    }
}
