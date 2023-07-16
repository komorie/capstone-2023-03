using System;
using System.Collections.Generic;
using DataStructs;
using UnityEngine;

//�ٸ� Ŭ�����鿡���� playData�� �����ؼ� ����� �����͸� ����ϰų�, ������ ��ȭ �� playData�� �ǽð����� ������.
public class PlayerData : Singleton<PlayerData>
{

    private int channelLevel;
    private int viewers;
    private float currentHp;
    private float maxHp;
    private int money;
    private int energy;

    public bool[] HasPartner { get; set; } = new bool[3]; //�ش� ���������� ���� ī�� �������

    public int ChannelLevel {
        get { return channelLevel; }
        set { channelLevel = value; OnPlayerDataChange?.Invoke(); }
    }  //ä�� ����

    public int Viewers {
        get { return viewers; }
        set { viewers = value; OnPlayerDataChange?.Invoke(); }
    }  //��û�� ��

    public float CurrentHp
    {
        get { return currentHp; }
        set { currentHp = value; OnPlayerDataChange?.Invoke(); }
    }  //���� ü��
    public float MaxHp { //�ִ� ü��
        get 
        {
            if (viewers == 0) maxHp = 80;
            int newMaxHp = 80 + (viewers / 100); //��û�ڼ� ��������� ü�� 1 ����

            if(newMaxHp - maxHp != 0) //���̰� ���� ���
            {
                currentHp += (newMaxHp - maxHp); //���� ü�� �׸�ŭ ����
                maxHp = newMaxHp; //���ο� �ִ�ü�� ����
            }

            return maxHp;
        }
        set 
        {
            maxHp = value;
            OnPlayerDataChange?.Invoke();
        }
    }

    public int Money
    {
        get { return money; }
        set { money = value; OnPlayerDataChange?.Invoke(); }
    }  //���� ��

    public int Energy
    {
        get { return energy; }
        set { energy = value; OnPlayerDataChange?.Invoke(); }
    } //���� ������

    public List<CardStruct> Deck { get; set; }

    //������ ���� �� �߻���ų �̺�Ʈ
    public event Action OnPlayerDataChange;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //�ٸ� ������ ������ �ڵ����� �����ǹǷ� ���� �ٲ���� ���� ȣ���ϱ�
    public void OnPlayerDataChanged()
    {
        OnPlayerDataChange?.Invoke();
    }


    public void InitPlayerData()
    {
        //�ʱ� ������ ���� �Լ�
        ChannelLevel = 1;
        Viewers = 0;
        CurrentHp = MaxHp;
        Money = 100;
        Energy = 3;

        List<CardStruct> cardList = GameData.Instance.CardList;
        Deck = new List<CardStruct>(){
            cardList[0],
            cardList[0],
            cardList[0],
            cardList[1],
            cardList[1],
            cardList[1],
            cardList[2],
            cardList[3]
        };

    }

    //������ �̺�Ʈ ��ȭ�� ���� �� ȣ��. viewer�� üũ�ؼ� �������ߴ��� Ȯ���ϰ�, ���������� ��� ���۵��� �ѹ��� ����
    public bool CheckLevelUp()
    {
        int newChannelLevel;

        if (viewers < 200) //Viewers ���� ���� ä�� ���� ����
        {
            newChannelLevel = 1;
        }
        else if (viewers < 700)
        {
            newChannelLevel = 2;
        }
        else if (viewers < 1300)
        {
            newChannelLevel = 3;
        }
        else if (viewers < 2000)
        {
            newChannelLevel = 4;
        }
        else
        {
            newChannelLevel = 5;
        }

        // ä�� ������ ����Ǿ����� Ȯ���� �����ϰ�, ����Ǿ����� ������ UI ��ȯ
        if (newChannelLevel != ChannelLevel)
        {
            ChannelLevel = newChannelLevel;

            if (channelLevel != 1 && channelLevel != 4) //������ �� (4 ����) ������ ���
            {
                Energy += 1;
            }

            return true; //������ �ߴٰ� ����
        }

        return false;
    }

}
