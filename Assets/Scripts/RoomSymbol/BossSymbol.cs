using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSymbol : RoomSymbol
{
    public override void Encounter()
    {
        base.Encounter();

        Debug.Log("���� ����!");
    }

    //���� Ŭ���� ��, ���� Ŭ���� �̺�Ʈ ����
    public override void End()
    {
        LevelManager.Instance.OnLevelClear();
    }
}
