using DataStructs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardRewardData : Singleton<CardRewardData> //���� ȭ�鿡 ������ ī�� ����Ʈ ������
{
    public List<CardStruct> Rewards { get; set; } = new List<CardStruct>(); //�������� ���� ������ ī�� �����͵�

    IEnumerable<CardStruct> BattleRewardCardsPool; //���� �������� ���� �� �ִ� ��ü ī��� ������ ���
    List<CardStruct> BattleRarity0Cards { get; set; } //�븻 ī�� ������ Ǯ
    List<CardStruct> BattleRarity1Cards { get; set; } //���� ī�� ������ Ǯ
    List<CardStruct> BattleRarity2Cards { get; set; } //����ũ ī�� ������ Ǯ

    List<CardStruct> ViewerCardsPool { get; set; } //������ ���� ī��� ī��� Ǯ
    List<CardStruct> NegoCardsPool { get; set; } //�װ� ���� ī��� Ǯ

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        //ó�� �⵿ �� ���� ī�� Ǯ �ʱ�ȭ
        BattleRewardCardsPool = GameData.Instance.CardList
             .Where(
                card =>
                (card.type == "Attack" ||
                card.type == "Skill") &&
                card.attribute != "Normal" //�⺻ ī��� �ȳ����� ����
             );

        BattleRarity0Cards = BattleRewardCardsPool.Where(card => card.rarity == 0).ToList();
        BattleRarity1Cards = BattleRewardCardsPool.Where(card => card.rarity == 1).ToList();
        BattleRarity2Cards = BattleRewardCardsPool.Where(card => card.rarity == 2).ToList();

        ViewerCardsPool = GameData.Instance.CardList.Where(card => card.type == "Viewer").ToList();
        NegoCardsPool = new List<CardStruct>();
    }


    //���� �������� ���� ��� ȣ��
    public void BattleReward()
    {
        Rewards.Clear();

        for (int i = 0; i < 3; i++)
        {
            float random = Random.Range(0f, 1f);

            //Ȯ���� ���� �븻, ����, ����ũ ī��Ǯ �߿��� �� ī�带 ��� ���� ī��� ����
            if (random < 0.63f)
            {
                int index = Random.Range(0, BattleRarity0Cards.Count);
                Rewards.Add(BattleRarity0Cards[index]);
                BattleRarity0Cards.RemoveAt(index); //�ѹ� ���� ī�尡 �ٽ� ������ �ʵ��� ����
            }
            else if (random < 0.95f)
            {
                int index = Random.Range(0, BattleRarity1Cards.Count);
                Rewards.Add(BattleRarity1Cards[index]);
                BattleRarity1Cards.RemoveAt(index);
            }
            else
            {
                int index = Random.Range(0, BattleRarity2Cards.Count);
                Rewards.Add(BattleRarity2Cards[index]);
                BattleRarity2Cards.RemoveAt(index);
            }
        }

        // Rewards ����Ʈ�� ����� ī����� �� Ǯ�� �ٽ� �߰�
        foreach (var card in Rewards)
        {
            if (card.rarity == 0)
            {
                BattleRarity0Cards.Add(card);
            }
            else if (card.rarity == 1)
            {
                BattleRarity1Cards.Add(card);
            }
            else if (card.rarity == 2)
            {
                BattleRarity2Cards.Add(card);
            }
        }
    }

    //�������� ���� ����
    public void BossBattleReward()
    {
        Rewards.Clear();

        for (int i = 0; i < 3; i++)
        {
            float random = Random.Range(0f, 1f);

            //Ȯ���� ���� ����, ����ũ ī��Ǯ �߿��� �� ī�带 ��� ���� ī��� ����
            if (random < 0.63f)
            {
                int index = Random.Range(0, BattleRarity1Cards.Count);
                Rewards.Add(BattleRarity1Cards[index]);
                BattleRarity1Cards.RemoveAt(index); //�ѹ� ���� ī�尡 �ٽ� ������ �ʵ��� ����
            }
            else
            {
                int index = Random.Range(0, BattleRarity2Cards.Count);
                Rewards.Add(BattleRarity2Cards[index]);
                BattleRarity2Cards.RemoveAt(index); //�ѹ� ���� ī�尡 �ٽ� ������ �ʵ��� ����
            }
        }

        // Rewards ����Ʈ�� ����� ī����� �� Ǯ�� �ٽ� �߰�
        foreach (var card in Rewards)
        {
            if (card.rarity == 0)
            {
                BattleRarity0Cards.Add(card);
            }
            else if (card.rarity == 1)
            {
                BattleRarity1Cards.Add(card);
            }
            else if (card.rarity == 2)
            {
                BattleRarity2Cards.Add(card);
            }
        }
    }

    //���� �� ī�� ����
    public void NegoReward(int index)
    {
        Rewards.Clear();

        //Ÿ���� ����� ī�� ��, ���ʹ� �ɺ��� �ε����� ���� ī�带 �����´�.
        switch (index)
        {
            //�Ϲ� ����̸� ī���� Ÿ���� ��� ī����� ī����� ��������, '���������� �´�' ��� ī�带 ������. 3���������� mob3 Ÿ���� ī��鸸 ������.
            //���÷�, 3���������� mob3 Ÿ���� ī��鸸 �������� �ȴ�.
            case 0:
                NegoCardsPool = GameData.Instance.CardList
                    .Where(card => card.type == $"Mob{Stage.Instance.StageLevel}")
                    .ToList();
                break;
            //�� ���� ���, index�� ���� Theme�� ��ȣ�� ����. Theme�� �ش��ϴ� Enum�� �ؽ�Ʈ(���� ��� index�� 1�̸� Define.ThemeType.Pirate�� ����)
            //�� ��������, ���� ���������� ��ȣ�� ���ϸ�, ���� �׸��� 1���������� ��� Pirate1 �̶�� ���ڿ��� ����� ����.
            //card�� type�� Pirate1�� ī����� �����ͼ� Ǯ�� �߰��Ѵ�.
            default:
                NegoCardsPool = GameData.Instance.CardList
                    .Where(card => card.type == $"{Stage.Instance.StageTheme}{Stage.Instance.StageLevel}")
                    .ToList();
                break;
        }

        Rewards.Add(NegoCardsPool[Random.Range(0, NegoCardsPool.Count)]);
    }

    //���� ���� �� ī�� ����
    public void BossNegoReward()
    {
        Rewards.Clear();

        //Ÿ���� ������ ī�� ��, ���ʹ��� �ε�����°�� ī�带 �����´�.
        Rewards.Add(GameData.Instance.CardList.Where(card => card.type == $"{Stage.Instance.StageTheme}Boss").ElementAtOrDefault(0));
    }

    //�̺�Ʈ ī�� ȹ�� �������� ���� ��� ȣ��
    public void EventReward()
    {
        Rewards.Clear();

        for (int i = 0; i < 3; i++)
        {
            float random = Random.Range(0f, 1f);

            //Ȯ���� ���� �븻, ����, ����ũ ī��Ǯ �߿��� �� ī�带 ��� ���� ī��� ����
            if (random < 0.63f)
            {
                int index = Random.Range(0, BattleRarity0Cards.Count);
                Rewards.Add(BattleRarity0Cards[index]);
                BattleRarity0Cards.RemoveAt(index); //�ѹ� ���� ī�尡 �ٽ� ������ �ʵ��� ����
            }
            else if (random < 0.95f)
            {
                int index = Random.Range(0, BattleRarity1Cards.Count);
                Rewards.Add(BattleRarity1Cards[index]);
                BattleRarity1Cards.RemoveAt(index);
            }
            else
            {
                int index = Random.Range(0, BattleRarity2Cards.Count);
                Rewards.Add(BattleRarity2Cards[index]);
                BattleRarity2Cards.RemoveAt(index);
            }
        }

        // Rewards ����Ʈ�� ����� ī����� �� Ǯ�� �ٽ� �߰�
        foreach (var card in Rewards)
        {
            if (card.rarity == 0)
            {
                BattleRarity0Cards.Add(card);
            }
            else if (card.rarity == 1)
            {
                BattleRarity1Cards.Add(card);
            }
            else if (card.rarity == 2)
            {
                BattleRarity2Cards.Add(card);
            }
        }

    }

    //�̺�Ʈ ���� ī�� ȹ��
    public void PartnerReward()
    {
        Rewards.Clear();

        //Ÿ���� ��Ʈ���� ī�� ��, ���� ���������� �´� ī�带 ��������.
        Rewards.Add(GameData.Instance.CardList.Where(card => card.type == $"Partner{Stage.Instance.StageLevel}").ElementAtOrDefault(0));

        //���� ī�� ȹ���Ѱɷ� ����
        PlayerData.Instance.HasPartner[Stage.Instance.StageLevel - 1] = true;
    }


    //���� �� �� ī�� ����
    public void LevelUpReward()
    {
        Rewards.Clear();

        List<int> rewardCardsIndexes = Define.GenerateRandomNumbers(0, ViewerCardsPool.Count, 3); // ������ ���� ī�� Ǯ �߿� �������� �ε��� 3�� �̱�
        for (int i = 0; i < rewardCardsIndexes.Count; i++)
        {
            Rewards.Add(ViewerCardsPool[rewardCardsIndexes[i]]); //3���� �ε����� �ش��ϴ� ��û�� ī����� ���� ī����� �߰�
        }
    }
}
