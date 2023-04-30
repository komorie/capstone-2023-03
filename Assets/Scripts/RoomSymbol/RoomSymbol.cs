using System;
using UnityEngine;

public class RoomSymbol : MonoBehaviour
{
    public int Index { get; set; }
    public Define.EventType Type { get; set; }

    //�÷��̾ �̺�Ʈ �ɺ��� ���� �ɾ��� ��
    public virtual void TalkStart()
    {
        //��ȭâ UI�� ���� UI�� ������ ��, �� �ɺ��� TalkEnd �Լ��� ����� (�ݹ�)
        UIManager.Instance.ShowUI("DialogUI")
            .GetComponent<DialogUI>()
            .Init(Index, TalkEnd);
    }

    //�̺�Ʈ �ɺ��� ��ȭ �̺�Ʈ�� ������ �� (���� ��ȭâ�� �ݾ��� ��) ȣ���
    public virtual void TalkEnd()
    {

        PlayerData.Instance.Viewers += 100;
        PlayerData.Instance.Money += 10;


        AssetLoader.Instance.Destroy(gameObject);
    }
}
