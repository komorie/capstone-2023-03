using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleWinUI : MonoBehaviour
{
    [SerializeField]
    private Button ReturnButton;

    string Room;
    GameObject NowRoom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(string Room)
    {
        this.Room = Room;
    }

    // Update is called once per frame
    public void BackButtonClick()
    {
        PlayerData.Instance.CurrentHp = BattleData.Instance.CurrentHealth;

        UIManager.Instance.HideUI("BattleWinUI");
        UIManager.Instance.HideUI("BattleUI");
        UIManager.Instance.HideUI("BackGroundUI");

        GameObject mainCameraParent = GameObject.Find("PlayerCameraParent");
        GameObject battleCameraParent = GameObject.Find("BattleCameraParent");
        if (battleCameraParent != null)
        {
            mainCameraParent.transform.GetChild(0).gameObject.SetActive(true);
            battleCameraParent.transform.GetChild(0).gameObject.SetActive(false);
        }
        SoundManager.Instance.Play("Sounds/StageBgm", Sound.Bgm);
        NowRoom = GameObject.Find(Room);
        NowRoom.transform.Find("EnemySymbol").GetComponent<EnemySymbol>().FightEnd();
    }

    
        
}
