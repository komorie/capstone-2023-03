using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSymbol : EventSymbol
{
    public override void SymbolEncounter()
    {
        base.SymbolEncounter();

        Debug.Log("���� ����");
    }

    public override void SymbolClear()
    {
        
    }
}
