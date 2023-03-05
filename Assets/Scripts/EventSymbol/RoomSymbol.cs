using System;
using UnityEngine;

public abstract class RoomSymbol : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")    
        {
            Encounter();
            gameObject.SetActive(false);
        }
    }

    //�̰� ���߿� �����ؾ���. ���� ������ �̰� ����Ǽ� ������Ʈ�� �����ϰ� ��
    private void OnDisable()
    {
        Clear();
    }

    //�÷��̾ �̺�Ʈ �ɺ��� ���� �ɾ��� ��
    public abstract void Encounter();

    //���� �̺�Ʈ ������ ��
    public abstract void Clear();
}
