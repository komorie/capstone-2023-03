using DataStructs;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopData : Singleton<ShopData>
{


    //상점에서 파는 5개의 카드 리스트
    public List<CardStruct> ShopCardsList { get; set; } = new List<CardStruct>(5);
    
    //카드 리롤 시 드는 비용
    public int RerollCost { get; set; }
    
    //카드 제거 시 드는 비용
    public int DiscardCost { get; set; }

    public event Action OnDataChange;

    protected override void Awake()
    {
        base.Awake();
        ClearShopData();
    }

    private void OnEnable()
    {
        StageManager.Instance.OnStageClear += ClearShopData; //레벨 클리어 시 이거 실행해서 상점 데이터 초기화
    }
    private void OnDisable()
    {
        StageManager.Instance.OnStageClear -= ClearShopData;
    }

    public void DataChanged()
    {
        OnDataChange?.Invoke(); 
    }

    //상점 카드 초기화
    public void InitShopCardList()
    {

        ShopCardsList.Clear();

        //공격, 스킬 카드들을 쿼리.
        List<CardStruct> shopCardsPool = GameData.Instance.CardList
             .Where(card => (card.type == "Attack" || card.type == "Skill")
                 && card.attribute != "Normal" //기본 카드는 안나오게 수정
             ).ToList();

        //5개를 랜덤으로 가져오기
        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, shopCardsPool.Count);
            ShopCardsList.Add(shopCardsPool[index]);
            shopCardsPool.RemoveAt(index); //한번 나온 카드가 다시 나오지 않도록 제거
        }
    }

    //레벨 바뀔 때마다 상점 데이터 초기화
    public void ClearShopData()
    {

        InitShopCardList();

        RerollCost = 50;
        DiscardCost = 75;
    }

}
