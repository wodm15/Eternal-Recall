using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.UI;

public class UI_CollectionPopup : UI_Popup
{
    List<int> Usercollections = new List<int>();
    List<CollectionData> CollectionData = new List<CollectionData>();
    
    //선택된 변수 마지막에 가지고 있는 인덱스면 타이틀화면에 가져가고 아니면 초기화
    private int nameidIndex = -1;
    private int idIndex = 0;
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
        Text1,
        Text2,
        Text3,
        Text4,
        Text5,
        Text6,
        Text7,
        Text8,
        Text9,
        Text10,
        Text11,
        Text12,
        Text13,
        Text14,
        Text15,
        Text16,
        Text17,
        Text18,
        Text19,
        Text20,
    }
    enum Buttons
    {
        ExitButton,
        Button1,
        Button2,
        Button3,
        Button4,
        Button5,
        Button6,
        Button7,
        Button8,
        Button9,
        Button10,
        Button11,
        Button12,
        Button13,
        Button14,
        Button15,
        Button16,
        Button17,
        Button18,
        Button19,
        Button20,
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

//    병아리 옷 1, 코트 2, 청바지 3,전투복 4,운동복 5, 수영복2 6,웨딩드레스 7,한복 8,유카타 9,공주옷 10,잠옷 11
// ,마법소녀 12 ,비키니 13,메이드 14,세일러복 15,수영복 16,치파오 17,가터벨트 18,교복 19,간호사 20

        GetData(); //유저 데이터와 컬렉션 데이터 가져오기

        //이름 매핑
        Dictionary<int, Texts> textMapping= new Dictionary<int, Texts>
        {
            { 1, Texts.Text1 },
            { 2, Texts.Text2 },
            { 3, Texts.Text3 },
            { 4, Texts.Text4 },
            { 5, Texts.Text5 },
            { 6, Texts.Text6 },
            { 7, Texts.Text7 },
            { 8, Texts.Text8 },
            { 9, Texts.Text9 },
            { 10, Texts.Text10 },
            { 11, Texts.Text11 },
            { 12, Texts.Text12 },
            { 13, Texts.Text13 },
            { 14, Texts.Text14 },
            { 15, Texts.Text15 },
            { 16, Texts.Text16 },
            { 17, Texts.Text17 },
            { 18, Texts.Text18 },
            { 19, Texts.Text19 },
            { 20, Texts.Text20 }
        };

        foreach (CollectionData data in CollectionData)
        {
            if (textMapping.ContainsKey(data.nameID))
            {
                GetText((int)textMapping[data.nameID]).text = data.name;
            }
        }

        Dictionary<int, Button> buttonMapping = new Dictionary<int, Button>
            {
                { 1, GetButton((int)Buttons.Button1) },
                { 2, GetButton((int)Buttons.Button2) },
                { 3, GetButton((int)Buttons.Button3) },
                { 4, GetButton((int)Buttons.Button4) },
                { 5, GetButton((int)Buttons.Button5) },
                { 6, GetButton((int)Buttons.Button6) },
                { 7, GetButton((int)Buttons.Button7) },
                { 8, GetButton((int)Buttons.Button8) },
                { 9, GetButton((int)Buttons.Button9) },
                { 10, GetButton((int)Buttons.Button10) },
                { 11, GetButton((int)Buttons.Button11) },
                { 12, GetButton((int)Buttons.Button12) },
                { 13, GetButton((int)Buttons.Button13) },
                { 14, GetButton((int)Buttons.Button14) },
                { 15, GetButton((int)Buttons.Button15) },
                { 16, GetButton((int)Buttons.Button16) },
                { 17, GetButton((int)Buttons.Button17) },
                { 18, GetButton((int)Buttons.Button18) },
                { 19, GetButton((int)Buttons.Button19) },
                { 20, GetButton((int)Buttons.Button20) }
            };

        foreach (KeyValuePair<int, Button> kvp in buttonMapping)
            {
                int nameID = kvp.Key; // 버튼에 대응되는 nameID
                kvp.Value.onClick.AddListener(() => OnImageClick(nameID));
            }

        GetFilter(); //화면에 컬렉션 이미지 생성

        GameObject Player = GameObject.Find("StaticPlayer");
        Player.transform.position = new Vector3(-6,-3,0);
        GameObject _customManager = GameObject.FindGameObjectWithTag("StaticManager");
        customManager = _customManager.GetComponent<CustomManager>();

        CheckFirstVisit();

        //첫번째 그림 클릭한 거로 일단 하기
        if (CollectionData.Count > 0)
        {
            OnImageClick(1); 
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

            foreach (CollectionData data in Managers.Data.Collections.Values)
            {
                CollectionData.Add(data);        
            }
        }

    // 이미지를 클릭했을 때 실행될 함수
    public void OnImageClick(int nameID)
    {
        nameidIndex = nameID;
        CollectionData selectedCollectionData = CollectionData.Find(data => data.nameID == nameID);
        idIndex = selectedCollectionData.ID;
        GetText((int)Texts.HowToText).gameObject.SetActive(true);
        GetText((int)Texts.DesToText).gameObject.SetActive(true);
        GetText((int)Texts.ClothesEffectText).gameObject.SetActive(true);

        GetImage((int)Images.HowToImage).gameObject.SetActive(true);
        GetImage((int)Images.DesToImage).gameObject.SetActive(true);

        GetText((int)Texts.HowToText).text = CollectionData[idIndex].How;
        GetText((int)Texts.DesToText).text = CollectionData[idIndex].description;
        GetText((int)Texts.ClothesEffectText).text = $"{Managers.Data.Stat[idIndex].Power}";

        HighlightSelectedImage(idIndex , nameidIndex);
    }

    // 클릭한 이미지를 강조 표시
    private void HighlightSelectedImage(int idIndex ,int nameidIndex )
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
        GameObject selectedItem = GetObject((int)GameObjects.Content).transform.GetChild(nameidIndex-1).gameObject;
        Image selectedImage = selectedItem.GetComponent<Image>();

        // 테두리 추가
        Outline outline = selectedImage.gameObject.AddComponent<Outline>();
        outline.effectColor = Color.black; 
        outline.effectDistance = new Vector2(6, 6); 

        //캐릭터 옷도 수정
        customManager.clothes = idIndex;

        customManager.numberCheck(1); 

        // 마지막 이미지 기록
        lastSelectedImage = selectedImage;
    }

    public void OnClickBackButton()
    {
        Managers.UI.ClosePopupUI(this);

        //옷이 소유권이 있으면 가져가고, 없으면 빼기
        if(Usercollections[idIndex] == 2) 
        {
            Managers.Game.ClothesIndex = idIndex;
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

    //업적 못한 옷은 필터시키기
    public void GetFilter()
    {
        // CollectionData를 ID 기반 Dictionary로 캐싱
        Dictionary<int, CollectionData> collectionDataDict = new Dictionary<int, CollectionData>();
        foreach (CollectionData data in CollectionData)
        {
            collectionDataDict[data.ID] = data;
        }

        // 버튼과 텍스트 오브젝트를 캐싱
        Dictionary<int, Button> buttonCache = new Dictionary<int, Button>();
        Dictionary<int, TMP_Text> textCache = new Dictionary<int, TMP_Text>();

        foreach (CollectionData data in CollectionData)
        {
            string buttonName = $"Button{data.nameID}";
            string textName = $"Text{data.nameID}";

            GameObject buttonObject = GameObject.Find(buttonName);
            GameObject textObject = GameObject.Find(textName);

            if (buttonObject != null)
            {
                Button button = buttonObject.GetComponent<Button>();
                if (button != null)
                {
                    buttonCache[data.ID] = button;
                }
            }

            if (textObject != null)
            {
                TMP_Text text = textObject.GetComponent<TMP_Text>();
                if (text != null)
                {
                    textCache[data.ID] = text;
                }
            }
        }

        // Usercollections를 기반으로 UI 업데이트
        for (int i = 0; i < Usercollections.Count; i++)
        {
            if (Usercollections[i] == 0 && collectionDataDict.ContainsKey(i))
            {
                CollectionData data = collectionDataDict[i];
                if (buttonCache.ContainsKey(i) && textCache.ContainsKey(i))
                {
                    Button button = buttonCache[i];
                    TMP_Text text = textCache[i];

                    // 버튼 및 텍스트 설정
                    button.image.sprite = Managers.Resource.Load<Sprite>("Sprites/ItemIcon/GetExpendTime");
                    text.color = Color.white;
                }
                else
                {
                    Debug.LogWarning($"Button or Text for ID {i} not found.");
                }
            }
        }
    }

}


