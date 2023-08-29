using DataStructs;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;


//�������� ī�� ��� UI
public class ShopPurchaseUI : MonoBehaviour
{
    [SerializeField]
    public TMP_Text reCostText; //���Ѻ��
    [SerializeField]
    public TMP_Text playerMoneyText; //�÷��̾� ��
    [SerializeField]
    public GameObject shopCards; //ī�� ������

    List<CardStruct> showedCardList;

    void Awake()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        UpdateUI();
        PlayerData.Instance.OnDataChange += UpdateUI; //���Ȱ��� ��ȭ �� UI ����
        ShopData.Instance.OnDataChange += UpdateUI; //���� �����Ϳ� ��ȭ �� UI ����
    }
    private void OnDisable()
    {
        PlayerData.Instance.OnDataChange -= UpdateUI;
        ShopData.Instance.OnDataChange -= UpdateUI;
    }

    //UI ���ΰ�ħ
    public void UpdateUI()
    {
        reCostText.text = ShopData.Instance.RerollCost.ToString();
        playerMoneyText.text = PlayerData.Instance.Money.ToString();
        ShowShopCards();
    }

    //ǥ������ ī�� ����
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

        //CardUI �������� ��������, shopCards�� ���� ������Ʈ��� �߰�
        //���� �ؽ�Ʈ�� ������ cardUI �Ʒ��� �ؽ�Ʈ�� ���

        for(int index = 0; index < showedCardList.Count; index++)
        {
            CardUI cardUI = AssetLoader.Instance.Instantiate("Prefabs/UI/CardUI", shopCards.transform).GetComponent<CardUI>();
            cardUI.ShowCardData(showedCardList[index]); //���� ī�� UI ǥ��

            //���� �ؽ�Ʈ ǥ��
            TMP_Text priceText = cardUI.transform.Find("Base/PriceText").GetComponent<TextMeshProUGUI>();
            priceText.gameObject.SetActive(true);   
            priceText.text = $"{ShopData.Instance.Prices[index]}��";

            //���콺 �̺�Ʈ ���
            cardUI.OnCardClicked += (cardUI) => { ShopData.Instance.Purchase(cardUI.Card); }; //Ŭ�� �� ����
            cardUI.OnCardEntered += (cardUI) => { cardUI.CardBig(); };
            cardUI.OnCardExited += (cardUI) => { cardUI.CardSmall(); };
        }
    }

    public void RerollClick()
    {
        ShopData.Instance.RerollShop(); //���� ���ΰ�ħ
    }    

    public void ExitClick()
    {
        UIManager.Instance.HideUI(name);
    }

}
