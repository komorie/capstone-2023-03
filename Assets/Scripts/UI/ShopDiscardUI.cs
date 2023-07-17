using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DataStructs;


//상점에서 카드 버리는 UI
public class ShopDiscardUI : BaseUI
{
    [SerializeField]
    public TMP_Text disCostText; //제거비용
    [SerializeField]
    public TMP_Text playerMoneyText; //플레이어 돈
    [SerializeField]
    public LibraryUI libraryUI;


    private void Awake()
    {
        libraryUI.Init(LibraryMode.ShopDiscard); //소지중인 라이브러리 UI를 상점 제거 모드로 초기화
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

    public void UpdateUI()
    {
        disCostText.text = ShopData.Instance.DiscardCost.ToString();
        playerMoneyText.text = PlayerData.Instance.Money.ToString();
    }

}
