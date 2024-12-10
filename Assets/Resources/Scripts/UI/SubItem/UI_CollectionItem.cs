using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_CollectionItem : UI_Popup
{
    int _collectionId;
    enum Texts
    {
        CollectionName,
        CollectionHow
    }
    enum Images
    {
        CollectionImage,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

		BindText(typeof(Texts));
        BindImage(typeof(Images));
        

        return true;
    }

    // 더미 데이터를 설정하는 함수
    public void SetData(CollectionData data)
    {
        Debug.Log(data.How);
        GetText((int)Texts.CollectionName).text = data.description;
        GetText((int)Texts.CollectionHow).text = data.How;
        GetImage((int)Images.CollectionImage).sprite = Managers.Resource.Load<Sprite>(data.iconPath);
    }
	public void SetCollectionInfo(int collectionId)
	{
		if (collectionId > 0)
			_collectionId = collectionId;
	}

	// void RefreshCollectionUI()
	// {
    //     if (_collectionId == 0)
	// 		return;

    //     Managers.Data.Collections.TryGetValue(_collectionId, out CollectionData data);

    //     string path = Managers.Data.Collections[_collectionId].iconPath;
    //     GetImage((int)Images.CollectionImage).sprite = Managers.Resource.Load<Sprite>(path);
    // 	GetText((int)Texts.CollectionText).text = Managers.GetText(data.nameID);
	// 	GetText((int)Texts.DescriptionText).text = $"+{data.description}";
    // }
}
