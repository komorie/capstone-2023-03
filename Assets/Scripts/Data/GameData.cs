
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

    //파일 경로에 맞는 스프라이트 파일 전체가 저장된 딕셔너리
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

    //I/O 한번에 미리 다 로딩
    public void LoadGameData()
    {
        if (isLoaded) return;
        LoadSpriteDic();
        GameDataLoader.LoadData("Data/CardLibrary", out cardList); //out으로 만든 거 -> 인자로 구별하도록 해서 오버라이드 하려고
        GameDataLoader.LoadData("Data/Dialog", out dialogDic);
        GameDataLoader.LoadData("Data/Reward", out rewardDic);
        isLoaded = true;
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
}
