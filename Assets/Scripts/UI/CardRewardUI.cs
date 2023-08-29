using DataStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardRewardUI : MonoBehaviour
{
    [SerializeField]
    GameObject rewardView; //보상 창 카드 띄우는 곳

    [SerializeField]
    TMP_Text rewardText; //보상 창 안내문

    [SerializeField]
    Button discardButton;

    List<CardStruct> rewardCards; //보상으로 선택 가능한 카드 데이터들 표시

    private Action CloseAction; //창 닫을 때 발생하는 이벤트


    private void OnDisable()
    {
        CloseAction?.Invoke();
    }

    public void SetCloseCallback(Action CloseCallback)
    {
        CloseAction = CloseCallback;
    }

    private void ShowReward()
    {
        //현재 보상 카드 데이터 가져오기
        rewardCards = CardRewardData.Instance.Rewards;

        //보상 카드들을 UI에 표시
        for (int i = 0; i < rewardCards.Count; i++)
        {
            CardUI cardUI = AssetLoader.Instance.Instantiate("Prefabs/UI/CardUI", rewardView.transform).GetComponent<CardUI>();
            cardUI.ShowCardData(rewardCards[i]); //현재 보상 카드를 보여줌.
            cardUI.OnCardClicked += (cardUI) => //카드 UI 클릭 시 해당 이벤트 발동
            {
                PlayerData.Instance.Deck.Add(cardUI.Card); //카드 클릭 시 해당 UI의 카드를 덱에 추가
                UIManager.Instance.HideUI("CardSelectUI"); //창 닫기
            };
            cardUI.OnCardEntered += (cardUI) => { cardUI.CardBig(); }; //카드에 마우스 들어갈 시 해당 카드 확대 수행하도록 등록.
            cardUI.OnCardExited += (cardUI) => { cardUI.CardSmall(); }; //카드에서 마우스 나갈 시 해당 카드 축소 수행하도록 등록.
        }
    }

    public void BattleReward()
    {
        rewardText.text = "전투에서 승리하셨습니다!\r\n보상으로 카드를 한 장 가져가세요.";
        CardRewardData.Instance.BattleReward(); //전투 보상 데이터 생성
        ShowReward(); //보상 표시
    }

    public void BossBattleReward()
    {
        rewardText.text = "보스와의 전투에서 승리하셨습니다!\r\n보상으로 희귀한 카드를 한 장 가져가세요.";
        CardRewardData.Instance.BossBattleReward(); //보스와의 전투 보상 데이터 생성 
        ShowReward();
    }

    //협상 후 카드 보상
    public void NegoReward(int index)
    {
        rewardText.text = "협상에 성공했습니다!\r\n상대 시청자가 합류합니다.";
        CardRewardData.Instance.NegoReward(index); //협상 보상 데이터 생성   
        ShowReward();
    }

    //보스 영입 시 카드 보상
    public void BossNegoReward()
    {
        rewardText.text = "보스를 크루원으로 영입하였습니다!\r\n강력한 동료가 되어 줄 것입니다.";
        discardButton.gameObject.SetActive(false); //협상 시에는 반드시 카드 택하기
        CardRewardData.Instance.BossNegoReward(); //협상 보상 데이터 생성
        ShowReward();
    }

    //이벤트 카드 획득 보상으로 띄우는 경우 호출
    public void EventReward()
    {

        rewardText.text = "애청자들의 지원입니다!\r\n보상으로 카드를 한 장 가져가세요.";
        CardRewardData.Instance.EventReward(); //이벤트 보상 데이터 생성
        ShowReward();
    }

    //이벤트 동료 카드 획득
    public void PartnerReward()
    {
        //동료 스위치
        rewardText.text = "동료가 크루원으로 합류하였습니다!\r\n강력한 동료가 되어 줄 것입니다.";
        discardButton.gameObject.SetActive(false); //협상 시에는 반드시 카드 택하기
        CardRewardData.Instance.PartnerReward(); //이벤트 동료 보상 데이터 생성    
        ShowReward();
    }


    //레벨 업 후 카드 보상
    public void LevelUpReward()
    {
        rewardText.text = "채널 레벨이 상승하였습니다!\r\n애청자들이 강력한 지원 카드를 보내왔습니다.";
        discardButton.gameObject.SetActive(false); //반드시 카드 택하기
        CardRewardData.Instance.LevelUpReward(); //레벨업 보상 데이터 생성    
        ShowReward();
    }

    //버리기 버튼 클릭
    public void ExitButtonClick()
    {
        UIManager.Instance.HideUI("CardSelectUI");
    }
}
