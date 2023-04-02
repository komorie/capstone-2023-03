
using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public int index;
    public string name;
    public string description;
    public int cost;
}

[System.Serializable]
public class LineData
{
    public string portrait;
    public string name;
    public string line;
}

//���� ���ӿ��� ����ϱ� ���� �ҷ��;��� �����͵� ����. ��ü ī��, ��ü ��ȭ ���, ��ü ��������Ʈ �̹��� ��� ��
public class GameDataCon : Singleton<GameDataCon>
{

    //���� ��ο� �´� ��������Ʈ ���� ��ü�� ����� ��ųʸ�
    //���� �ε��ϴ� ������ I/O ���̷���
    public Dictionary<string, Sprite> SpriteDic { get; set; } = new Dictionary<string, Sprite>();

    //ī�� ��ü�� ����� ����Ʈ
    public List<CardData> CardList { get; set; } = new List<CardData>();

    //��ȭ �α� ��ü�� ����� ����Ʈ
    public Dictionary<int, List<LineData>> DialogDic { get; set; } = new Dictionary<int, List<LineData>>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        LoadSpriteDic();
        LoadCardList();
        LoadDialogDic();
    }




    //��������Ʈ ���� �ε�
    public void LoadSpriteDic()
    {
        Sprite[] sprites = AssetLoader.Instance.LoadAll<Sprite>("Images/Portrait");
        foreach (Sprite sprite in sprites)
        {
            SpriteDic.Add(sprite.name, sprite);
        }
    }

    //ī�� ����Ʈ �ε�
    public void LoadCardList()
    {
        string filePath = "Assets/Resources/Data/CardLibrary.json";
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            CardList = JsonMapper.ToObject<List<CardData>>(jsonData);
        }
    }

    //��ȭ �α� ��ü �ε�
    public void LoadDialogDic()
    {
        string filePath = "Assets/Resources/Data/Dialog.json";
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            JsonData dialogData = JsonMapper.ToObject(jsonString);
            for(int i = 0; i < dialogData.Count; i++)
            {
                int index = (int)dialogData[i]["index"];
                List<LineData> lines = new List<LineData>();

                for(int j = 0; j < dialogData[i]["lines"].Count; j++)
                {
                    LineData line = new LineData();
                    line.portrait = dialogData[i]["lines"][j]["portrait"]?.ToString();
                    line.name = dialogData[i]["lines"][j]["name"]?.ToString();
                    line.line = dialogData[i]["lines"][j]["line"].ToString();
                    lines.Add(line);
                }
                DialogDic.Add(index, lines);   
            }
        }
    }

    //��ȭ ��Ͽ��� �� �� �ҷ�����
    public LineData GetLine(int index, int lineIndex)
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
}
