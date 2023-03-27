using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PanelManager : Singleton<PanelManager>
{
    //��ȯ�� ��� UI�� ��Ʈ�� ���ӿ�����Ʈ
    public GameObject PanelRoot
    {
        get
        {
            GameObject root = GameObject.Find("PanelRoot");
            if (root == null)
            {
                root = new GameObject();
                root.name = "PanelRoot";
            }
            return root;
        }
    }

    //�˾� UI���� �������� ����
    private Stack<GameObject> panelStack = new Stack<GameObject>();


    protected override void Awake()
    {
        base.Awake();
    }

    //ESCŰ�� UI �ݱ�.
    private void OnEnable()
    {
        InputActions.keyActions.UI.ESC.started += context => { ClosePanel(); };
    }

    private void OnDisable()
    {
        InputActions.keyActions.UI.ESC.started -= context => { ClosePanel(); };
    }   

    //�Ϲ� UI�� �ε��ؼ� ȭ�鿡 ���� �Լ�
    //������ UI�� Ȱ��ȭ�Ǹ� ����ȭ/���ĺ��� ������ ���� �Ʒ��� �� UI�� ��Ȱ��ȭ
    public GameObject ShowPanel(string name, bool hidePreviousPanel = true)
    {
        GameObject panel = AssetLoader.Instance.Instantiate($"Prefabs/UI/{name}", PanelRoot.transform);

        if (panelStack.Count > 0 && hidePreviousPanel)
        {
            panelStack.Peek().SetActive(false);
        }
        panelStack.Push(panel);
        return panel;
    }

    //UI ���ÿ��� �� ���� �ִ� UI�� ����
    //���� UI�� ����ó�� �Ǿ������� �ٽ� ������
    //ESCŰ�� ���������� UI�� ���ﶧ ���µ�, �׷��Ƿ� ������ ���� UI�� �� ���������� ��
    public void ClosePanel()
    {
        if (panelStack.Count > 1)
        {
            GameObject panel = panelStack.Pop();
            AssetLoader.Instance.Destroy(panel);
            panelStack.Peek().SetActive(true);
        }
    }

    //UI ���ÿ��� �� ���� �ִ� Ư�� UI�� ����
    //���� UI�� ����ó�� �Ǿ������� �ٽ� ������
    //���������� ����� �Լ��� �����ϴ� ������, ������ UI ������ �� ��Ȳ������ ����� �Լ���.
    public void ClosePanel(string name)
    {
        if (panelStack.Count > 0 && panelStack.Peek().name == name)
        {
            GameObject panel = panelStack.Pop();

            AssetLoader.Instance.Destroy(panel);
            
            if(panelStack.Count > 0)
            {
                panelStack.Peek().SetActive(true);
            }
        }
    }

    public void Clear()
    {
        panelStack.Clear();
    }
}
