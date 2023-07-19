using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text message;
    [SerializeField]
    Action CloseAction;

    public void Init(string text, Action CloseCallback = null)
    {
        message.text = text;    
        CloseAction = CloseCallback;
    }

    public void ConfirmClick()
    {
        UIManager.Instance.HideUI("ConfirmUI");
        CloseAction?.Invoke();
    }
}
