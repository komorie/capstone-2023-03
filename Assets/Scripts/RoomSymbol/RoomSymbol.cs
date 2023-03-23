using System;
using UnityEngine;

public class RoomSymbol : MonoBehaviour
{
    public int Index { get; set; }
    public Define.EventType Type { get; set; }

    //�÷��̾ �̺�Ʈ �ɺ��� ���� �ɾ��� ��
    public virtual void Encounter()
    {
        //��ȭâ UI�� ���� ��ȭâ�� ������ ��, �� �ɺ��� End�� �����
        DialogManager.Instance.OpenDialog(Index, End);
    }

    //���� �̺�Ʈ �ɺ��� �̺�Ʈ�� ������ ��
    public virtual void End()
    {
        AssetLoader.Instance.Destroy(gameObject);
    }
}
