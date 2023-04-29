
using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DataStructs;

//���� ���ӿ��� ����ϱ� ���� �ҷ��;��� �����͵� ����. ��ü ī��, ��ü ��ȭ ���, ��ü ��������Ʈ �̹��� ��� ��
public class GameData : Singleton<GameData>
{

    private bool isLoaded = false;

    //���� ��ο� �´� ��������Ʈ ���� ��ü�� ����� ��ųʸ�, ���� �ε��ϴ� ������ I/O ���̷���
    public Dictionary<string, Sprite> SpriteDic { get; set; } = new Dictionary<string, Sprite>();

    //ī�� ��ü�� ����� ����Ʈ
    public List<CardStruct> CardList { get; set; } = new List<CardStruct>();

    //��ȭ �α� ��ü�� ����� ����Ʈ
    public Dictionary<int, List<LineStruct>> DialogDic { get; set; } = new Dictionary<int, List<LineStruct>>();

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
        LoadCardList();
        LoadDialogDic();
        isLoaded = true;
    }

    //��ȭ ��Ͽ��� Ư���� �� �ҷ�����
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


    //��������Ʈ ���� �ε�
    public void LoadSpriteDic()
    {
        Debug.Log("��������Ʈ �ε�");
        Sprite[] sprites = AssetLoader.Instance.LoadAllInSubfolders<Sprite>("Images");
        foreach (Sprite sprite in sprites)
        {
            SpriteDic.Add(sprite.name, sprite);
        }
    }

    //ī�� ����Ʈ �ε�
    public void LoadCardList()
    {
        Debug.Log("ī�� ����Ʈ �ε�");
        string filePath = "Assets/Resources/Data/CardLibrary.json";
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            CardList = JsonMapper.ToObject<List<CardStruct>>(jsonData);
        }
    }

    //��ȭ �α� ��ü �ε�
    public void LoadDialogDic()
    {
        Debug.Log("���̾�α� ����Ʈ �ε�");
        string filePath = "Assets/Resources/Data/Dialog.json";
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            JsonData dialogData = JsonMapper.ToObject(jsonString);
            for(int i = 0; i < dialogData.Count; i++)
            {
                int index = (int)dialogData[i]["index"];

                List<LineStruct> lines;
                if (!DialogDic.TryGetValue(index, out lines)) //index�� ������ �� ��� �������� �Ǵ��ϰ� index�� Ű�� ���� ����Ʈ ����.
                {
                    lines = new List<LineStruct>(); 
                    DialogDic.Add(index, lines);
                }


                LineStruct line = new LineStruct(); //��ųʸ��� ����Ʈ�� ������ �� �� ������ ��ȭ
                line.portrait = dialogData[i]["portrait"]?.ToString();
                line.name = dialogData[i]["name"]?.ToString();
                line.line = dialogData[i]["line"].ToString();

                lines.Add(line);
            }
        }
    }
}
