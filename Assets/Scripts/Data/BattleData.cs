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
    public float Shiled = 0;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        if (GameObject.Find("PlayerData") != null)
        {
            playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
            Deck = playerData.Deck;

            foreach (CardStruct card in Deck)
            {
                Origin_Deck.Add(card);
            }

            CurrentHealth = playerData.CurrentHp;
            MaximumHealth = playerData.MaxHp;
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
    void Update()
    {
        
    }


    
}
