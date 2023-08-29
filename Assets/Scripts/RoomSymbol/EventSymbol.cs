using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventSymbol : RoomSymbol
{
    public override void TalkStart()
    {

        if(index == 307) //동료 획득 인덱스가 나오면
        {
            if(PlayerData.Instance.HasPartner[StageManager.Instance.StageLevel - 1] == false) //현재 스테이지서 얻을 수 있는 동료를 획득 안한 경우에만 카드 획득
            {
                index = 307 + StageManager.Instance.StageLevel - 1;
            }
            else
            {
                index = Random.Range(301, 307); //아닌 경우 랜덤으로 이벤트 다시 뽑기
            }    
        }

        base.TalkStart();
    }

    public override void TalkEnd()
    {
        base.TalkEnd();

        switch (index)
        {
            case 300: //카드 획득 이벤트 ㅇ
                UIManager.Instance.ShowUI("CardSelectUI").GetComponent<CardRewardUI>().EventReward();
                break;
            case 301: //애청자 구매 이벤트 ㅇ

                int takeViewers; //받을 애청자
                int giveMoney; //줄 돈

                takeViewers = GameData.Instance.RewardDic[StageManager.Instance.StageLevel].viewers * 2;
                giveMoney = GameData.Instance.RewardDic[StageManager.Instance.StageLevel].money; //대충 현 스테이지의 보상만큼 가져옴

                if(giveMoney <= PlayerData.Instance.Money) //돈 안부족하면 선택창 띄우고 교환 실행
                {
                    UIManager.Instance.ShowUI("SelectUI", false).GetComponent<SelectUI>().Init(
                        $"돈을 {giveMoney}만큼 주고 애청자를 {takeViewers}만큼 받아오시겠습니까?",
                        () => {
                            PlayerData.Instance.Viewers += takeViewers;
                            PlayerData.Instance.Money -= giveMoney;
                            if (PlayerData.Instance.CheckLevelUp()) //레벨업 체크
                            {
                                UIManager.Instance.ShowUI("CardSelectUI")
                                    .GetComponent<CardRewardUI>()
                                    .LevelUpReward();
                            } 
                        }, //예 선택하면 돈 감소, 애청자 증가
                        null //아니오 선택하면 잉어킹 튀어오르기
                    );
                }

                break;
            case 302: //애청자 판매 이벤트 ㅇ

                int giveViewers;
                int takeMoney;

                giveViewers = GameData.Instance.RewardDic[StageManager.Instance.StageLevel].viewers;
                takeMoney = GameData.Instance.RewardDic[StageManager.Instance.StageLevel].money * 2;

                if (giveViewers <= PlayerData.Instance.Viewers) //애청자 안부족하면 선택창 띄우고 교환 실행
                {
                    UIManager.Instance.ShowUI("SelectUI", false).GetComponent<SelectUI>().Init(
                        $"애청자를 {giveViewers}만큼 주고 돈을 {takeMoney}만큼 받아오시겠습니까?",
                        () => {
                            PlayerData.Instance.Viewers -= giveViewers;
                            PlayerData.Instance.Money += takeMoney;
                        }, //예 선택하면 애청자 감소, 돈 상승
                        null
                    );
                }
                break;
            case 303: //카드 버림 이벤트 ㅇ
                UIManager.Instance.ShowUI("LibraryUI").GetComponent<LibraryUI>().Init(LibraryMode.EventDiscard);
                break;
            case 304: //습격당함 ㅇ
                
                int minusHp = (int)(PlayerData.Instance.MaxHp * 0.2f); //HP 감소: 현재 HP에서 MaxHp의 2할 감소
                PlayerData.Instance.CurrentHp -= minusHp;

                if(PlayerData.Instance.CurrentHp <= 0)
                {
                    UIManager.Instance.ShowUI("GameOverUI"); //체력 0이면 게임 오버
                }
                else
                {
                    UIManager.Instance.ShowUI("ConfirmUI").GetComponent<ConfirmUI>().Init($"습격당해 체력이\n {minusHp} 만큼 감소했다!"); //아니면 체력 줄었다고 알림
                }

                break;
            case 305: //체력 조공 ㅇ

                int plusHp = (int)Mathf.Min(
                    PlayerData.Instance.MaxHp * 0.2f,
                    PlayerData.Instance.MaxHp - PlayerData.Instance.CurrentHp 
                ); //HP 증가: 현재 HP에서 MaxHp의 2할 회복
                
                PlayerData.Instance.CurrentHp += plusHp;
                UIManager.Instance.ShowUI("ConfirmUI").GetComponent<ConfirmUI>().Init($"시청자들과 잠시 쉰 덕분에\n 체력이 {plusHp} 만큼 회복했다!");
                break;
            case 306: //돈 조공 ㅇ

                int deltaMoney;
                float random = Random.Range(0f, 1f);

                if (random < 0.63f)
                {
                    deltaMoney = 100;
                }
                else if (random < 0.95f)
                {
                    deltaMoney = 150;
                }
                else
                {
                    deltaMoney = 300;
                }

                PlayerData.Instance.Money += deltaMoney; //돈 증가
                UIManager.Instance.ShowUI("ConfirmUI").GetComponent<ConfirmUI>().Init($"시청자들이\n 돈을 {deltaMoney}원만큼 조공했다!"); 
                break;
            case 307: //이벤트 동료 1
            case 308: //이벤트 동료 2
            case 309: //이벤트 동료 3
                UIManager.Instance.ShowUI("CardSelectUI").GetComponent<CardRewardUI>().PartnerReward();
                break;
        }
    }
}
