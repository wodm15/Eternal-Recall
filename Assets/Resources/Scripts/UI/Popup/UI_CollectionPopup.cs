using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class UI_CollectionPopup : UI_Popup
{
    List<int> Usercollections = new List<int>();
    List<CollectionData> CollectionData = new List<CollectionData>();
    
    //선택된 변수 마지막에 가지고 있는 인덱스면 타이틀화면에 가져가고 아니면 초기화
    private int selectedIndex = -1;
    CustomManager customManager;
    

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
        ClothesEffectText,
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
        GetText((int)Texts.ClothesEffectText).gameObject.SetActive(false);
        GetImage((int)Images.HowToImage).gameObject.SetActive(false);
        GetImage((int)Images.DesToImage).gameObject.SetActive(false);

        GameObject Player = GameObject.Find("StaticPlayer");
        Player.transform.position = new Vector3(-6,-3,0);
        GameObject _customManager = GameObject.FindGameObjectWithTag("StaticManager");
        customManager = _customManager.GetComponent<CustomManager>();

        CheckFirstVisit();
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
            // 사용자 컬렉션 깬지 확인
            foreach (int ZeroOrTwo in Managers.Game.Collections)
            {
                Usercollections.Add(ZeroOrTwo);
            }

            // purchase가 0인 데이터를 추가
            List<CollectionData> purchaseOneList = new List<CollectionData>(); // purchase가 1인 데이터 저장용
            foreach (CollectionData data in Managers.Data.Collections.Values)
            {
                if (data.purchase == 0) // purchase가 0인 경우
                {
                    CollectionData.Add(data);
                }
                else if (data.purchase == 1) // purchase가 1인 경우
                {
                    purchaseOneList.Add(data);
                }
            }

            // purchase가 1인 데이터를 마지막에 추가
            foreach (CollectionData data in purchaseOneList)
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
        selectedIndex = index;
        GetText((int)Texts.HowToText).gameObject.SetActive(true);
        GetText((int)Texts.DesToText).gameObject.SetActive(true);
        GetText((int)Texts.ClothesEffectText).gameObject.SetActive(true);

        GetImage((int)Images.HowToImage).gameObject.SetActive(true);
        GetImage((int)Images.DesToImage).gameObject.SetActive(true);

        GetText((int)Texts.HowToText).text = CollectionData[index].How;
        GetText((int)Texts.DesToText).text = CollectionData[index].description;
        GetText((int)Texts.ClothesEffectText).text = $"{Managers.Data.Stat[index].Power}";

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

        //캐릭터 옷도 수정
        customManager.clothes = index;

        customManager.numberCheck(1); 

        // 마지막 이미지 기록
        lastSelectedImage = selectedImage;
    }


    public void OnClickBackButton()
    {
        Managers.UI.ClosePopupUI(this);

        //옷이 소유권이 있으면 가져가고, 없으면 빼기
        if(Usercollections[selectedIndex] == 2) 
        {
            Managers.Game.ClothesIndex = selectedIndex;
        }
        else
        {
            Managers.Game.ClothesIndex = 2;
        }
    

        customManager.clothes = Managers.Game.ClothesIndex;
        customManager.numberCheck(1); 

        GameObject Player = GameObject.Find("StaticPlayer");
        Player.transform.position = new Vector3(0,-1,0);
    }

    void CheckFirstVisit()
	{
		//처음 접속일 경우 병아리 옷 획득 후 저장
		if (Managers.Game.Collections[2] == CollectionState.None)
		{
        	Managers.Game.Collections[2] = CollectionState.Done;
		}
	}

}


