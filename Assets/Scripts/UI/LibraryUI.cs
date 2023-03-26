using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LibraryUI : BaseUI
{
    // Start is called before the first frame update
    public void Awake()
    {
        
    }

    private void OnEnable()
    {
        //�÷��̾� ���� ��Ȱ��ȭ, UI ���� Ȱ��ȭ
        InputActions.keyActions.Player.Disable();
        InputActions.keyActions.UI.Enable();

        InputActions.keyActions.UI.Menu.started += Close;
    }

    // Update is called once per frame
    private void OnDisable()
    {
        InputActions.keyActions.Player.Enable();
        InputActions.keyActions.UI.Disable();

        InputActions.keyActions.UI.Menu.started -= Close;
    }

    public void ShowPlayerCards()
    {
        //���� �÷��̾ �� ����Ʈ�� ����
        //����Ʈ�� ��ȸ�ϸ�, ī��UI�� show card�� �ϸ� �ɵ�.
    }

    public void ShowAllCards()
    {
        //���� �������� ��ü ī�� ����Ʈ�� ����
        //����Ʈ�� ��ȸ�ϸ�, ī��UI�� show card�� �ϸ� �ɵ�.
    }
    private void Close(InputAction.CallbackContext context)
    {
        PanelManager.Instance.ClosePanel("LibraryUI");
    }
}
