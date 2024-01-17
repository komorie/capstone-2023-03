
using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DataStructs;

//���� ���ӿ��� ����ϱ� ���� �ҷ��;��� �����͵� ����. ��ü ī��, ��ü ��ȭ ���, ��ü ��������Ʈ �̹��� ��� ��
public class GameData : Singleton<GameData>
{

    private bool isLoaded = false;

    private Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
    private List<CardStruct> cardList;
    private Dictionary<int, List<LineStruct>> dialogDic;
    private Dictionary<int, RewardStruct> rewardDic;

    //���� ��ο� �´� ��������Ʈ ���� ��ü�� ����� ��ųʸ�
    public Dictionary<string, Sprite> SpriteDic { get => spriteDic; set => spriteDic = value; }
    //ī�� ��ü�� ����� ����Ʈ
    public List<CardStruct> CardList { get => cardList; set => cardList = value; }
    //��ȭ �α� ��ü�� ����� ����Ʈ
    public Dictionary<int, List<LineStruct>> DialogDic { get => dialogDic; set => dialogDic = value; }
    //���������� ���� ��ųʸ� (�ε���, ���� ����ü)
    public Dictionary<int, RewardStruct> RewardDic { get => rewardDic; set => rewardDic = value; }


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        LoadGameData();
    }

    //I/O �ѹ��� �̸� �� �ε�
    public void LoadGameData()
    {
        if (isLoaded) return;
        LoadSpriteDic();
        GameDataLoader.LoadData("Data/CardLibrary", out cardList); //out���� ���� �� -> ���ڷ� �����ϵ��� �ؼ� �������̵� �Ϸ���
        GameDataLoader.LoadData("Data/Dialog", out dialogDic);
        GameDataLoader.LoadData("Data/Reward", out rewardDic);
        isLoaded = true;
    }

    //��������Ʈ ���� �ε�
    public void LoadSpriteDic()
    {
        Debug.Log("��������Ʈ �ε�");
        Sprite[] sprites = AssetLoader.Instance.LoadAll<Sprite>("Images");
        foreach (Sprite sprite in sprites)
        {
            SpriteDic.Add(sprite.name, sprite);
        }
    }
}
