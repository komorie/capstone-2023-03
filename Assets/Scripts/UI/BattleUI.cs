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
        GameObject battleCameraParent = GameObject.Find("BattleCameraParent");
        Camera mainCamera = Camera.main;
        if (battleCameraParent != null)
        {
            canvas.worldCamera = battleCameraParent.transform.GetChild(0).GetComponent<Camera>();
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
