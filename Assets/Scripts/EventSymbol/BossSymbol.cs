using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSymbol : RoomSymbol
{
    public override void Encounter()
    {
        Debug.Log("BossRoom");
    }

    //���� Ŭ���� ��, ���� Ŭ���� �̺�Ʈ ����
    public override void Clear()
    {

    }
}
