using System;
using UnityEngine;

public class RoomSymbol : MonoBehaviour
{
    Room room;

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
    public virtual void SymbolEncounter()
    {

    }

    //���� �̺�Ʈ �ɺ��� �̺�Ʈ�� ������ ��
    public virtual void SymbolClear()
    {

    }
}
