using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


//C# LInq ���: ������ ������ C#���� ��ũ��Ʈ�� ����� �� �ֵ��� �ϴ� ���.
//�迭 �� �ٸ� �÷��ǿ��� ���� ���ϴ� ������ ������ �� ����.

public class LibraryUI : BaseUI
{
    private bool showAllCards = true;
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

    private List<CardStruct> showedCardList= new List<CardStruct>();


    //��ü ī�� ����Ʈ ��������
    public void Awake()
    {
       
    }

    private void OnEnable()
    {
        //������ �÷��̾� ���� ��Ȱ��ȭ, UI ���� Ȱ��ȭ
        InputActions.keyActions.Player.Disable();
        InputActions.keyActions.UI.Enable();
        InputActions.keyActions.UI.Menu.started += Close;
    }

    private void OnDisable()
    {
        InputActions.keyActions.Player.Enable();
        InputActions.keyActions.UI.Disable();
        InputActions.keyActions.UI.Menu.started -= Close;
    }

    public void Init(bool showAllCards)
    {
        this.showAllCards = showAllCards;

        //ī�� ��ü�� ��������, �÷��̾��� ī�带 �������� �� 1
        if (showAllCards)
        {
            showedCardList = GameData.Instance.CardList;
        }
        else
        {
            showedCardList = PlayerData.Instance.playerDeck;
        }

        ShowCards();
        SortByCostButtonClick();
    }

    public void ShowCards()
    {
        //Linq�� ���. ���� �������� ���� �з���ŭ ī�� ����Ʈ���� ����.
        List<CardStruct> cardList = showedCardList.Skip(currentPage * cardsPerPage).Take(cardsPerPage).ToList();

        for (int i = 0; i < cardList.Count; i++)
        {
            CardUI cardUI = UIManager.Instance.ShowUIElement("CardUI", deckDisplayer.transform).GetComponent<CardUI>();
            cardUI.ShowCardData(cardList[i]);
        }

        UpdateButtons();

    }

    //ǥ������ ī�� ����
    private void ClearCards()
    {
        for (int i = 0; i < deckDisplayer.transform.childCount; i++)
        {
            AssetLoader.Instance.Destroy(deckDisplayer.transform.GetChild(i).gameObject);
        }
    }
    
    // ����/���� ��ư Ȱ��ȭ
    private void UpdateButtons()
    {
        prevButton.interactable = currentPage > 0;
        nextButton.interactable = (currentPage + 1) * cardsPerPage < showedCardList.Count;
    }

    //���� ��ư Ŭ���� �߻��� �̺�Ʈ
    public void NextButtonClick()
    {
        currentPage++;
        ClearCards();
        ShowCards();
        UpdateButtons();

    }

    //���� ��ư Ŭ���� �߻��� �̺�Ʈ
    public void PreviousButtonClick()
    {
        currentPage--;
        ClearCards();
        ShowCards();
        UpdateButtons();
    }


    public void SortByCostButtonClick()
    {
        sortByCostButton.interactable = false;
        sortByNameButton.interactable = true;
        showedCardList = showedCardList.OrderBy(card => card.cost).ToList();
        currentPage = 0;
        ClearCards();
        ShowCards();
        UpdateButtons();
    }

    public void SortByNameButtonClick()
    {
        sortByNameButton.interactable = false;
        sortByCostButton.interactable = true;
        showedCardList = showedCardList.OrderBy(card => card.name).ToList();
        currentPage = 0;
        ClearCards();
        ShowCards();
        UpdateButtons();
    }

    private void Close(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClosePanel("LibraryUI");
    }
}
