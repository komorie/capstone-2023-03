using System;
using UnityEngine;

public class RoomSymbol : MonoBehaviour
{
    public int Index { get; set; }
    public Define.EventType Type { get; set; }

    //�÷��̾ �̺�Ʈ �ɺ��� ���� �ɾ��� ��
    public virtual void Encounter()
    {
        //��ȭâ UI�� ������ ��, �� �ɺ��� End�� �����
        DialogUI dialog = PanelManager.Instance.ShowPanel("DialogUI", false, null, End).GetComponent<DialogUI>();
        dialog.ShowDialog(Index);
    }

    //���� �̺�Ʈ �ɺ��� �̺�Ʈ�� ������ ��
    public virtual void End()
    {
        AssetLoader.Instance.Destroy(gameObject);
    }
}
