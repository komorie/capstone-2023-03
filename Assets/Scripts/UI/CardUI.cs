using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUI : MonoBehaviour
{
    public Card BindedCard { get; set; }

    private void Awake()
    {
        //UI ��ҵ� ��������
    }

    public void ShowCard(Card card)
    {
        BindedCard = card;
        
        //ī���� �̸� �̹��� ���� UI ��ҷ� �����ֱ�
    }


}
