using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingUI : MonoBehaviour
{

    private bool isFullScreen;

    [SerializeField]
    public TMP_Text ScreenButtonText;

    private void Awake()
    {
        // ���� ��ũ�� ��� ��������
        isFullScreen = Screen.fullScreen;
        ScreenButtonText.text = isFullScreen ? "��üȭ��" : "â���";
    }

    //Ǯ��ũ�� ���ο� ���� Ŭ���� ȭ�� ��� ��ȯ�ϰ� ��ư �ؽ�Ʈ�� �ٲٱ�.
    public void ScreenModeButtonClick()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        ScreenButtonText.text =  isFullScreen ?  "��üȭ��" : "â���";
    }    

    public void ExitButtonClick()
    {
        UIManager.Instance.HideUI("SettingUI");
    }
}
