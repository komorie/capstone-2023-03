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

    private List<CardStruct> shopCardsPool; //상점에서 파는 카드들의 풀    

    //상점에서 파는 5개의 카드 리스트
    public List<CardStruct> ShopCardsList { get; set; } = new List<CardStruct>(5);

    public List<int> Prices { get; set; } = new List<int>(5);

    //카드 리롤 시 드는 비용
    public int RerollCost { get; set; }

    //카드 제거 시 드는 비용
    public int DiscardCost{ get; set; }

    public event Action OnDataChange;

    protected override void Awake()
    {
        base.Awake();

        //공격, 스킬 카드들을 가져올 풀 쿼리
        shopCardsPool = GameData.Instance.CardList
             .Where(card => (card.type == "Attack" || card.type == "Skill") && card.attribute != "Normal") //기본 카드는 안나오게 수정
             .ToList();

        ClearShopData();
    }

    private void OnEnable()
    {
        Stage.Instance.OnStageClear += ClearShopData; //레벨 클리어 시 이거 실행해서 상점 데이터 초기화
    }
    private void OnDisable()
    {
        Stage.Instance.OnStageClear -= ClearShopData;
    }

    //상점 카드 초기화
    public void InitShopData()
    {

        ShopCardsList.Clear();
        Prices.Clear();

        //5개를 랜덤으로 가져오기
        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, shopCardsPool.Count);
            ShopCardsList.Add(shopCardsPool[index]);
            shopCardsPool.RemoveAt(index); //한번 나온 카드가 다시 나오지 않도록 제거

            switch (ShopCardsList[i].rarity) //가격 설정
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

    //다른 Shop 기능 오브젝트를 만들어서 옮기는 게 나을 것 같긴 한데...
    public void Purchase(CardStruct card) //해당 카드 구매
    {

        int index = ShopCardsList.FindIndex(x => card.index == x.index); //상점 리스트에서 인덱스가 같은 카드의 인덱스 가져오기


        // 플레이어가 충분한 돈을 가지고 있는지 확인
        if (PlayerData.Instance.Money >= Prices[index])
        {
            PlayerData.Instance.Money -= Prices[index]; // 돈을 지불

            PlayerData.Instance.AddCard(ShopCardsList[index]); // 카드를 덱에 추가

            ShopCardsList.RemoveAt(index);  //상점에서 카드 삭제
            Prices.RemoveAt(index); //가격 삭제
            OnDataChange.Invoke(); //상점 데이터 변경 알려서 UI 새로고침하도록!
        }
    }

    public void Discard(CardStruct card) //해당 카드 제거
    {
        int newMoney = PlayerData.Instance.Money - DiscardCost;
        if (newMoney >= 0) //돈이 남은 경우만
        {
            PlayerData.Instance.RemoveCard(card); //해당 카드를 버리기
            PlayerData.Instance.Money = newMoney; //제거 비용만큼 플레이어 돈에서 차감하기
            DiscardCost += 25; //삭제 비용 25 추가
            OnDataChange.Invoke(); //상점 데이터 변경 알려서 UI 새로고침하도록!
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

    //레벨 바뀔 때마다 상점 데이터 초기화
    public void ClearShopData()
    {

        InitShopData();

        RerollCost = 50;
        DiscardCost = 75;
    }

}
