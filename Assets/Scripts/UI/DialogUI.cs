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
    private Define.EventType dialogEventType;
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

        InputManager.Instance.KeyActions.Player.Disable();
        InputManager.Instance.KeyActions.UI.Enable();

        //����, ���콺 Ŭ������ ��ȭâ �����ϴ� �Լ� �����ϰ� �̺�Ʈ ���
        //��ǲ�ý��ۿ� �̺�Ʈ �Լ� ����� ��, ���ٷ� ������� �ʴ� �� ���� ��. ������ �𸣰ڴµ� �ߺ� ���� ��������
        InputManager.Instance.KeyActions.UI.Check.started += ProgressDialogByCheck;
        buttonEvents.PointerDown += context => { ProgressDialog(); };
    }

    private void OnDisable()
    {
        //�̺�Ʈ ����, �÷��̾� ���� Ȱ��ȭ
        InputManager.Instance.KeyActions.Player.Enable();
        InputManager.Instance.KeyActions.UI.Disable();

        InputManager.Instance.KeyActions.UI.Check.started -= ProgressDialogByCheck;
        buttonEvents.PointerDown -= context => { ProgressDialog(); };
    }

    //ó�� ��ȭâ�� ���� �Լ�
    public void ShowDialog(int index, Define.EventType eventType)
    {
        dialogIndex = index;
        dialogEventType = eventType;
        currentLine = 0;
        ProgressDialog();
    }

    //��ȭâ�� �����Ű�� �Լ�. JSON���� ��ȭ ������ ��������, ���̻� ������ ��ȭâ�� �ݴ´�.
    //ī���͸� �÷��� �������� ���� ��ȭ ������ ��µǵ��� ��.
    //��ȭ ���뿡 ���� UI�� ��Ʈ����Ʈ�� �����Ѵ�.
    public void ProgressDialog()
    {
        dialog = DialogManager.Instance.GetLine(dialogIndex, currentLine);
        if (dialog == null) 
        {
            Debug.Log("��ȭ ����");
            PanelManager.Instance.ClosePanel("DialogUI");
            return;
        }
        Debug.Log(dialog.name);
        Debug.Log(dialog.line);


        currentLine++;
    }

    public void ProgressDialogByCheck(InputAction.CallbackContext context)
    {
        ProgressDialog();
    }

}
