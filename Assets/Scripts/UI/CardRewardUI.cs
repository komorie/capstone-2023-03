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
    GameObject rewardView; //���� â ī�� ���� ��

    [SerializeField]
    TMP_Text rewardText; //���� â �ȳ���

    [SerializeField]
    Button discardButton;

    List<CardStruct> rewardCards; //�������� ���� ������ ī�� �����͵� ǥ��

    private Action CloseAction; //â ���� �� �߻��ϴ� �̺�Ʈ


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
        //���� ���� ī�� ������ ��������
        rewardCards = CardRewardData.Instance.Rewards;

        //���� ī����� UI�� ǥ��
        for (int i = 0; i < rewardCards.Count; i++)
        {
            CardUI cardUI = AssetLoader.Instance.Instantiate("Prefabs/UI/CardUI", rewardView.transform).GetComponent<CardUI>();
            cardUI.ShowCardData(rewardCards[i]); //���� ���� ī�带 ������.
            cardUI.OnCardClicked += (cardUI) => //ī�� UI Ŭ�� �� �ش� �̺�Ʈ �ߵ�
            {
                PlayerData.Instance.Deck.Add(cardUI.Card); //ī�� Ŭ�� �� �ش� UI�� ī�带 ���� �߰�
                UIManager.Instance.HideUI("CardSelectUI"); //â �ݱ�
            };
            cardUI.OnCardEntered += (cardUI) => { cardUI.CardBig(); }; //ī�忡 ���콺 �� �� �ش� ī�� Ȯ�� �����ϵ��� ���.
            cardUI.OnCardExited += (cardUI) => { cardUI.CardSmall(); }; //ī�忡�� ���콺 ���� �� �ش� ī�� ��� �����ϵ��� ���.
        }
    }

    public void BattleReward()
    {
        rewardText.text = "�������� �¸��ϼ̽��ϴ�!\r\n�������� ī�带 �� �� ����������.";
        CardRewardData.Instance.BattleReward(); //���� ���� ������ ����
        ShowReward(); //���� ǥ��
    }

    public void BossBattleReward()
    {
        rewardText.text = "�������� �������� �¸��ϼ̽��ϴ�!\r\n�������� ����� ī�带 �� �� ����������.";
        CardRewardData.Instance.BossBattleReward(); //�������� ���� ���� ������ ���� 
        ShowReward();
    }

    //���� �� ī�� ����
    public void NegoReward(int index)
    {
        rewardText.text = "���� �����߽��ϴ�!\r\n��� ��û�ڰ� �շ��մϴ�.";
        CardRewardData.Instance.NegoReward(index); //���� ���� ������ ����   
        ShowReward();
    }

    //���� ���� �� ī�� ����
    public void BossNegoReward()
    {
        rewardText.text = "������ ũ������� �����Ͽ����ϴ�!\r\n������ ���ᰡ �Ǿ� �� ���Դϴ�.";
        discardButton.gameObject.SetActive(false); //���� �ÿ��� �ݵ�� ī�� ���ϱ�
        CardRewardData.Instance.BossNegoReward(); //���� ���� ������ ����
        ShowReward();
    }

    //�̺�Ʈ ī�� ȹ�� �������� ���� ��� ȣ��
    public void EventReward()
    {

        rewardText.text = "��û�ڵ��� �����Դϴ�!\r\n�������� ī�带 �� �� ����������.";
        CardRewardData.Instance.EventReward(); //�̺�Ʈ ���� ������ ����
        ShowReward();
    }

    //�̺�Ʈ ���� ī�� ȹ��
    public void PartnerReward()
    {
        //���� ����ġ
        rewardText.text = "���ᰡ ũ������� �շ��Ͽ����ϴ�!\r\n������ ���ᰡ �Ǿ� �� ���Դϴ�.";
        discardButton.gameObject.SetActive(false); //���� �ÿ��� �ݵ�� ī�� ���ϱ�
        CardRewardData.Instance.PartnerReward(); //�̺�Ʈ ���� ���� ������ ����    
        ShowReward();
    }


    //���� �� �� ī�� ����
    public void LevelUpReward()
    {
        rewardText.text = "ä�� ������ ����Ͽ����ϴ�!\r\n��û�ڵ��� ������ ���� ī�带 �����Խ��ϴ�.";
        discardButton.gameObject.SetActive(false); //�ݵ�� ī�� ���ϱ�
        CardRewardData.Instance.LevelUpReward(); //������ ���� ������ ����    
        ShowReward();
    }

    //������ ��ư Ŭ��
    public void ExitButtonClick()
    {
        UIManager.Instance.HideUI("CardSelectUI");
    }
}
