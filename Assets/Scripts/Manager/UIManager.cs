using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    //��ȯ�� ��� UI�� ��Ʈ�� ���ӿ�����Ʈ
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

    //�˾� UI���� �������� ����
    private Stack<GameObject> UIStack = new Stack<GameObject>();


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //ESCŰ�� UI �ݱ�.
    private void OnEnable()
    {
        InputActions.keyActions.UI.ESC.started += context => { CloseUI(); };
    }

    private void OnDisable()
    {
        InputActions.keyActions.UI.ESC.started -= context => { CloseUI(); };
    }   

    //ShowUI�� �������� UI ���ÿ� ���� �ʴ´ٴ� ��.
    //��Ȯ���� UI ���ÿ� ���� ���� UI ��ҵ��� UIElement��� ������ �����ϵ���.

    public GameObject ShowUIElement(string name, Transform parent)
    {
        return AssetLoader.Instance.Instantiate($"Prefabs/UIElement/{name}", parent);
    }

    //�Ϲ� UI�� �ε��ؼ� ȭ�鿡 ���� �Լ�
    //������ UI�� Ȱ��ȭ�Ǹ� ����ȭ/���ĺ��� ������ ���� �Ʒ��� �� UI�� ��Ȱ��ȭ�ϴ� �� �⺻ ����.
    public GameObject ShowUI(string name, bool hidePreviousPanel = true)
    {
        GameObject ui = AssetLoader.Instance.Instantiate($"Prefabs/UI/{name}", UIRoot.transform);

        if (UIStack.Count > 0 && hidePreviousPanel)
        {
            UIStack.Peek().SetActive(false);
        }
        UIStack.Push(ui);
        return ui;
    }

    //UI ���ÿ��� �� ���� �ִ� UI�� ����
    //���� UI�� ����ó�� �Ǿ������� �ٽ� ������
    //ESCŰ�� ���������� UI�� ���ﶧ ���µ�, �׷��Ƿ� ������ ���� UI�� �� ���������� ��
    public void CloseUI()
    {
        if (UIStack.Count > 1)
        {
            GameObject ui = UIStack.Pop();
            AssetLoader.Instance.Destroy(ui);
            UIStack.Peek().SetActive(true);
        }
    }

    //UI ���ÿ��� �� ���� �ִ� Ư�� UI�� ����
    //���� UI�� ����ó�� �Ǿ������� �ٽ� ������
    //���������� ����� �Լ��� �����ϴ� ������, ������ UI ������ �� ��Ȳ������ ����� �Լ���.
    public void ClosePanel(string name)
    {
        if (UIStack.Count > 0 && UIStack.Peek().name == name)
        {
            GameObject ui = UIStack.Pop();

            AssetLoader.Instance.Destroy(ui);
            
            if(UIStack.Count > 0)
            {
                UIStack.Peek().SetActive(true);
            }
        }
    }

    public void Clear()
    {
        UIStack.Clear();
    }
}
