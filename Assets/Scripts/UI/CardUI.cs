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
    private Image image;
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

    //�ش� UI�� ǥ���ϴ� ī�� ȹ��... �� ������ UI���� ó���ؾ߰���.
    //��� ī�� UI�� Ŭ�� �� ȹ���� �� �ְ�, Ŭ�� �̺�Ʈ�� �̰����� ó���ϵ�, ��������Ʈ�� �̿��� ������ UI���� Ŭ�� �̺�Ʈ�� �߰����ش�.
}
