using DataStructs;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopData : Singleton<ShopData>
{

    private List<CardStruct> shopCardsPool; //�������� �Ĵ� ī����� Ǯ    

    //�������� �Ĵ� 5���� ī�� ����Ʈ
    public List<CardStruct> ShopCardsList { get; set; } = new List<CardStruct>(5);

    public List<int> Prices { get; set; } = new List<int>(5);

    //ī�� ���� �� ��� ���
    public int RerollCost { get; set; }

    //ī�� ���� �� ��� ���
    public int DiscardCost{ get; set; }

    public event Action OnDataChange;

    protected override void Awake()
    {
        base.Awake();

        //����, ��ų ī����� ������ Ǯ ����
        shopCardsPool = GameData.Instance.CardList
             .Where(card => (card.type == "Attack" || card.type == "Skill") && card.attribute != "Normal") //�⺻ ī��� �ȳ����� ����
             .ToList();

        ClearShopData();
    }

    private void OnEnable()
    {
        Stage.Instance.OnStageClear += ClearShopData; //���� Ŭ���� �� �̰� �����ؼ� ���� ������ �ʱ�ȭ
    }
    private void OnDisable()
    {
        Stage.Instance.OnStageClear -= ClearShopData;
    }

    //���� ī�� �ʱ�ȭ
    public void InitShopData()
    {

        ShopCardsList.Clear();
        Prices.Clear();

        //5���� �������� ��������
        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, shopCardsPool.Count);
            ShopCardsList.Add(shopCardsPool[index]);
            shopCardsPool.RemoveAt(index); //�ѹ� ���� ī�尡 �ٽ� ������ �ʵ��� ����

            switch (ShopCardsList[i].rarity) //���� ����
            {
                case 0:
                    Prices.Add(Random.Range(8, 11) * 5); 
                    break; //40, 45, 50
                case 1:
                    Prices.Add(Random.Range(13, 16) * 5); 
                    break; //65, 70, 75
                case 2:
                    Prices.Add(Random.Range(24, 30) * 5); 
                    break; //120, 125... 145, 150
                default:
                    break;
            }
        }

        foreach (var item in ShopCardsList)
        {
            shopCardsPool.Add(item);
        }

    }

    //�ٸ� Shop ��� ������Ʈ�� ���� �ű�� �� ���� �� ���� �ѵ�...
    public void Purchase(CardStruct card) //�ش� ī�� ����
    {

        int index = ShopCardsList.FindIndex(x => card.index == x.index); //���� ����Ʈ���� �ε����� ���� ī���� �ε��� ��������


        // �÷��̾ ����� ���� ������ �ִ��� Ȯ��
        if (PlayerData.Instance.Money >= Prices[index])
        {
            PlayerData.Instance.Money -= Prices[index]; // ���� ����

            PlayerData.Instance.AddCard(ShopCardsList[index]); // ī�带 ���� �߰�

            ShopCardsList.RemoveAt(index);  //�������� ī�� ����
            Prices.RemoveAt(index); //���� ����
            OnDataChange.Invoke(); //���� ������ ���� �˷��� UI ���ΰ�ħ�ϵ���!
        }
    }

    public void Discard(CardStruct card) //�ش� ī�� ����
    {
        int newMoney = PlayerData.Instance.Money - DiscardCost;
        if (newMoney >= 0) //���� ���� ��츸
        {
            PlayerData.Instance.RemoveCard(card); //�ش� ī�带 ������
            PlayerData.Instance.Money = newMoney; //���� ��븸ŭ �÷��̾� ������ �����ϱ�
            DiscardCost += 25; //���� ��� 25 �߰�
            OnDataChange.Invoke(); //���� ������ ���� �˷��� UI ���ΰ�ħ�ϵ���!
        }
    }

    public void RerollShop()
    {
        if(PlayerData.Instance.Money >= RerollCost)
        {
            RerollCost += 25;
            InitShopData();
            OnDataChange.Invoke();
        }
    }

    //���� �ٲ� ������ ���� ������ �ʱ�ȭ
    public void ClearShopData()
    {

        InitShopData();

        RerollCost = 50;
        DiscardCost = 75;
    }

}
