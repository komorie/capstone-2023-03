using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DataStructs;
using UnityEngine.EventSystems;
using System;


public enum CardMode
{
    Library, //���̺귯������ ī��UI ���. Ŭ��/������ �� ���� ����
    Select, //ī�� ���� UI���� ī��UI ���. Ŭ��/������ �� ī�� ȹ��
    Battle,
    EventDiscard, //ī�� ������ �̺�Ʈ���� ī��UI ���. Ŭ��/������ �� ī�� ������
    ShopDiscard //ī�� ������ �������� ī��UI ���. Ŭ��/������ �� ī�� ������, ���� �� ���� �� �ִ�.
}

public class CardUI : BaseUI, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    //UI�� ����� ī�� ��ü
    private CardStruct card;

    //ī�� UI�� �̹���, �ؽ�Ʈ ������Ʈ��
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text costText;
    [SerializeField]
    private TMP_Text descriptionText;

    private float scaleOnHover = 1.1f;
    private Vector3 originalScale = Vector3.one;

    private CardMode cardMode;


    public void OnPointerDown(PointerEventData eventData) //ī�� Ŭ�� �� �߻�
    {
        switch (cardMode)
        {
            case CardMode.Library:
                break;
            case CardMode.Select:
                PlayerData.Instance.Deck.Add(card);
                UIManager.Instance.HideUI("CardSelectUI"); //ȹ�� �Ŀ��� �ٷ� ī�� ���� UI �ݱ�.
                break;
            case CardMode.Battle:
                break;
            case CardMode.EventDiscard: //���� (�̺�Ʈ)
                PlayerData.Instance.Deck.Remove(card);
                UIManager.Instance.HideUI("LibraryUI"); //���� �Ŀ��� �ٷ� ���̺귯�� UI �ݱ�.
                break;
            case CardMode.ShopDiscard: //���� (����)
                int newMoney = PlayerData.Instance.Money - ShopData.Instance.DiscardCost;
                if(newMoney > 0) //���� ���� ��츸
                {
                    PlayerData.Instance.Deck.Remove(card); //������ ���̺귯�� UI �ȴ���.
                    PlayerData.Instance.Money = newMoney; //���� ��븸ŭ �÷��̾� ������ �����ϱ�
                    PlayerData.Instance.DataChanged(); //�� ���� �˷��� ���̺귯���� ���ΰ�ħ�ϵ���!
                    ShopData.Instance.DiscardCost += 25; //���� ��� 25 �߰�
                    ShopData.Instance.DataChanged(); //���� ������ ���� �˷��� ������� ���ΰ�ħ�ϵ���!
                }
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) //ī�� ���콺 �ø� ��
    {
        switch (cardMode)
        {
            case CardMode.Library:
                break;
            case CardMode.Select:
                transform.localScale = originalScale * scaleOnHover; //Ȯ��
                break;
            case CardMode.Battle:
                break;
            case CardMode.EventDiscard:
            case CardMode.ShopDiscard:
                transform.localScale = originalScale * scaleOnHover;
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (cardMode)
        {
            case CardMode.Library:
                break;
            case CardMode.Select:
                transform.localScale = originalScale; //Ȯ�� �ǵ�����
                break;
            case CardMode.Battle:
                break;
            case CardMode.EventDiscard:
            case CardMode.ShopDiscard:
                transform.localScale = originalScale;
                break;
        }
    }



    //���ڷ� ���� ī���� �����͸� UI�� �����ִ� �Լ�
    public void ShowCardData(CardStruct showCard, CardMode mode)
    {
        card = showCard;
        cardMode = mode;
        nameText.text = card.name;
        descriptionText.text = card.description;
        costText.text =  card.cost == 99 ? "X" : card.cost.ToString();



        switch(card.rarity)
        {
            case 0:
                nameText.color = Color.white; break;
            case 1:
                nameText.color = Color.magenta; break;
            case 2:
                nameText.color = Color.yellow; break;
        }

        //ī�忡 �Ӽ��� ����� �̹��� ��������
        if (card.attribute != null && card.attribute != "") image.sprite = GameData.Instance.SpriteDic[card.attribute];
        else
        {  
            //���Ӽ��� ���� �ٸ� �ʵ�� �̹��� ���� 
        }

    }
}
