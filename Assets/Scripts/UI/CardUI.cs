using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : BaseUI
{

    //ī�� UI�� �̹���, �ؽ�Ʈ ������Ʈ��
    [SerializeField]
    private Image portrait;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text costText;
    [SerializeField]
    private TMP_Text descriptionText;

    //UI�� ����� ī�� ��ü
    public CardData BindedCard { get; set; }

    //���ڷ� ���� ī���� �����͸� UI�� �����ִ� �Լ�
    public void ShowCardData(CardData card)
    {
        BindedCard = card;

        nameText.text = card.name;
        descriptionText.text = card.description;
        costText.text = card.cost.ToString();
    }


}
