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
        HealthData = GameObject.Find("Enemy").GetComponent<EnemyData>();

    }

    // Update is called once per frame
    void Update()
    {
        HealthData = GameObject.Find("Enemy").GetComponent<EnemyData>();
        ChangeHPText();
    }

    public void init(int num)
    {
        EnemyNum = num;
    }

    public void ChangeHPText() // Text�� �ؽ�Ʈ ������ CurrentHealth / MaximumHealth�� �ٲ��ִ� �Լ�
    {

        if (HealthData.Isalive[EnemyNum-1])
        {
            TextUI.text = HealthData.HP[EnemyNum-1].ToString();
        }
        else
        {
            TextUI.text = "Dead";
        }

        image.fillAmount = 1;
    }

}

