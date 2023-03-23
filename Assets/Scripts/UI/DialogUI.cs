using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogUI : BaseUI
{
    private int dialogIndex;
    private int currentLine;
    private Dialog dialog;
    private ButtonEvents buttonEvents;

    [SerializeField]
    private Image portrait;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text lineText;


    private void Awake()
    {
        buttonEvents = GetComponent<ButtonEvents>();
    }

    private void OnEnable()
    {
        //�÷��̾� ���� ��Ȱ��ȭ, UI ���� Ȱ��ȭ
        InputManager.Instance.KeyActions.Player.Disable();
        InputManager.Instance.KeyActions.UI.Enable();

        //����, ���콺 Ŭ������ ��ȭâ �����ϴ� �Լ� �����ϰ� �̺�Ʈ ���
        //��ǲ�ý��ۿ� �̺�Ʈ �Լ� ����� ��, ���ٷ� ������� �ʴ� �� ���� ��. ������ �𸣰ڴµ� �ߺ� ���� ��������
        InputManager.Instance.KeyActions.UI.Check.started += ProgressDialogByCheck;
        buttonEvents.PointerDown += context => { NextDialog(); };
    }

    private void OnDisable()
    {
        InputManager.Instance.KeyActions.Player.Enable();
        InputManager.Instance.KeyActions.UI.Disable();

        InputManager.Instance.KeyActions.UI.Check.started -= ProgressDialogByCheck;
        buttonEvents.PointerDown -= context => { NextDialog(); };
    }

    //ó�� ��ȭâ�� ���� �� ���� �Լ�
    public void FirstDialog(int index)
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
        dialog = DialogManager.Instance.GetLine(dialogIndex, currentLine);

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

    public void ProgressDialogByCheck(InputAction.CallbackContext context)
    {
        NextDialog();
    }

}
