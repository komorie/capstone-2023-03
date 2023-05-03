using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//���� â���� ����Ű�� UI ����
public class InGameUI : MonoBehaviour
{

    private void OnEnable()
    {
        UIManager.Instance.UIChange += ChangeUIControll;
        InputActions.keyActions.Player.Menu.started += OnMenuStarted;
    }

    private void OnDisable()
    {
        UIManager.Instance.UIChange -= ChangeUIControll;
        InputActions.keyActions.Player.Menu.started -= OnMenuStarted;

    }

    private void ChangeUIControll(GameObject currentUI)
    {
        if (currentUI?.name == "StatUI") //HUD ��������, �� �÷��̾� ���� �߿��� UI ����Ű ��Ȱ��ȭ
        {
            InputActions.keyActions.UI.Disable();
            InputActions.keyActions.Player.Enable();
        }
        else
        {
            InputActions.keyActions.Player.Disable();
            InputActions.keyActions.UI.Enable();
        }
    }

    //I��ư���� �κ��丮 UI ����
    public void OnMenuStarted(InputAction.CallbackContext context)
    {
        LibraryUI libraryUI = UIManager.Instance.ShowUI("LibraryUI").GetComponent<LibraryUI>();
        libraryUI.Init(false);
    }
}
