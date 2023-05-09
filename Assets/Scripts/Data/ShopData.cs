using DataStructs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopData : MonoBehaviour
{


    //�������� �Ĵ� 5���� ī�� ����Ʈ
    public List<CardStruct> ShopCardsList { get; set; } = new List<CardStruct>(5);
    
    //ī�� ���� �� ��� ���
    public int RerollMoney { get; set; }
    
    //ī�� ���� �� ��� ���
    public int DiscardMoney { get; set; }

    public void Awake()
    {
        ClearShopData();
    }

    public void OnEnable()
    {
        StageManager.Instance.OnLevelClear += ClearShopData; //���� Ŭ���� �� �̰� �����ؼ� ���� ������ �ʱ�ȭ
    }
    public void OnDisable()
    {
        StageManager.Instance.OnLevelClear -= ClearShopData;
    }

    //���� �ٲ� ������ ���� ������ �ʱ�ȭ
    public void ClearShopData()
    {

        //����, ��ų ī����� ����.
        List<CardStruct> shopCardsPool = GameData.Instance.CardList
             .Where(card => (card.type == "Attack" || card.type == "Skill")
                 && card.attribute != "Normal" //�⺻ ī��� �ȳ����� ����
             ).ToList();

        //5���� �������� ��������
        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, shopCardsPool.Count);
            ShopCardsList.Add(shopCardsPool[index]); 
        }

        RerollMoney = 50;
        DiscardMoney = 75;
    }

}
