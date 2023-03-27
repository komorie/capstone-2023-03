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
    private int cardsPerPage = 2;
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

    private List<CardData> showedCardList= new List<CardData>();


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

        ShowCards();

    }

    public void ShowCards()
    {
        //ī�� ��ü�� ��������, �÷��̾��� ī�带 �������� �� 1
        if(showAllCards)
        {
            showedCardList = GameDataCon.Instance.CardList;
        }
        else
        {
            showedCardList = PlayDataCon.Instance.PlayData.playerCardData;
        }

        //Linq�� ���. ���� �������� ���� �з���ŭ ī�� ����Ʈ���� ����.
        var cardList = showedCardList.Skip(currentPage * cardsPerPage).Take(cardsPerPage);

        foreach (CardData cardData in cardList)
        {
            CardUI cardUI = AssetLoader.Instance.Instantiate("Prefabs/Button/CardButton").GetComponent<CardUI>();
            cardUI.transform.SetParent(deckDisplayer.transform);   
            cardUI.ShowCardData(cardData);
        }

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


    private void Close(InputAction.CallbackContext context)
    {
        PanelManager.Instance.ClosePanel("LibraryUI");
    }
}
