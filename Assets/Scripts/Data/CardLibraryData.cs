using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1. ��ü ī�� �����͸� ��� �ִ� Ŭ����.
//2. �ٸ� UI ��� �����, Ư�� ī�� �������� �Լ� ����.

public class CardLibraryData : Singleton<CardLibraryData>
{
    //��� ī�尡 ��� ��ųʸ�
    private Dictionary<int, Card> CardLibraryDic { get; set; } = new Dictionary<int, Card>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        JsonData CardLibraryData = DataManager.Instance.LoadJson("CardLibrary");

        for (int i = 0; i < CardLibraryData.Count; i++)
        {
            Card card = new Card(
                int.Parse(CardLibraryData[i]["index"].ToString()),
                CardLibraryData[i]["name"].ToString(),
                CardLibraryData[i]["description"].ToString(),
                int.Parse(CardLibraryData[i]["cost"].ToString()),
                int.Parse(CardLibraryData[i]["damage"].ToString()),
                int.Parse(CardLibraryData[i]["attackCount"].ToString()),
                CardLibraryData[i]["attackRange"].ToString(),
                CardLibraryData[i]["attribute"]?.ToString(),
                CardLibraryData[i]["damagetype"]?.ToString(),
                CardLibraryData[i]["rarity"].ToString()
            );

            CardLibraryDic.Add(card.index, card);    

        }


    }

    //Ư�� ��ȣ�� ī�带 �����´�.
    public Card GetCard(int index)
    {
        Card card;
        CardLibraryDic.TryGetValue(index, out card);
        return card;
    }
}
