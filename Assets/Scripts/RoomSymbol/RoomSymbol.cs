using System;
using UnityEngine;

public class RoomSymbol : MonoBehaviour
{
    public int Index { get; set; }
    public Define.EventType SymbolType { get; set; }

    //�÷��̾ �̺�Ʈ �ɺ��� ���� �ɾ��� ��
    public virtual void SymbolEncounter()
    {

    }

    //���� �̺�Ʈ �ɺ��� �̺�Ʈ�� ������ ��
    public virtual void SymbolClear()
    {
        
    }
}
