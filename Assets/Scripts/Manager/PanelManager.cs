using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    //UI���� �������� ����
    private Stack<GameObject> panelStack = new Stack<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //ESCŰ�� UI �ݱ�. �ϴ� �� ������ UI�� �� �ݰ� �س���
    private void OnEnable()
    {
        InputManager.Instance.KeyActions.UI.ESC.started += context => { 
            if (panelStack.Count == 1) panelStack.Peek().SetActive(!panelStack.Peek().activeSelf);  
            if (panelStack.Count > 1) HideLastPanel(); 
        };
    }

    private void OnDisable()
    {
        InputManager.Instance.KeyActions.UI.Disable();
        InputManager.Instance.KeyActions.Player.Enable();

        InputManager.Instance.KeyActions.UI.ESC.started += context => {
            if (panelStack.Count == 1) panelStack.Peek().SetActive(!panelStack.Peek().activeSelf);
            if (panelStack.Count > 1) HideLastPanel();
        };
    }

    //Ư�� UI�� �ε��ؼ� ȭ�鿡 ���� �Լ�
    //UI ���ÿ� �߰����� �����Ƿ� HideLastPanel�� ������ �ʴ´�
    public GameObject ShowPanel(string name)
    {
        return AssetLoader.Instance.Instantiate($"Prefabs/UI/{name}", PanelRoot.transform);
    }

    //Ư�� UI�� �ε��ؼ� ȭ�鿡 ���� UI ���ÿ� �߰��ϴ� �Լ�.
    //hidePreviousPanel�� true�̸� ���� UI�� ���� ó��
    //UI�� ������ ������ �Ź� ���� �ٽ� �ε��ؾ� �ϴ� ������... ������Ʈ Ǯ���� ���߿� �ϱ��
    public void ShowPanelOnStack(string name, bool hidePreviousPanel = false)
    {
        if(hidePreviousPanel && panelStack.Count > 0)
        { 
            panelStack.Peek().SetActive(false);
        }

        GameObject panel = ShowPanel(name);

        if(panel != null)
        { 
            panelStack.Push(panel); 
        }
    }

    //UI ���ÿ��� �� ���� �ִ� UI�� ����
    //�ƴϸ� �̸����� �����ϵ��� �ұ�? �׳� �迭�� �����ϰ�?
    //�ǵڿ� �ִ°� ���� �����̶� �̸����� ���� ������ ����...
    //���� UI�� ����ó�� �Ǿ������� �ٽ� ������
    public void HideLastPanel()
    {
        if (panelStack.Count > 0)
        {
            GameObject panel = panelStack.Pop();
            AssetLoader.Instance.Destroy(panel);

            if (panelStack.Count > 0) 
            {
                panelStack.Peek().SetActive(true);    
            }
        }
    }
}
