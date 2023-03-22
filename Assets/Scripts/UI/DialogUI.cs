using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    private int dialogIndex;
    private int currentLine;
    private Dialog dialog;
    private ButtonEvents buttonEvents;

    private Text nameText;
    private Text lineText;


    private void Awake()
    {
        buttonEvents = GetComponent<ButtonEvents>();
    }

    private void OnEnable()
    {
        currentLine = 0;

        //�÷��̾� ���� ��Ȱ��ȭ
        InputManager.Instance.KeyActions.Player.Disable();

        //����, ���콺 Ŭ������ ��ȭâ �����ϴ� �Լ� �����ϰ� �̺�Ʈ ���
        //��ǲ�ý��ۿ� �̺�Ʈ �Լ� ����� ��, ���ٷ� ������� �ʴ� �� ���� ��. ������ �𸣰ڴµ� �ߺ� ���� ��������
        InputManager.Instance.KeyActions.UI.Check.started += ProgressDialogByCheck;
        buttonEvents.PointerDown += context => { ProgressDialog(); };
    }

    private void OnDisable()
    {
        //�̺�Ʈ ����, �÷��̾� ���� Ȱ��ȭ
        InputManager.Instance.KeyActions.Player.Enable();
        InputManager.Instance.KeyActions.UI.Check.started -= ProgressDialogByCheck;
        buttonEvents.PointerDown -= context => { ProgressDialog(); };
    }

    //ó�� ��ȭâ�� ���� �Լ�
    public void ShowDialog(int index)
    {
        dialogIndex = index;
        currentLine = 0;
        ProgressDialog();
    }

    //��ȭâ�� �����Ű�� �Լ�. JSON���� ��ȭ ������ ��������, ���̻� ������ ��ȭâ�� �ݴ´�.
    //ī���͸� �÷��� �������� ���� ��ȭ ������ ��µǵ��� ��.
    //��ȭ ���뿡 ���� UI�� ��Ʈ����Ʈ�� �����Ѵ�.
    public void ProgressDialog()
    {
        Dialog currentDialog = DialogManager.Instance.GetLine(dialogIndex, currentLine);
        if (currentDialog == null) 
        {
            Debug.Log("��ȭ ����");
            PanelManager.Instance.ClosePanel("DialogUI");
            return;
        }
        Debug.Log(currentDialog.name);
        Debug.Log(currentDialog.line);

        currentLine++;
    }

    public void ProgressDialogByCheck(InputAction.CallbackContext context)
    {
        ProgressDialog();
    }

}
