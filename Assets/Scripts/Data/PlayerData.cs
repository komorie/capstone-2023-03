using System.Collections.Generic;
using DataStructs;

//�ٸ� Ŭ�����鿡���� playData�� �����ؼ� ����� �����͸� ����ϰų�, ������ ��ȭ �� playData�� �ǽð����� ������.
public class PlayerData : Singleton<PlayerData>
{
    public int ChannelLevel { get; set; } //ä�� ����

    public int Viewers { get; set; } //��û�� ��

    public int CurrentHp { get; set; } //���� ü��

    public int MaxHp { //�ִ� ü��
        get 
        {
            int maxhp = 80 + (100 / Viewers);  
            return maxhp;
        }
    }

    public int Money { get; set; }  //���� ��

    public int Energy { get; set; } //���� ������

    public List<CardStruct> Deck { get; set; }

    protected override void Awake()
    {
        base.Awake();

        //�ʱ� ������ ����

        ChannelLevel = 1;
        Viewers = 0;
        CurrentHp = MaxHp;
        Money = 0;
        Energy = 5;

        Deck = new List<CardStruct>(){
            GameData.Instance.CardList[0],
            GameData.Instance.CardList[0], 
            GameData.Instance.CardList[0],
            GameData.Instance.CardList[1],
            GameData.Instance.CardList[1],
            GameData.Instance.CardList[1],
            GameData.Instance.CardList[2],
            GameData.Instance.CardList[3]
        };  

    }

    //������ �� ������ ������� �ѹ���...
    public void ChannelLevelUp()
    {

    }

}
