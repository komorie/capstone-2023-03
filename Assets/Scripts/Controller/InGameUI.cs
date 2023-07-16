using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//인게임 창에서 단축키로 UI 띄우는 스크립트
public class InGameUI : MonoBehaviour
{

    private void OnEnable()
    {
        UIManager.Instance.OnUIChanged += ChangeUIControll;
        InputActions.keyActions.Player.Deck.started += OnDeckStarted;
        InputActions.keyActions.Player.Pause.started += OnPauseStarted;
        InputActions.keyActions.Player.MiniMap.started += OnMiniMapStarted;
    }

    private void OnDisable()
    {
        UIManager.Instance.OnUIChanged -= ChangeUIControll;
        InputActions.keyActions.Player.Deck.started -= OnDeckStarted;
        InputActions.keyActions.Player.Pause.started -= OnPauseStarted;
        InputActions.keyActions.Player.MiniMap.started -= OnMiniMapStarted;
    }

    private void ChangeUIControll(GameObject currentUI)
    {
        if (currentUI?.name == "StatUI") //HUD 떠있을때, 즉 플레이어 조작 중에는 UI 조작키 비활성화
        {
            InputActions.keyActions.UI.Disable();
            InputActions.keyActions.Player.Enable();
        }
        else //그 이외의 UI가 떠있으면 플레이어 조작키 비활성화
        {
            InputActions.keyActions.Player.Disable();
            InputActions.keyActions.UI.Enable();
        }
    }

    //I버튼으로 인벤토리 UI 열기
    public void OnDeckStarted(InputAction.CallbackContext context)
    {
        LibraryUI libraryUI = UIManager.Instance.ShowUI("LibraryUI").GetComponent<LibraryUI>();
        libraryUI.Init(LibraryMode.Deck);
    }

    //ESC버튼으로 일시정지 UI 열기
    public void OnPauseStarted(InputAction.CallbackContext context)
    {
        UIManager.Instance.ShowUI("TitleBG");
        UIManager.Instance.ShowUI("PauseUI", false);
    }

    //Tab으로 미니맵 UI 열기
    public void OnMiniMapStarted(InputAction.CallbackContext context)
    {
        UIManager.Instance.ShowUI("MiniMapUI");
    }

}
