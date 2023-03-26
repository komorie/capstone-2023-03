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


    }

    //Ư�� ��ȣ�� ī�带 �����´�.
    public Card GetCard(int index)
    {
        Card card;
        CardLibraryDic.TryGetValue(index, out card);
        return card;
    }
}
