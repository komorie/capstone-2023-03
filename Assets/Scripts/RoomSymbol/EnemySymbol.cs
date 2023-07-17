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
                    .Init(index + Define.FIGHT_INDEX, Fight);
                } 
            );
    }

    // 
    public void TryNegotiate()
    {
        float random = Random.Range(0f, 1f);

        if (random < (0.5f + PlayerData.Instance.Viewers / 10000.0f)) 
        {
            UIManager.Instance.ShowUI("DialogUI").GetComponent<DialogUI>().Init(index + Define.NEGO_INDEX, NegotiateEnd);
        }
        else
        {
            UIManager.Instance.ShowUI("DialogUI").GetComponent<DialogUI>().Init(index + Define.NEGOFAIL_INDEX, Fight);
        }
    }

    public void Fight()
    {
        GameObject Room = transform.parent.gameObject;
        SoundManager.Instance.Play("Sounds/BattleBgm", Sound.Bgm);
        UIManager.Instance.ShowUI("BackGroundUI");
        UIManager.Instance.ShowUI("BattleUI",false).GetComponent<BattleUI>().Init(index, Room.name, StageManager.Instance.StageLevel);
        BattleData.Instance.LoadData();
    }

    public void FightEnd()
    {
        PlayerData.Instance.Money += GameData.Instance.RewardDic[StageManager.Instance.StageLevel].money; 
        PlayerData.Instance.Viewers += GameData.Instance.RewardDic[StageManager.Instance.StageLevel].viewers;


        CardSelectUI cardSelectUI = UIManager.Instance.ShowUI("CardSelectUI").GetComponent<CardSelectUI>();
        cardSelectUI.SetCloseCallback(TalkEnd);
        cardSelectUI.BattleReward();
    }

    public void NegotiateEnd()
    {
        PlayerData.Instance.Money += GameData.Instance.RewardDic[StageManager.Instance.StageLevel].money / 2;
        PlayerData.Instance.Viewers += GameData.Instance.RewardDic[StageManager.Instance.StageLevel].viewers / 2;

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