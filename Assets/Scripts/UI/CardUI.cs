using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DataStructs;
using UnityEngine.EventSystems;
using System;

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

    public Action PointerDown;
    public Action PointerEnter;
    public Action PointerExit;

    private float scaleOnHover = 1.1f;
    private Vector3 originalScale = Vector3.one;

    private void OnDisable()
    {
        PointerDown = null;
        PointerEnter = null;
        PointerExit = null;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDown?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit?.Invoke();
    }



    //���ڷ� ���� ī���� �����͸� UI�� �����ִ� �Լ�
    public void ShowCardData(CardStruct showCard)
    {
        card = showCard;
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

    //ī�� ũ�� Ȯ��
    public void HoverEnter()
    {
        transform.localScale = originalScale * scaleOnHover;
    }

    //ī�� ũ�� �������
    public void HoverExit()
    {
        transform.localScale = originalScale;
    }    
    
    //���� ī�� �߰�
    public void AddCardToDeck()
    {
        PlayerData.Instance.Deck.Add(card);
    }

    //�ش� UI�� ǥ���ϴ� ī�� ȹ��... �� ������ UI���� ó���ؾ߰���.
    //��� ī�� UI�� Ŭ�� �� ȹ���� �� �ְ�, Ŭ�� �̺�Ʈ�� �̰����� ó���ϵ�, ��������Ʈ�� �̿��� ������ UI���� Ŭ�� �̺�Ʈ�� �߰����ش�.
}
