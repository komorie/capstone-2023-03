using DataStructs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class CardEffect : MonoBehaviour
{
    private void OnEnable()
    {

    }

    private void OnDisable()
    {


    }

    public static async void UseCardEffect(CardStruct card)
    {
        BattleUI battleUI = FindObjectOfType<BattleUI>();

        int index = card.index;
        string name = card.name;
        string type = card.type;
        string attack_type = card.attack_type;
        string target = card.target;
        int damage = card.damage;
        int times = card.times;
        string special = card.special;
        int special_stat = card.special_stat;
        if (type == "Attack")
        {
            if (target == "One")
            {
                TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
                UIManager.Instance.ShowUI("SelectEnemyUI", false).GetComponent<SelectEnemyUI>().init(taskCompletionSource);


                await taskCompletionSource.Task;
                if (times > 0)
                {
                    for (int i = 0; i < times; i++)
                    {

                        if (attack_type == "Physical")
                        {
                            Battle.ChangeEnemyShield(BattleData.Instance.SelectedEnemy, -(damage + BattleData.Instance.Str));
                        }
                        else if (attack_type == "Magic")
                        {
                            Battle.ChangeEnemyShield(BattleData.Instance.SelectedEnemy, -(damage + BattleData.Instance.Int));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < BattleData.Instance.UseEnergy; i++)
                    {
                        if (attack_type == "Physical")
                        {
                            Battle.ChangeEnemyShield(BattleData.Instance.SelectedEnemy, -(damage + BattleData.Instance.Str));
                        }
                        else if (attack_type == "Magic")
                        {
                            Battle.ChangeEnemyShield(BattleData.Instance.SelectedEnemy, -(damage + BattleData.Instance.Int));
                        }
                    }
                }
                if (special != "None")
                {
                    Battle.EnemyDebuff(BattleData.Instance.SelectedEnemy, special, special_stat);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (EnemyData.Instance.Isalive[i])
                    {
                        if (times > 0)
                        {
                            for (int j = 0; j < times; j++)
                            {

                                if (attack_type == "Physical")
                                {
                                    Battle.ChangeEnemyShield(i, -(damage + BattleData.Instance.Str));
                                }
                                else if (attack_type == "Magic")
                                {
                                    Battle.ChangeEnemyShield(i, -(damage + BattleData.Instance.Int));
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < BattleData.Instance.UseEnergy; j++)
                            {
                                if (attack_type == "Physical")
                                {
                                    Battle.ChangeEnemyShield(i, -(damage + BattleData.Instance.Str));
                                }
                                else if (attack_type == "Magic")
                                {
                                    Battle.ChangeEnemyShield(i, -(damage + BattleData.Instance.Int));
                                }
                            }
                        }
                        if (special != "None")
                        {
                            Battle.EnemyDebuff(i, special, special_stat);
                        }
                    }
                }
            }
        }
        else if (type == "Skill")
        {
            if (target == "One")
            {
                TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
                UIManager.Instance.ShowUI("SelectEnemyUI", false).GetComponent<SelectEnemyUI>().init(taskCompletionSource);


                await taskCompletionSource.Task;

                if (special != "None")
                {
                    Battle.EnemyDebuff(BattleData.Instance.SelectedEnemy, special, special_stat);
                }
            }
            else if (target == "All")
            {
                for (int i = 0; i < 3; i++)
                {
                    if (EnemyData.Instance.Isalive[i])
                    {
                        if (special != "None")
                        {
                            Battle.EnemyDebuff(i, special, special_stat);
                        }
                    }
                }
                if (index == 20)
                {
                    Battle.ChangeCurrentShield(5, -1);
                }
                else if (index == 26)
                {
                    Battle.ChangeCurrentShield(15, -1);
                }
            }
            else if (target == "Self")
            {
                switch (special)
                {
                    case "Shield":
                        Battle.ChangeCurrentShield(special_stat, -1);
                        break;
                    case "burn":
                        BattleData.Instance.burn = true;
                        BattleData.Instance.Int += 2;
                        break;
                }
                if(index == 25)
                {
                    battleUI.Draw();
                }
            }
        }

        else if (type == "Viewer")
        {
            switch (index)
            {
                case 28:
                    BattleData.Instance.CurrentEnergy *= 2;
                    break;
                case 29:
                    BattleData.Instance.CurrentEnergy += 2;
                    BattleData.Instance.Int += 1;
                    BattleData.Instance.Str += 1;
                    break;
                case 30:
                    for (int i = 0; i < 3; i++)
                    {
                        battleUI.Draw();
                    }
                    break;
                case 31:
                    for (int i = 0; i < 2; i++)
                    {
                        battleUI.Draw();
                    }
                    BattleData.Instance.CurrentEnergy += 2;
                    break;
            }
        }
        else
        {
            if (target == "Ran")
            {
                int num = Battle.RandomEnemy();
                if (times > 0)
                {
                    for (int i = 0; i < times; i++)
                    {
                        Battle.ChangeEnemyShield(num, -damage);
                    }
                }
                if (special != "None")
                {
                    Battle.EnemyDebuff(num, special, special_stat);
                }
            }
            else if (target == "All")
            {
                for (int i = 0; i < 3; i++)
                {
                    if (EnemyData.Instance.Isalive[i])
                    {
                        if (times > 0)
                        {
                            for (int j = 0; j < times; j++)
                            {

                                Battle.ChangeEnemyShield(i, -damage);
                            }
                        }
                        if (special != "None")
                        {
                            Battle.EnemyDebuff(i, special, special_stat);
                        }
                    }
                }
            }
            else
            {
                if (special == "Shield")
                {
                    Battle.ChangeCurrentShield(special_stat, -1);
                }
                else
                {
                    BattleData.Instance.CurrentEnergy += 1;
                    battleUI.Draw();
                }
            }
            battleUI.Draw();
        }
        
    }
}
