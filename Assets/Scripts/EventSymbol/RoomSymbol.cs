using System;
using UnityEngine;

public abstract class RoomSymbol : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")    
        {
            SymbolEncounter();
            gameObject.SetActive(false);
        }
    }

    //�̰� ���߿� �����ؾ���. 
    private void OnDisable()
    {
        SymbolClear();
    }

    //�÷��̾ �̺�Ʈ �ɺ��� ���� �ɾ��� ��
    public abstract void SymbolEncounter();

    //���� �̺�Ʈ �ɺ��� �̺�Ʈ�� ������ ��
    public abstract void SymbolClear();
}
