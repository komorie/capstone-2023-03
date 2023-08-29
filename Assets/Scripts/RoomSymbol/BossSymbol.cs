using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSymbol : RoomSymbol
{
    public override void TalkStart()
    {
        UIManager.Instance.ShowUI("DialogUI")
            .GetComponent<DialogUI>()
            .Init(index, Fight); //대화가 끝나면 전투 UI 오픈

    }

    public void Fight()
    {
        GameObject Room = transform.parent.gameObject;
        SoundManager.Instance.Play("Sounds/BattleBgm", Sound.Bgm);
        UIManager.Instance.ShowUI("BackGroundUI");
        UIManager.Instance.ShowUI("BattleUI", false).GetComponent<BattleUI>().Init(index, Room.name, StageManager.Instance.StageLevel);
        BattleData.Instance.LoadData();
    }

    //전투 후 대화
    public void AfterFight()
    {
        //최종 스테이지면 그냥 종료
        if (StageManager.Instance.StageTheme == Define.ThemeType.Final)
        {
            UIManager.Instance.ShowUI("DialogUI")
                .GetComponent<DialogUI>()
                .Init(index + Define.BOSS_AFTER_INDEX, TalkEnd);
        }
        else
        {
            UIManager.Instance.ShowUI("DialogUI")
              .GetComponent<DialogUI>()
              .Init(index + Define.BOSS_AFTER_INDEX, SelectOpen); //대화가 끝나면 영입 제의 팝업 오픈 
        }
    }

    //영입 제의 팝업 오픈
    public void SelectOpen()
    {
        UIManager.Instance.ShowUI("SelectUI")
            .GetComponent<SelectUI>()
            .Init(
                "보스에게 크루원 영입 제의를 하시겠습니까?",
                AfterNego, //예 선택 시 영입 보상 호출
                FightEnd //아니오 선택 시 전투 보상 호출
            );
    }

    public void FightEnd() //전투 끝나고 영입 제의를 하지 않을 시 호출
    {

        //스탯을 ... 만큼 증가
        PlayerData.Instance.Money += GameData.Instance.RewardDic[StageManager.Instance.StageLevel + Define.BOSS_INDEX].money; 
        PlayerData.Instance.Viewers += GameData.Instance.RewardDic[StageManager.Instance.StageLevel + Define.BOSS_INDEX].viewers;

        //보상 카드 UI 닫을 시, TalkEnd 호출
        CardRewardUI cardSelectUI = UIManager.Instance.ShowUI("CardSelectUI").GetComponent<CardRewardUI>();
        cardSelectUI.SetCloseCallback(TalkEnd);
        cardSelectUI.BossBattleReward();
    }

    //영입 제의 했을 때 후속 대화
    public void AfterNego()
    {
        UIManager.Instance.ShowUI("DialogUI")
            .GetComponent<DialogUI>()
            .Init(index + Define.BOSS_NEGO_INDEX, NegotiateEnd); //대화가 끝나면 협상 보상 획득
    }


    public void NegotiateEnd() //영입 후 호출
    {
        //스탯을 ... 만큼 증가
        PlayerData.Instance.Money += GameData.Instance.RewardDic[StageManager.Instance.StageLevel + Define.BOSS_INDEX].money / 2;
        PlayerData.Instance.Viewers += GameData.Instance.RewardDic[StageManager.Instance.StageLevel + Define.BOSS_INDEX].viewers / 2;

        //보상 카드 UI 닫을 시, TalkEnd 호출
        CardRewardUI cardSelectUI = UIManager.Instance.ShowUI("CardSelectUI").GetComponent<CardRewardUI>();
        cardSelectUI.SetCloseCallback(TalkEnd);
        cardSelectUI.BossNegoReward();
    }

    public override void TalkEnd()
    {
        base.TalkEnd();
        if(PlayerData.Instance.CheckLevelUp())
        {
            UIManager.Instance.ShowUI("CardSelectUI")
                .GetComponent<CardRewardUI>()
                .LevelUpReward();
        }
        StageManager.Instance.NotifyLevelClear();
    }
}
