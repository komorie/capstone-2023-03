using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemyUI : MonoBehaviour
{
    public EnemyData HealthData;
    public int EnemyNum;
    public bool HasAnimationWhenHealthChanges = true;
    public float AnimationDuration = 0.1f;

    TextMeshProUGUI TextUI;
    Image image;



    // Start is called before the first frame update
    void Start()
    {
        TextUI = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponentInChildren<Image>();
        HealthData = GameObject.Find("EnemyData").GetComponent<EnemyData>();

    }

    // Update is called once per frame
    void Update()
    {
        ChangeHPText();
    }

    public void init(int num)
    {
        EnemyNum = num;
    }

    public void ChangeHPText() // Text의 텍스트 내용을 CurrentHealth / MaximumHealth로 바꿔주는 함수
    {

        if (HealthData.Isalive[EnemyNum-1])
        {
            TextUI.text = HealthData.CurrentHP[EnemyNum-1] + " / " + HealthData.MaxHP[EnemyNum-1];
        }
        else
        {
            TextUI.text = "Dead";
        }

        image.fillAmount = HealthData.CurrentHP[EnemyNum-1]/HealthData.MaxHP[EnemyNum-1];
    }

}

