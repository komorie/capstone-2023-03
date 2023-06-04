using System.Collections;
using System.Collections.Generic;
using DataStructs;
using UnityEngine;

public class BattleData : Singleton<BattleData>
{
    public List<CardStruct> Origin_Deck = new List<CardStruct>();
    public List<CardStruct> Deck = new List<CardStruct>();
    public List<CardStruct> Hand = new List<CardStruct>();
    public List<CardStruct> Trash = new List<CardStruct>();

    public PlayerData playerData;

    public int CurrentEnergy = 3;
    public int MaxEnergy = 3;
    public int UseEnergy = 0;
    public int CurrentTurn = 0;
    public int MaxHand = 10;
    public int StartHand = 5;

    public bool IsAlive = true;
    public float CurrentHealth = 100;
    public float MaximumHealth = 100;
    public float CurrentHealthPercentage
    {
        get
        {
            return (CurrentHealth / MaximumHealth) * 100;
        }
    }
    public float Shield = 0;
    public int Str = 0; // 힘
    public int Int = 0; // 마력
    public int weak = 0; // 취약
    public int crack = 0; // 균열
    public int drained = 0; // 탈진
    public bool stun = false; // 기절
    public bool restraint = false; // 속박
    public bool blind = false; // 암흑
    public bool confusion = false; // 혼란

    public bool burn = false; // 발화 사용 여부

    public int SelectedEnemy = 0;// 선택된 적

    public CardStruct LastUse;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        if (GameObject.Find("PlayerData") != null)
        {
            playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
            foreach (CardStruct card in playerData.Deck)
            {
                Deck.Add(card);
            }

            foreach (CardStruct card in Deck)
            {
                Origin_Deck.Add(card);
            }

            CurrentHealth = playerData.CurrentHp;
            MaximumHealth = playerData.MaxHp;
            MaxEnergy = playerData.Energy;
            CurrentEnergy = MaxEnergy;
            CurrentTurn = 0;
            IsAlive = true;

        }
        else
        {
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

        
    }

    // Update is called once per frame

    public void LoadData() {
        Deck.Clear();
        Origin_Deck.Clear();
        Hand.Clear();
        Trash.Clear();
        playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
        foreach (CardStruct card in playerData.Deck)
        {
            Deck.Add(card);
        }

        foreach (CardStruct card in Deck)
        {
            Origin_Deck.Add(card);
        }

        CurrentHealth = playerData.CurrentHp;
        MaximumHealth = playerData.MaxHp;
        MaxEnergy = playerData.Energy;
        CurrentEnergy = MaxEnergy;
        UseEnergy = 0;
        CurrentTurn = 0;
        IsAlive = true;
        Shield = 0;
        Str = 0;
        Int = 0;
        weak = 0;
        crack = 0;
        drained = 0;
        stun = false;
        restraint = false;
        blind = false;
        confusion = false;
        SelectedEnemy = 0;
}

    
}
