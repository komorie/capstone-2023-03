using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DataStructs;



public enum LibraryMode
{
    Library, //�Ϲ� ���̺귯�� ���
    Deck, //�÷��̾� �� �����ֱ� ���
    EventDiscard, //�÷��̾� �� �����ֱ� + �̺�Ʈ�� ī�� ������ ���
    ShopDiscard //�÷��̾� �� �����ֱ� + �������� ī�� ������ ���
}

//C# LInq ���: ������ ������ C#���� ��ũ��Ʈ�� ����� �� �ֵ��� �ϴ� ���.
//�迭 �� �ٸ� �÷��ǿ��� ���� ���ϴ� ������ ������ �� ����.

public class LibraryUI : BaseUI
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
        InputActions.keyActions.UI.Menu.started += Close;
        PlayerData.Instance.OnDataChange += RefreshLibrary; //�̰Ŵ� ���� �ٲ� ������ �װ��� �����Ͽ� ī�� UI�� ���ΰ�ħ�ϱ� ����.
    }

    private void OnDisable()
    {
        InputActions.keyActions.UI.Menu.started -= Close;
        PlayerData.Instance.OnDataChange -= RefreshLibrary;
    }

    public void Init(LibraryMode libraryMode = LibraryMode.Library)
    {
        this.libraryMode = libraryMode;

        switch (libraryMode) //����, ��ų, ��û�� ī�� ���� �����ֱ�
        {
            case LibraryMode.Library:
                showedCardList = GameData.Instance.CardList
                .Where(card => card.type == "Attack" || card.type == "Skill" || card.type == "Viewer")
                .ToList();
                break;
            case LibraryMode.Deck: //���� �� �����ֱ�
                showedCardList = PlayerData.Instance.Deck;
                break;
            case LibraryMode.EventDiscard: //���� �� �����ֱ� + �̺�Ʈ�� ī�� �� �� ������
                showedCardList = PlayerData.Instance.Deck;
                BackButton.gameObject.SetActive(false);
                break;
            case LibraryMode.ShopDiscard: //���� �� �����ֱ� + �̺�Ʈ�� Ŭ���ϴ� ��ŭ ������
                showedCardList = PlayerData.Instance.Deck;
                break;

        }
        ShowCards();
        SortByCostButtonClick();
    }

    public void RefreshLibrary() //���� �ٲ���� �� ȣ��Ǿ�, ī�� UI�� ���ΰ�ħ�Ѵ�.
    {
        switch (libraryMode) //����, ��ų, ��û�� ī�� ���� �����ֱ�
        {
            case LibraryMode.Library:
                showedCardList = GameData.Instance.CardList
                .Where(card => card.type == "Attack" || card.type == "Skill" || card.type == "Viewer")
                .ToList();
                break;
            case LibraryMode.Deck: //���� �� �����ֱ�
                showedCardList = PlayerData.Instance.Deck;
                break;
            case LibraryMode.EventDiscard: //���� �� �����ֱ� + �̺�Ʈ�� ī�� �� �� ������
                showedCardList = PlayerData.Instance.Deck;
                BackButton.gameObject.SetActive(false);
                break;
            case LibraryMode.ShopDiscard: //���� �� �����ֱ� + �̺�Ʈ�� Ŭ���ϴ� ��ŭ ������
                showedCardList = PlayerData.Instance.Deck;
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
            AssetLoader.Instance.Destroy(deckDisplayer.transform.GetChild(i).gameObject);
        }
    }


    public void ShowCards()
    {

        ClearCards(); //���� ǥ�õǴ� ī�� ����

        //Linq�� ���. ���� �������� ���� �з���ŭ ī�� ����Ʈ���� �����ؼ� �����ֱ�
        List<CardStruct> cardList = showedCardList.Skip(currentPage * cardsPerPage).Take(cardsPerPage).ToList();

        for (int i = 0; i < cardList.Count; i++)
        {
            switch (libraryMode)
            {
                case LibraryMode.Library: //����, ��ų, ��û�� ī�� ���� �����ֱ�
                case LibraryMode.Deck: //���� �� �����ֱ�
                    AssetLoader.Instance.Instantiate("Prefabs/UI/CardUI", deckDisplayer.transform)
                        .GetComponent<CardUI>()
                        .ShowCardData(cardList[i], CardMode.Library); //ī�带 ���̺귯�� ������ ��ȯ(Ŭ�� �̺�Ʈ X)
                    break;
                case LibraryMode.EventDiscard: //���� �� �����ֱ� + ī�� ������
                    AssetLoader.Instance.Instantiate("Prefabs/UI/CardUI", deckDisplayer.transform)
                        .GetComponent<CardUI>()
                        .ShowCardData(cardList[i], CardMode.EventDiscard); //ī�带 ������ ���� ��ȯ(Ŭ�� �� ������ �̺�Ʈ)
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

        showedCardList = showedCardList.OrderBy(card => card.cost).ToList();
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

        showedCardList = showedCardList.OrderBy(card => card.name).ToList();
        currentPage = 0;
        ShowCards();
        UpdateButtons();
    }

    //������ ��ư, UI �ݱ�
    public void BackButtonClick()
    {
        UIManager.Instance.HideUI("LibraryUI");
    }

    private void Close(InputAction.CallbackContext context)
    {
        UIManager.Instance.HideUI("LibraryUI");
    }
}
