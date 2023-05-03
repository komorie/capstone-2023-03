using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    //소환한 모든 UI의 루트인 게임오브젝트
    public GameObject UIRoot
    {
        get
        {
            GameObject root = GameObject.Find("UIRoot");
            if (root == null)
            {
                root = new GameObject();
                root.name = "UIRoot";
            }
            return root;
        }
    }

    //팝업 UI들을 스택으로 관리
    private Stack<GameObject> UIStack = new Stack<GameObject>();

    //UI 변경 시 발생될 이벤트
    public Action<GameObject> UIChange;


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }


    //한 전체 UI를 로드해서 화면에 띄우는 함수
    //위층의 UI가 활성화되면 최적화/겹쳐보임 방지를 위해 아래에 깔린 UI를 비활성화하는 게 기본 설정. (스택을 통해 구현)
    public GameObject ShowUI(string name, bool hidePreviousPanel = true)
    {
        GameObject ui = AssetLoader.Instance.Instantiate($"Prefabs/UI/{name}", UIRoot.transform);

        if (UIStack.Count > 0 && hidePreviousPanel)
        {
            UIStack.Peek().SetActive(false);
        }
        UIStack.Push(ui);
        UIChange?.Invoke(ui);
        return ui;
    }


    //UI 스택에서 맨 위에 있는 특정 UI를 제거, 이전 UI가 숨김처리 되어있으면 다시 보여줌
    public void HideUI(string name)
    {
        if (UIStack.Count > 0 && UIStack.Peek().name == name)
        {
            GameObject ui = UIStack.Pop();

            AssetLoader.Instance.Destroy(ui);
            
            if(UIStack.Count > 0)
            {
                UIStack.Peek().SetActive(true);
                UIChange?.Invoke(UIStack.Peek());    
            }
            else
            {
                UIChange?.Invoke(null);
            }
        }
    }

    public void Clear()
    {
        UIStack.Clear();
    }
}
