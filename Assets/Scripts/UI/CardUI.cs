using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUI : BaseUI
{
    public Card BindedCard { get; set; }

    private void Awake()
    {
        //UI ��ҵ� ��������
    }

    public void ShowCard(Card card)
    {
        BindedCard = card;
        
        //���ڷ� ���� ī���� �̸� �̹��� ���� UI ��ҷ� �����ֱ�
    }


}
