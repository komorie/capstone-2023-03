using LitJson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    private int dialogIndex;
    private int currentLine;
    private Dialog dialog;
    private Define.EventType dialogType;
    private RoomSymbol talkingSymbol;
    private ButtonEvents buttonEvents;

    private Image portrait;
    private TMP_Text nameText;
    private TMP_Text lineText;


    private void Awake()
    {
        buttonEvents = GetComponent<ButtonEvents>();
        portrait = GameObject.Find("Portrait").GetComponent<Image>();
        nameText = transform.GetChild(1).Find("Name").Find("NameText").GetComponent<TMP_Text>();
        lineText = transform.GetChild(1).Find("Line").Find("LineText").GetComponent<TMP_Text>();
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
    public void ShowDialog(RoomSymbol symbol)
    {
        talkingSymbol = symbol;
        dialogIndex = symbol.Index;
        dialogType = symbol.SymbolType;
        currentLine = 0;
        ProgressDialog();
    }

    //��ȭâ�� �����Ű�� �Լ�. JSON���� ��ȭ ������ ��������, ���̻� ������ ��ȭâ�� �ݴ´�.
    //ī���͸� �÷��� �������� ���� ��ȭ ������ ��µǵ��� ��.
    //��ȭ ���뿡 ���� UI�� ��Ʈ����Ʈ�� ����.
    public void ProgressDialog()
    {
        dialog = DialogManager.Instance.GetLine(dialogIndex, currentLine);
        if (dialog == null) 
        {
            Debug.Log("��ȭ ����");
            AssetLoader.Instance.Destroy(talkingSymbol.gameObject);
            PanelManager.Instance.ClosePanel("DialogUI");
            return;
        }

        portrait.sprite = DialogManager.Instance.GetSprite(dialog.portrait);
        nameText.text = dialog.name;
        lineText.text = dialog.line;

        currentLine++;
    }

    public void ProgressDialogByCheck(InputAction.CallbackContext context)
    {
        ProgressDialog();
    }

}
