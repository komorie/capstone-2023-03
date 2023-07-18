
using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DataStructs;

//대충 게임에서 사용하기 위해 불러와야할 데이터들 모임. 전체 카드, 전체 대화 목록, 전체 스프라이트 이미지 목록 등
public class GameData : Singleton<GameData>
{

    private bool isLoaded = false;

    private Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
    private List<CardStruct> cardList;
    private Dictionary<int, List<LineStruct>> dialogDic;
    private Dictionary<int, RewardStruct> rewardDic;

    //파일 경로에 맞는 스프라이트 파일 전체가 저장된 딕셔너리, 굳이 로드하는 이유는 I/O 줄이려고
    public Dictionary<string, Sprite> SpriteDic { get => spriteDic; set => spriteDic = value; }
    //카드 전체가 저장된 리스트
    public List<CardStruct> CardList { get => cardList; set => cardList = value; }
    //대화 로그 전체가 저장된 리스트
    public Dictionary<int, List<LineStruct>> DialogDic { get => dialogDic; set => dialogDic = value; }
    //스테이지별 보상 딕셔너리 (인덱스, 보상 구조체)
    public Dictionary<int, RewardStruct> RewardDic { get => rewardDic; set => rewardDic = value; }


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        LoadGameData();
    }

    public void LoadGameData()
    {
        if (isLoaded) return;
        LoadSpriteDic();
        GameDataLoader.LoadData("Data/CardLibrary", out cardList);
        GameDataLoader.LoadData("Data/Dialog", out dialogDic);
        GameDataLoader.LoadData("Data/Reward", out rewardDic);
        isLoaded = true;
    }

    //대화 목록에서 특정한 줄 불러오기
    public LineStruct GetLine(int index, int lineIndex)
    {
        if (lineIndex >= DialogDic[index].Count)
        {
            return null;
        }
        else
        {
            return DialogDic[index][lineIndex];
        }
    }


    //스프라이트 전부 로드
    public void LoadSpriteDic()
    {
        Debug.Log("스프라이트 로드");
        Sprite[] sprites = AssetLoader.Instance.LoadAll<Sprite>("Images");
        foreach (Sprite sprite in sprites)
        {
            SpriteDic.Add(sprite.name, sprite);
        }
    }


    //일반화 전 코드들
   /* //카드 리스트 로드
    public void LoadCardList()
    {
        Debug.Log("카드 리스트 로드");
        string filePath = "Data/CardLibrary";
        TextAsset jsonData = AssetLoader.Instance.Load<TextAsset>(filePath);
        if (jsonData != null)
        {
            CardList = JsonMapper.ToObject<List<CardStruct>>(jsonData.text);
        }
        else
        {
            Debug.LogError("Cannot find file at " + filePath);
        }
    }

    public void LoadRewardDic()
    {
        Debug.Log("보상 리스트 로드");
        string filePath = "Data/Reward";
        TextAsset jsonData = AssetLoader.Instance.Load<TextAsset>(filePath);
        if (jsonData != null) {
            string jsonString = jsonData.text;
            JsonData rewardData = JsonMapper.ToObject(jsonString);

            for (int i = 0; i < rewardData.Count; i++)
            {
                int index = (int)rewardData[i]["index"];

                RewardStruct reward = new RewardStruct();
                reward.money = (int)rewardData[i]["money"];
                reward.viewers = (int)rewardData[i]["viewers"];
                RewardDic.Add(index, reward);
            }
        }
    }

    //대화 로그 전체 로드
    public void LoadDialogDic()
    {
        Debug.Log("다이얼로그 리스트 로드");
        string filePath = "Data/Dialog";
        TextAsset jsonData = AssetLoader.Instance.Load<TextAsset>(filePath);
        if (jsonData != null)
        {
            string jsonString = jsonData.text;
            JsonData dialogData = JsonMapper.ToObject(jsonString);
            for(int i = 0; i < dialogData.Count; i++)
            {
                int index = (int)dialogData[i]["index"];

                List<LineStruct> lines;
                if (!DialogDic.TryGetValue(index, out lines)) //index가 같으면 한 대사 묶음으로 판단하고 index를 키로 갖는 리스트 저장.
                {
                    lines = new List<LineStruct>(); 
                    DialogDic.Add(index, lines);
                }


                LineStruct line = new LineStruct(); //딕셔너리의 리스트에 저장할 한 줄 단위의 대화
                line.portrait = dialogData[i]["portrait"]?.ToString();
                line.name = dialogData[i]["name"]?.ToString();
                line.line = dialogData[i]["line"].ToString();

                lines.Add(line);
            }
        }
    }*/
}
