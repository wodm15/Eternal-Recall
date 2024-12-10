using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class UI_CollectionPopup : UI_Popup
{
    List<int> Usercollections = new List<int>();
    List<CollectionData> CollectionData = new List<CollectionData>();

    // 마지막으로 클릭된 이미지 확인용
    private Image lastSelectedImage = null;
    enum GameObjects
	{
		Content
	}
    enum Texts
    {
        CollectionTitle,
        HowToText,
        DesToText,
        ExitButtonText,
    }
    enum Buttons
    {
        ExitButton,
    }
    enum Images
    {
        HowToImage,
        DesToImage,
    }




    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickBackButton); 
        GetText((int)Texts.CollectionTitle).text = Managers.GetText(Define.CollectionTitle);
        GetText((int)Texts.ExitButtonText).text = Managers.GetText(Define.GoToTitleText);
        GetText((int)Texts.HowToText).gameObject.SetActive(false);
        GetText((int)Texts.DesToText).gameObject.SetActive(false);
        GetImage((int)Images.HowToImage).gameObject.SetActive(false);
        GetImage((int)Images.DesToImage).gameObject.SetActive(false);


        GetData(); //유저 데이터와 컬렉션 데이터 가져오기
        MakeImage(GetObject((int)GameObjects.Content)); //화면에 컬렉션 이미지 생성

        //첫번째 그림 클릭한 거로 일단 하기
        if (CollectionData.Count > 0)
        {
            OnImageClick(2); 
        }
        return true;

    }

    public void GetData()
    {
        //사용자 컬렉션 깬지 확인
        foreach (int ZeroOrTwo in Managers.Game.Collections)
        {
            Usercollections.Add(ZeroOrTwo);
        }

        //데이터 정보 가져오기
        foreach (CollectionData data in Managers.Data.Collections.Values)
        {
            CollectionData.Add(data);
        }

    }

    public void MakeImage(GameObject content)
    {
        for (int i = 0; i < CollectionData.Count; i++)
        {
            string path = $"{CollectionData[i].iconPath}";
            Sprite sprite = Managers.Resource.Load<Sprite>(path);

            GameObject item = new GameObject($"CollectionItem_{i}");


            Image image = item.AddComponent<Image>();
            image.sprite = sprite; 

            Button button = item.AddComponent<Button>(); 
            int index = i; // Lambda용
            button.onClick.AddListener(() => OnImageClick(index));

            item.transform.SetParent(content.transform, false); // 로컬 변환을 유지

            // Usercollections 컬렉션을 얻지 못한 경우
            if (Usercollections[i] == 0)
            {
                Color color = image.color;
                color.a = 0.5f; 
                image.color = color;
            }
            else if (Usercollections[i] == 2)
            {
                Color color = image.color;
                color.a = 1f;
                image.color = color;
            }

            // Debug.Log(CollectionData[i].iconPath); 
        }
    }

    // 이미지를 클릭했을 때 실행될 함수
    public void OnImageClick(int index)
    {
        GetText((int)Texts.HowToText).gameObject.SetActive(true);
        GetText((int)Texts.DesToText).gameObject.SetActive(true);

        GetImage((int)Images.HowToImage).gameObject.SetActive(true);
        GetImage((int)Images.DesToImage).gameObject.SetActive(true);

        GetText((int)Texts.HowToText).text = CollectionData[index].How;
        GetText((int)Texts.DesToText).text = CollectionData[index].description;

        HighlightSelectedImage(index);
    }

    // 클릭한 이미지를 강조 표시
    private void HighlightSelectedImage(int index)
    {
        // 마지막으로 클릭된 이미지 원래 상태 복원
        if (lastSelectedImage != null)
        {
            Outline border = lastSelectedImage.GetComponent<Outline>();
            if (border != null)
            {
                Destroy(border);
            }
        }

        // 새로운 이미지 선택 시 강조 표시
        GameObject selectedItem = GetObject((int)GameObjects.Content).transform.GetChild(index).gameObject;
        Image selectedImage = selectedItem.GetComponent<Image>();

        // 테두리 추가
        Outline outline = selectedImage.gameObject.AddComponent<Outline>();
        outline.effectColor = Color.black; 
        outline.effectDistance = new Vector2(6, 6); 

        // 마지막 이미지 기록
        lastSelectedImage = selectedImage;
    }


    public void OnClickBackButton()
    {
        Managers.UI.ClosePopupUI(this);
        GameObject Player = GameObject.Find("StaticPlayer");
        Player.transform.position = new Vector3(0,-1,0);
    }

}


