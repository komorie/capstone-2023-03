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

        for(int index = 0; index < showedCardList.Count; index++)
        {
            CardUI cardUI = AssetLoader.Instance.Instantiate("Prefabs/UI/CardUI", shopCards.transform).GetComponent<CardUI>();
            cardUI.ShowCardData(showedCardList[index]); //현재 카드 UI 표시

            //가격 텍스트 표시
            TMP_Text priceText = cardUI.transform.Find("Base/PriceText").GetComponent<TextMeshProUGUI>();
            priceText.gameObject.SetActive(true);   
            priceText.text = $"{ShopData.Instance.Prices[index]}원";

            //마우스 이벤트 등록
            cardUI.OnCardClicked += (cardUI) => { ShopData.Instance.Purchase(cardUI.Card); }; //클릭 시 구매
            cardUI.OnCardEntered += (cardUI) => { cardUI.CardBig(); };
            cardUI.OnCardExited += (cardUI) => { cardUI.CardSmall(); };
        }
    }

    public void RerollClick()
    {
        ShopData.Instance.RerollShop(); //상점 새로고침
    }    

    public void ExitClick()
    {
        UIManager.Instance.HideUI(name);
    }

}
