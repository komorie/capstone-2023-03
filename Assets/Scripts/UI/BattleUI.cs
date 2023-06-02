using DataStructs;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BattleUI : BaseUI
{
    [SerializeField]
    private GameObject HandUI;
    [SerializeField]
    private Button TrashUI;
    [SerializeField]
    private Button DeckUI;
    [SerializeField]
    private Button EndUI;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject Enemy;


    TextMeshProUGUI DeckNum;
    TextMeshProUGUI TrashNum;
    TextMeshProUGUI EnergyText;
    TextMeshProUGUI TurnText;
    TextMeshProUGUI Enemy1Text;
    TextMeshProUGUI Enemy2Text;
    TextMeshProUGUI Enemy3Text;

    bool IsCoroutineRun = false;

    int EnemyInfo;
    string Room;
    int stage;

    

    NoticeUI noticeUI;
    // Start is called before the first frame update
    void Start()
    {

        DeckNum = GameObject.Find("Deck").GetComponentInChildren<TextMeshProUGUI>();
        TrashNum = GameObject.Find("Trash").GetComponentInChildren<TextMeshProUGUI>();
        EnergyText = GameObject.Find("Energy").GetComponentInChildren<TextMeshProUGUI>();
        TurnText = GameObject.Find("Turn").GetComponentInChildren<TextMeshProUGUI>();
        Enemy1Text = GameObject.Find("Enemy1").GetComponentInChildren<TextMeshProUGUI>();
        Enemy2Text = GameObject.Find("Enemy2").GetComponentInChildren<TextMeshProUGUI>();
        Enemy3Text = GameObject.Find("Enemy3").GetComponentInChildren<TextMeshProUGUI>();

        noticeUI = FindObjectOfType<NoticeUI>();

        //Canvas�� ī�޶� BattleCamera�� ����, �׷� ī�޶� ���ٸ� ���� ī�޶�� ����
        Canvas canvas = GetComponent<Canvas>();
        GameObject battleCameraParent = GameObject.Find("BattleCameraParent");
        Camera mainCamera = Camera.main;
        if (battleCameraParent != null)
        {
            canvas.worldCamera = battleCameraParent.transform.GetChild(0).GetComponent<Camera>();
        }
        else
        {
            canvas.worldCamera = mainCamera;
        }
        LoadEnemy();
        StartCoroutine(Turn_Start());
    }

    private void Update()
    {
        HandSpacingChange();
        UpdateDeckTrashNum();
        UpdateEnergy();
        UpdateTurn();
        

        if (!IsCoroutineRun)
        {

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                OnPauseStarted();
            }

            if (Input.GetKeyUp(KeyCode.L))
            {
                PlayerWin();
            }

            if (BattleData.Instance.CurrentHealth <= 0)
            {
                StartCoroutine(PlayerDie());
            }

            for(int i = 0; i < 3; i++)
            {
                if (!EnemyData.Instance.Isalive[i] && Enemy.transform.Find("Enemy"+(i+1).ToString()).childCount > 1)
                {
                    Debug.Log(Enemy.transform.Find("Enemy" + (i+1).ToString()).childCount);
                    Debug.Log(i);
                    StartCoroutine(EnemyDie(i));
                }
            }

            if (!EnemyData.Instance.Isalive[0] && !EnemyData.Instance.Isalive[1] && !EnemyData.Instance.Isalive[2])
            {
                PlayerWin();
            }
        }

        
    }

    public void Init(int EnemyInfo, string Room, int stage)
    {
        this.EnemyInfo = EnemyInfo;
        this.Room = Room;
        this.stage = stage;
        Debug.Log(EnemyInfo);
    }



    public void TrashClick()
    {
        UIManager.Instance.ShowUI("LibraryUI")
            .GetComponent<LibraryUI>()
            .Init(LibraryMode.Battle_Trash);
    }

    public void DeckClick()
    {
        UIManager.Instance.ShowUI("LibraryUI")
            .GetComponent<LibraryUI>()
            .Init(LibraryMode.Battle_Deck);
    }

    //Hand UI�� �ڽ��� ���ڿ� ���� Hand UI�� Horizontal Layout Group�� Spacing�� ����
    public void HandSpacingChange()
    {

        int childCount = transform.Find("Thing").Find("Hand").childCount;
        float spacing = 0;
        switch (childCount)
        {
            case < 1:
                spacing = 0;
                break;
            case 2:
                spacing = -500;
                break;
            case 3:
                spacing = -390;
                break;
            case 4:
                spacing = -275;
                break;
            case 5:
                spacing = -165;
                break;
            case 6:
                spacing = -250;
                break;
            case > 6:
                spacing = -225;
                break;
        }
        transform.Find("Thing").Find("Hand").GetComponent<UnityEngine.UI.HorizontalLayoutGroup>().spacing = spacing;
    }

    //������ ������ ī�带 �̾Ƽ� �տ� �߰�
    public void Draw()
    {
        bool CanDraw;
        CardUI cardUI;

        CanDraw = Battle.Draw();

        if (CanDraw)
        {
            cardUI = AssetLoader.Instance.Instantiate("Prefabs/UI/CardUI", HandUI.transform).GetComponent<CardUI>();
            cardUI.ShowCardData(BattleData.Instance.Hand[BattleData.Instance.Hand.Count - 1]);

            Vector3 pos = cardUI.transform.localPosition;
            pos.z = 43.25f;
            cardUI.transform.localPosition = pos;
            cardUI.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            cardUI.AddComponent<Draggable>();

            SoundManager.Instance.Play("Sounds/DrawBgm");
        }
        else
        {
            noticeUI.ShowNotice("뽑을 카드가 없습니다!");
        }
    }
    // �Է¹��� ī�带 Hand UI���� ����
    public void Discard(CardStruct card)
    {

        Battle.Discard(card);
        
        for (int i = 0; i < HandUI.transform.childCount; i++)
        {
            if (HandUI.transform.GetChild(i).GetComponent<CardUI>().Card.name == card.name)
            {
                Destroy(HandUI.transform.GetChild(i).gameObject);
                break;
            }
        }
    }

    public void SelectCard(CardStruct card)
    {
        if(BattleData.Instance.LastUse.index == 23)
        {

        }
        else if(BattleData.Instance.LastUse.index == 26)
        {

        }
    }

    //DeckNum�� TrashNum�� ����
    public void UpdateDeckTrashNum()
    {
        DeckNum.text = "남은 카드\n" + BattleData.Instance.Deck.Count.ToString();
        TrashNum.text = "버려진 카드\n" + BattleData.Instance.Trash.Count.ToString();
    }

    public void UpdateEnergy()
    {
        EnergyText.text = BattleData.Instance.CurrentEnergy.ToString() + "/" + BattleData.Instance.MaxEnergy.ToString();
    }

    public void UpdateTurn()
    {
        TurnText.text = "턴      " + BattleData.Instance.CurrentTurn.ToString();
    }

    //�� ���� ��ư�� ������ ����
    public async void EndClick()
    {
        for (int i = 0; i < HandUI.transform.childCount; i++)
        {

            Destroy(HandUI.transform.GetChild(i).gameObject);
        }
        TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
        StartCoroutine(Battle.End_turn(taskCompletionSource));
        await taskCompletionSource.Task;
        Debug.Log("End");
        StartCoroutine(Turn_Start());
    }

    public IEnumerator Turn_Start()
    {
        Debug.Log("Start");
        IsCoroutineRun = true;
        DeckUI.interactable = false;
        TrashUI.interactable = false;
        EndUI.interactable = false;
        Battle.Start_turn();
        for (int i = 0; i < 3; i++)
        {
            if(i == 0)
            {
                if (EnemyData.Instance.Pat[i] != 0)
                {
                    //Enemy1Text.text = EnemyData.Instance.PatText[i];
                    Enemy1Text.text = "공격 " + (4 + (stage * 2)).ToString();
                }
                else
                {
                    Enemy1Text.text = "";
                }
            }
            if(i == 1)
            {
                if (EnemyData.Instance.Pat[i] != 0)
                {
                    //Enemy2Text.text = EnemyData.Instance.PatText[i];
                    Enemy2Text.text = "공격 " + (4 + (stage * 2)).ToString();
                }
                else
                {
                    Enemy2Text.text = "";
                }
            }
            if (i == 2)
            {
                if (EnemyData.Instance.Pat[i] != 0)
                {
                    //Enemy3Text.text = EnemyData.Instance.PatText[i];
                    Enemy3Text.text = "공격 " + (4 + (stage * 2)).ToString();
                }
                else
                {
                    Enemy3Text.text = "";
                }
            }
        }
        for (int i = 0; i < BattleData.Instance.StartHand; i++)
        {

            yield return new WaitForSecondsRealtime(0.5f);
            Draw();
        }
        DeckUI.interactable = true;
        TrashUI.interactable = true;
        EndUI.interactable = true;
        IsCoroutineRun = false;
    }

    //ESC키로 PauseUI 띄우기
    public void OnPauseStarted()
    {
        UIManager.Instance.ShowUI("TitleBG");
        UIManager.Instance.ShowUI("PauseUI", false);
    }

    public void SelectEnemy()
    {
        //UIManager.Instance.ShowUI("SelectEnemyUI").GetComponent<SelectEnemyUI>().Init(EnemyData.Instance.AllEnemyData);
    }

    public IEnumerator PlayerDie()
    {
        IsCoroutineRun=true;
        Player.GetComponent<Animator>().SetTrigger("die");
        yield return new WaitForSecondsRealtime(1.5f);
        IsCoroutineRun = false;
        UIManager.Instance.ShowUI("GameOverUI").GetComponent<GameOverUI>();
    }

    public IEnumerator EnemyDie(int num)
    {
        IsCoroutineRun = true;
        Enemy.transform.Find("Enemy" + (num + 1).ToString()).GetChild(1).GetComponent<Animator>().SetTrigger("die");
        yield return new WaitForSecondsRealtime(0.7f);
        Destroy(Enemy.transform.Find("Enemy" + (num + 1).ToString()).GetChild(1).gameObject);
        GameObject.Find("Enemy" + (num + 1).ToString()).GetComponentInChildren<TextMeshProUGUI>().text = "";
        IsCoroutineRun = false;
    }

    public void PlayerHurt()
    {
        Player.GetComponent<Animator>().SetTrigger("hurt");
    }

    public void PlayerAttack()
    {
        Player.GetComponent<Animator>().SetTrigger("attack");
    }

    public void EnemyHurt(int num)
    {
        Enemy.transform.Find("Enemy" + (num + 1).ToString()).GetChild(1).GetComponent<Animator>().SetTrigger("hurt");
    }
    public void EnemyAttack(int num)
    {
        Enemy.transform.Find("Enemy" + (num + 1).ToString()).GetChild(1).GetComponent<Animator>().SetTrigger("attack");
    }

    public void PlayerWin()
    {
        UIManager.Instance.ShowUI("BattleWinUI").GetComponent<BattleWinUI>().Init(Room, EnemyInfo);
    }

    //EnemyInfo가 0이라면 AllEnemyData의 NoneEnemyNames 리스트에서 랜덤으로 이름을 뽑아 $Images/EnemyUI + 이름으로 된 UI를 불러온다.
    public void LoadEnemy()
    {
        EnemyData.Instance.Reset();
        if (stage < 4)
        {
            if(EnemyInfo < 100)
            {
                for (int i = 1; i < stage + 1; i++)
                {
                    int random = EnemyInfo == 0 ? Random.Range(0, 2 + stage) : Random.Range(0, 2);
                    Transform EnemyNum = transform.Find("Enemy").Find("Enemy" + i.ToString());
                    GameObject EnemyUI;
                    if (EnemyInfo == 0)
                    {
                        EnemyUI = AssetLoader.Instance.Instantiate($"Images/EnemyUI/" + AllEnemyData.Instance.NoneEnemyNames[random], EnemyNum);
                        EnemyData.Instance.init(i, AllEnemyData.Instance.NoneEnemyNames[random], stage);
                    }
                    else if (EnemyInfo == 1)
                    {
                        EnemyUI = AssetLoader.Instance.Instantiate($"Images/EnemyUI/" + AllEnemyData.Instance.NoneEnemyNames[random], EnemyNum);
                        EnemyData.Instance.init(i, AllEnemyData.Instance.PirateEnemyNames[random], stage);
                    }
                    else if (EnemyInfo == 2)
                    {
                        EnemyUI = AssetLoader.Instance.Instantiate($"Images/EnemyUI/" + AllEnemyData.Instance.NoneEnemyNames[random], EnemyNum);
                        EnemyData.Instance.init(i, AllEnemyData.Instance.DruidEnemyNames[random], stage);
                    }
                    else if (EnemyInfo == 3)
                    {
                        EnemyUI = AssetLoader.Instance.Instantiate($"Images/EnemyUI/" + AllEnemyData.Instance.NoneEnemyNames[random], EnemyNum);
                        EnemyData.Instance.init(i, AllEnemyData.Instance.PriestEnemyNames[random], stage);
                    }
                    else if (EnemyInfo == 4)
                    {
                        EnemyUI = AssetLoader.Instance.Instantiate($"Images/EnemyUI/" + AllEnemyData.Instance.NoneEnemyNames[random], EnemyNum);
                        EnemyData.Instance.init(i, AllEnemyData.Instance.MechanicEnemyNames[random], stage);
                    }

                    EnemyNum.GetChild(1).Find("HealthBar").GetChild(0).GetComponent<HealthBarEnemyUI>().init(i);
                    EnemyNum.GetChild(1).Find("HealthBar").GetChild(1).GetComponent<ShieldBarEnemyUI>().init(i);
                    EnemyNum.GetChild(1).Find("EnemyStat").GetComponent<EnemyStatUI>().init(i);
                }
            }
            else
            {
                int ran = EnemyInfo == 0 ? Random.Range(0, 2 + stage) : Random.Range(0, 2);
                Transform EnemyNum = transform.Find("Enemy").Find("Enemy1");
                GameObject EnemyUI;
                if (EnemyInfo == 100)
                {
                    EnemyUI = AssetLoader.Instance.Instantiate($"Images/EnemyUI/PirateBoss", EnemyNum);
                    EnemyData.Instance.init(1, "PirateBoss", stage);
                }
                else if (EnemyInfo == 101)
                {
                    EnemyUI = AssetLoader.Instance.Instantiate($"Images/EnemyUI/" + AllEnemyData.Instance.NoneEnemyNames[ran], EnemyNum);
                    EnemyData.Instance.init(1, "DruidBoss", stage);
                }
                else if (EnemyInfo == 102)
                {
                    EnemyUI = AssetLoader.Instance.Instantiate($"Images/EnemyUI/" + AllEnemyData.Instance.NoneEnemyNames[ran], EnemyNum);
                    EnemyData.Instance.init(1, "PriestBoss", stage);
                }
                else if (EnemyInfo == 103)
                {
                    EnemyUI = AssetLoader.Instance.Instantiate($"Images/EnemyUI/" + AllEnemyData.Instance.NoneEnemyNames[ran], EnemyNum);
                    EnemyData.Instance.init(1, "MechanicBoss", stage);
                }
                EnemyNum.GetChild(1).Find("HealthBar").GetChild(0).GetComponent<HealthBarEnemyUI>().init(1);
                EnemyNum.GetChild(1).Find("HealthBar").GetChild(1).GetComponent<ShieldBarEnemyUI>().init(1);
                EnemyNum.GetChild(1).Find("EnemyStat").GetComponent<EnemyStatUI>().init(1);
            }
        }
        else
        {
            Transform EnemyNum = transform.Find("Enemy").Find("Enemy1");
            GameObject EnemyUI;
            EnemyUI = AssetLoader.Instance.Instantiate($"Images/EnemyUI/" + AllEnemyData.Instance.NoneEnemyNames[1], EnemyNum);
            EnemyData.Instance.init(1, "LastBoss", stage);
            EnemyNum.GetChild(1).Find("HealthBar").GetChild(0).GetComponent<HealthBarEnemyUI>().init(1);
            EnemyNum.GetChild(1).Find("HealthBar").GetChild(1).GetComponent<ShieldBarEnemyUI>().init(1);
            EnemyNum.GetChild(1).Find("EnemyStat").GetComponent<EnemyStatUI>().init(1);
        }
    }
}
