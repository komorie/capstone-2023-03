using System;
using UnityEngine;

public class EventSymbol : MonoBehaviour
{
    Room room;

    //�̰� ���߿� �����ؾ���. 
    private void OnDisable()
    {
        SymbolClear();
    }

    //�÷��̾ �̺�Ʈ �ɺ��� ���� �ɾ��� ��
    public virtual void SymbolEncounter()
    {
        gameObject.SetActive(false);
    }

    //���� �̺�Ʈ �ɺ��� �̺�Ʈ�� ������ ��
    public virtual void SymbolClear()
    {

    }
}
