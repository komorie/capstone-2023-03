using DataStructs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardRewardData : Singleton<CardRewardData> //보상 화면에 나오는 카드 리스트 데이터
{
    public List<CardStruct> Rewards { get; set; } = new List<CardStruct>(); //보상으로 선택 가능한 카드 데이터들

    IEnumerable<CardStruct> BattleRewardCardsPool; //전투 보상으로 나올 수 있는 전체 카드들 쿼리한 결과
    List<CardStruct> BattleRarity0Cards { get; set; } //노말 카드 데이터 풀
    List<CardStruct> BattleRarity1Cards { get; set; } //레어 카드 데이터 풀
    List<CardStruct> BattleRarity2Cards { get; set; } //유니크 카드 데이터 풀

    List<CardStruct> ViewerCardsPool { get; set; } //레벨업 보상 카드들 카드들 풀
    List<CardStruct> NegoCardsPool { get; set; } //네고 보상 카드들 풀

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        //처음 기동 시 보상 카드 풀 초기화
        BattleRewardCardsPool = GameData.Instance.CardList
             .Where(
                card =>
                (card.type == "Attack" ||
                card.type == "Skill") &&
                card.attribute != "Normal" //기본 카드는 안나오게 수정
             );

        BattleRarity0Cards = BattleRewardCardsPool.Where(card => card.rarity == 0).ToList();
        BattleRarity1Cards = BattleRewardCardsPool.Where(card => card.rarity == 1).ToList();
        BattleRarity2Cards = BattleRewardCardsPool.Where(card => card.rarity == 2).ToList();

        ViewerCardsPool = GameData.Instance.CardList.Where(card => card.type == "Viewer").ToList();
        NegoCardsPool = new List<CardStruct>();
    }


    //전투 보상으로 띄우는 경우 호출
    public void BattleReward()
    {
        Rewards.Clear();

        for (int i = 0; i < 3; i++)
        {
            float random = Random.Range(0f, 1f);

            //확률에 따라 노말, 레어, 유니크 카드풀 중에서 한 카드를 골라 보상 카드로 지정
            if (random < 0.63f)
            {
                int index = Random.Range(0, BattleRarity0Cards.Count);
                Rewards.Add(BattleRarity0Cards[index]);
                BattleRarity0Cards.RemoveAt(index); //한번 나온 카드가 다시 나오지 않도록 제거
            }
            else if (random < 0.95f)
            {
                int index = Random.Range(0, BattleRarity1Cards.Count);
                Rewards.Add(BattleRarity1Cards[index]);
                BattleRarity1Cards.RemoveAt(index);
            }
            else
            {
                int index = Random.Range(0, BattleRarity2Cards.Count);
                Rewards.Add(BattleRarity2Cards[index]);
                BattleRarity2Cards.RemoveAt(index);
            }
        }

        // Rewards 리스트에 저장된 카드들을 각 풀에 다시 추가
        foreach (var card in Rewards)
        {
            if (card.rarity == 0)
            {
                BattleRarity0Cards.Add(card);
            }
            else if (card.rarity == 1)
            {
                BattleRarity1Cards.Add(card);
            }
            else if (card.rarity == 2)
            {
                BattleRarity2Cards.Add(card);
            }
        }
    }

    //보스와의 전투 보상
    public void BossBattleReward()
    {
        Rewards.Clear();

        for (int i = 0; i < 3; i++)
        {
            float random = Random.Range(0f, 1f);

            //확률에 따라 레어, 유니크 카드풀 중에서 한 카드를 골라 보상 카드로 지정
            if (random < 0.63f)
            {
                int index = Random.Range(0, BattleRarity1Cards.Count);
                Rewards.Add(BattleRarity1Cards[index]);
                BattleRarity1Cards.RemoveAt(index); //한번 나온 카드가 다시 나오지 않도록 제거
            }
            else
            {
                int index = Random.Range(0, BattleRarity2Cards.Count);
                Rewards.Add(BattleRarity2Cards[index]);
                BattleRarity2Cards.RemoveAt(index); //한번 나온 카드가 다시 나오지 않도록 제거
            }
        }

        // Rewards 리스트에 저장된 카드들을 각 풀에 다시 추가
        foreach (var card in Rewards)
        {
            if (card.rarity == 0)
            {
                BattleRarity0Cards.Add(card);
            }
            else if (card.rarity == 1)
            {
                BattleRarity1Cards.Add(card);
            }
            else if (card.rarity == 2)
            {
                BattleRarity2Cards.Add(card);
            }
        }
    }

    //협상 후 카드 보상
    public void NegoReward(int index)
    {
        Rewards.Clear();

        //타입이 잡몹인 카드 중, 에너미 심볼의 인덱스에 따른 카드를 가져온다.
        switch (index)
        {
            //일반 잡몹이면 카드의 타입이 잡몹 카드들인 카드들을 가져오되, '스테이지에 맞는' 잡몹 카드를 가져옴. 3스테이지면 mob3 타입의 카드들만 가져옴.
            //예시로, 3스테이지면 mob3 타입의 카드들만 가져오게 된다.
            case 0:
                NegoCardsPool = GameData.Instance.CardList
                    .Where(card => card.type == $"Mob{Stage.Instance.StageLevel}")
                    .ToList();
                break;
            //그 외인 경우, index는 현재 Theme의 번호와 같다. Theme에 해당하는 Enum의 텍스트(예를 들어 index가 1이면 Define.ThemeType.Pirate와 대응)
            //를 가져오고, 현재 스테이지의 번호를 더하면, 해적 테마의 1스테이지인 경우 Pirate1 이라는 문자열이 만들어 진다.
            //card의 type이 Pirate1인 카드들을 가져와서 풀에 추가한다.
            default:
                NegoCardsPool = GameData.Instance.CardList
                    .Where(card => card.type == $"{Stage.Instance.StageTheme}{Stage.Instance.StageLevel}")
                    .ToList();
                break;
        }

        Rewards.Add(NegoCardsPool[Random.Range(0, NegoCardsPool.Count)]);
    }

    //보스 영입 시 카드 보상
    public void BossNegoReward()
    {
        Rewards.Clear();

        //타입이 보스인 카드 중, 에너미의 인덱스번째의 카드를 가져온다.
        Rewards.Add(GameData.Instance.CardList.Where(card => card.type == $"{Stage.Instance.StageTheme}Boss").ElementAtOrDefault(0));
    }

    //이벤트 카드 획득 보상으로 띄우는 경우 호출
    public void EventReward()
    {
        Rewards.Clear();

        for (int i = 0; i < 3; i++)
        {
            float random = Random.Range(0f, 1f);

            //확률에 따라 노말, 레어, 유니크 카드풀 중에서 한 카드를 골라 보상 카드로 지정
            if (random < 0.63f)
            {
                int index = Random.Range(0, BattleRarity0Cards.Count);
                Rewards.Add(BattleRarity0Cards[index]);
                BattleRarity0Cards.RemoveAt(index); //한번 나온 카드가 다시 나오지 않도록 제거
            }
            else if (random < 0.95f)
            {
                int index = Random.Range(0, BattleRarity1Cards.Count);
                Rewards.Add(BattleRarity1Cards[index]);
                BattleRarity1Cards.RemoveAt(index);
            }
            else
            {
                int index = Random.Range(0, BattleRarity2Cards.Count);
                Rewards.Add(BattleRarity2Cards[index]);
                BattleRarity2Cards.RemoveAt(index);
            }
        }

        // Rewards 리스트에 저장된 카드들을 각 풀에 다시 추가
        foreach (var card in Rewards)
        {
            if (card.rarity == 0)
            {
                BattleRarity0Cards.Add(card);
            }
            else if (card.rarity == 1)
            {
                BattleRarity1Cards.Add(card);
            }
            else if (card.rarity == 2)
            {
                BattleRarity2Cards.Add(card);
            }
        }

    }

    //이벤트 동료 카드 획득
    public void PartnerReward()
    {
        Rewards.Clear();

        //타입이 파트너인 카드 중, 현재 스테이지에 맞는 카드를 가져오기.
        Rewards.Add(GameData.Instance.CardList.Where(card => card.type == $"Partner{Stage.Instance.StageLevel}").ElementAtOrDefault(0));

        //동료 카드 획득한걸로 설정
        PlayerData.Instance.HasPartner[Stage.Instance.StageLevel - 1] = true;
    }


    //레벨 업 후 카드 보상
    public void LevelUpReward()
    {
        Rewards.Clear();

        List<int> rewardCardsIndexes = Define.GenerateRandomNumbers(0, ViewerCardsPool.Count, 3); // 레벨업 보상 카드 풀 중에 랜덤으로 인덱스 3개 뽑기
        for (int i = 0; i < rewardCardsIndexes.Count; i++)
        {
            Rewards.Add(ViewerCardsPool[rewardCardsIndexes[i]]); //3개의 인덱스에 해당하는 애청자 카드들을 보상 카드란에 추가
        }
    }
}
