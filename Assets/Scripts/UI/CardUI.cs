using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DataStructs;

public class CardUI : BaseUI
{

    //UI�� ����� ī�� ��ü�� ��ȣ
    private int cardIndex;

    //ī�� UI�� �̹���, �ؽ�Ʈ ������Ʈ��
    [SerializeField]
    private Image portrait;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text costText;
    [SerializeField]
    private TMP_Text descriptionText;

    //���ڷ� ���� ī���� �����͸� UI�� �����ִ� �Լ�
    public void ShowCardData(CardStruct card)
    {
        cardIndex = card.index;

        nameText.text = card.name;
        descriptionText.text = card.description;
        costText.text = card.cost.ToString();
        if (card.cost == -1) costText.text = "X";

        switch(card.rarity)
        {
            case 1:
                nameText.color = Color.magenta; break;
            case 2:
                nameText.color = Color.yellow; break;
        }
    }

    //�̰ŷ� ���ľ� �� ��.
    public void ShowCardData(int index)
    {

    }

    //�ش� UI�� ǥ���ϴ� ī�� ȹ��... �� ������ UI���� ó���ؾ߰���
}
