using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{ 
    private void Awake()
    {
        SoundManager.Instance.Play("Sounds/StageBgm", Sound.Bgm);
        Stage.Instance.InitStageManager();
        PlayerData.Instance.InitPlayerData(); //���� ����ø��� �÷��̾� ������ �ʱ�ȭ
        UIManager.Instance.ShowUI("StatUI");
        GameObject.Find("BattleCamera").GetComponent<Camera>().gameObject.SetActive(false);
        Camera.main.gameObject.SetActive(true);
        
    }
}
