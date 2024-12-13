using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager
{
    int _order = -20;
	Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
	public UI_Scene SceneUI { get; private set; }

    // UI 생성 시 모아두는 객체 (@UI_Root)
	public GameObject Root
	{
		get
		{
			GameObject root = GameObject.Find("@UI_Root");
			if (root == null)
				root = new GameObject { name = "@UI_Root" };

			return root;
		}
	}

public void SetCanvas(GameObject go, Camera camera, bool sort = true)
{
    // Canvas 컴포넌트를 가져오거나 추가
    Canvas canvas = Utils.GetOrAddComponent<Canvas>(go);
    
    // RenderMode를 ScreenSpaceCamera로 설정
    canvas.renderMode = RenderMode.ScreenSpaceCamera;
    canvas.worldCamera = camera; // 렌더링할 카메라를 설정

    // Sorting Order 설정
    canvas.overrideSorting = true;
    if (sort)
    {
        canvas.sortingOrder = _order;
        _order++;
    }
    else
    {
        canvas.sortingOrder = 0;
    }
}


	public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject prefab = Managers.Resource.Load<GameObject>($"Prefabs/UI/SubItem/{name}");

		GameObject go = Managers.Resource.Instantiate(prefab);
		if (parent != null)
			go.transform.SetParent(parent);

		go.transform.localScale = Vector3.one;
		go.transform.localPosition = prefab.transform.position;

		return Utils.GetOrAddComponent<T>(go);
	}

	public T ShowSceneUI<T>(string name = null) where T : UI_Scene
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
		T sceneUI = Utils.GetOrAddComponent<T>(go);
		SceneUI = sceneUI;

		go.transform.SetParent(Root.transform);

		return sceneUI;
	}

	public T ShowPopupUI<T>(string name = null, Transform parent = null) where T : UI_Popup
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject prefab = Managers.Resource.Load<GameObject>($"Prefabs/UI/Popup/{name}");

		GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
		T popup = Utils.GetOrAddComponent<T>(go);
		_popupStack.Push(popup);

		if (parent != null)
			go.transform.SetParent(parent);
		else if (SceneUI != null)
			go.transform.SetParent(SceneUI.transform);
		else
			go.transform.SetParent(Root.transform);

		go.transform.localScale = Vector3.one;
		go.transform.localPosition = prefab.transform.position;

		return popup;
	}

	public T FindPopup<T>() where T : UI_Popup
	{
		return _popupStack.Where(x => x.GetType() == typeof(T)).FirstOrDefault() as T;
	}

	public T PeekPopupUI<T>() where T : UI_Popup
	{
		if (_popupStack.Count == 0)
			return null;

		return _popupStack.Peek() as T;
	}

	public void ClosePopupUI(UI_Popup popup)
	{
		if (_popupStack.Count == 0)
			return;

		if (_popupStack.Peek() != popup)
		{
			Debug.Log("Close Popup Failed!");
			return;
		}

		ClosePopupUI();
	}

	public void ClosePopupUI()
	{
		if (_popupStack.Count == 0)
			return;

		UI_Popup popup = _popupStack.Pop();

		if (popup != null) 
		{
			Managers.Resource.Destroy(popup.gameObject);
			popup = null; 
		}

		_order--;
	}

	public void CloseAllPopupUI()
	{
		while (_popupStack.Count > 0)
			ClosePopupUI();
	}

	public void Clear()
	{
		CloseAllPopupUI();
		SceneUI = null;
	}

	public T GetSceneUI<T>() where T : UI_Scene
    {
        if (SceneUI is T sceneUI)
        {
            return sceneUI;
        }

        return null; // UI_Scene이 T 타입이 아니면 null 반환
    }


	public void ClosePlayerSceneUI()
	{
		GameObject playerScene = GameObject.Find("UI_PlayerScene");
		if (playerScene != null)
		{
			// popup을 상위 계층으로 이동
			Transform popup1 = playerScene.transform.Find("UI_TitlePopup");
			if (popup1 != null)
			{
				popup1.SetParent(playerScene.transform.parent);
			}


			// UI_PlayerScene 삭제
			Managers.Resource.Destroy(playerScene);
		}
		Managers.Game.CharacterDelete();
	}


}
