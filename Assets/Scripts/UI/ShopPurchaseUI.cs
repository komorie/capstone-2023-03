using DataStructs;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;


//상점에서 카드 사는 UI
public class ShopPurchaseUI : MonoBehaviour
{
    [SerializeField]
    public TMP_Text reCostText; //리롤비용
    [SerializeField]
    public TMP_Text playerMoneyText; //플레이어 돈
    [SerializeField]
    public GameObject shopCards; //카드 전시장

    List<CardStruct> showedCardList;

    void Awake()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        UpdateUI();
        PlayerData.Instance.OnDataChange += UpdateUI; //스탯값에 변화 시 UI 갱신
        ShopData.Instance.OnDataChange += UpdateUI; //상점 데이터에 변화 시 UI 갱신
    }
    private void OnDisable()
    {
        PlayerData.Instance.OnDataChange -= UpdateUI;
        ShopData.Instance.OnDataChange -= UpdateUI;
    }

    //UI 새로고침
    public void UpdateUI()
    {
        reCostText.text = ShopData.Instance.RerollCost.ToString();
        playerMoneyText.text = PlayerData.Instance.Money.ToString();
        ShowShopCards();
    }

    //표시중인 카드 제거
    private void ClearCards()
    {
        for (int i = 0; i < shopCards.transform.childCount; i++)
        {
            AssetLoader.Instance.Destroy(shopCards.transform.GetChild(i).gameObject);
        }
    }


    public void ShowShopCards()
    {

        ClearCards();

        showedCardList = ShopData.Instance.ShopCardsList;

        //CardUI 프리팹을 가져오고, shopCards의 하위 오브젝트들로 추가
        //가격 텍스트를 각각의 cardUI 아래에 텍스트로 출력

        for(int i = 0; i < showedCardList.Count; i++)
        {
            CardUI cardUI = AssetLoader.Instance.Instantiate("Prefabs/UI/CardUI", shopCards.transform).GetComponent<CardUI>();
            cardUI.ShowCardData(showedCardList[i]); //현재 카드 UI 표시

            int price; //카드의 레어도에 따라 가격을 변환합니다.

            switch (showedCardList[i].rarity)
            {
                case 0:
                    price = Random.Range(8, 11) * 5; break; //40, 45, 50
                case 1:
                    price = Random.Range(13, 16) * 5; break; //65, 70, 75
                case 2:
                    price = Random.Range(24, 30) * 5; break; //120, 125... 145, 150
                default:
                    price = 99999; break;
            }

            //가격 텍스트 가져와서 수정
            TMP_Text priceText = cardUI.transform.Find("Base/PriceText").GetComponent<TextMeshProUGUI>();
            priceText.gameObject.SetActive(true);   
            priceText.text = $"{price}원";

            //마우스 이벤트 등록
            cardUI.OnCardClicked += (cardUI) =>
            {
                // 플레이어가 충분한 돈을 가지고 있는지 확인
                if (PlayerData.Instance.Money >= price)
                {
                    PlayerData.Instance.Money -= price; // 돈을 지불
                    AssetLoader.Instance.Destroy(cardUI.gameObject); //카드 UI 제거

                    PlayerData.Instance.Deck.Add(cardUI.Card); // 카드를 덱에 추가
                    PlayerData.Instance.NotifyDataChange(); //덱 변경 알려서 UI 새로고침하도록!

                    ShopData.Instance.ShopCardsList.Remove(cardUI.Card);  //상점에서 카드 삭제
                    ShopData.Instance.NotifyDataChange(); //상점 데이터 변경 알려서 UI 새로고침하도록!
                }
                else
                {
                    Debug.Log("돈이 부족합니다.");
                }
            };
            cardUI.OnCardEntered += (cardUI) => { cardUI.CardBig(); };
            cardUI.OnCardExited += (cardUI) => { cardUI.CardSmall(); };
        }
    }

    public void RerollClick()
    {
        // 플레이어가 리롤하기 충분한 돈을 가지고 있는지 확인
        int rerollCost = ShopData.Instance.RerollCost;
        if (PlayerData.Instance.Money >= rerollCost)
        {
            PlayerData.Instance.Money -= rerollCost; // 돈을 지불
            ShopData.Instance.RerollCost += 25; //리롤비용 25 증가
            ShopData.Instance.InitShopData(); //상점 카드 리스트 초기화
            ShopData.Instance.NotifyDataChange(); //상점 데이터 변경 알려서 UI 새로고침하도록!
        }
        else
        {
            Debug.Log("돈이 부족합니다.");
        }
    }    

    public void ExitClick()
    {
        UIManager.Instance.HideUI(name);
    }

}
