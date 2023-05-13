using UnityEngine;

public class EnemySymbol : RoomSymbol
{
    public override void TalkStart()
    {
        UIManager.Instance.ShowUI("DialogUI")
        .GetComponent<DialogUI>()
        .Init(index, SelectOpen);
    }

    public void SelectOpen()
    {
        UIManager.Instance.ShowUI("SelectUI")
            .GetComponent<SelectUI>()
            .Init(
                "회유를 시도하시겠습니까?",
                TryNegotiate, 
                () => {
                    UIManager.Instance.ShowUI("DialogUI")
                    .GetComponent<DialogUI>()
                    .Init(index + Define.FIGHT_INDEX, FightEnd);
                } 
            );
    }

    // 
    public void TryNegotiate()
    {
        float random = Random.Range(0f, 1f);

        if (random < 0.5f && StageManager.Instance.NegoInLevel == false) 
        {
            UIManager.Instance.ShowUI("DialogUI").GetComponent<DialogUI>().Init(index + Define.NEGO_INDEX, NegotiateEnd);
        }
        else
        {
            UIManager.Instance.ShowUI("DialogUI").GetComponent<DialogUI>().Init(index + Define.NEGOFAIL_INDEX, FightEnd);
        }
    }

    public void Fight()
    {
        SoundManager.Instance.Play("Sounds/BattleBgm", Sound.Bgm);
        UIManager.Instance.ShowUI("BackGroundUI");
        UIManager.Instance.ShowUI("BattleUI",false).GetComponent<BattleUI>();
    }

    public void FightEnd()
    {
        PlayerData.Instance.Money += GameData.Instance.RewardDic[StageManager.Instance.Stage].money; 
        PlayerData.Instance.Viewers += GameData.Instance.RewardDic[StageManager.Instance.Stage].viewers;


        CardSelectUI cardSelectUI = UIManager.Instance.ShowUI("CardSelectUI").GetComponent<CardSelectUI>();
        cardSelectUI.SetCloseCallback(TalkEnd);
        cardSelectUI.BattleReward();
    }

    public void NegotiateEnd()
    {
        PlayerData.Instance.Money += GameData.Instance.RewardDic[StageManager.Instance.Stage].money / 2;
        PlayerData.Instance.Viewers += GameData.Instance.RewardDic[StageManager.Instance.Stage].viewers / 2;

        CardSelectUI cardSelectUI = UIManager.Instance.ShowUI("CardSelectUI").GetComponent<CardSelectUI>();
        cardSelectUI.SetCloseCallback(TalkEnd);
        cardSelectUI.NegoReward(index); 
    }

    public override void TalkEnd()
    {
        base.TalkEnd();
        if (PlayerData.Instance.CheckLevelUp())
        {
            UIManager.Instance.ShowUI("CardSelectUI")
                .GetComponent<CardSelectUI>()
                .LevelUpReward();
        }
    }
}