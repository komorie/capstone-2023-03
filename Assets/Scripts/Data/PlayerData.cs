using System.Collections.Generic;


//�ٸ� Ŭ�����鿡���� playData�� �����ؼ� ����� �����͸� ����ϰų�, ������ ��ȭ ��(Ȥ�� ���̺� �õ� ��) playData�� �ǽð����� ������.
public class PlayerData : Singleton<PlayerData>
{
    public int channelLevel;
    public List<CardStruct> playerDeck;

    protected override void Awake()
    {
        base.Awake();
    }
}
