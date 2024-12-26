using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UI_ShopPopup : UI_Popup
{

    CustomManager customManager;
    List<int> Usercollections = new List<int>();
    List<CollectionData> CollectionData = new List<CollectionData>();

    private int nameidIndex = -1;
    private int idIndex = 0;
    enum Buttons
    {
        Button16, //수영복
        Button17, //치파오
        Button18, //가터벨트
        Button19, //교복
        Button20, //간호복
        Button21, //돈 교환
        ExitButton,
    }
    enum Texts
    {
        Text16,
        Text17,
        Text18,
        Text19,
        Text20,
        Text21,
        ExitButtonText,

    }
    public override bool Init()
        {
            if (base.Init() == false)
                return false;

            BindButton(typeof(Buttons));
            BindText(typeof(Texts));

            GameObject Player = GameObject.Find("StaticPlayer");
            GameObject _customManager = GameObject.FindGameObjectWithTag("StaticManager");
            customManager = _customManager.GetComponent<CustomManager>();

            GetData();
Dictionary<int, Texts> textMapping= new Dictionary<int, Texts>
        {
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

            GetText((int)Texts.ExitButtonText).text = Managers.GetText(Define.GoToTitleText);
            GetText((int)Texts.Text16).text = Managers.GetText(Define.PurchaseText1);
            GetText((int)Texts.Text17).text = Managers.GetText(Define.PurchaseText2);
            GetText((int)Texts.Text18).text = Managers.GetText(Define.PurchaseText3);
            GetText((int)Texts.Text19).text = Managers.GetText(Define.PurchaseText4);
            GetText((int)Texts.Text20).text = Managers.GetText(Define.PurchaseText5);
            GetText((int)Texts.Text21).text = Managers.GetText(Define.PurchaseText6);
            GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickGoBack); 

            GetButton((int)Buttons.Button16).gameObject.BindEvent(() =>
            {
                PurchaseByIndex(16,1);
            });
            GetButton((int)Buttons.Button17).gameObject.BindEvent(() =>
            {
                PurchaseByIndex(17,2);
            });
            GetButton((int)Buttons.Button18).gameObject.BindEvent(() =>
            {
                PurchaseByIndex(18,3);
            });
            GetButton((int)Buttons.Button19).gameObject.BindEvent(() =>
            {
                PurchaseByIndex(19,4);
            });
            GetButton((int)Buttons.Button20).gameObject.BindEvent(() =>
            {
                PurchaseByIndex(20,5);
            });

            GetButton((int)Buttons.Button21).gameObject.BindEvent(() =>
            {
                GetGold();
            });

            return true;
        }

    public void PurchaseByIndex(int index , int purchaseID)
    {
        string productID = "purchase" +$"{purchaseID}";  // 또는 필요한 데이터 형식으로 변환
        //이미 산 경우 return
        if(Managers.Game.Collections[index] == CollectionState.Done)
            return;

        Managers.IAP.Purchase(productID, (product, failureReason) =>
        {
            // 구매 완료 로그
                Debug.Log($"Purchase Done: {product.transactionID} | Failure Reason: {failureReason}");

                // 구매가 완료되면 CollectionState를 변경
                Managers.Game.Collections[index] = CollectionState.Done;

                // 구매가 실패한 경우에 대한 처리
                if (failureReason == PurchaseFailureReason.Unknown)
                {
                    Debug.LogError("Purchase failed due to an unknown reason.");
                }
                else if (failureReason == PurchaseFailureReason.DuplicateTransaction)
                {
                    Debug.LogWarning("Duplicate transaction detected.");

                }
                else if (failureReason == PurchaseFailureReason.UserCancelled)
                {
                    Debug.Log("User cancelled the purchase.");
                }
                else
                {

                    Debug.LogError($"Purchase failed with reason: {failureReason}");
                }
            });

    }

    public void GetGold()
    {
        //이미 산 경우 return
        if(Managers.Game.Money > 990000)
            return;
        
        string productID = "purchase6";

       Managers.IAP.Purchase(productID, (product, failureReason) =>
        {
            // 구매 완료 로그
                Debug.Log($"Purchase Done: {product.transactionID} | Failure Reason: {failureReason}");
                // 구매가 완료되면 CollectionState를 변경
                Managers.Game.Money += 100000;

                // 구매가 실패한 경우에 대한 처리
                if (failureReason == PurchaseFailureReason.Unknown)
                {
                    Debug.LogError("Purchase failed due to an unknown reason.");
                }
                else if (failureReason == PurchaseFailureReason.DuplicateTransaction)
                {
                    Debug.LogWarning("Duplicate transaction detected.");

                }
                else if (failureReason == PurchaseFailureReason.UserCancelled)
                {
                    Debug.Log("User cancelled the purchase.");
                }
                else
                {

                    Debug.LogError($"Purchase failed with reason: {failureReason}");
                }
            });
    }

    public void OnImageClick(int nameID)
    {
        CollectionData selectedCollectionData = CollectionData.Find(data => data.nameID == nameID);
        idIndex = selectedCollectionData.ID;
        changeClothes(idIndex , nameidIndex);
    }

    public void OnClickGoBack()
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
        Player.transform.localScale = new Vector3(0.5f, 0.5f , 0.5f);
    }

        private void changeClothes(int idIndex ,int nameidIndex)
        {
            customManager.clothes = idIndex;
            customManager.numberCheck(1); 
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


