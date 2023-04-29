
public class EnemySymbol : RoomSymbol
{

    public override void TalkStart()
    {
        UIManager.Instance.ShowUI("DialogUI")
        .GetComponent<DialogUI>()
        .Init(Index, SelectOpen);

    }

    //���� �˾� ����
    public void SelectOpen()
    {
        UIManager.Instance.ShowUI("SelectUI")
            .GetComponent<SelectUI>()
            .Init(
                "�����Ͻðڽ��ϱ�?", 
                () => { UIManager.Instance.ShowUI("DialogUI").GetComponent<DialogUI>().Init(Index + 6000, TalkEnd); }, //���� ��ȭ �� ī��, ���� ȹ��
                () => { UIManager.Instance.ShowUI("DialogUI").GetComponent<DialogUI>().Init(Index + 7000, TalkEnd); } //������ ��ȭ �� ���� UI ȣ��
            );
    }

    //���� �õ�
    public void TryNegotiate()
    {
        //���� ���� ������ Ȯ���� ���� ��ȭ, �����ϸ� ���� ���� ��ȭ ȣ��
    }

    public void Fight()
    {
        //���� UI ����
        //���� UI ���� ��, FightEnd ȣ��
    }

    public void FightEnd()
    {
        //���� ���� �� ȣ��
        //������ UI ���� ��, TalkEnd ȣ��
    }

    public void Negotiate()
    {
        //���� ���� �� ī�� ȹ��
        //������ UI ���� ��, TalkEnd ȣ��
    }

    public override void TalkEnd()
    {
        base.TalkEnd();

    }
}
