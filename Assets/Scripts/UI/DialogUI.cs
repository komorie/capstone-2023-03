using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DataStructs;

//��ȭâ UI Ŭ����
//1. ��ȭ�� ��ȭâ UI�� ������.
//2. ��ǲ�� ���� ��ȭâ�� ���� ��ȭ�� ����.

public class DialogUI : BaseUI, IPointerDownHandler
{
    private bool isInited = false;
    private int dialogIndex;
    private int lineCount;
    private LineStruct currentLine;

    [SerializeField]
    private Image portrait;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text lineText;

    private Action CloseAction;

    private void OnEnable()
    {
        //���ͷ� ��ȭâ �����ϴ� �Լ� �����ϰ� �̺�Ʈ ���
        //��ǲ�ý��ۿ� �̺�Ʈ �Լ� ����� ��, ���ٷ� ������� �ʴ� �� ���� ��. ������ �𸣰ڴµ� �ߺ� ���� ��������
        InputActions.keyActions.UI.Check.started += NextDialogByCheck;
    }

    private void OnDisable()
    {
        InputActions.keyActions.UI.Check.started -= NextDialogByCheck;
    }

    //���콺�� UI Ŭ�� �� �۵�
    public void OnPointerDown(PointerEventData eventData)
    {
        NextDialog();
    }

    //���� Ű ���� �� �۵�
    public void NextDialogByCheck(InputAction.CallbackContext context)
    {
        if(isInited == true)
        {
            NextDialog();
        }
    }

    //ó�� ��ȭâ�� ���� �� �ʱ�ȭ
    public void Init(int index, Action CloseCallback = null)
    {
        dialogIndex = index;
        CloseAction = CloseCallback;
        lineCount = 0;
        isInited = true;
        NextDialog();
    }

/*    //��ȭâ �ʱ�ȭ - ���ڿ� ���ڸ� ���� �ܹ� ��ȭ ����
    public void Init(string line, string portrait = null, string name = null, Action CloseCallback = null)
    {
        dialogIndex = -1;

        if (portrait == null || portrait == "")
        {
            this.portrait.gameObject.SetActive(false);
        }
        else
        {
            this.portrait.gameObject.SetActive(true);
            this.portrait.sprite = GameData.Instance.SpriteDic[portrait];
        }

        if (name == null || name == "")
        {
            nameText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            nameText.transform.parent.gameObject.SetActive(true);
            nameText.text = name;
        }

        lineText.text = line;
        isInited = true;
        CloseAction = CloseCallback; 

    }*/

    //��ȭâ�� �����Ű�� �Լ�. ��ȭ ������ ��ųʸ����� ��ȭ ������ ��������, ������ ������ ���̻� ������ ��ȭâ�� �ݴ´�.
    //��ȭ ���뿡 ���� UI ������ ��Ʈ����Ʈ�� ����.
    public void NextDialog()
    {
        //���� ��ȭ�� ������ â �ݰ� ����
        if (dialogIndex == -1 || GameData.Instance.DialogDic[dialogIndex].Count == lineCount) 
        {
            UIManager.Instance.HideUI("DialogUI");
            CloseAction?.Invoke();
            return;
        }

        //�� �� ��������
        currentLine = GameData.Instance.DialogDic[dialogIndex][lineCount];

        //�̸�, �ʻ�ȭ ���� ���� ���� �̸�, �ʻ�ȭ â�� ����
        if (currentLine.portrait == null || currentLine.portrait == "")
        {
            portrait.gameObject.SetActive(false);   
        }
        else
        {
            portrait.gameObject.SetActive(true);
            portrait.sprite = GameData.Instance.SpriteDic[currentLine.portrait];
        }

        if(currentLine.name == null || currentLine.name == "")
        {
            nameText.transform.parent.gameObject.SetActive(false);  
        }
        else
        {
            nameText.transform.parent.gameObject.SetActive(true);
            nameText.text = currentLine.name;
        }
        lineText.text = currentLine.line;

        lineCount++;
    }
}
