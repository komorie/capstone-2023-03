using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBarEnemyUI : MonoBehaviour
{
    public EnemyData ShieldData;
    public bool HasAnimationWhenHealthChanges = true;
    public float AnimationDuration = 0.1f;
    public int EnemyNum;

    TextMeshProUGUI TextUI;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        TextUI = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponentInChildren<Image>();
        ShieldData = GameObject.Find("EnemyData").GetComponent<EnemyData>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeShieldText();
    }

    void ChangeShieldText() // Text의 텍스트 내용을 Shield로 바꿔주는 함수
    {
        TextUI.text = ShieldData.Shield[0].ToString();
        if (ShieldData.Shield[EnemyNum-1] == 0)
        {
            image.fillAmount = 0;
        }
        else
        {
            image.fillAmount = 1;
        }
    }

    public void init(int num)
    {
        EnemyNum = num;
    }
}
