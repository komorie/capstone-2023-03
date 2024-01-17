using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{ 
    private void Awake()
    {
        SoundManager.Instance.Play("Sounds/StageBgm", Sound.Bgm);
        Stage.Instance.InitStageManager();
        PlayerData.Instance.InitPlayerData(); //게임 입장시마다 플레이어 데이터 초기화
        UIManager.Instance.ShowUI("StatUI");
        GameObject.Find("BattleCamera").GetComponent<Camera>().gameObject.SetActive(false);
        Camera.main.gameObject.SetActive(true);
        
    }
}
