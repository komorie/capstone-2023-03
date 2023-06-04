using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DataStructs;
using Unity.VisualScripting;

public class EnemyData : Singleton<EnemyData>
{
    public List<bool> Isalive { get; set; } = new List<bool> { false, false, false };
    public List<float> CurrentHP { get; set; } = new List<float> { 5, 5, 5 };
    public List<float> MaxHP { get; set; } = new List<float>{5, 5, 5};
    public List<float> Shield { get; set; } = new List<float> { 0, 0, 0 };
    public List<int> Pat { get; set; } = new List<int> {0,0,0};
    public List<string> PatText { get; set; } = new List<string> { "", "", "" };
    public List<int> Str { get; set; } = new List<int> { 0, 0, 0 };
    public List<int> Fire { get; set; } = new List<int> { 0, 0, 0 };
    public List<int> Ice { get; set; } = new List<int> { 0, 0, 0 };
    public List<int> Ready { get; set; } = new List<int> { 0, 0, 0 };
    public List<int> Boom { get; set; } = new List<int> { 0, 0, 0 };
    public List<int> Load { get; set; } = new List<int> { 0, 0, 0 };
    public List<bool> IsLoad { get; set; } = new List<bool> { false, false, false };
    public List<int> Regen { get; set; } = new List<int> { 0, 0, 0 };
    public List<int> Guard { get; set; } = new List<int> { 0, 0, 0 };
    public List<int> Sal { get; set; } = new List<int> { 0, 0, 0 };
    public EnemyStruct Enemy1;
    public EnemyStruct Enemy2;
    public EnemyStruct Enemy3;
    public List<EnemyStruct> EnemyList;

    public int stage;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        EnemyList = new List<EnemyStruct> { Enemy1, Enemy2, Enemy3 };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(int num, string name, int stage)
    {
        this.stage = stage;
        Isalive[num - 1] = true;
        int random;
        switch (num)
        {
            case 1:
                Enemy1 = AllEnemyData.Instance.GetEnemyData(name, stage);
                random = UnityEngine.Random.Range(Enemy1.minHP, Enemy1.maxHP+1);
                MaxHP[0] = random;
                CurrentHP[0] = MaxHP[0];
                EnemyList[0] = Enemy1;
                break;
            case 2:
                Enemy2 = AllEnemyData.Instance.GetEnemyData(name, stage);
                random = UnityEngine.Random.Range(Enemy2.minHP, Enemy2.maxHP + 1);
                MaxHP[1] = random;
                CurrentHP[1] = MaxHP[1];
                EnemyList[1] = Enemy2;
                break;
            case 3:
                Enemy3 = AllEnemyData.Instance.GetEnemyData(name, stage);
                random = UnityEngine.Random.Range(Enemy3.minHP, Enemy3.maxHP + 1);
                MaxHP[2] = random;
                CurrentHP[2] = MaxHP[2];
                EnemyList[2] = Enemy3;
                break;
        }
    }

    public void Reset()
    {
        Isalive = new List<bool> { false, false, false };
        MaxHP = new List<float> { 5,5,5 };
        CurrentHP = new List<float> { 5, 5, 5 };
        Shield = new List<float> { 0, 0, 0 };
        Enemy1 = new EnemyStruct();
        Enemy2 = new EnemyStruct();
        Enemy3 = new EnemyStruct();
        EnemyList = new List<EnemyStruct> { Enemy1, Enemy2, Enemy3 };
        Pat = new List<int> { 0, 0, 0 };
        PatText = new List<string> { "", "", "" };
        Str = new List<int> { 0, 0, 0 };
        Fire = new List<int> { 0, 0, 0 };
        Ice = new List<int> { 0, 0, 0 };
        Ready = new List<int> { 0, 0, 0 };
        Boom = new List<int> { 0, 0, 0 };
        Load = new List<int> { 0, 0, 0 };
        IsLoad = new List<bool> { false, false, false };
        Regen = new List<int> { 0, 0, 0 };
        Guard = new List<int> { 0, 0, 0 };
        Sal = new List<int> { 0, 0, 0 };

    }

    public void SetPat(int num)
    {
        int random;
        EnemyStruct enemy = EnemyList[num];
        random = UnityEngine.Random.Range(1,5);
        bool set = false;
        while (!set)
        {
            switch (random)
            {
                case 1:
                    if (enemy.pat1 != "None")
                    {
                        Pat[num] = 1;
                        PatText[num] = enemy.pat1;
                        set = true; 
                        break;
                    }
                    else
                    {
                        random = UnityEngine.Random.Range(1,5);
                        break;
                    }
                case 2:
                    if (enemy.pat2 != "None")
                    {
                        Pat[num] = 2;
                        PatText[num] = enemy.pat2;
                        set = true;
                        break;
                    }
                    else
                    {
                        random = UnityEngine.Random.Range(1, 5);
                        break;
                    }
                case 3:
                    if (enemy.pat3 != "None")
                    {
                        Pat[num] = 3;
                        PatText[num] = enemy.pat3;
                        set = true;
                        break;
                    }
                    else
                    {
                        random = UnityEngine.Random.Range(1, 5);
                        break;
                    }
                case 4:
                    if (enemy.pat4 != "None")
                    {
                        Pat[num] = 4;
                        PatText[num] = enemy.pat4;
                        set = true;
                        break;
                    }
                    else
                    {
                        random = UnityEngine.Random.Range(1, 5);
                        break;
                    }
            }

        }
    }
}
