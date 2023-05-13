using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : BaseUI
{
    // Start is called before the first frame update
    void Start()
    {
        //Canvas�� ī�޶� BattleCamera�� ����, �׷� ī�޶� ���ٸ� ���� ī�޶�� ����
        Canvas canvas = GetComponent<Canvas>();
        Camera battleCamera = GameObject.Find("BattleCamera").GetComponent<Camera>();
        Camera mainCamera = Camera.main;
        if (battleCamera != null)
        {
            canvas.worldCamera = battleCamera;           
        }
        else
        {
            canvas.worldCamera = mainCamera;
        }
    }

    public void TrashClick()
    {
        UIManager.Instance.ShowUI("LibraryUI")
            .GetComponent<LibraryUI>()
            .Init(LibraryMode.Battle_Trash);
    }

    public void DeckClick()
    {
        UIManager.Instance.ShowUI("LibraryUI")
            .GetComponent<LibraryUI>()
            .Init(LibraryMode.Battle_Deck);
    }
}
