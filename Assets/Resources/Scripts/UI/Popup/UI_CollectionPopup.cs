using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class UI_CollectionPopup : UI_Popup
{	
	public List<CollectionData> _collections = new List<CollectionData>();
	CollectionState[] _Collections = new CollectionState[Define.MAX_COLLECTION_COUNT];
	enum Buttons
	{
		ExitButton,
	}
	enum Texts
	{
		ArchiveTitle1,
		ArchiveEffect1,
		ArchiveTitle2,
		ArchiveEffect2,
	}
	enum Images
	{
		ArchiveIcon1,
		ArchiveIcon2,
	}
	
	public override bool Init()
	{
		if (base.Init() == false)
			return false;
		
		BindButton(typeof(Buttons));
		BindText(typeof(Texts));
		BindImage(typeof(Images));

		//설정 collection 파일 
		foreach (CollectionData data in Managers.Data.Collections.Values)
        {
            _collections.Add(data);
        }
		//사용자 업적했는지 확인 데이터
		CollectionState[] _CollectionState = Managers.Game.coordCollections;

		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnCloseCollection);
		
		Debug.Log(_collections[0].description);
		GetText((int)Texts.ArchiveTitle1).text = $"{_collections[0].description}";
		GetText((int)Texts.ArchiveEffect1).text = $"{_collections[0].difHp}";
		return true;
	}

	public void OnCloseCollection()
	{
		Managers.UI.ClosePopupUI(this);
	}
}
