using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSymbol : RoomSymbol
{
    public override void SymbolEncounter()
    {
        Debug.Log("BossRoom");
    }

    //���� Ŭ���� ��, ���� Ŭ���� �̺�Ʈ ����
    public override void SymbolClear()
    {
        GameManager.Instance.OnLevelClear();
    }
}
