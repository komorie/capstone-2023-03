using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSymbol : RoomSymbol
{
    public override void TalkStart()
    {
        base.TalkStart();
    }

    //���� Ŭ���� ��, ���� Ŭ���� �̺�Ʈ ����
    public override void TalkEnd()
    {
        base.TalkEnd();

        MapManager.Instance.OnLevelClear();
    }
}
