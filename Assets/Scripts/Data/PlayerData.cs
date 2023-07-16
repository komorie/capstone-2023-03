using System;
using System.Collections.Generic;
using DataStructs;
using UnityEngine;

//다른 클래스들에서는 playData에 접근해서 저장된 데이터를 사용하거나, 정보가 변화 시 playData를 실시간으로 갱신함.
public class PlayerData : Singleton<PlayerData>
{

    private int channelLevel;
    private int viewers;
    private float currentHp;
    private float maxHp;
    private int money;
    private int energy;

    public bool[] HasPartner { get; set; } = new bool[3]; //해당 스테이지의 동료 카드 얻었는지

    public int ChannelLevel {
        get { return channelLevel; }
        set { channelLevel = value; OnPlayerDataChange?.Invoke(); }
    }  //채널 레벨

    public int Viewers {
        get { return viewers; }
        set { viewers = value; OnPlayerDataChange?.Invoke(); }
    }  //애청자 수

    public float CurrentHp
    {
        get { return currentHp; }
        set { currentHp = value; OnPlayerDataChange?.Invoke(); }
    }  //현재 체력
    public float MaxHp { //최대 체력
        get 
        {
            if (viewers == 0) maxHp = 80;
            int newMaxHp = 80 + (viewers / 100); //애청자수 백단위마다 체력 1 증가

            if(newMaxHp - maxHp != 0) //차이가 생긴 경우
            {
                currentHp += (newMaxHp - maxHp); //현재 체력 그만큼 증가
                maxHp = newMaxHp; //새로운 최대체력 설정
            }

            return maxHp;
        }
        set 
        {
            maxHp = value;
            OnPlayerDataChange?.Invoke();
        }
    }

    public int Money
    {
        get { return money; }
        set { money = value; OnPlayerDataChange?.Invoke(); }
    }  //현재 돈

    public int Energy
    {
        get { return energy; }
        set { energy = value; OnPlayerDataChange?.Invoke(); }
    } //현재 에너지

    public List<CardStruct> Deck { get; set; }

    //데이터 변경 시 발생시킬 이벤트
    public event Action OnPlayerDataChange;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //다른 데이터 변경은 자동으로 감지되므로 덱이 바뀌었을 때만 호출하기
    public void OnPlayerDataChanged()
    {
        OnPlayerDataChange?.Invoke();
    }


    public void InitPlayerData()
    {
        //초기 데이터 설정 함수
        ChannelLevel = 1;
        Viewers = 0;
        CurrentHp = MaxHp;
        Money = 100;
        Energy = 3;

        List<CardStruct> cardList = GameData.Instance.CardList;
        Deck = new List<CardStruct>(){
            cardList[0],
            cardList[0],
            cardList[0],
            cardList[1],
            cardList[1],
            cardList[1],
            cardList[2],
            cardList[3]
        };

    }

    //전투나 이벤트 대화가 끝날 때 호출. viewer를 체크해서 레벨업했는지 확인하고, 레벨업했을 경우 동작들을 한번에 수행
    public bool CheckLevelUp()
    {
        int newChannelLevel;

        if (viewers < 200) //Viewers 값에 따라 채널 레벨 설정
        {
            newChannelLevel = 1;
        }
        else if (viewers < 700)
        {
            newChannelLevel = 2;
        }
        else if (viewers < 1300)
        {
            newChannelLevel = 3;
        }
        else if (viewers < 2000)
        {
            newChannelLevel = 4;
        }
        else
        {
            newChannelLevel = 5;
        }

        // 채널 레벨이 변경되었는지 확인후 변경하고, 변경되었으면 레벨업 UI 소환
        if (newChannelLevel != ChannelLevel)
        {
            ChannelLevel = newChannelLevel;

            if (channelLevel != 1 && channelLevel != 4) //레벨업 시 (4 제외) 에너지 상승
            {
                Energy += 1;
            }

            return true; //레벨업 했다고 리턴
        }

        return false;
    }

}
