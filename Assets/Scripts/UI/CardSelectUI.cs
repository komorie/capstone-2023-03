using DataStructs;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class CardSelectUI : MonoBehaviour
{
    [SerializeField]
    GameObject rewardView;

    List<CardStruct> rewardCards;

    private Action CloseAction;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    private void OnDisable()
    {
        CloseAction?.Invoke();
    }

    public void Init(Action CloseCallback)
    {
        CloseAction = CloseCallback;
    }

    public void BattleReward()
    {
        //������ �������� ���� ī����� ����. �׸��� ��� ���� �� ������.
        List<CardStruct> rewardCardsPool = GameData.Instance.CardList.Where(card => card.type == "����" || card.type == "��ų").ToList();
        List<CardStruct> rarity0Cards = rewardCardsPool.Where(card => card.rarity == 0).ToList();
        List<CardStruct> rarity1Cards = rewardCardsPool.Where(card => card.rarity == 1).ToList();
        List<CardStruct> rarity2Cards = rewardCardsPool.Where(card => card.rarity == 2).ToList();

        rewardCards = new List<CardStruct>();

        for (int i = 0; i < 3; i++)
        {
            float random = Random.Range(0f, 1f);

            //Ȯ���� ���� �븻, ����, ����ũ ī��Ǯ �߿��� �� ī�带 ��� ���� ī��� ����
            if (random < 0.63f)
            {
                int index = Random.Range(0, rarity0Cards.Count);
                rewardCards.Add(rarity0Cards[index]);
            }
            else if (random < 0.95f)
            {
                int index = Random.Range(0, rarity1Cards.Count);
                rewardCards.Add(rarity1Cards[index]);
            }
            else
            {
                int index = Random.Range(0, rarity2Cards.Count);
                rewardCards.Add(rarity2Cards[index]);
            }
        }

        //���� ī����� UI�� ǥ��
        for (int i = 0; i < rewardCards.Count; i++)
        {
            CardUI cardUI = AssetLoader.Instance.Instantiate("Prefabs/UI/CardUI", rewardView.transform).GetComponent<CardUI>();
            cardUI.ShowCardData(rewardCards[i]); //���� ���� ī�带 ������.
            cardUI.PointerDown += () =>
            {
                cardUI.AddCardToDeck(); //�ش� ī�� UI�� ī�� ȹ�� ����� �ش� ī���� Ŭ�� �̺�Ʈ�� �����ϵ��� �Ѵ�.
                UIManager.Instance.HideUI("CardSelectUI"); //ȹ�� �Ŀ��� �ٷ� �� UI �ݱ�.
            };
            cardUI.PointerEnter += cardUI.HoverEnter; //�ش� ī�� UI�� Ȯ��/��� ����� �ش� ī���� ȣ�� �̺�Ʈ�� �����ϵ��� �Ѵ�.
            cardUI.PointerExit += cardUI.HoverExit;
        }
    }

    //���� �� ī�� ����
    public void NegoReward()
    {

    }

    //���� �� �� ī�� ����
    public void LevelUpReward()
    {

    }
}
