using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾ ������ ī�带 ����ִ� Ŭ����
//JSON ���Ϸ� ���̺��ϴ� �Լ��� �ʿ��� ��.

public class CardPlayerData : Singleton<CardPlayerData>
{
    public List<Card> PlayerDeck { get; set; } = new List<Card>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        JsonData dialogData = DataManager.Instance.LoadJson("PlayerDeck");
    }

    public void DiscardCard()
    {

    }
}
