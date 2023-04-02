using System;
using UnityEngine;

public class RoomSymbol : MonoBehaviour
{
    public int Index { get; set; }
    public Define.EventType Type { get; set; }

    //�÷��̾ �̺�Ʈ �ɺ��� ���� �ɾ��� ��
    public virtual void Encounter()
    {
        //��ȭâ UI�� ���� UI�� ������ ��, �� �ɺ��� End �Լ��� ����� (�ݹ�)
        DialogUI dialog = UIManager.Instance.ShowUI("DialogUI").GetComponent<DialogUI>();
        dialog.Init(Index, End);
    }

    //�̺�Ʈ �ɺ��� �̺�Ʈ�� ������ �� (���� ��ȭâ�� �ݾ��� ��) ȣ���
    public virtual void End()
    {
        AssetLoader.Instance.Destroy(gameObject);
    }
}
