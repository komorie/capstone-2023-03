using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSymbol : EventSymbol
{
    public override void SymbolEncounter()
    {
        base.SymbolEncounter();

        Debug.Log("���� ����!");
    }

    //���� Ŭ���� ��, ���� Ŭ���� �̺�Ʈ ����
    public override void SymbolClear()
    {
        GameManager.Instance.OnLevelClear();
    }
}
