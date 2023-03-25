using System;
using UnityEngine;

public class RoomSymbol : MonoBehaviour
{
    public int Index { get; set; }
    public Define.EventType Type { get; set; }

    //�÷��̾ �̺�Ʈ �ɺ��� ���� �ɾ��� ��
    public virtual void Encounter()
    {
        //��ȭâ UI�� ���� UI�� ������ ��, �� �ɺ��� End�� ����� (�ݹ�)
        DialogUI dialog = PanelManager.Instance.ShowPanel("DialogUI", true, End).GetComponent<DialogUI>();
        dialog.Init(Index);
    }

    //���� �̺�Ʈ �ɺ��� �̺�Ʈ�� ������ ��
    public virtual void End()
    {
        AssetLoader.Instance.Destroy(gameObject);
    }
}
