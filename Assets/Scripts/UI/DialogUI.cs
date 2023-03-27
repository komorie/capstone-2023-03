using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//��ȭâ UI Ŭ����
//1. ��ȭ�� ��ȭâ UI�� ������.
//2. ��ǲ�� ���� ��ȭâ�� ���� ��ȭ�� ����.

public class DialogUI : BaseUI
{
    private int dialogIndex;
    private int lineCount;
    private LineData currentLine;
    private CustomButton customButton;

    [SerializeField]
    private Image portrait;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text lineText;

    private Action DialogClosed;

    private void Awake()
    {
        customButton = GetComponent<CustomButton>();
    }

    private void OnEnable()
    {
        //�÷��̾� ���� ��Ȱ��ȭ, UI ���� Ȱ��ȭ
        InputActions.keyActions.Player.Disable();
        InputActions.keyActions.UI.Enable();

        //����, ���콺 Ŭ������ ��ȭâ �����ϴ� �Լ� �����ϰ� �̺�Ʈ ���
        //��ǲ�ý��ۿ� �̺�Ʈ �Լ� ����� ��, ���ٷ� ������� �ʴ� �� ���� ��. ������ �𸣰ڴµ� �ߺ� ���� ��������
        InputActions.keyActions.UI.Check.started += NextDialogByCheck;
        customButton.PointerDown += context => { NextDialog(); };
    }

    private void OnDisable()
    {
        InputActions.keyActions.Player.Enable();
        InputActions.keyActions.UI.Disable();

        InputActions.keyActions.UI.Check.started -= NextDialogByCheck;
        customButton.PointerDown -= context => { NextDialog(); };
    }

    //ó�� ��ȭâ�� ���� �� �ʱ�ȭ
    public void Init(int index, Action CloseCallback = null)
    {
        dialogIndex = index;
        DialogClosed = CloseCallback;
        lineCount = 0;
        NextDialog();
    }

    //��ȭâ�� �����Ű�� �Լ�. ��ȭ ������ ��ųʸ����� ��ȭ ������ ��������, ������ ������ ���̻� ������ ��ȭâ�� �ݴ´�.
    //��ȭ ���뿡 ���� UI ������ ��Ʈ����Ʈ�� ����.
    public void NextDialog()
    {
        //���� ��ȭ�� ������ â �ݰ� ����
        if (GameDataCon.Instance.DialogDic[dialogIndex].Count == lineCount) 
        {
            DialogClosed?.Invoke();
            PanelManager.Instance.ClosePanel("DialogUI");
            return;
        }

        //�� �� ��������
        currentLine = GameDataCon.Instance.DialogDic[dialogIndex][lineCount];

        //�̸�, �ʻ�ȭ ���� ���� ���� �̸�, �ʻ�ȭ â�� ����
        if (currentLine.portrait == null)
        {
            portrait.gameObject.SetActive(false);   
        }
        else
        {
            portrait.sprite = GameDataCon.Instance.SpriteDic[currentLine.portrait];
        }

        if(currentLine.name == null)
        {
            nameText.transform.parent.gameObject.SetActive(false);  
        }
        else
        {
            nameText.text = currentLine.name;
        }
        lineText.text = currentLine.line;

        lineCount++;
    }

    public void NextDialogByCheck(InputAction.CallbackContext context)
    {
        NextDialog();
    }

}
