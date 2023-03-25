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
    private int currentLine;
    private Dialog dialog;
    private CustomButton customButton;

    [SerializeField]
    private Image portrait;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text lineText;


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
    public void Init(int index)
    {
        dialogIndex = index;
        currentLine = 0;
        NextDialog();
    }

    //��ȭâ�� �����Ű�� �Լ�. JSON���� ��ȭ ������ ��������, ������ ������ ���̻� ������ ��ȭâ�� �ݴ´�.
    //��ȭ ���뿡 ���� UI ������ ��Ʈ����Ʈ�� ����.
    public void NextDialog()
    {
        //�� �� ��������
        dialog = DialogData.Instance.GetLine(dialogIndex, currentLine);

        //���� ��ȭ�� ������ â �ݰ� ����
        if (dialog == null) 
        {
            PanelManager.Instance.ClosePanel("DialogUI");
            return;
        }
        
        //�̸�, �ʻ�ȭ ���� ���� ���� �̸�, �ʻ�ȭ â�� ����
        if(dialog.portrait == null)
        {
            portrait.gameObject.SetActive(false);   
        }
        else
        {
            portrait.sprite = dialog.portrait;
        }

        if(dialog.name == null)
        {
            nameText.transform.parent.gameObject.SetActive(false);  
        }
        else
        {
            nameText.text = dialog.name;
        }
        lineText.text = dialog.line;

        currentLine++;
    }

    public void NextDialogByCheck(InputAction.CallbackContext context)
    {
        NextDialog();
    }

}
