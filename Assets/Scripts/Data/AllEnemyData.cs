using DataStructs;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AllEnemyData : Singleton<AllEnemyData>
{
    private List<EnemyStruct> enemyStructs;

    public List<EnemyStruct> EnemyStructs { get => enemyStructs; set => enemyStructs = value; }

    public List<string> NoneEnemyNames { get; set;} = new List<string> {"Knight","Fighter","Peasant","Priest","Thief"};
    public List<string> PirateEnemyNames { get; set; } = new List<string> { "Dealer", "Tanker", "Supporter"};
    public List<string> DruidEnemyNames { get; set; } = new List<string> { "Bird", "Dog", "Bear" };
    public List<string> PriestEnemyNames { get; set; } = new List<string> { "Believer", "Bruth", "Pagan" };
    public List<string> MechanicEnemyNames { get; set; } = new List<string> { "Attacker", "Shielder", "Repairer"};

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        LoadAllEnemyData();
    }

    public void LoadAllEnemyData()
    {
        GameDataLoader.LoadData("Data/EnemyData", out enemyStructs);
    }

    public EnemyStruct GetEnemyData(string name, int stage)
    {
        foreach (EnemyStruct enemyStruct in EnemyStructs)
        {
            if (enemyStruct.name == name && enemyStruct.stage == stage)
            {
                return enemyStruct;
            }
        }
        return null;
    }
}
