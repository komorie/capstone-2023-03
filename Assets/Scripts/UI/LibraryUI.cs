using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DataStructs;
using System;

public enum LibraryMode
{
    Library, //일반 라이브러리 모드
    Deck, //플레이어 덱 보여주기 모드
    EventDiscard, //플레이어 덱 보여주기 + 이벤트로 카드 버리기 모드
    ShopDiscard, //플레이어 덱 보여주기 + 상점에서 카드 버리기 모드
    Battle_Deck, //배틀 중에 남은 덱 보여주기
    Battle_Trash, //배틀 중에 버린 카드 보여주기
    Battle_Trash_Hand, // 배틀 중에 손패 보여주기 + 버리기
    Battle_Use_Hand // 배틀 중에 손패 보여주기 + 한 장 선택
}

//C# LInq 사용: 데이터 쿼리를 C#에서 스크립트로 사용할 수 있도록 하는 기술.
//배열 및 다른 컬렉션에서 쉽게 원하는 구역만 가져올 수 있음.

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
        PlayerData.Instance.OnDataChange += RefreshLibrary; //이거는 덱이 바뀔 때마다 그것을 감지하여 카드 UI를 새로고침하기 위함.
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

    public void RefreshLibrary() //처음 혹은 덱이 바뀌었을 때 호출되어, 카드 UI를 새로고침한다.
    {
        switch (libraryMode) //공격, 스킬, 애청자 카드 전부 보여주기
        {
            case LibraryMode.Library:
                showedCardList = LibraryData.Instance.Library;
                break;
            case LibraryMode.Deck: //현재 덱 보여주기
                showedCardList = PlayerData.Instance.Deck;
                break;
            case LibraryMode.EventDiscard: //현재 덱 보여주기 + 이벤트로 카드 한 장 버리기
                showedCardList = PlayerData.Instance.Deck;
                break;
            case LibraryMode.ShopDiscard: //현재 덱 보여주기 + 이벤트로 클릭하는 만큼 버리기 + 버리든 말든 자유
                showedCardList = PlayerData.Instance.Deck;
                break;
            case LibraryMode.Battle_Deck: //배틀 중에 남은 덱 보여주기
                showedCardList = BattleData.Instance.Deck;
                break;
            case LibraryMode.Battle_Trash: //배틀 중에 버린 카드 보여주기
                showedCardList = BattleData.Instance.Trash;
                break;
            case LibraryMode.Battle_Trash_Hand: //배틀 중에 손패 보여주기 + 버리기
                showedCardList = BattleData.Instance.Hand;
                break;
            case LibraryMode.Battle_Use_Hand: //배틀 중에 손패 보여주기 + 한 장 선택
                showedCardList = BattleData.Instance.Hand;
                break;
        }
        ShowCards();
        SortByCostButtonClick();
    }

    //표시중인 카드 제거
    private void ClearCards()
    {
        for (int i = 0; i < deckDisplayer.transform.childCount; i++)
        {
            AssetLoader.Instance.Destroy(deckDisplayer.transform.GetChild(i).gameObject);
        }
    }

    public void ShowCards()
    {

        ClearCards(); //전에 표시되던 카드 제거

        int start = currentPage * cardsPerPage;
        int end = Math.Min(showedCardList.Count, start + cardsPerPage); //시작페이지와 끝페이지

        for (int i = start; i < end; i++)
        {

            CardUI cardUI;
            BattleUI battleUI;

            cardUI = AssetLoader.Instance.Instantiate("Prefabs/UI/CardUI", deckDisplayer.transform).GetComponent<CardUI>();
            cardUI.ShowCardData(showedCardList[i]); //카드를 소환

            switch (libraryMode)
            {
                case LibraryMode.EventDiscard: //현재 덱 보여주기 + 카드 버리기 1회
                    cardUI.OnCardClicked += (cardUI) => //카드 클릭 시 하단의 이벤트 발동하도록 등록
                    {
                        PlayerData.Instance.Deck.Remove(cardUI.Card); //해당 카드 UI의 카드를 덱에서 제거
                        UIManager.Instance.HideUI("LibraryUI"); //버림 후에는 바로 라이브러리 UI 닫기.
                    };
                    cardUI.OnCardEntered += (cardUI) => { cardUI.CardBig(); }; //카드에 마우스 들어갈 시 해당 카드 확대 수행하도록 등록.
                    cardUI.OnCardExited += (cardUI) => { cardUI.CardSmall(); }; //카드에서 마우스 나갈 시 해당 카드 축소 수행하도록 등록.
                    break;
                case LibraryMode.ShopDiscard: //현재 덱 보여주기 + 카드 버리기 제한X
                    cardUI.OnCardClicked += (cardUI) => //카드 클릭 시 하단의 이벤트 발동하도록 등록
                    {
                        int newMoney = PlayerData.Instance.Money - ShopData.Instance.DiscardCost;
                        if (newMoney > 0) //돈이 남은 경우만
                        {
                            PlayerData.Instance.Deck.Remove(cardUI.Card); //해당 카드를 버리기
                            PlayerData.Instance.Money = newMoney; //제거 비용만큼 플레이어 돈에서 차감하기
                            PlayerData.Instance.NotifyDataChange(); //덱 변경 알리기
                            ShopData.Instance.DiscardCost += 25; //삭제 비용 25 추가
                            ShopData.Instance.NotifyDataChange(); //상점 데이터 변경 알리기
                        }
                    };
                    cardUI.OnCardEntered += (cardUI) => { cardUI.CardBig(); }; 
                    cardUI.OnCardExited += (cardUI) => { cardUI.CardSmall(); };
                    break;
                case LibraryMode.Battle_Trash_Hand: //배틀 중에 손패 카드 보여주기 + 카드 버리기 1회
                    battleUI = GameObject.Find("UIRoot").transform.GetChild(2).GetComponent<BattleUI>();
                    cardUI.OnCardClicked += (cardUI) => 
                    {
                        battleUI.Discard(cardUI.Card); 
                        UIManager.Instance.HideUI("LibraryUI"); 
                    };
                    cardUI.OnCardEntered += (cardUI) => { cardUI.CardBig(); }; 
                    cardUI.OnCardExited += (cardUI) => { cardUI.CardSmall(); };
                    break;
                case LibraryMode.Battle_Use_Hand: //배틀 중에 손패 카드 보여주기 + 한 장 선택
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
    
    // 이전/다음 버튼 활성화
    private void UpdateButtons()
    {
        prevButton.gameObject.SetActive(currentPage > 0);
        nextButton.gameObject.SetActive((currentPage + 1) * cardsPerPage < showedCardList.Count);
    }

    //다음 버튼 클릭시 발생할 이벤트
    public void NextButtonClick()
    {
        currentPage++;
        ShowCards();
        UpdateButtons();

    }

    //이전 버튼 클릭시 발생할 이벤트
    public void PreviousButtonClick()
    {
        currentPage--;
        ShowCards();
        UpdateButtons();
    }

    //코스트순 정렬 버튼. 
    public void SortByCostButtonClick()
    {
        sortByCostButton.interactable = false;
        sortByNameButton.interactable = true;

        sortByCostButton.GetComponentInChildren<TMP_Text>().color = Color.grey;
        sortByNameButton.GetComponentInChildren<TMP_Text>().color = Color.white;

        showedCardList.Sort((card1, card2) => card1.cost.CompareTo(card2.cost)); //코스트를 기준으로 정렬
        currentPage = 0;
        ShowCards();
        UpdateButtons();
    }

    //이름순 정렬 버튼. 
    public void SortByNameButtonClick()
    {
        sortByNameButton.interactable = false;
        sortByCostButton.interactable = true;

        sortByNameButton.GetComponentInChildren<TMP_Text>().color = Color.grey;
        sortByCostButton.GetComponentInChildren<TMP_Text>().color = Color.white;

        showedCardList.Sort((card1, card2) => card1.name.CompareTo(card2.name)); //이름를 기준으로 정렬
        currentPage = 0;
        ShowCards();
        UpdateButtons();
    }

    //나가기 버튼, UI 닫기
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
