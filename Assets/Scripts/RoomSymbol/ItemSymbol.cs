using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSymbol : RoomSymbol
{
    public override void SymbolEncounter()
    {
        base.SymbolEncounter();
        Debug.Log("������ ȹ��!");
    }

    public override void SymbolClear()
    {
    }
}
