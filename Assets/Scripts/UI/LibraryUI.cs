using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DataStructs;
using System;

public enum LibraryMode
{
    Library, //�Ϲ� ���̺귯�� ���
    Deck, //�÷��̾� �� �����ֱ� ���
    EventDiscard, //�÷��̾� �� �����ֱ� + �̺�Ʈ�� ī�� ������ ���
    ShopDiscard, //�÷��̾� �� �����ֱ� + �������� ī�� ������ ���
    BattleDeck, //��Ʋ �߿� ���� �� �����ֱ�
    BattleTrash, //��Ʋ �߿� ���� ī�� �����ֱ�
    BattleTrashHand, // ��Ʋ �߿� ���� �����ֱ� + ������
    BattleUseHand // ��Ʋ �߿� ���� �����ֱ� + �� �� ����
}

//C# LInq ���: ������ ������ C#���� ��ũ��Ʈ�� ����� �� �ֵ��� �ϴ� ���.

public class LibraryUI : MonoBehaviour
{
    private LibraryMode libraryMode;
    private int cardsPerPage = 8;
    private int currentPage = 0;

    [SerializeField]
    private GameObject deckDisplayer;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Button prevButton;
    [SerializeField]
    private Button sortByCostButton;
    [SerializeField]
    private Button sortByNameButton;
    [SerializeField]
    private Button BackButton;

    private List<CardStruct> showedCardList= new List<CardStruct>();


    private void OnEnable()
    {
        InputActions.keyActions.UI.Deck.started += Close;
        PlayerData.Instance.OnDataChange += RefreshLibrary; //�̰Ŵ� ���� �ٲ� ������ �װ��� �����Ͽ� ī�� UI�� ���ΰ�ħ�ϱ� ����.
    }

    private void OnDisable()
    {
        InputActions.keyActions.UI.Deck.started -= Close;
        PlayerData.Instance.OnDataChange -= RefreshLibrary;
    }

    public void Init(LibraryMode libraryMode = LibraryMode.Library)
    {
        this.libraryMode = libraryMode;
        RefreshLibrary();
    }

    public void RefreshLibrary() //ó�� Ȥ�� ���� �ٲ���� �� ȣ��Ǿ�, ī�� UI�� ���ΰ�ħ�Ѵ�.
    {
        switch (libraryMode) //����, ��ų, ��û�� ī�� ���� �����ֱ�
        {
            case LibraryMode.Library:
                showedCardList = LibraryData.Instance.Library;
                break;
            case LibraryMode.Deck: //���� �� �����ֱ�
                showedCardList = PlayerData.Instance.Deck;
                break;
            case LibraryMode.EventDiscard: //���� �� �����ֱ� + �̺�Ʈ�� ī�� �� �� ������
                showedCardList = PlayerData.Instance.Deck;
                break;
            case LibraryMode.ShopDiscard: //���� �� �����ֱ� + �̺�Ʈ�� Ŭ���ϴ� ��ŭ ������ + ������ ���� ����
                showedCardList = PlayerData.Instance.Deck;
                break;
            case LibraryMode.BattleDeck: //��Ʋ �߿� ���� �� �����ֱ�
                showedCardList = BattleData.Instance.Deck;
                break;
            case LibraryMode.BattleTrash: //��Ʋ �߿� ���� ī�� �����ֱ�
                showedCardList = BattleData.Instance.Trash;
                break;
            case LibraryMode.BattleTrashHand: //��Ʋ �߿� ���� �����ֱ� + ������
                showedCardList = BattleData.Instance.Hand;
                break;
            case LibraryMode.BattleUseHand: //��Ʋ �߿� ���� �����ֱ� + �� �� ����
                showedCardList = BattleData.Instance.Hand;
                break;
        }
        ShowCards();
        SortByCostButtonClick();
    }

    //ǥ������ ī�� ����
    private void ClearCards()
    {
        for (int i = 0; i < deckDisplayer.transform.childCount; i++)
        {
            deckDisplayer.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ShowCards()
    {

        ClearCards(); //���� ǥ�õǴ� ī�� ����

        int start = currentPage * cardsPerPage;
        int end = start + cardsPerPage; //������ ���۰� ���� + 8��

        for (int now = start; now < end; now++)
        {
            CardUI cardUI;
            BattleUI battleUI;

            if (now > Math.Min(showedCardList.Count - 1, start + cardsPerPage)) continue;

            cardUI = deckDisplayer.transform.GetChild(now - start).GetComponent<CardUI>();
            cardUI.gameObject.SetActive(true);  
            cardUI.ShowCardData(showedCardList[now]); //�ش� ī�带 �����ֱ�

            switch (libraryMode)
            {
                case LibraryMode.EventDiscard: //���� �� �����ֱ� + ī�� ������ 1ȸ
                    cardUI.OnCardClicked += (cardUI) => //ī�� Ŭ�� �� �ϴ��� �̺�Ʈ �ߵ��ϵ��� ���
                    {
                        PlayerData.Instance.RemoveCard(cardUI.Card); //�ش� ī�� UI�� ī�带 ������ ����
                        UIManager.Instance.HideUI("LibraryUI"); //���� �Ŀ��� �ٷ� ���̺귯�� UI �ݱ�.
                    };
                    cardUI.OnCardEntered += (cardUI) => { cardUI.CardBig(); }; //ī�忡 ���콺 �� �� �ش� ī�� Ȯ�� �����ϵ��� ���.
                    cardUI.OnCardExited += (cardUI) => { cardUI.CardSmall(); }; //ī�忡�� ���콺 ���� �� �ش� ī�� ��� �����ϵ��� ���.
                    break;
                case LibraryMode.ShopDiscard: //���� �� �����ֱ� + ī�� ������ ����X
                    cardUI.OnCardClicked += (cardUI) => //ī�� Ŭ�� �� �ϴ��� �̺�Ʈ �ߵ��ϵ��� ���
                    {
                        ShopData.Instance.Discard(cardUI.Card);
                    };
                    cardUI.OnCardEntered += (cardUI) => { cardUI.CardBig(); }; 
                    cardUI.OnCardExited += (cardUI) => { cardUI.CardSmall(); };
                    break;
                case LibraryMode.BattleTrashHand: //��Ʋ �߿� ���� ī�� �����ֱ� + ī�� ������ 1ȸ
                    battleUI = GameObject.Find("UIRoot").transform.GetChild(2).GetComponent<BattleUI>();
                    cardUI.OnCardClicked += (cardUI) => 
                    {
                        battleUI.Discard(cardUI.Card); 
                        UIManager.Instance.HideUI("LibraryUI"); 
                    };
                    cardUI.OnCardEntered += (cardUI) => { cardUI.CardBig(); }; 
                    cardUI.OnCardExited += (cardUI) => { cardUI.CardSmall(); };
                    break;
                case LibraryMode.BattleUseHand: //��Ʋ �߿� ���� ī�� �����ֱ� + �� �� ����
                    battleUI = GameObject.Find("UIRoot").transform.GetChild(2).GetComponent<BattleUI>();
                    cardUI.OnCardClicked += (cardUI) => 
                    {
                        battleUI.SelectCard(cardUI.Card); 
                        UIManager.Instance.HideUI("LibraryUI"); 
                    };
                    cardUI.OnCardEntered += (cardUI) => { cardUI.CardBig(); }; 
                    cardUI.OnCardExited += (cardUI) => { cardUI.CardSmall(); }; 
                    break;
                default: 
                    break;
            }
        }
        UpdateButtons();
    }
    
    // ����/���� ��ư Ȱ��ȭ
    private void UpdateButtons()
    {
        prevButton.gameObject.SetActive(currentPage > 0);
        nextButton.gameObject.SetActive((currentPage + 1) * cardsPerPage < showedCardList.Count);
    }

    //���� ��ư Ŭ���� �߻��� �̺�Ʈ
    public void NextButtonClick()
    {
        currentPage++;
        ShowCards();
        UpdateButtons();

    }

    //���� ��ư Ŭ���� �߻��� �̺�Ʈ
    public void PreviousButtonClick()
    {
        currentPage--;
        ShowCards();
        UpdateButtons();
    }

    //�ڽ�Ʈ�� ���� ��ư. 
    public void SortByCostButtonClick()
    {
        sortByCostButton.interactable = false;
        sortByNameButton.interactable = true;

        sortByCostButton.GetComponentInChildren<TMP_Text>().color = Color.grey;
        sortByNameButton.GetComponentInChildren<TMP_Text>().color = Color.white;

        showedCardList.Sort((card1, card2) => card1.cost.CompareTo(card2.cost)); //�ڽ�Ʈ�� �������� ����
        currentPage = 0;
        ShowCards();
        UpdateButtons();
    }

    //�̸��� ���� ��ư. 
    public void SortByNameButtonClick()
    {
        sortByNameButton.interactable = false;
        sortByCostButton.interactable = true;

        sortByNameButton.GetComponentInChildren<TMP_Text>().color = Color.grey;
        sortByCostButton.GetComponentInChildren<TMP_Text>().color = Color.white;

        showedCardList.Sort((card1, card2) => card1.name.CompareTo(card2.name)); //�̸��� �������� ����
        currentPage = 0;
        ShowCards();
        UpdateButtons();
    }

    //������ ��ư, UI �ݱ�
    public void BackButtonClick()
    {
        switch(libraryMode)
        {
            case LibraryMode.ShopDiscard:
                UIManager.Instance.HideUI("ShopDiscardUI");
                break;
            default:
                UIManager.Instance.HideUI("LibraryUI");
                break;
        }
    }

    private void Close(InputAction.CallbackContext context)
    {
        ClearCards();
        UIManager.Instance.HideUI("LibraryUI");
    }
    
}
